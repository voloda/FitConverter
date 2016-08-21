using Dynastream.Fit;
using FitConverter.Sigma;

namespace FitConverter.FitConvert
{
    public class ActivityConverter : IConverter<SmfEntry>
    {
        private readonly IDateTimeService _dateTimeService;

        public ActivityConverter(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public void ProcessSection(SmfEntry source, IFitEncoderAdapter encoder)
        {
            var a = new ActivityMesg();

            a.SetTimestamp(new DateTime(_dateTimeService.Now));
            a.SetTotalTimerTime(source.GeneralInformation.TrainingTime / 100);
            a.SetNumSessions(1);

            encoder.Write(a);
        }
    }
}