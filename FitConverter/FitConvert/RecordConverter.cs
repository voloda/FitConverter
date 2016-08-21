using System;
using System.Collections.Generic;
using Dynastream.Fit;
using FitConverter.Sigma;
using DateTime = System.DateTime;

namespace FitConverter.FitConvert
{
    public class RecordConverter : IConverter<SmfEntry>
    {
        public void ProcessSection(SmfEntry source, IFitEncoderAdapter encoder)
        {
            var smfGeneralInformation = source.GeneralInformation;

            if (smfGeneralInformation.TimeUnderHRTargetZone > 0 || smfGeneralInformation.TimeOverHRTargetZone > 0)
            {
                throw new InvalidOperationException("Time outside zones is not supported yet - submit sample file");
            }

            var startDate = smfGeneralInformation.StartDate;

            var totalTime = (smfGeneralInformation.TimeInHRZone1 + smfGeneralInformation.TimeInHRZone2 +
                            smfGeneralInformation.TimeInHRZone3) / 100;

            var altitude = smfGeneralInformation.MaximumAltitude * 10 - smfGeneralInformation.AltitudeDifferencesUphill;

            byte hr1;
            byte hr2;
            byte hr3;

            CalculateHR(out hr1, out hr2, out hr3, smfGeneralInformation);

            // up to 3 messages based on needs

            var averageCadence = smfGeneralInformation.AverageCadence;
            var totalDistance = smfGeneralInformation.Distance;
            var distance = 0;

            if (HasTimeInHRZone1(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone1 / 100;

                startDate = startDate.AddSeconds(.5);

                WriteEntry(encoder, startDate, averageCadence, hr1, altitude, distance);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr1, altitude, distance);
            }

            if (HasTimeInHRZone2(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone2 / 100;
                
                startDate = startDate.AddSeconds(.5);

                WriteEntry(encoder, startDate, averageCadence, hr2, altitude, distance);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr2, altitude, distance);
            }

            if (HasTimeInHRZone3(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone3 / 100;
                
                startDate = startDate.AddSeconds(.5);

                WriteEntry(encoder, startDate, averageCadence, hr3, altitude, distance);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr3, altitude, distance);
            }

        }

        private static void WriteEntry(IFitEncoderAdapter encoder, DateTime startDate, byte averageCadence, byte hr,
            int altitude, int distance)
        {
            var r = new RecordMesg();
            r.SetTimestamp(new Dynastream.Fit.DateTime(startDate));
            r.SetDeviceIndex(2);
            r.SetCadence(averageCadence);
            r.SetHeartRate(hr);

            var alt = altitude / 1000f;
            r.SetAltitude(alt);
            
            var f = distance / 1f;
            r.SetDistance(f);
            encoder.Write(r);
        }

        //
        // avg = (hr1 * t1 + h2 * t2 + hr3 * t3) / (t1 + t2 + t3)
        // 3 * avg = hr1 * t1 + hr2 * t2 + hr3 * t3
        // 

        public static void CalculateHR(out byte hr1, out byte hr2, out byte hr3, SmfGeneralInformation info)
        {
            var hr1Start = info.HRZone1Start;
            var hr1End = info.HRZone2Start;
            var hr1Time = info.TimeInHRZone1;

            var hr2Start = hr1End;
            var hr2End = info.HRZone3Start;
            var hr2Time = info.TimeInHRZone2;


            var hr3Start = hr2End;
            var hr3End = info.HRZone3End;
            var hr3Time = info.TimeInHRZone3;

            var variance = 0;
            var totalTime = hr1Time + hr2Time + hr3Time;

            // not too many combinations thus skipping dynamic programming
            while (true)
            {
                for (hr1 = hr1End; hr1 >= hr1Start; hr1--)
                {
                    for (hr2 = (byte)(hr2Start + 1); hr2 <= hr2End; hr2++)
                    {
                        for (hr3 = hr3End; hr3 > hr3Start; hr3--)
                        {
                            var currentAvg = (hr1*hr1Time + hr2*hr2Time + hr3*hr3Time)/totalTime;

                            if ((currentAvg + variance == info.AverageHR) || (currentAvg - variance == info.AverageHR))
                            {
                                return;
                            }
                        }
                    }
                }

                // allow for some variance if exact value cannot be found
                variance++;
            }
        }

        private static bool HasTimeInHRZone3(SmfGeneralInformation smfGeneralInformation)
        {
            return smfGeneralInformation.TimeInHRZone3 > 0;
        }

        private static bool HasTimeInHRZone2(SmfGeneralInformation smfGeneralInformation)
        {
            return smfGeneralInformation.TimeInHRZone2 > 0;
        }

        private static bool HasTimeInHRZone1(SmfGeneralInformation smfGeneralInformation)
        {
            return smfGeneralInformation.TimeInHRZone1 > 0;
        }
    }
}