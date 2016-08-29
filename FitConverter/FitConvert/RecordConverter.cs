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

            var altitude = smfGeneralInformation.MaximumAltitude - smfGeneralInformation.AltitudeDifferencesUphill;
            
            byte hr1;
            byte hr2;
            byte hr3;

            CalculateHR(out hr1, out hr2, out hr3, smfGeneralInformation);

            // up to 3 messages based on needs

            var averageCadence = smfGeneralInformation.AverageCadence;
            int totalDistance = (int) smfGeneralInformation.Distance;
            var distance = 0;

            sbyte minTemperature = (sbyte) smfGeneralInformation.MinimumTemperature;
            sbyte maxTemperature = (sbyte) smfGeneralInformation.MaximumTemperature;

            if (!HasTimeInHRZone1(smfGeneralInformation) && !HasTimeInHRZone2(smfGeneralInformation) &&
                !HasTimeInHRZone3(smfGeneralInformation))
            {
                totalTime = smfGeneralInformation.TrainingTime;
                var timeInZone = totalTime / 100;

                startDate = startDate.AddSeconds(1);

                WriteEntry(encoder, startDate, averageCadence, smfGeneralInformation.AverageHR, altitude, distance, minTemperature);

                startDate = startDate.AddSeconds(timeInZone);
                
                altitude += smfGeneralInformation.AltitudeDifferencesUphill;
                distance += totalDistance;

                WriteEntry(encoder, startDate, averageCadence, smfGeneralInformation.AverageHR, altitude, distance, maxTemperature);
            }

            if (HasTimeInHRZone1(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone1 / 100;

                startDate = startDate.AddSeconds(1);

                WriteEntry(encoder, startDate, averageCadence, hr1, altitude, distance, minTemperature);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr1, altitude, distance, maxTemperature);
            }

            if (HasTimeInHRZone2(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone2 / 100;
                
                startDate = startDate.AddSeconds(1);

                WriteEntry(encoder, startDate, averageCadence, hr2, altitude, distance, minTemperature);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr2, altitude, distance, maxTemperature);
            }

            if (HasTimeInHRZone3(smfGeneralInformation))
            {
                var timeInZone = smfGeneralInformation.TimeInHRZone3 / 100;
                
                startDate = startDate.AddSeconds(1);

                WriteEntry(encoder, startDate, averageCadence, hr3, altitude, distance, minTemperature);

                startDate = startDate.AddSeconds(timeInZone);
                altitude += smfGeneralInformation.AltitudeDifferencesUphill * timeInZone /
                            totalTime;
                distance += totalDistance * timeInZone / totalTime;

                WriteEntry(encoder, startDate, averageCadence, hr3, altitude, distance, maxTemperature);
            }

        }

        private static void WriteEntry(IFitEncoderAdapter encoder, DateTime startDate, byte averageCadence, byte hr,
            int altitude, int distance, sbyte temperature)
        {
            var r = new RecordMesg();
            r.SetTimestamp(new Dynastream.Fit.DateTime(startDate));
            r.SetDeviceIndex(2);
            r.SetCadence(averageCadence);
            r.SetHeartRate(hr);
            r.SetTemperature(temperature);

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
            var maxHR = info.MaximumHR;

            var hr1Start = Math.Min(maxHR, info.HRZone1Start);
            var hr1End = Math.Min(maxHR, info.HRZone2Start);
            var hr1Time = info.TimeInHRZone1;

            var hr2Start = hr1End;
            var hr2End = Math.Min(maxHR, info.HRZone3Start);
            var hr2Time = info.TimeInHRZone2;


            var hr3Start = hr2End;
            var hr3End = Math.Min(maxHR, info.HRZone3End);
            var hr3Time = info.TimeInHRZone3;

            var variance = 0;
            var totalTime = hr1Time + hr2Time + hr3Time;

            hr1 = hr2 = hr3 = 0;
            if (totalTime == 0) return;

            if (hr1Start <= maxHR && maxHR <= hr1End)
            {
                hr1Start = hr1End = maxHR;
            }
            else if (hr2Start <= maxHR && maxHR <= hr2End)
            {
                hr2Start = hr2End = maxHR;
            }
            else if (hr3Start <= maxHR && maxHR <= hr3End)
            {
                hr3Start = hr3End = maxHR;
            }

            // not too many combinations thus skipping dynamic programming
            while (true)
            {
                for (hr1 = hr1End; hr1 >= hr1Start; hr1--)
                {
                    for (hr2 = hr2End; hr2 >= hr2Start; hr2--)
                    {
                        for (hr3 = hr3End; hr3 >= hr3Start; hr3--)
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