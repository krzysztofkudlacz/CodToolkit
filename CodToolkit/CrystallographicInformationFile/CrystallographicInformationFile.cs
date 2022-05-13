using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BioCif.Core;
using BioCif.Core.Parsing;
using CodToolkit.Crystallography;

namespace CodToolkit.CrystallographicInformationFile
{
    public class CrystallographicInformationFile
    {
        private class AtomicPosition
        {
            public string[] Labels { get; set; }

            public double X { get; set; }

            public double Y { get; set; }

            public double Z { get; set; }

            public double Occupancy { get; set; }
        }

        public ICrystalLatticeParameters Parameters { get; }

        public string[] SpaceGroupSymbols { get; }

        public IReadOnlyList<IAtomInUnitCell> AtomsInUnitCell { get; }

        public static CrystallographicInformationFile Parse(string cifText) =>
            new CrystallographicInformationFile(cifText);

        private CrystallographicInformationFile(string cifText)
        {
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(cifText));

            var block = CifParser.Parse(stream,
                    new CifParsingOptions
                    {
                        FileEncoding = Encoding.Unicode
                    })
                .FirstBlock;

            SpaceGroupSymbols = GetSpaceGroupSymbols(block);

            Parameters = GetParameters(block);

            var dataTables = block.OfType<DataTable>().ToList();

            var atomicBase = GetAtomicBase(dataTables);

            var symmetryEquivalentPositions = dataTables
                .First(dt => dt.Headers.Contains(new DataName("symmetry_equiv_pos_as_xyz")))
                .Rows
                .SelectMany(row => row.Select(r => r.GetStringValue()))
                .ToList();

            AtomsInUnitCell = GetAtomsInUnitCell(
                atomicBase, 
                symmetryEquivalentPositions);
        }

        private static ICrystalLatticeParameters GetParameters(
            DataBlock block)
        {
            block.TryGet("cell_length_a", out IDataValue cellLengthA);
            block.TryGet("cell_length_b", out IDataValue cellLengthB);
            block.TryGet("cell_length_c", out IDataValue cellLengthC);
            block.TryGet("cell_angle_alpha", out IDataValue alphaAngle);
            block.TryGet("cell_angle_beta", out IDataValue betaAngle);
            block.TryGet("cell_angle_gamma", out IDataValue gammaAngle);

            return new CrystalLatticeParameters
            {
                ConstA = cellLengthA.GetDoubleValue().Cast(),
                ConstB = cellLengthB.GetDoubleValue().Cast(),
                ConstC = cellLengthC.GetDoubleValue().Cast(),
                Alpha = alphaAngle.GetDoubleValue().Cast(),
                Beta = betaAngle.GetDoubleValue().Cast(),
                Gamma = gammaAngle.GetDoubleValue().Cast()
            };
        }

        private static string[] GetSpaceGroupSymbols(
            DataBlock block)
        {
            var labels = new List<string>();

            if (block.TryGet("symmetry_space_group_name_Hall", out IDataValue hallName))
                labels.Add(hallName.GetStringValue());

            if (block.TryGet("symmetry_space_group_name_H-M", out IDataValue hermannMaguin))
                labels.Add(hermannMaguin.GetStringValue());

            return labels.ToArray();
        }

        private static IEnumerable<AtomicPosition> GetAtomicBase(
            IEnumerable<DataTable> dataTables)
        {
            var atomicBase = new List<AtomicPosition>();

            var searchedLabels = new List<string>
            {
                "atom_site_label",
                "atom_site_type_symbol",
                "atom_type_symbol"
            };

            var atomicBaseTable = dataTables
                .First(dt => dt
                    .Headers
                    .Select(h => h.Tag)
                    .Any(h => searchedLabels
                        .Contains(h)));

            var atomicLabelIndices = searchedLabels
                .Select(l => GetColumnIndex(atomicBaseTable, l))
                .Where(i => i > -1)
                .ToList();

            var xIndex = GetColumnIndex(atomicBaseTable, "atom_site_fract_x");
            var yIndex = GetColumnIndex(atomicBaseTable, "atom_site_fract_y");
            var zIndex = GetColumnIndex(atomicBaseTable, "atom_site_fract_z");
            var occupancyIndex = GetColumnIndex(atomicBaseTable, "atom_site_occupancy");

            for (var r = 0; r < atomicBaseTable.Count; r++)
            {
                atomicBase.Add(new AtomicPosition
                {
                    Labels = atomicLabelIndices.Select(i => atomicBaseTable.Rows[r][i].GetStringValue()).ToArray(),
                    X = atomicBaseTable.Rows[r][xIndex].GetDoubleValue().Cast().ToFractionalValue(),
                    Y = atomicBaseTable.Rows[r][yIndex].GetDoubleValue().Cast().ToFractionalValue(),
                    Z = atomicBaseTable.Rows[r][zIndex].GetDoubleValue().Cast().ToFractionalValue(),
                    Occupancy = occupancyIndex > -1
                        ? atomicBaseTable.Rows[r][occupancyIndex].GetDoubleValue().Cast()
                        : 1
                });
            }

            return atomicBase;
        }

        private static IReadOnlyList<IAtomInUnitCell> GetAtomsInUnitCell(
            IEnumerable<AtomicPosition> atomicBase, 
            IReadOnlyCollection<string> symmetryOperators)
        {
            const double tolerance = 1e-6;

            var atomsInUnitCell = new List<IAtomInUnitCell>();

            foreach (var atom in atomicBase)
            {
                var parser = new Mathos.Parser.MathParser();

                var symmetricSites = symmetryOperators.Select(
                    s => s
                        .Replace("x", atom.X.AsString())
                        .Replace("y", atom.Y.AsString())
                        .Replace("z", atom.Z.AsString())
                        .Split(",".ToCharArray())
                        .Select(e => e.Trim())
                        .ToArray());

                List<(double x, double y, double z)> positions =
                    symmetricSites.Select(
                            s => (
                                parser.Parse(s[0]).ToFractionalValue(),
                                parser.Parse(s[1]).ToFractionalValue(),
                                parser.Parse(s[2]).ToFractionalValue()))
                        .ToList();

                var uniquePositions = new List<(double x, double y, double z)>();

                positions.ForEach(p =>
                {
                    if (!uniquePositions.Exists(u => Distance(p, u) <= tolerance))
                    {
                        uniquePositions.Add(p);
                    }
                });

                atomsInUnitCell.Add(new AtomInUnitCell
                {
                    Labels = atom.Labels, 
                    Occupancy = atom.Occupancy,
                    Positions = uniquePositions
                });
            }

            return atomsInUnitCell;
        }

        private static int GetColumnIndex(
            DataTable dataTable, 
            string header)
        {
            var headers = dataTable.Headers;
            for (var i = 0; i < headers.Count; i++)
            {
                if (headers[i].Tag == header) return i;
            }

            return -1;
        }

        private static double Distance(
            (double x, double y, double z) point1, 
            (double x, double y, double z) point2)
        {
            var x2 = Math.Pow(point1.x - point2.x, 2);
            var y2 = Math.Pow(point1.y - point2.y, 2);
            var z2 = Math.Pow(point1.z - point2.z, 2);

            return Math.Sqrt(x2 + y2 + z2);
        }
    }

    public static class CifExtensions
    {
        public static T Cast<T>(this T? value) where T : struct
        {
            return (T)value!;
        }

        public static double ToFractionalValue(this double coordinate)
        {
            var value = coordinate % 1;
            if (value < 0) value += 1;

            return value;
        }

        public static string AsString(this double value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
