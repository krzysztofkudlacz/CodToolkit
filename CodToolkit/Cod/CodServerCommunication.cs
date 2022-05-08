using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace CodToolkit.Cod
{
    public class CodServerCommunication
    {
        public const string CodUri = "https://www.crystallography.net/cod/result";

        public static async Task<IReadOnlyList<CodEntry>> SearchCod(
            CodSearchParameters parameters)
        {
            var requiredElements = !string.IsNullOrEmpty(parameters.RequiredElements)
                ? new List<string>()
                : parameters
                    .RequiredElements
                    .Split(",".ToCharArray())
                    .Select(e => e.Trim())
                    .ToList();

            var excludedElements = !string.IsNullOrEmpty(parameters.ExcludedElements)
                ? new List<string>()
                : parameters
                    .ExcludedElements
                    .Split(",".ToCharArray())
                    .Select(e => e.Trim())
                    .ToList();

            const string dataFormat = "json";

            var codUrl = new Url(CodUri);

            for (var i = 0; i < requiredElements.Count; i++)
                codUrl.SetQueryParam($"el{i + 1}", requiredElements[i]);

            for (var i = 0; i < excludedElements.Count; i++)
                codUrl.SetQueryParam($"nel{i + 1}", excludedElements[i]);

            var parameterValues = new Dictionary<string, string>
            {
                {"strictmin", parameters.MinNumberOfDistinctElements},
                {"strictmax", parameters.MaxNumberOfDistinctElements},
                {"amin", parameters.MinA},
                {"amax", parameters.MaxA},
                {"bmin", parameters.MinB},
                {"bmax", parameters.MaxB},
                {"cmin", parameters.MinC},
                {"cmax", parameters.MaxC},
                {"alpmin", parameters.MinAlpha},
                {"alpmax", parameters.MaxAlpha},
                {"betmin", parameters.MinBeta},
                {"betmax", parameters.MaxBeta},
                {"gamin", parameters.MinGamma},
                {"gamax", parameters.MaxGamma},
            };

            foreach (var parameterValue in parameterValues.Where(parameterValue =>
                !string.IsNullOrEmpty(parameterValue.Value)))
            {
                codUrl.SetQueryParam(parameterValue.Key, parameterValue.Value.Trim());
            }

            codUrl.SetQueryParam("format", dataFormat);

            var entries = await codUrl.GetAsync().ReceiveJson<List<CodEntry>>();

            for (var i = 0; i < entries.Count; i++)
                entries[i].NumberInCollection = i + 1;

            return entries;
        }
    }
}
