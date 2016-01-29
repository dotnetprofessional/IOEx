using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOEx;

namespace SpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = @"D:\Passwords\testfiles\cred-sort-unique.txt";
            //if (args[0] == "x")
            //{
                FileStreamEx(filename);
            //}
            //else 
            //    FileStream(filename);
        }

        static void FileStreamEx(string filename)
        {
            Console.WriteLine("FileStreamEx");
            var sw = new Stopwatch();
            sw.Start();
            using (var reader = new FileStreamEx(filename))
            {
                Line line;
                while ((line = reader.ReadLineAsync().Result) != null)
                {
                    //var x = line.Text;
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        static void FileStream(string filename)
        {
            Console.WriteLine("FileStream");
            var sw = new Stopwatch();
            sw.Start();
            using (var reader = new StreamReader(filename))
            {
                var line = "";
                while ((line = reader.ReadLineAsync().Result) != null)
                {
                    //var x = line;
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }
    }
}
