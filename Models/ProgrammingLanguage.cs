using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProgLangDownloader.Models
{
    public class ProgrammingLanguage : INotifyPropertyChanged
    {
        private string _name;
        private string _version;
        private string _currentVersion;
        private int _progress;
        private bool _isDownloadEnabled;
        private ICommand _downloadCommand;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Version
        {
            get => _version;
            set
            {
                if (SetProperty(ref _version, value))
                {
                    OnPropertyChanged(nameof(IsOutdated));
                }
            }
        }

        public string CurrentVersion
        {
            get => _currentVersion;
            set
            {
                if (SetProperty(ref _currentVersion, value))
                {
                    OnPropertyChanged(nameof(IsOutdated));
                    OnPropertyChanged(nameof(Notinstalled));
                }
            }
        }

        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public bool IsDownloadEnabled
        {
            get => _isDownloadEnabled;
            set => SetProperty(ref _isDownloadEnabled, value);
        }

        public ICommand DownloadCommand
        {
            get => _downloadCommand;
            set => SetProperty(ref _downloadCommand, value);
        }

        public bool IsOutdated
        {
            get
            {
                Console.WriteLine(_currentVersion);
                if (string.IsNullOrEmpty(CurrentVersion) || string.IsNullOrEmpty(Version))
                    return false;

                try
                {
                    var installedVersion = new Version(CurrentVersion.Replace("Installed: ", "").TrimStart('v'));
                    var latestVersion = new Version(Version.Replace("LTS: ", "").TrimStart('v'));
                    return installedVersion < latestVersion;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Notinstalled
        {  
            get => CurrentVersion == "Installed: None";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}