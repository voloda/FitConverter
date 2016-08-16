using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynastream.Fit;
using DateTime = System.DateTime;

namespace FitConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fitDest = new FileStream("ExampleMonitoringFile.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            var systemTimeNow = DateTime.Now;

            // Create file encode object
            Encode encodeDemo = new Encode(ProtocolVersion.V10);

            // Write our header
            encodeDemo.Open(fitDest);

            // Generate some FIT messages
            FileIdMesg fileIdMesg = new FileIdMesg(); // Every FIT file MUST contain a 'File ID' message as the first message
            fileIdMesg.SetSerialNumber(54321);
            fileIdMesg.SetTimeCreated(new Dynastream.Fit.DateTime(systemTimeNow));
            fileIdMesg.SetManufacturer(Manufacturer.Sigmasport);
            fileIdMesg.SetProduct(1001);
            fileIdMesg.SetNumber(0);
            fileIdMesg.SetType(Dynastream.Fit.File.Activity); // See the 'FIT FIle Types Description' document for more information about this file type.
            encodeDemo.Write(fileIdMesg); // Write the 'File ID Message'

            DeviceInfoMesg deviceInfoMesg = new DeviceInfoMesg();
            deviceInfoMesg.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));
            deviceInfoMesg.SetSerialNumber(54321);
            deviceInfoMesg.SetManufacturer(Manufacturer.Sigmasport);
            deviceInfoMesg.SetBatteryStatus(Dynastream.Fit.BatteryStatus.Good);
            encodeDemo.Write(deviceInfoMesg);

            var a = new ActivityMesg();
            a.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));
            a.SetTotalTimerTime(25);
            a.SetNumSessions(1);
            encodeDemo.Write(a);

            var r = new RecordMesg();
            
            r.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));


            r.SetAltitude(0);
            r.SetCadence(95);
            r.SetDistance(0);
            r.SetHeartRate(120);

            encodeDemo.Write(r);

            r = new RecordMesg();
            r.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow.AddSeconds(999)));

            r.SetAltitude(235);
            r.SetCadence(95);
            r.SetDistance(1200f);
            r.SetHeartRate(120);

            encodeDemo.Write(r);

            r = new RecordMesg();
            r.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow.AddSeconds(1000)));
            
            r.SetAltitude(235);
            r.SetCadence(95);
            r.SetDistance(1200f);
            r.SetHeartRate(170);

            encodeDemo.Write(r);

            r = new RecordMesg();

            r.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow.AddSeconds(1999)));

            r.SetAltitude(235);
            r.SetCadence(95);
            r.SetDistance(3600f);
            r.SetHeartRate(170);

            encodeDemo.Write(r);

            r = new RecordMesg();

            r.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow.AddSeconds(2000)));
            
            r.SetAltitude(235);
            r.SetCadence(95);
            r.SetDistance(3600f);
            r.SetHeartRate(120);
            
            encodeDemo.Write(r);
            // Update header datasize and file CRC
            encodeDemo.Close();
            fitDest.Close();

            Console.WriteLine("Encoded FIT file ExampleMonitoringFile.fit");

        }
    }
}
