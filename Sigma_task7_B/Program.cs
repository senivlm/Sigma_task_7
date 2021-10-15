using System;
using System.IO;

namespace Sigma_task7_B
{
    class Program
    {
        static void Main(string[] args)
        {
            Tuple<string, double, double>[] resultList = default;
            string pathToMenu = string.Empty;
            string pathToPrices = string.Empty;

            bool success = false;
            while(!success)
            {
                Console.Write("Enter path to menu : ");
                pathToMenu = Console.ReadLine();
                Console.Write("Enter path to prices : ");
                pathToPrices = Console.ReadLine();

                try
                {
                    resultList = ProductsListCompiler.CreateList(pathToMenu, pathToPrices);
                }
                catch(InvalidDataException exeption)
                {
                    Console.WriteLine("Computing error : " + exeption.Message);
                    Console.Read();
                    return;
                }
                catch(FileNotFoundException exeption)
                {
                    Console.WriteLine(exeption.Message + "\nChoice right files!");
                    continue;
                }
                catch(ArgumentException)
                {
                    Console.WriteLine("You have to enter paht to both files to continue!");
                    continue;
                }
                catch
                {
                    Console.WriteLine("Unknown error has occured!");
                    Console.Read();
                    return;
                }

                success = true;
            }

            foreach (var item in resultList)
            {
                Console.WriteLine("{0,-15} | total price : {1,-10:F2} | total amount : {2,-15:F2}",
                    item.Item1, item.Item2, item.Item3);
            }

            Console.ReadLine();
        }
    }
}
