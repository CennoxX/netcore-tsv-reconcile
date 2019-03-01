using System;
using System.IO;
using System.Linq;

namespace reconcile2
{
    class Program
    {

        static string filename_1 { get; set; }
        static string rowname_1 { get; set; }
        static string filename_2 { get; set; }
        static string rowname_2 { get; set; }
        static string output { get; set; }
        static void Main(string[] args)
        {
            loadArguments(args);
            int rowToGet_1 = getRowNumber(filename_1, rowname_1);
            Console.WriteLine(rowToGet_1);
            int numberOfLines_1 = getNumberOfLines(filename_1);
            Console.WriteLine(numberOfLines_1);
            string firstLine = File.ReadLines(filename_1).First();
            string sortedLine = getSortedLine(firstLine, rowToGet_1);
            Console.WriteLine(sortedLine);
        }

        private static string getSortedLine(string line, int rowToGet_1)
        {
            var lineArray = line.Split('\t');
            line = lineArray[rowToGet_1];
            for (int i = 0; i < lineArray.Length; i++)
            {
                if (i != rowToGet_1)
                    line += '\t' + lineArray[i];
            }
            return line;
        }

        private static int getNumberOfLines(string filename_1)
        {
            return File.ReadLines(filename_1).Count();
        }

        private static void loadArguments(string[] args)
        {
            if (args.Length != 5)
                Console.WriteLine("format: dotnet run <input file 1> <input row 1> <input file 2> <input row 2> <output file>");

            filename_1 = args[0];
            rowname_1 = args[1];
            filename_2 = args[2];
            rowname_2 = args[3];
            output = args[4];
        }

        private static int getRowNumber(string filename_1, string rowname_1)
        {
            return File.ReadLines(filename_1).First().Split(rowname_1)[0].Split('\t').Length - 1;
        }
    }
}
