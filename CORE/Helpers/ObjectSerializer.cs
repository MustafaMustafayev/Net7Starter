using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CORE.Helpers;

public static class ObjectSerializer
{
    public static string SerializeToXml<T>(T value)
    {
        var serializer = new XmlSerializer(typeof(T));

        var settings = new XmlWriterSettings
        {
            Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
            Indent = false,
            OmitXmlDeclaration = false
        };

        using var textWriter = new StringWriter();
        using (var xmlWriter = XmlWriter.Create(textWriter, settings))
        {
            serializer.Serialize(xmlWriter, value);
        }

        return textWriter.ToString();
    }

    public static string SerializeToJson<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    public static HttpContent GetHttpContentObject(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8);
    }
}