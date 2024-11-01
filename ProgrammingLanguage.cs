using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProgLangDownloader;

public class ProgrammingLanguage : INotifyPropertyChanged
{
    private int _progress;

    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = "v1.0.0";
    
    public int Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            OnPropertyChanged();
        }
    }

    public ICommand? DownloadCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}