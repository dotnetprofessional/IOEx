using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
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
                        streamText.ShouldBeEquivalentTo(fileStreamText);
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
                fileStreamEx.ReadLine();
                fileStreamEx.ReadLine();
                Action a = () => fileStreamEx.ReadLine();

                a.ShouldThrow<ArgumentException>();
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
                        streamText.ShouldBeEquivalentTo(fileStreamText);
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
                        streamText.ShouldBeEquivalentTo(fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void When_reading_file_without_a_line_terminator_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLineWithoutReturn.txt";
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
                        streamText.ShouldBeEquivalentTo(fileStreamText);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void When_reading_file_with_a_single_line_terminator_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
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
                        streamText.ShouldBeEquivalentTo(fileStreamText);
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

        [TestMethod]
        public void When_splitting_a_line_returns_an_array_of_Line_objects()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var expectedParts = line.Text.Split(' ');
                    var columns = line.Split(' ');
                    columns.Count.Should().Be(expectedParts.Length);
                    for (int i = 0; i < expectedParts.Length; i++)
                    {
                        columns[i].Text.ShouldBeEquivalentTo(expectedParts[i]);
                    }
                }
            }

        }

        [TestMethod]
        public void When_splitting_a_line_with_seperator_at_end_of_line_returns_an_array_of_Line_objects()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLineEndingWithSpace.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var expectedParts = line.Text.Split(' ');
                    var columns = line.Split(' ');
                    columns.Count.Should().Be(expectedParts.Length);
                    for (int i = 0; i < expectedParts.Length; i++)
                    {
                        columns[i].Text.ShouldBeEquivalentTo(expectedParts[i]);
                    }
                }
            }
        }
    }
}
