using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace reconcile2
{
    class Reconcile2
    {

        static string filename_1 { get; set; }
        static string rowname_1 { get; set; }
        static string filename_2 { get; set; }
        static string rowname_2 { get; set; }
        static string output { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine(">> reconcile");
            loadArguments(args);

            //read file 1
            Console.Write("reading {0} ...", filename_1);
            int rowToGet_1 = getRowNumber(filename_1, rowname_1);
            List<string> list_1 = new List<string>();
            List<string> tempList_1 = new List<string>(File.ReadLines(filename_1));
            tempList_1.ForEach(i => list_1.Add(getSortedLine(i, rowToGet_1)));
            Console.WriteLine("\rreading {0} 100%", filename_1);

            //sort file 1
            Console.Write("sorting {0} ...", filename_1);
            var header = list_1[0];
            list_1.RemoveAt(0);
            list_1.Sort(); //dont sort the header
            Console.WriteLine("\rsorting {0} 100%", filename_1);

            //read file 2
            Console.Write("reading {0} ...", filename_2);
            int rowToGet_2 = getRowNumber(filename_2, rowname_2);
            List<string> list_2 = new List<string>();
            List<string> tempList_2 = new List<string>(File.ReadLines(filename_2));
            tempList_2.ForEach(i => list_2.Add(getSortedLine(i, rowToGet_2)));
            Console.WriteLine("\rreading {0} 100%", filename_2);
            header = list_2[0] + '\t' + header.Split(new[] { '\t' }, 2)[1];
            list_2.RemoveAt(0);

            //search in file 1
            var allLines = compareLines(list_1, list_2, new compareIds());
            allLines.Insert(0, header);
            Console.WriteLine("\rsearching {0} 100%", filename_2);
            File.WriteAllLines(output, allLines);
            Console.WriteLine("\rwriting {0} 100%", output);

        }

        private static List<string> compareLines(List<string> list_1, List<string> list_2, compareIds compIds)
        {
            var lines = new List<string>();
            list_2.ForEach(i =>
            {
                var item = list_1.BinarySearch(i, compIds);
                if (item >= 0)
                {
                    lines.Add(i + '\t' + list_1[item].Split(new[] { '\t' }, 2)[1]);
                }
                item = 0;
            });
            return lines;
        }

        private static void loadArguments(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("format: dotnet run <input file 1> <input row 1> <input file 2> <input row 2> <output file>");
                Environment.Exit(-1);
            }
            else
            {
                filename_1 = args[0];
                rowname_1 = args[1];
                filename_2 = args[2];
                rowname_2 = args[3];
                output = args[4];
            }
        }

        private static int getRowNumber(string filename, string rowname)
        {
            var firstLine = File.ReadLines(filename).First();
            return firstLine.Split(new string[] { rowname }, StringSplitOptions.None)[0].Split('\t').Length - 1;
        }

        private static string getSortedLine(string line, int rowToGet)
        {
            var lineArray = line.Split('\t');
            line = lineArray[rowToGet];
            for (int i = 0; i < lineArray.Length; i++)
            {
                if (i != rowToGet)
                    line += '\t' + lineArray[i];
            }
            return line;
        }
    }
    public class compareIds : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x.Split('\t')[0], y.Split('\t')[0]);
        }
    }
}
