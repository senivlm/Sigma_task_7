using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Sigma_task7
{
    class Program
    {
        private static Dictionary<string, string> Vocabulary = new Dictionary<string, string>();
        private const string exitWord = "end";
        private static Regex TranslationPattern = new Regex(@"\b(\w+)\s?-\s?(\w+)\b");
        private static Regex WordPattern = new Regex(@"\b\w+\b");
        static void Main(string[] args)
        {
            FillDictionary();

            string sentence = string.Empty;
            while (true)
            {
                Console.Write("Type sentence : ");
                sentence = Console.ReadLine();
                if (sentence == exitWord)
                    break;
           
                string result = Regex.Replace(sentence, WordPattern.ToString(), 
                    (m) => Vocabulary.ContainsKey(m.Value) ? Vocabulary[m.Value] : FindOutTranslation(m.Value), 
                    RegexOptions.IgnoreCase);

                Console.WriteLine($"Result : {result} ");
            }
            Console.Read();
        }
        private static string FindOutTranslation(string word)
        {
            Console.WriteLine($"There is no translation for '{word}', give your definition:");
            string input = string.Empty;
            while(true)
            {
                input = Console.ReadLine();

                if (!WordPattern.IsMatch(input))
                    Console.Write("Incorrect input. Try again : ");
                else
                {
                    Vocabulary.Add(word, input);
                    return input;
                }
            }
        }
        private static void FillDictionary()
        {
            Console.WriteLine("Fill dictionary (enter '{0}' to finish):", exitWord);
            string input = string.Empty;
            while(true)
            {
                input = Console.ReadLine();
                if (input == exitWord)
                    break;

                var translation = TranslationPattern.Match(input);

                if (translation.Value != string.Empty)
                    Vocabulary[translation.Groups[1].Value] = translation.Groups[2].Value;
                else
                    Console.WriteLine("Incorrect input. It wasn't saved. Try again.");
            }
        }
    }
}
