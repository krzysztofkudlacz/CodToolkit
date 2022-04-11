using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using CodToolkit.Algebra;

namespace CodToolkit.LaueClass
{
    public static class LaueClassCreator
    {
        private static IReadOnlyDictionary<string, double[][,]> _laueClassRotations;

        public static ILaueClass CreateLaueClass(string laueClass)
        {
            _laueClassRotations ??= GetLaueClassRotations();

            return new LaueClass(_laueClassRotations[laueClass]);
        }

        private static IReadOnlyDictionary<string, double[][,]> GetLaueClassRotations()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = assembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith("LaueClasses.xml"));

            using var xmlStream = assembly.GetManifestResourceStream(resourcePath);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlStream ?? throw new ArgumentException("Cannot read Laue classes"));

            var laueClassRotations = new Dictionary<string, double[][,]>();

            var laueClassNodes = xmlDocument.ChildNodes[1].ChildNodes;

            for (var i = 0; i < laueClassNodes.Count; i++)
            {
                var node = laueClassNodes[i];
                var symbol = node.Attributes?["Symbol"]?.Value;

                laueClassRotations.Add(symbol ?? string.Empty, GetRotationMatrices(node));
            }

            return laueClassRotations;
        }

        private static double[][,] GetRotationMatrices(XmlNode laueClass)
        {
            var rotationNodes = laueClass.ChildNodes;
            var rotations = new List<double[,]>();

            for (var i = 0; i < rotationNodes.Count; i++)
            {
                var node = rotationNodes[i];
                var elements = node.Attributes?["Matrix"]?.Value?.Split(";".ToCharArray());

                var index = 0;
                var rotation = new double[3, 3];

                for (var k = 0; k < 3; k++)
                {
                    for (var l = 0; l < 3; l++)
                    {
                        if (elements != null) rotation[k, l] = double.Parse(elements[index].Trim());
                        index++;
                    }
                }

                rotations.Add(rotation);
            }

            return rotations.ToArray();
        }

        private class LaueClass : ILaueClass
        {
            private readonly IReadOnlyList<IMatrix3X3> _rotations;

            public LaueClass(IEnumerable<double[,]> rotations)
            {
                _rotations = rotations.Select(rotation =>
                    {
                        var matrix = new Matrix3X3();
                        for (var i = 0; i < 3; i++)
                        {
                            for (var j = 0; j < 3; j++)
                                matrix[i, j] = rotation[i, j];
                        }

                        return matrix;
                    })
                    .ToList();

                Count = _rotations.Count;
            }

            public IEnumerator<IMatrix3X3> GetEnumerator()
            {
                return _rotations.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Count { get; }

            public IMatrix3X3 this[int index] => _rotations[index];
        }
    }
}
