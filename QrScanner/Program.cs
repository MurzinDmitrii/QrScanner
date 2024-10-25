using System;
using System.Text;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using QrScanner.Services;

class Program
{
    static string result;
    static IKeyboardMouseEvents globalHook;
    static StringBuilder inputBuffer = new StringBuilder();

    static void Main(string[] args)
    {
        result = "";
        System.Threading.Timer _timer = new System.Threading.Timer(ClearConsole, null, 0, 50);

        globalHook = Hook.GlobalEvents();
        globalHook.KeyDown += GlobalHookKeyDown;
        globalHook.KeyPress += GlobalHookKeyPress;

        // Ожидание завершения
        Application.Run();

        // Остановка перехватчика
        globalHook.KeyDown -= GlobalHookKeyDown;
        globalHook.KeyPress -= GlobalHookKeyPress;
        globalHook.Dispose();
    }

    private static void GlobalHookKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Q)
        {
            Application.Exit();
        }
    }

    private static void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)13) // Enter
        {
            result = inputBuffer.ToString();
            result = RussianTextConverter.ConvertText(result);
            result = BrowserService.ExtractUrl(result);

            try
            {
                BrowserService.OpenUrlInBrowser(result);
                inputBuffer.Clear();
            }
            catch
            {
                Console.WriteLine("Некорректный адрес");
            }
        }
        else
        {
            inputBuffer.Append(e.KeyChar);
            Console.Write(e.KeyChar);
        }
    }

    private static void ClearConsole(object state)
    {
        Console.Clear();
    }
}
