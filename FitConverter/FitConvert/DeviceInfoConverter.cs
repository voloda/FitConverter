using Dynastream.Fit;
using FitConverter.Sigma;

namespace FitConverter.FitConvert
{
    public class DeviceInfoConverter : IConverter<SmfEntry>
    {
        private readonly IDateTimeService _dateTimeService;

        public DeviceInfoConverter(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public void ProcessSection(SmfEntry source, IFitEncoderAdapter encoder)
        {
            DeviceInfoMesg deviceInfoMesg = new DeviceInfoMesg();

            deviceInfoMesg.SetTimestamp(new DateTime(_dateTimeService.Now));
            deviceInfoMesg.SetSerialNumber(source.Computer.Serial);
            deviceInfoMesg.SetManufacturer(Manufacturer.Sigmasport);
            deviceInfoMesg.SetBatteryStatus(Dynastream.Fit.BatteryStatus.Good);

            encoder.Write(deviceInfoMesg);
        }
    }
}