using System;
using System.Text;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static string result;
    static IKeyboardMouseEvents globalHook;
    static StringBuilder inputBuffer = new StringBuilder();
    private static bool preventNewLine = false;

    static Dictionary<char, char> translationMap = new Dictionary<char, char>()
    {
        {'й', 'q'}, {'ц', 'w'}, {'у', 'e'}, {'к', 'r'}, {'е', 't'},
        {'н', 'y'}, {'г', 'u'}, {'ш', 'i'}, {'щ', 'o'}, {'з', 'p'},
        {'х', '['}, {'ъ', ']'}, {'ф', 'a'}, {'ы', 's'}, {'в', 'd'},
        {'а', 'f'}, {'п', 'g'}, {'р', 'h'}, {'о', 'j'}, {'л', 'k'},
        {'д', 'l'}, {'ж', ';'}, {'э', '\''}, {'я', 'z'}, {'ч', 'x'},
        {'с', 'c'}, {'м', 'v'}, {'и', 'b'}, {'т', 'n'}, {'ь', 'm'},
        {'б', ','}, {'ю', '.'},
        {'Й', 'Q'}, {'Ц', 'W'}, {'У', 'E'}, {'К', 'R'}, {'Е', 'T'},
        {'Н', 'Y'}, {'Г', 'U'}, {'Ш', 'I'}, {'Щ', 'O'}, {'З', 'P'},
        {'Х', '{'}, {'Ъ', '}'}, {'Ф', 'A'}, {'Ы', 'S'}, {'В', 'D'},
        {'А', 'F'}, {'П', 'G'}, {'Р', 'H'}, {'О', 'J'}, {'Л', 'K'},
        {'Д', 'L'}, {'Ж', ':'}, {'Э', '"'}, {'Я', 'Z'}, {'Ч', 'X'},
        {'С', 'C'}, {'М', 'V'}, {'И', 'B'}, {'Т', 'N'}, {'Ь', 'M'},
        {'Б', '<'}, {'Ю', '>'}, {'.', '/'}
    };

    static void Main(string[] args)
    {
        result = "";
        System.Threading.Timer _timer = new System.Threading.Timer(ClearConsole, null, 0, 50);
        Console.WriteLine("Программа запущена. Нажмите Ctrl + Q, чтобы завершить.");

        globalHook = Hook.GlobalEvents();
        globalHook.KeyDown += GlobalHookKeyDown;
        globalHook.KeyPress += GlobalHookKeyPress;

        // Ожидание завершения
        Application.Run();

        // Остановка перехватчика
        globalHook.KeyDown -= GlobalHookKeyDown;
        globalHook.KeyPress -= GlobalHookKeyPress;
        globalHook.Dispose();

        Console.WriteLine("\nПрограмма завершена.");
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
            result = ConvertText(result);
            
            result = ExtractUrl(result);
            Console.WriteLine("\nВведенная строка: " + result);
            try
            {
                OpenUrlInBrowser(result);
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

    static void OpenUrlInBrowser(string url)
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

    static string ConvertText(string input)
    {
        char[] convertedChars = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            if (translationMap.ContainsKey(input[i]))
            {
                convertedChars[i] = translationMap[input[i]];
            }
            else
            {
                convertedChars[i] = input[i];
            }
        }
        return new string(convertedChars);
    }

    static string ExtractUrl(string input)
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

    static void ClearConsole(object state)
    {
        Console.Clear();
    }
}
