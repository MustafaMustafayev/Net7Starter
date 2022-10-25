using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Project.CORE.Helper;

public class ObjectSerializer
{
    public static string SerializeToXml<T>(T value)
    {
        var serializer = new XmlSerializer(typeof(T));

        var settings = new XmlWriterSettings();
        settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
        settings.Indent = false;
        settings.OmitXmlDeclaration = false;

        using (var textWriter = new StringWriter())
        {
            using (var xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                serializer.Serialize(xmlWriter, value);
            }

            return textWriter.ToString();
        }
    }

    public static string SerializeToJson<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }
}