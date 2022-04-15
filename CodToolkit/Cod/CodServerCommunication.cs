using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Flurl;
using Flurl.Http;

namespace CodToolkit.Cod
{
    public class CodServerCommunication
    {
        public const string CodUri = "https://www.crystallography.net/cod/result";

        public async Task<IReadOnlyList<CodEntry>> QueryCod()
        {
            var codEntries = await GetEntriesInfo();
            for (var i = 0; i < codEntries.Count; i++)
                codEntries[i].NumberInCollection = i + 1;

            return codEntries;
        }

        private async Task<IReadOnlyList<CodEntry>> GetEntriesInfo()
        {
            var requiredElements = new List<string> { "Ca", "C", "O", "Mg" };
            var excludedElements = new List<string> { "H", "Al", "B", "Si" };

            const string dataFormat = "json";

            var codUrl = new Url(CodUri);

            for (var i = 0; i < requiredElements.Count; i++)
                codUrl.SetQueryParam($"el{i + 1}", requiredElements[i]);

            for (var i = 0; i < excludedElements.Count; i++)
                codUrl.SetQueryParam($"nel{i + 1}", excludedElements[i]);

            codUrl.SetQueryParam("format", dataFormat);

            var entries = await codUrl.GetAsync().ReceiveJson<List<CodEntry>>();

            return entries;
        }
    }
}
