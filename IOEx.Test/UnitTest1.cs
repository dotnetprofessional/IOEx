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
        public void When_reading_file_with_large_Lines_and_large_buffer_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\LargeLineFile.txt";
            var sw = new Stopwatch();
            sw.Start();
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                using (var streamEx = new StreamReader(filename))
                {
                    Line line;
                    while ((line = fileStreamEx.ReadLine()) != null)
                    {
                        var streamText = streamEx.ReadLine();
                        var fileStreamText = line.Text;
                        Assert.AreEqual(streamText, fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void When_reading_file_with_large_Lines_and_small_buffer_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\LargeLineFile.txt";
            var sw = new Stopwatch();
            sw.Start();
            using (var fileStreamEx = new FileStreamEx(filename, 1000))
            {
                var a = 0;
                using (var streamEx = new StreamReader(filename))
                {
                    Line line;
                    while ((line = fileStreamEx.ReadLine()) != null)
                    {
                        var streamText = streamEx.ReadLine();
                        var fileStreamText = line.Text;
                        Assert.AreEqual(streamText, fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }


        [TestMethod]
        public void When_reading_file_with_medium_Lines_and_large_buffer_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\MediumLineFile.txt";
            var sw = new Stopwatch();
            sw.Start();
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                using (var streamEx = new StreamReader(filename))
                {
                    Line line;
                    while ((line = fileStreamEx.ReadLine()) != null)
                    {
                        var streamText = streamEx.ReadLine();
                        var fileStreamText = line.Text;
                        Assert.AreEqual(streamText, fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void When_reading_file_with_medium_Lines_and_small_buffer_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\MediumLineFile.txt";
            var sw = new Stopwatch();
            sw.Start();
            using (var fileStreamEx = new FileStreamEx(filename, 1000))
            {
                var a = 0;
                using (var streamEx = new StreamReader(filename))
                {
                    Line line;
                    while ((line = fileStreamEx.ReadLine()) != null)
                    {
                        var streamText = streamEx.ReadLine();
                        var fileStreamText = line.Text;
                        Assert.AreEqual(streamText, fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        string GetDataDirectory()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        }
    }
}
