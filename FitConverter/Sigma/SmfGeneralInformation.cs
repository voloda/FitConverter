using System;
using System.Xml.Serialization;

namespace FitConverter.Sigma
{
    public class SmfGeneralInformation
    {
        [XmlElement("startDate")]
        public string StartDateAsString { get; set; }

        [XmlIgnore]
        public DateTime StartDate
        {
            get { return DateTime.Parse(StartDateAsString); }
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("GUID")]
        public Guid Guid { get; set; }

        [XmlElement("altitudeDifferencesDownhill")]
        public int AltitudeDifferencesDownhill { get; set; }

        [XmlElement("altitudeDifferencesUphill")]
        public int AltitudeDifferencesUphill { get; set; }

        [XmlElement("averageCadence")]
        public int AverageCadence { get; set; }

        [XmlElement("averageHeartrate")]
        public int AverageHR { get; set; }

        [XmlElement("calories")]
        public int Calories { get; set; }

        [XmlElement("distance")]
        public int Distance { get; set; }

        [XmlElement("distanceDownhill")]
        public int DistanceDownhill { get; set; }

        [XmlElement("distanceUphill")]
        public int DistanceUphill { get; set; }

        [XmlElement("maximumAltitude")]
        public int MaximumAltitude { get; set; }

        [XmlElement("maximumCadence")]
        public byte MaximumCadence { get; set; }
        

        [XmlElement("maximumTemperature")]
        public float MaximumTemperature { get; set; }

        [XmlElement("minimumTemperature")]
        public float MinimumTemperature { get; set; }

        #region Times
        [XmlElement("trainingTimeDownhill")]
        public int trainingTimeDownhill { get; set; }

        [XmlElement("trainingTimeUphill")]
        public int trainingTimeUphill { get; set; }


        [XmlElement("timeUnderTargetZone")]
        public int TimeUnderHRTargetZone { get; set; }

        [XmlElement("timeInZone1")]
        public int TimeInHRZone1 { get; set; }

        [XmlElement("timeInZone2")]
        public int TimeInHRZone2 { get; set; }

        [XmlElement("timeInZone3")]
        public int TimeInHRZone3 { get; set; }

        [XmlElement("timeOverTargetZone")]
        public int TimeOverHRTargetZone { get; set; }

        [XmlElement("trainingTime")]
        public int TrainingTime { get; set; }
        #endregion

        #region HR Zones definition
        [XmlElement("zone1Start")]
        public byte HRZone1Start { get; set; }

        [XmlElement("zone2Start")]
        public byte HRZone2Start { get; set; }

        [XmlElement("zone3Start")]
        public byte HRZone3Start { get; set; }

        [XmlElement("zone3End")]
        public byte HRZone3End { get; set; }
        #endregion

    }
}