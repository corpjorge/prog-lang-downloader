using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using ProgLangDownloader.Commands;
using ProgLangDownloader.Models;
using ProgLangDownloader.Services;

namespace ProgLangDownloader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadLanguagesAsync();
        }

        private async void LoadLanguagesAsync()
        {
            var latestNodeVersion = await NodeVersionService.GetLatestLTSNodeVersionAsync();
            var installedNodeVersion = NodeVersionService.GetInstalledNodeVersion();

            var languages = new List<ProgrammingLanguage>
            {
                new ProgrammingLanguage
                {
                    Name = "NODEJS (LTS)",
                    Version = $"LTS: {latestNodeVersion}",
                    currentVersion = $"Instalada: {installedNodeVersion}",
                    Progress = 0,
                    IsDownloadEnabled = true,
                    DownloadCommand = new RelayCommand(async () => await StartDownload("NODEJS"))
                },
                // Otras entradas de lenguajes de programación aquí
            };

            LanguageListView.ItemsSource = languages;
        }


        private async Task StartDownload(string languageName)
        {
            var language = ((List<ProgrammingLanguage>)LanguageListView.ItemsSource).Find(l => l.Name == languageName);
            if (language != null)
            {
                language.IsDownloadEnabled = false;

                for (int i = 0; i <= 100; i += 10)
                {
                    language.Progress = i;
                    await Task.Delay(100);
                }

                MessageBox.Show($"{languageName} descargado con éxito!");
                language.IsDownloadEnabled = true;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Actualizando...");
        }
    }
}