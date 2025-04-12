using System.IO;
using System.Xml.Serialization;

public class XMLSerializer : ISerializer
{
    public string FileExtension { get; protected set; } = ".xml";

    public string Closer { get; protected set; } = "";

    public string Opener { get; protected set; } = "";

    public string Separator { get; protected set; } = "";

    public string Serialize(TrackerEvent e) 
    {
        XmlSerializer xmlSerializer = new XmlSerializer(e.GetType());
        using (StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, e);
            return textWriter.ToString();
        }
    }
}
