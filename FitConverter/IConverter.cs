using System.Collections.Generic;
using System.IO;
using System.Text;
using Dynastream.Fit;

namespace FitConverter
{
    public interface IConverter<T>
    {
        void ProcessSection(T source, IFitEncoderAdapter encoder);
    }

    public interface IFitEncoderAdapter
    {
        void Write(Mesg messge);
    }

    public class FitEncoderAdapter : IFitEncoderAdapter
    {
        private readonly Encode _fitEncode;

        public FitEncoderAdapter(Encode fitEncode)
        {
            _fitEncode = fitEncode;
        }

        public void Write(Mesg mesg)
        {
            _fitEncode.Write(mesg);
        }
        
    }
}