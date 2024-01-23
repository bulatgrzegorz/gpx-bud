using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace gpxSlopeCalculator.Logic;

public static class GpxSerializer
{
    public static Gpx Deserialize(Stream fileStream)
    {
        var serializer = new XmlSerializer(typeof(Gpx), new XmlRootAttribute("gpx"));
        using var xmlReader = new XmlTextReader(fileStream);
        xmlReader.Namespaces = false;
        return (Gpx)serializer.Deserialize(xmlReader)!;
    }
    
    public static Gpx Deserialize(string fileStream)
    {
        var serializer = new XmlSerializer(typeof(Gpx), new XmlRootAttribute("gpx"));

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileStream));
        using var xmlReader = new XmlTextReader(memoryStream);
        xmlReader.Namespaces = false;
        return (Gpx)serializer.Deserialize(xmlReader)!;
    }
}