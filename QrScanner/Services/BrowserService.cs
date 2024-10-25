using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QrScanner.Services
{
    internal class BrowserService
    {
        internal static void OpenUrlInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при открытии URL: " + ex.Message);
            }
        }

        internal static string ExtractUrl(string input)
        {
            string pattern = @"(http[s]?://[\wа-яА-Я.:/?&=#-]+)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                return match.Value;
            }
            return string.Empty;
        }
    }
}
