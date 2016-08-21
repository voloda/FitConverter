using System.Xml.Serialization;

namespace FitConverter.Sigma
{
    public class SmfComputer
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; }

        [XmlAttribute("serial")]
        public string Serial { get; set; }
    }
}