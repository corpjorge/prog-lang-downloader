using System.Windows.Input;

namespace ProgLangDownloader.Models
{
    public class ProgrammingLanguage
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string currentVersion { get; set; }
        public int Progress { get; set; }
        public bool IsDownloadEnabled { get; set; }
        public ICommand DownloadCommand { get; set; }
    }
}