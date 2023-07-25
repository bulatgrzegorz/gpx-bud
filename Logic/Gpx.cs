using System.Xml.Serialization;

namespace gpxSlopeCalculator.Logic;

[XmlRoot(ElementName="gpx")]
public class Gpx
{
    [XmlElement(ElementName="trk")]
    public Trk Trk { get; set; }
    [XmlAttribute(AttributeName="xmlns")]
    public string Xmlns { get; set; }
    [XmlAttribute(AttributeName="version")]
    public string Version { get; set; }
    [XmlAttribute(AttributeName="creator")]
    public string Creator { get; set; }
}

[XmlRoot(ElementName="trk")]
public class Trk
{
    [XmlElement(ElementName="name")]
    public string Name { get; set; }
    [XmlElement(ElementName="trkseg")]
    public Trkseg Trkseg { get; set; }
}

[XmlRoot(ElementName="trkseg")]
public class Trkseg
{
    [XmlElement(ElementName="trkpt")]
    public List<Trkpt> Trkpt { get; set; }
}

[XmlRoot(ElementName="trkpt")]
public class Trkpt
{
    [XmlElement(ElementName="ele")]
    public double Ele { get; set; }
    
    [XmlAttribute(AttributeName="lat")]
    public double Lat { get; set; }
    
    [XmlAttribute(AttributeName="lon")]
    public double Lon { get; set; }
}
