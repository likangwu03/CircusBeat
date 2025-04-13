public interface ISerializer
{
    string FileExtension { get; }
    string Closer { get; }
    string Opener { get; }
    string Separator { get; }
    string Serialize(TrackerEvent e);
}
