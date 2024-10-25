﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrScanner.Services
{
    internal class RussianTextConverter
    {
        private static Dictionary<char, char> translationMap = new Dictionary<char, char>()
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

        internal static string ConvertText(string input)
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
    }
}
