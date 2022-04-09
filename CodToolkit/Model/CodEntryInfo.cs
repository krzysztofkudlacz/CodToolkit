using Newtonsoft.Json;

namespace CodToolkit.Model
{
    public class CodEntryInfo
    {
        [JsonIgnore]
        public int NumberInCollection { get; set; }

        [JsonIgnore]
        public string CellParameters => $"{a}; {b}; {c}; {alpha}; {beta}; {gamma}";

        [JsonProperty("file")]
        public string FileId { get; set; }

        public string a { get; set; }

        public object siga { get; set; }

        public string b { get; set; }

        public object sigb { get; set; }

        public string c { get; set; }

        public object sigc { get; set; }

        public string alpha { get; set; }

        public object sigalpha { get; set; }

        public string beta { get; set; }

        public object sigbeta { get; set; }

        public string gamma { get; set; }

        public object siggamma { get; set; }

        public string vol { get; set; }

        public object sigvol { get; set; }

        public string celltemp { get; set; }

        public object sigcelltemp { get; set; }

        public string diffrtemp { get; set; }

        public object sigdiffrtemp { get; set; }

        public object cellpressure { get; set; }

        public object sigcellpressure { get; set; }

        public object diffrpressure { get; set; }

        public object sigdiffrpressure { get; set; }

        public object thermalhist { get; set; }

        public object pressurehist { get; set; }

        public object compoundsource { get; set; }

        public string nel { get; set; }

        [JsonProperty("sg")]
        public string SpaceGroup { get; set; }

        [JsonProperty("sgHall")]
        public string SpaceGroupHall { get; set; }

        [JsonProperty("commonname")]
        public object CommonName { get; set; }

        public object chemname { get; set; }

        [JsonProperty("mineral")]
        public object Mineral { get; set; }

        [JsonProperty("formula")]
        public string Formula { get; set; }

        public string calcformula { get; set; }

        public string cellformula { get; set; }

        public string Z { get; set; }

        public string Zprime { get; set; }

        public object acce_code { get; set; }

        public string authors { get; set; }

        public string title { get; set; }

        public object journal { get; set; }

        public object year { get; set; }

        public object volume { get; set; }

        public object issue { get; set; }

        public object firstpage { get; set; }

        public object lastpage { get; set; }

        public object doi { get; set; }

        public object method { get; set; }

        public object radiation { get; set; }

        public string wavelength { get; set; }

        public string radType { get; set; }

        public object radSymbol { get; set; }

        public object Rall { get; set; }

        public string Robs { get; set; }

        public object Rref { get; set; }

        public object wRall { get; set; }

        public object wRobs { get; set; }

        public string wRref { get; set; }

        public object RFsqd { get; set; }

        public object RI { get; set; }

        public object gofall { get; set; }

        public object gofobs { get; set; }

        public object gofgt { get; set; }

        public string gofref { get; set; }

        public object duplicateof { get; set; }

        public object optimal { get; set; }

        public object status { get; set; }

        public string flags { get; set; }

        public string svnrevision { get; set; }

        public string date { get; set; }

        public string time { get; set; }

        public object onhold { get; set; }
    }
}
