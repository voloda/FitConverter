using System;
using System.IO;
using System.Text;
using FitConverter.Sigma;
using NUnit.Framework;

namespace FitConveter.Tests.Sigma
{
    [TestFixture]
    public class SmfReaderTest
    {
        const string validEntry = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Activity fileDate=""Tue Aug 16 21:00:41 GMT+0200 2016"" revision=""400"">
  <Computer unit=""rox90"" serial=""90126995"" activityType=""Cycling"" dateCode=""Sat Jul 9 10:01:00 GMT+0200 2016""/>
  <GeneralInformation>
    <user color=""0"" gender=""male""><![CDATA[voloda]]></user>
    <sport><![CDATA[racing_bycicle]]></sport>
    <GUID>D16C0F9A-BE77-083A-0F61-12A125FF5F0E</GUID>
    <altitudeDifferencesDownhill>722000</altitudeDifferencesDownhill>
    <altitudeDifferencesUphill>721000</altitudeDifferencesUphill>
    <averageCadence>94</averageCadence>
    <averageHeartrate>152</averageHeartrate>
    <averageInclineDownhill>2</averageInclineDownhill>
    <averageInclineUphill>3</averageInclineUphill>
    <averageRiseRateUphill>0</averageRiseRateUphill>
    <averageRiseRateDownhill>0</averageRiseRateDownhill>
    <averageSpeed>8.155555555555555</averageSpeed>
    <averageSpeedDownhill>9.025</averageSpeedDownhill>
    <averageSpeedUphill>6.708333333333333</averageSpeedUphill>
    <bike>bike1</bike>
    <calories>2811</calories>
    <description><![CDATA[]]></description>
    <distance>110130</distance>
    <distanceDownhill>23180</distanceDownhill>
    <distanceUphill>19200</distanceUphill>
    <externalLink><![CDATA[]]></externalLink>
    <hrMax>187</hrMax>
    <linkedTrackGUID/>
    <manualTemperature>0</manualTemperature>
    <maximumAltitude>439000</maximumAltitude>
    <maximumCadence>119</maximumCadence>
    <maximumHeartrate>174</maximumHeartrate>
    <maximumInclineDownhill>-6</maximumInclineDownhill>
    <maximumInclineUphill>8</maximumInclineUphill>
    <maximumRiseRate>0</maximumRiseRate>
    <maximumSpeed>13.305555555555555</maximumSpeed>
    <maximumTemperature>34.1</maximumTemperature>
    <measurement>kmh</measurement>
    <minimumRiseRate>0</minimumRiseRate>
    <minimumTemperature>22.9</minimumTemperature>
    <name><![CDATA[Jahodnice - Nymburk - Český Brod - Mukařov - Jahodnice]]></name>
    <pauseTime>0</pauseTime>
    <rating>1</rating>
    <feeling>2</feeling>
    <trainingTimeDownhill>256900</trainingTimeDownhill>
    <trainingTimeUphill>286200</trainingTimeUphill>
    <samplingRate>0</samplingRate>
    <startDate>Sat Jul 9 10:01:00 GMT+0200 2016</startDate>
    <statistic>true</statistic>
    <timeInTargetZone>0</timeInTargetZone>
    <timeInZone1>70400</timeInZone1>
    <timeInZone2>523800</timeInZone2>
    <timeInZone3>736600</timeInZone3>
    <timeOverTargetZone>2</timeOverTargetZone>
    <timeUnderTargetZone>1</timeUnderTargetZone>
    <trackProfile>0</trackProfile>
    <trainingTime>1350300</trainingTime>
    <trainingType/>
    <weather>0</weather>
    <wheelSize>2133</wheelSize>
    <wind>0</wind>
    <zone1Start>103</zone1Start>
    <zone2Start>132</zone2Start>
    <zone3End>188</zone3End>
    <zone3Start>150</zone3Start>
    <sharingInfo>{""twitterId"":""0"",""twoPeaksId"":""0"",""stravaId"":""0"",""facebookId"":""0"",""trainingPeaksId"":""0""}</sharingInfo>
    <Participant/>
  </GeneralInformation>
</Activity>";

        [Test]
        public void DeserializedEntryShouldContainExpectedInformation()
        {
            var reader = new SmfReader();
            using (var data = new MemoryStream(Encoding.UTF8.GetBytes(validEntry)))
            {
                var entry = reader.Read(data);

                Assert.That(entry.Computer.Serial, Is.EqualTo("90126995"));

                // Assert.That(entry.GeneralInformation.StartDate, Is.EqualTo(new DateTime(2016, 7, 9, 10, 1, 0)));

                Assert.That(entry.GeneralInformation.Guid, Is.EqualTo(new Guid("D16C0F9A-BE77-083A-0F61-12A125FF5F0E")));

                Assert.That(entry.GeneralInformation.Name, Is.EqualTo("Jahodnice - Nymburk - Český Brod - Mukařov - Jahodnice"));

                Assert.That(entry.GeneralInformation.AverageCadence, Is.EqualTo(94));
                Assert.That(entry.GeneralInformation.AverageHR, Is.EqualTo(152));

                Assert.That(entry.GeneralInformation.TimeUnderHRTargetZone, Is.EqualTo(1));
                Assert.That(entry.GeneralInformation.TimeInHRZone1, Is.EqualTo(70400));
                
                Assert.That(entry.GeneralInformation.TimeInHRZone2, Is.EqualTo(523800));
                Assert.That(entry.GeneralInformation.TimeInHRZone3, Is.EqualTo(736600));
                Assert.That(entry.GeneralInformation.TrainingTime, Is.EqualTo(1350300));
                Assert.That(entry.GeneralInformation.TimeOverHRTargetZone, Is.EqualTo(2));

                Assert.That(entry.GeneralInformation.HRZone1Start, Is.EqualTo(103));
                Assert.That(entry.GeneralInformation.HRZone2Start, Is.EqualTo(132));
                Assert.That(entry.GeneralInformation.HRZone3Start, Is.EqualTo(150));
                Assert.That(entry.GeneralInformation.HRZone3End, Is.EqualTo(188));
            }
        }
    }
}