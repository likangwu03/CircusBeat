public interface ISerializer
{
    string FileExtension { get; }
    string Serialize(TrackerEvent e);
}
