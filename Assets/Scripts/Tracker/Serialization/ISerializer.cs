public interface ISerializer
{
    string FileExtension { get; }
    string Serialize(TrackerEvent e);
    string Closer { get; }
    string Opener { get; }
    string Separator { get; }
}
