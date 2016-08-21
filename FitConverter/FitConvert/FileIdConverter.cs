using Dynastream.Fit;
using FitConverter.Sigma;

namespace FitConverter.FitConvert
{
    public class FileIdConverter : IConverter<SmfEntry>
    {
        private readonly IDateTimeService _dateTimeService;

        public FileIdConverter(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public void ProcessSection(SmfEntry source, IFitEncoderAdapter encoder)
        {
            var fileIdMesg = new FileIdMesg(); // Every FIT file MUST contain a 'File ID' message as the first message
            fileIdMesg.SetSerialNumber((uint?) source.GeneralInformation.Guid.GetHashCode());
            fileIdMesg.SetTimeCreated(new DateTime(_dateTimeService.Now));
            fileIdMesg.SetManufacturer(Manufacturer.Sigmasport);
            fileIdMesg.SetProduct(1001);
            fileIdMesg.SetNumber(0);
            fileIdMesg.SetType(File.Activity); // See the 'FIT FIle Types Description' document for more information about this file type.
            encoder.Write(fileIdMesg); // Write the 'File ID Message'
        }
    }
}