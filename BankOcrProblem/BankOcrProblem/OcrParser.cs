using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BankOcrProblem
{
    public class OcrParser
    {
        private readonly IDictionary<TextReader, IList<List<string>>> _dataCache;
        private readonly IDictionary<string, int> _map;

        public OcrParser()
        {
            _dataCache = new Dictionary<TextReader, IList<List<string>>>();
            _map = new Dictionary<string, int>();
        }

        public IEnumerable<int[]> Parse(TextReader data)
        {
            return ChunkCached(data).Select(r => r.Select(c => Map(c)).ToArray());
        }

        public void Train(TextReader data, int row, int col, int value)
        {
            _map[ChunkCached(data).ElementAt(row)[col]] = value;
        }

        internal IEnumerable<List<string>> ChunkCached(TextReader data)
        {
            if (_dataCache.Count == 0) _dataCache[data] = Chunk(data).ToList();

            return _dataCache[data];
        }

        internal IEnumerable<List<string>> Chunk(TextReader data)
        {
            return Chunk(Iterate(data));
        }

        private IEnumerable<string> Iterate2(TextReader reader)
        {
            yield return "1";
            yield return "2";
        }

        private IEnumerable<string> Iterate(TextReader reader)
        {
            while (true)
            {
                var nextLine = reader.ReadLine();

                if (nextLine == null) break;

                yield return nextLine;
            }
        }

        internal IEnumerable<List<string>> Chunk(IEnumerable<string> lines)
        {
            int skip = 0;
            var allRows = lines.ToList();

            while (skip*4<allRows.Count)
            {
                var nextLines = allRows.Skip(skip * 4).Take(3).ToArray();
                Debug.Assert(nextLines.All(l => l.Length == 27),"Row=" + skip + "\n" + nextLines[0] + "\n" + nextLines[1] + "\n" + nextLines[2]);
                yield return ProcessNextThreeRows(nextLines).Select(x => new string(x)).ToList();
                
                skip += 1;
            }
        }

        private IEnumerable<char[]> ProcessNextThreeRows(string[] row)
        {
            int pos = 0;

            while (pos < 9)
            {
                var topRow = row[0].Substring(pos * 3, 3);
                var secondRow = row[1].Substring(pos * 3, 3);
                var thirdRow = row[2].Substring(pos * 3, 3);

                pos++;

                yield return (topRow + secondRow + thirdRow).ToCharArray();
            }
        }

        private int Map(string characterData)
        {
            return _map[characterData];
        }
    }
}