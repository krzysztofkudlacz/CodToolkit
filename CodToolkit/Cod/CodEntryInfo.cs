using Newtonsoft.Json;

namespace CodToolkit.Cod
{
    public class CodEntry
    {
        [JsonIgnore]
        public int NumberInCollection { get; set; }

        [JsonIgnore]
        public string CellParameters => $"{A}; {B}; {C}; {Alpha}; {Beta}; {Gamma}";

        [JsonProperty("file")]
        public string FileId { get; set; }

        [JsonProperty("a")]
        public string A { get; set; }

        [JsonProperty("b")]
        public string B { get; set; }

        [JsonProperty("c")]
        public string C { get; set; }

        [JsonProperty("alpha")]
        public string Alpha { get; set; }

        [JsonProperty("beta")]
        public string Beta { get; set; }

        [JsonProperty("gamma")]
        public string Gamma { get; set; }

        [JsonProperty("sg")]
        public string SpaceGroup { get; set; }

        [JsonProperty("sgHall")]
        public string SpaceGroupHall { get; set; }

        [JsonProperty("commonname")]
        public object CommonName { get; set; }

        [JsonProperty("mineral")]
        public object Mineral { get; set; }

        [JsonProperty("formula")]
        public string Formula { get; set; }
    }
}
