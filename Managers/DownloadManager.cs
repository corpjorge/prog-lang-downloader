using System.Windows;
using ProgLangDownloader.Models;
using ProgLangDownloader.Services;

namespace ProgLangDownloader.Managers
{
    public class DownloadManager
    {
        public async Task StartDownloadAsync(ProgrammingLanguage language, IProgress<int> progressHandler)
        {
            Console.WriteLine($"StartDownloadAsync {language.Name}...");

            if (language.Name == "NODEJS (LTS)")
            {
                try
                {
                    await NodeVersionService.DownloadAndSaveLTSNodeVersionAsync(progressHandler);
                    MessageBox.Show("Node.js LTS download completed.");

                    language.CurrentVersion = $"Installed: {NodeVersionService.GetInstalledNodeVersion()}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading: {ex.Message}");
                }
                finally
                {
                    language.IsDownloadEnabled = true;
                }
            }
        }
    }
}