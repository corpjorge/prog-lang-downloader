using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.IO.Compression;
using System.Text.Json.Serialization;
using ProgLangDownloader.Helpers;

namespace ProgLangDownloader.Services
{
    public static class GoVersionService
    {
        public static async Task<string> GetLatestLTSGoVersionAsync()
        {
            using var client = new HttpClient();

            var versions = await client.GetFromJsonAsync<List<GoVersion>>("https://go.dev/dl/?mode=json");

            var latestWindowsVersion = versions
                .Where(v => v.Stable && v.Files.Any(f => f.OS == "windows" && f.Kind == "installer"))
                .FirstOrDefault();

            return latestWindowsVersion?.Version ?? "vN/A";
        }

        public static string GetInstalledGoVersion()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "go",
                        Arguments = "version",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();

                string version = process.StandardOutput.ReadLine();
                process.WaitForExit();

                return version;
            }
            catch
            {
                return "None";
            }
        }

        public static async Task DownloadAndSaveLTSNodeVersionAsync(IProgress<int> progress)
        {
            var latestLTSVersion = await GetLatestLTSGoVersionAsync();

            var downloadUrl = $"https://nodejs.org/dist/{latestLTSVersion}/node-{latestLTSVersion}-win-x64.zip";
            Console.WriteLine($"downloadUrl ...{downloadUrl}");
            if (downloadUrl == null)
            {
                throw new Exception("Could not get download URL for LTS release.");
            }

            var outputPath = @$"C:\Dev\bin\temp\node-lts-{latestLTSVersion}.zip";
            var directoryPath = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            Console.WriteLine($"Directory created (if not existing): {directoryPath}");


            using var client = new HttpClient();
            var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? 1L;
            var stream = await response.Content.ReadAsStreamAsync();
            var buffer = new byte[8192];
            var bytesRead = 0L;
            var percentComplete = 0;

            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read);
                    bytesRead += read;

                    var newProgress = (int)((double)bytesRead / totalBytes * 80);
                    if (newProgress > percentComplete)
                    {
                        percentComplete = newProgress;
                        progress?.Report(percentComplete);
                    }
                }
            }

            var extractPath = @$"C:\Dev\bin\temp\node-lts-{latestLTSVersion}";

            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }

            ZipFile.ExtractToDirectory(outputPath, extractPath);
            Console.WriteLine($"Unzipped file in: {extractPath}");

            var extractedFolder = Directory.GetDirectories(extractPath).FirstOrDefault();
            var destinationPath = @"C:\Dev\bin";

            if (extractedFolder != null)
            {
                var originalFolderName = Path.GetFileName(extractedFolder);
                var adjustedFolderName = originalFolderName.Replace("-win-x64", "");
                var finalDestination = Path.Combine(destinationPath, adjustedFolderName);

                if (Directory.Exists(finalDestination))
                {
                    Directory.Delete(finalDestination, true);
                }

                Directory.Move(extractedFolder, finalDestination);
                Console.WriteLine($"Folder moved to: {finalDestination}");
                NodePathHelper.UpdateNodePathAndInstallNpm(finalDestination);
            }

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }

            Console.WriteLine("'Temp' directory cleaned.");
        }
    }

    public class GoVersion
    {
        [JsonPropertyName("version")] public string Version { get; set; }

        [JsonPropertyName("stable")] public bool Stable { get; set; }

        [JsonPropertyName("files")] public List<GoFile> Files { get; set; }
    }

    public class GoFile
    {
        [JsonPropertyName("filename")] public string Filename { get; set; }

        [JsonPropertyName("os")] public string OS { get; set; }

        [JsonPropertyName("arch")] public string Arch { get; set; }

        [JsonPropertyName("kind")] public string Kind { get; set; }
    }
}