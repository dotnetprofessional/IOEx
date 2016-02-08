using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOEx.Test
{
    [TestClass]
    public class FileStreamExTest
    {
        [TestMethod]
        public void When_reading_file_with_large_Lines_and_large_buffer_matches_result_of_streamreader()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\LargeLineFile.txt";
            var count = 0;
            var sw = new Stopwatch();
            sw.Start();
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                using (var streamEx = new StreamReader(filename))
                {
                    Line line;
                    while ((line = fileStreamEx.ReadLine()) != null)
                    {
                        count++;

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

        [TestMethod]
        public void When_splitting_a_line_with_that_does_not_have_the_seperator_returns_a_single_item_array_of_Line_objects()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\ThreeLinesWIthSeondEmpty.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var expectedParts = line.Text.Split('\t');
                    var columns = line.Split('\t');
                    columns.Count.Should().Be(expectedParts.Length);
                    for (int i = 0; i < expectedParts.Length; i++)
                    {
                        columns[i].Text.ShouldBeEquivalentTo(expectedParts[i]);
                    }
                }
            }
        }
        [TestMethod]
        public void When_checking_start_of_a_line_returns_true_if_line_starts_with_any_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLineEndingWithSpace.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.StartsWith('X', 'T');
                    found.Should().BeTrue();
                }
            }
        }

        [TestMethod]
        public void When_checking_start_of_a_line_returns_true_if_line_starts_with_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLineEndingWithSpace.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.StartsWith('T');
                    found.Should().BeTrue();
                }
            }
        }

        [TestMethod]
        public void When_checking_start_of_a_line_returns_false_if_line_does_not_start_with_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLineEndingWithSpace.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.StartsWith('Z');
                    found.Should().BeFalse();
                }
            }
        }

        [TestMethod]
        public void When_checking_end_of_a_line_returns_true_if_line_ends_with_any_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.EndsWith('Q', '.');
                    found.Should().BeTrue();
                }
            }
        }

        [TestMethod]
        public void When_checking_end_of_a_line_returns_true_if_line_ends_with_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.EndsWith('.');
                    found.Should().BeTrue();
                }
            }
        }

        [TestMethod]
        public void When_checking_end_of_a_line_returns_false_if_line_does_not_end_with_character_supplied()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var found = line.EndsWith('Z');
                    found.Should().BeFalse();
                }
            }
        }

        [TestMethod]
        public void When_stripping_characters_from_a_line_the_characters_are_removed()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var newLine = line.Strip(' ', '.');

                    var expected = line.Text.Replace(" ", "").Replace(".", "");
                    expected.ShouldBeEquivalentTo(newLine.Text);
                }
            }
        }

        [TestMethod]
        public void When_checking_line_contains_characters_true_is_returned_if_they_do()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var actual = line.Contains('Y', 'x');

                    actual.Should().BeTrue();
                }
            }
        }

        [TestMethod]
        public void When_checking_line_contains_characters_false_is_returned_if_they_do_not()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\SingleLine.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                while ((line = fileStreamEx.ReadLine()) != null)
                {
                    var actual = line.Contains('Y');

                    actual.Should().BeFalse();
                }
            }
        }

        [TestMethod]
        public void When_checking_line_IsNullOrEmpty_false_is_returned_if_line_contains_characters_other_than_spaces()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\ThreeLinesWIthSeondEmpty.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                line = fileStreamEx.ReadLine(); // Not Empty
                var actual = line.IsNullOrEmpty();

                actual.Should().BeFalse();
            }
        }

        [TestMethod]
        public void When_checking_line_IsNullOrEmpty_true_is_returned_if_line_does_not_contain_characters_other_than_spaces()
        {
            var filename = $"{this.GetDataDirectory()}\\data\\ThreeLinesWIthSeondEmpty.txt";
            using (var fileStreamEx = new FileStreamEx(filename))
            {
                Line line;
                line = fileStreamEx.ReadLine(); // Not Empty
                line = fileStreamEx.ReadLine(); // Empty
                var actual = line.IsNullOrEmpty();

                actual.Should().BeTrue();
            }
        }
    }
}
