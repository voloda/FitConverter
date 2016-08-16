using System.Xml.Serialization;

namespace FitConverter.Sigma
{
    [XmlRoot("Activity")]
    public class SmfEntry
    {
        [XmlElement]
        public SmfComputer Computer { get; set; }

        [XmlElement]
        public SmfGeneralInformation GeneralInformation { get; set; }
    }
}