
namespace CA.Application.Helpers
{
    public class FileSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
        public bool IsAcceptedFiles(string fileName)
        {
            return AcceptedFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());
        }
        public string FilePath { get; set; }
        public string FolderName { get; set; }
    }
}
