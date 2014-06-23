using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SendEmail
{
	/// <summary>
	/// http://www.jonasjohn.de/snippets/csharp/xmlserializer-example.htm
	/// </summary>
	public class SerializerHelper
	{
		public static void Serialize(Type type, object obj, string file)
		{
			// Create a new XmlSerializer instance with the type of the test class
			XmlSerializer serializerObj = new XmlSerializer(type);

			// Create a new file stream to write the serialized object to a file
			TextWriter writeFileStream = new StreamWriter(file);
			serializerObj.Serialize(writeFileStream, obj);

			// Cleanup
			writeFileStream.Close();
		}

		public static object Deserialize(Type type, string file)
		{
			// Create a new XmlSerializer instance with the type of the test class
			XmlSerializer serializerObj = new XmlSerializer(type);

			// Create a new file stream for reading the XML file
			FileStream readFileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

			// Load the object saved above by using the Deserialize function
			object loadedObj = serializerObj.Deserialize(readFileStream);

			// Cleanup
			readFileStream.Close();

			return loadedObj;
		}
	}
}