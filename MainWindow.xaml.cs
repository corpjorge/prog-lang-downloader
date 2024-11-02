using System.Windows;
using ProgLangDownloader.Commands;
using ProgLangDownloader.Managers;
using ProgLangDownloader.Models;
using ProgLangDownloader.Services;

namespace ProgLangDownloader
{
    public partial class MainWindow : Window
    {
        private readonly DownloadManager _downloadManager;

        public MainWindow()
        {
            InitializeComponent();
            _downloadManager = new DownloadManager();
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
                    CurrentVersion = $"Installed: {installedNodeVersion}",
                    Progress = 0,
                    IsDownloadEnabled = true,
                    DownloadCommand = new RelayCommand(async () => await StartDownload("NODEJS (LTS)"))
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
                var progressHandler = new Progress<int>(value => language.Progress = value);
                await _downloadManager.StartDownloadAsync(language, progressHandler);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NodeVersionService.GetLatestLTSNodeVersionAsync();
            NodeVersionService.GetInstalledNodeVersion();
        }
    }
}