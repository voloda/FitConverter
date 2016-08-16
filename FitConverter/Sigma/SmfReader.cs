using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FitConverter.Sigma
{
	public class SmfReader
	{
		public SmfEntry Read(string file)
		{
		    using (var fileStream = File.OpenRead(file))
		    {
		        return Read(fileStream);
		    }
		}

		public SmfEntry Read(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(SmfEntry));

		    return (SmfEntry) serializer.Deserialize(stream);
		}
	}
}