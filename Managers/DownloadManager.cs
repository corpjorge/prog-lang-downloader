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
                    MessageBox.Show("Descarga de Node.js LTS completada.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al descargar: {ex.Message}");
                }
                finally
                {
                    language.IsDownloadEnabled = true;
                }
            }
        }
    }
}