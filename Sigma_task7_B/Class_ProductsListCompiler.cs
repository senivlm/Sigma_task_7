using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Sigma_task7_B
{
    public static class ProductsListCompiler
    {
        private static Regex RecordPattern = new Regex(@"\b([A-Z]\w+)\s{1,10}([0-9]+(,[0-9]+)?)\b");

        public static Tuple<string,double,double>[] CreateList(string menuFilePath, string pricesFilePath)
        {
            var prices = ReadPrices(pricesFilePath);
            var amounts = ReadMenu(menuFilePath);

            var resultList = from p in prices
                             join a in amounts on p.Key equals a.Key
                             select new Tuple<string,double,double>(p.Key, a.Value * p.Value, a.Value);

            if (amounts.Count != resultList.Count())
                throw new InvalidDataException("Could not find price for each item.");

            return resultList.ToArray();
        }

        private static Dictionary<string, double> ReadMenu(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("This string can not be path to file with menu. ");
            if (!File.Exists(path))
                throw new FileNotFoundException("File is not exist. Path to menu is wrong." + $"({path})");

            var amounts = new Dictionary<string, double>();
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                var matches = RecordPattern.Matches(content);

                foreach (var match in matches.ToArray())
                    //groups[1] - product name, groups[2] - amount in double format
                    if (amounts.ContainsKey(match.Groups[1].Value))
                        amounts[match.Groups[1].Value] += double.Parse(match.Groups[2].Value);
                    else
                        amounts.Add(match.Groups[1].Value, double.Parse(match.Groups[2].Value));
            }
            return amounts;
        }

        private static Dictionary<string, double> ReadPrices(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path can not be path to file with prices.");
            if (!File.Exists(path))
                throw new FileNotFoundException("File is not exist. Path to prices is wrong." + $"({path})");

            var prices = new Dictionary<string, double>();
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                var matches = RecordPattern.Matches(content);

                foreach (var match in matches.ToArray())
                    //groups[1] - product name, groups[2] - price in double format
                    prices.Add(match.Groups[1].Value, double.Parse(match.Groups[2].Value));
            }
            return prices;
        }
    }
}
