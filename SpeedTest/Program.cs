using System;
using System.Diagnostics;
using System.IO;
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
                //FileStream(filename);
            //}
            //else 
                FileStreamEx(filename);
        }

        static void FileStreamEx(string filename)
        {
            Console.WriteLine("FileStreamEx");
            var sw = new Stopwatch();
            sw.Start();
            var count = 0;
            using (var reader = new FileStreamEx(filename))
            {
                Line line;
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                    //var x = line.Text;
                }
            }
            sw.Stop();
            Console.WriteLine($"Count: {count:N0}");
            Console.WriteLine($"Lines per sec: {count/sw.ElapsedMilliseconds * 1000}");
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
                while ((line = reader.ReadLine()) != null)
                {
                    var x = line;
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }
    }
}
