using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProgLangDownloader.Services
{
    public static class NodeVersionService
    {
        public static async Task<string> GetLatestLTSNodeVersionAsync()
        {
            using var client = new HttpClient();
            var versions = await client.GetFromJsonAsync<List<NodeVersion>>("https://nodejs.org/dist/index.json");

            var latestLTSVersion = versions
                .Where(v => v.LTS != null && v.LTS.ToString() != "False")
                .OrderByDescending(v => DateTime.Parse(v.Date))
                .FirstOrDefault();

            return latestLTSVersion?.Version ?? "vN/A";
        }

        public static string GetInstalledNodeVersion()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "node",
                        Arguments = "-v",
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
                return "No instalado";
            }
        }
    }

    // Clase auxiliar para deserializar los datos de versiones de Node.js
    public class NodeVersion
    {
        public string Version { get; set; }
        public string Date { get; set; }
        public object LTS { get; set; }
    }
}