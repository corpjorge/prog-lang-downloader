using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace ProgLangDownloader.Helpers
{
    public static class NodePathHelper
    {
        public static void UpdateNodePathEnvironmentVariable(string newNodePath)
        {
            // Obtener la ruta exacta de la instalación actual de Node.js
            string currentNodePath = GetExistingNodePath();

            // Obtener el valor actual de la variable de entorno PATH
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            var pathParts = currentPath.Split(';');
            var updatedPathParts = new List<string>();

            foreach (var part in pathParts)
            {
                // Eliminar la ruta actual de Node.js (si existe) de PATH
                if (!string.IsNullOrEmpty(currentNodePath) && 
                    part.Equals(Path.GetDirectoryName(currentNodePath), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                updatedPathParts.Add(part);
            }

            // Añadir la nueva ruta de Node.js al principio de PATH
            updatedPathParts.Insert(0, newNodePath);

            // Unir todos los segmentos para actualizar PATH
            var newPath = string.Join(";", updatedPathParts);

            // Actualizar la variable de entorno PATH
            Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
            Console.WriteLine($"Variable de entorno PATH actualizada con la nueva ruta: {newNodePath}");
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

                return output?.Trim(); // Devuelve la ruta exacta de node.exe
            }
            catch
            {
                MessageBox.Show("No se pudo obtener la ruta de Node.js actual");
                Console.WriteLine("No se pudo obtener la ruta de Node.js actual.");
                return null;
            }
        }
    }
}
