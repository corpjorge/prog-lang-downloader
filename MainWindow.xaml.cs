using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProgLangDownloader;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var languages = new List<ProgrammingLanguage>
        {
            new()
            {
                Name = "NODEJS",
                Progress = 0,
                Version = "v8.4.3",
                DownloadCommand = new RelayCommand(() => StartDownload("NODEJS"))
            },
            new()
            {
                Name = "PHP",
                Progress = 0,
                Version = "v8.4.3",
                DownloadCommand = new RelayCommand(() => StartDownload("PHP"))
            },
            new()
            {
                Name = "PYTHON",
                Progress = 0,
                Version = "v8.4.3",
                DownloadCommand = new RelayCommand(() => StartDownload("PYTHON"))
            },
            new()
            {
                Name = "GO",
                Progress = 75,
                Version = "v8.4.3",
                 DownloadCommand = new RelayCommand(() => StartDownload("PYTHON"))
            },
            new()
            {
                Name = "RUST",
                Progress = 75,
                Version = "v8.4.3",
                 DownloadCommand = new RelayCommand(() => StartDownload("PYTHON"))
            },
            new()
            {
                Name = "GIT",
                Progress = 75,
                Version = "v8.4.3",
                 DownloadCommand = new RelayCommand(() => StartDownload("PYTHON"))
            },
            new()
            {
                Name = "TERRAFORM",
                Progress = 75,
                Version = "v8.4.3",
                 DownloadCommand = new RelayCommand(() => StartDownload("PYTHON"))
            }

            // Agrega más lenguajes aquí si lo deseas
        };

        LanguageListView.ItemsSource = languages;
    }

    private async void StartDownload(string languageName)
    {
        var language = ((List<ProgrammingLanguage>)LanguageListView.ItemsSource).Find(l => l.Name == languageName);
        if (language != null)
        {
            for (int i = 0; i <= 100; i += 10)
            {
                language.Progress = i;
                await Task.Delay(100); // Simulación de tiempo de descarga
            }

            MessageBox.Show($"{languageName} descargado con éxito!");
        }
    }
}