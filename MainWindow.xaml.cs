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
            new ProgrammingLanguage
            {
                Name = "NODEJS",
                Version = "v8.4.3",
                Progress = 0,
                IsDownloadEnabled = true,
                DownloadCommand = new RelayCommand(async () => await StartDownload("NODEJS"))
            },
            new ProgrammingLanguage
            {
                Name = "PHP",
                Version = "v8.4.3",
                Progress = 0,
                IsDownloadEnabled = true,
                DownloadCommand = new RelayCommand(async () => await StartDownload("PHP"))
            },
            new ProgrammingLanguage
            {
                Name = "PYTHON",
                Version = "v8.4.3",
                Progress = 0,
                IsDownloadEnabled = true,
                DownloadCommand = new RelayCommand(async () => await StartDownload("PYTHON"))
            }
        };

        LanguageListView.ItemsSource = languages;
    }
    
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        // Lógica para actualizar la lista o el contenido de la aplicación
        MessageBox.Show("Actualizando...");
    }

    private async Task StartDownload(string languageName)
    {
        var language = ((List<ProgrammingLanguage>)LanguageListView.ItemsSource).Find(l => l.Name == languageName);
        if (language != null)
        {
            language.IsDownloadEnabled = false; // Deshabilitar el botón al iniciar la descarga

            for (int i = 0; i <= 100; i += 10)
            {
                language.Progress = i;
                await Task.Delay(100); // Simulación de tiempo de descarga
            }

            MessageBox.Show($"{languageName} descargado con éxito!");
            language.IsDownloadEnabled = true; // Habilitar el botón al terminar la descarga
        }
    }
}