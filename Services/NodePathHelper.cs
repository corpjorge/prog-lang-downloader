using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ProgLangDownloader.Helpers
{
    public static class NodePathHelper
    {
        public static void UpdateNodePathEnvironmentVariable(string newNodePath)
        {
            string currentNodePath = GetExistingNodePath();

            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            var pathParts = currentPath.Split(';');
            var updatedPathParts = new List<string>();

            foreach (var part in pathParts)
            {
                if (!string.IsNullOrEmpty(currentNodePath) &&
                    part.Equals(Path.GetDirectoryName(currentNodePath), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                updatedPathParts.Add(part);
            }

            updatedPathParts.Insert(0, newNodePath);

            var newPath = string.Join(";", updatedPathParts);

            Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
            Console.WriteLine($"PATH environment variable updated with the new path: {newNodePath}");
        }

        private static string GetExistingNodePath()
        {
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = "-Command \"(Get-Command node).Path\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadLine();
                process.WaitForExit();

                return output?.Trim();
            }
            catch
            {
                MessageBox.Show("Failed to get current Node.js path");
                Console.WriteLine("Failed to get current Node.js path.");
                return null;
            }
        }
    }
}