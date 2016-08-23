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
        private static readonly List<IConverter<SmfEntry>>  _converters = new List<IConverter<SmfEntry>>()
            {
                new FileIdConverter(new DateTimeService()),
                new DeviceInfoConverter(new DateTimeService()),
                new ActivityConverter(new DateTimeService()),
                new RecordConverter()
            };

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("FitConverter");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  FileConverter *.smf");
                Console.WriteLine("  FileConverter file.smf");
                return;
            }

            var path = ".";
            var mask = args[0];
            var pos = mask.LastIndexOf(Path.DirectorySeparatorChar);

            if (pos != -1)
            {
                path = mask.Substring(0, pos);
                mask = mask.Substring(pos + 1);
            }

            var files = Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly).OrderBy(v => v);
            var subfolderIndex = 0;

            foreach (var fileName in files)
            {
                
                var fileInfo = new FileInfo(fileName);
                var destinationDirectory = Path.Combine(fileInfo.DirectoryName, (subfolderIndex / 20).ToString());
                var di = new DirectoryInfo(destinationDirectory);
                if (!di.Exists) di.Create();

                var destinationFileName = Path.Combine(destinationDirectory, fileInfo.Name);

                destinationFileName = Path.ChangeExtension(destinationFileName, "fit");

                Console.WriteLine("Processing file {0}", destinationFileName);
                ProcessFile(fileName, destinationFileName);

                subfolderIndex++;
            }
        }

        private static void ProcessFile(string fileName, string destinationFileName)
        {
            var source = new SmfReader().Read(fileName);

            using (var fitDest = new FileStream(destinationFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                // Create file encode object
                var encodeDemo = new Encode(ProtocolVersion.V10);

                // Write our header
                encodeDemo.Open(fitDest);

                using (var encoder = new FitEncoderAdapter(encodeDemo))
                {
                    _converters.ForEach(c => c.ProcessSection(source, encoder));
                }
            }

            Console.WriteLine("Encoded FIT file {0}", destinationFileName);
        }
    }
}
