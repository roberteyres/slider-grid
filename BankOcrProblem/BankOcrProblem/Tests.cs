using NUnit.Framework;
using System;
using System.Linq;

namespace BankOcrProblem
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Parse_SimpleExample1()
        {
            using (var fileData = new DataSource().Open("9"))
            {
                var data = new OcrParser().Parse(fileData);

                Assert.That(data.Count(), Is.EqualTo(1), "We should only get 1 row back");
                Assert.That(data.Single().Count(), Is.EqualTo(9), "We should get 9 numbers back");
                Assert.That(data.Single().All(x => x == 9), "Every number should equal 9");
            }
        }

        [Test]
        public void Parse_Fucking_Everything()
        {
            using (var fileData = new DataSource().Open("examples"))
            {
                var data = new OcrParser().Parse(fileData);

                Assert.That(data.Count(), Is.EqualTo(12), "We should get 12 rows back");
                Assert.That(data.First().Count(), Is.EqualTo(9), "We should get 9 numbers back from the first row");
                Assert.That(data.First().All(x => x == 0), "Every number should equal 0");
            }
        }

        [Test]
        public void Chunk_ReturnsExpectedStrings()
        {
            using (var fileData = new DataSource().Open("examples"))
            {
                var chunkedData = new OcrParser().Chunk(fileData);

                var row1 = chunkedData.First();

                var character1 = row1.First();

                Assert.That(character1 != null);
            }
        }

        [Test]
        public void Train_ExampleData()
        {
            using (var fileData = new DataSource().Open("examples"))
            {
                var parser = new OcrParser();

                foreach(var n in Enumerable.Range(0, 10))
                {
                    parser.Train(fileData, n, 0, n);
                }

                int r = 1;
                foreach(var output in parser.Parse(fileData))
                {
                    Console.WriteLine("Row: " + r++);

                    foreach(var col in output)
                    {
                        Console.Write("{0}|", col);
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}