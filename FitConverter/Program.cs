using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynastream.Fit;
using FitConverter.FitConvert;
using FitConverter.Sigma;
using DateTime = System.DateTime;

namespace FitConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var converters = new List<IConverter<SmfEntry>>()
            {
                new FileIdConverter(new DateTimeService()),
                new DeviceInfoConverter(new DateTimeService()),
                new ActivityConverter(new DateTimeService()),
                new RecordConverter()
            };

            FileStream fitDest = new FileStream("ExampleMonitoringFile.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            
            // Create file encode object
            Encode encodeDemo = new Encode(ProtocolVersion.V10);

            // Write our header
            encodeDemo.Open(fitDest);

            var source = new SmfReader().Read(args[0]);
            var encoder = new FitEncoderAdapter(encodeDemo);

            foreach (var c in converters)
            {
                c.ProcessSection(source, encoder);
            }

            encodeDemo.Close();
            fitDest.Close();

            Console.WriteLine("Encoded FIT file ExampleMonitoringFile.fit");

        }
    }
}
