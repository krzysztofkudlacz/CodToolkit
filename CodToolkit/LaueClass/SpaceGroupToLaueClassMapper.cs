using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodToolkit.Crystallography;

namespace CodToolkit.LaueClass
{
    public static class SpaceGroupToLaueClassMapper
    {
        private static IEnumerable<ISpaceGroupInfo>
            _spaceGroupInfo;

        public static string LaueClassSymbol(
            string spaceGroupHermannMaguinName)
        {
            var hmName = spaceGroupHermannMaguinName
                .ToUpper()
                .Replace(" ", string.Empty)
                .Replace(":R", string.Empty)
                .Replace(":H", string.Empty);

            return SpaceGroupInfos()
                .First(info =>
                    info.HermannMaguinName == hmName ||
                    info.HallName == hmName)
                .LaueClassSymbol;
        }

        private static IEnumerable<ISpaceGroupInfo> 
            SpaceGroupInfos()
        {
            if (_spaceGroupInfo != null)
                return _spaceGroupInfo;

            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = assembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith("SpaceGroupToLaueMap.txt"));

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader =
                new StreamReader(stream ??
                                 throw new ArgumentException("Cannot read space group to Laue class map"));

            var spaceGroupInfos = new List<SpaceGroupInfo>();

            while (!reader.EndOfStream)
            {
                var input = reader.
                    ReadLine()?.
                    Split(";".ToCharArray());

                if (input == null) continue;

                spaceGroupInfos.Add(new SpaceGroupInfo
                {
                    LaueClassSymbol = input[0].Trim(),
                    HallName = RemoveWhiteSpacesTrimToUpper(input[3].Trim()),
                    HermannMaguinName = RemoveWhiteSpacesTrimToUpper(input[4].Trim())
                });
            }

            _spaceGroupInfo = spaceGroupInfos;
            return _spaceGroupInfo;
        }

        private static string RemoveWhiteSpacesTrimToUpper(
            string str)
        {
            return str.Replace(" ", string.Empty).Trim().ToUpper();
        }
    }
}
