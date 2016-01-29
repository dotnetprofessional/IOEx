using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOEx.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LargeFile()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var reader = new FileStreamEx(@"D:\dev\git.private\DumpManager\Sort.Test\largetext.txt"))
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

        [TestMethod]
        public void SmallFile()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var reader = new FileStreamEx(@"D:\dev\git.private\DumpManager\Sort.Test\smalltext.txt"))
            {
                Line line;
                while ((line = reader.ReadLineAsync().Result) != null)
                {
                    var x = line.Text;
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void LargeFileStandardWay()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var reader = new StreamReader(@"D:\dev\git.private\DumpManager\Sort.Test\largetext.txt"))
            {
                var line = "";
                while ((line = reader.ReadLineAsync().Result) != null)
                {
                    var x = line;
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }
    }
}
