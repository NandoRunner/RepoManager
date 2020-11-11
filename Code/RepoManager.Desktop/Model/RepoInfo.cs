namespace RepoManager.Desktop.Model
{

    public enum RepoType
    {
        undefined,
        VisualStudio,
        AndroidStudio,
        VSCode
    }

    public class RepoInfo
    {
        public string Name { get; set; }

        public string Path { get; set; }

    }
}
