using System;
using System.Collections.Generic;

namespace LZWCompressor
{
    public class LZW
    {
        public static DataTypes.LinkedList<int> Compress(string uncompressed, int bitsCount, int count)
        {
            DataTypes.LinkedList<int> compressed = new DataTypes.LinkedList<int>();
            double max_code = Math.Pow(2, bitsCount) - 1;

            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            for (int i = 0; i < count; ++i)
                dictionary.Add(((char)i).ToString(), i);

            int code = count;

            string w = string.Empty;

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    compressed.Add(dictionary[w]);
                    w = c.ToString();

                    if (code <= max_code)
                    {
                        dictionary.Add(wc, dictionary.Count);
                        code++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        public static string Decompress(DataTypes.LinkedList<int> compressed, int bitsCount, int count)
        {
            double max_code = Math.Pow(2, bitsCount) - 1;
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            for (int i = 0; i < count; i++)
                dictionary.Add(i, ((char)i).ToString());

            int code = count;

            string v = dictionary[compressed.Head.Data];
            compressed.Remove(compressed.Head);

            string decompressed = v;
            string pv = v;

            foreach (int num in compressed)
            {
                if (!dictionary.ContainsKey(num))
                    v = pv + pv[0];
                else
                    v = dictionary[num];

                decompressed += v;

                if(code <= max_code)
                {
                    dictionary.Add(code, pv + v[0]);
                    ++code;
                }

                pv = v;
            }

            return decompressed;
        }
    }
}
