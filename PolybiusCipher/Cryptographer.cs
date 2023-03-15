using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace PolybiusCipher
{
    internal sealed class Cryptographer
    {
        private readonly Serializer _serializer;

        public Cryptographer(string path) => _serializer = new Serializer(path);

        private readonly Dictionary<string, (int, int)> Keywords = new Dictionary<string, (int, int)>()
        {
            { "a", (1, 1) }, { "b", (1, 2) }, { "c", (1, 3) }, { "d", (1, 4) }, { "e", (1, 5) },
            { "f", (2, 1) }, { "g", (2, 2) }, { "h", (2, 3) }, { "ij",(2, 4) }, { "k", (2, 5) },
            { "l", (3, 1) }, { "m", (3, 2) }, { "n", (3, 3) }, { "o", (3, 4) }, { "p", (3, 5) },
            { "q", (4, 1) }, { "r", (4, 2) }, { "s", (4, 3) }, { "t", (4, 4) }, { "u", (4, 5) },
            { "v", (5, 1) }, { "w", (5, 2) }, { "x", (5, 3) }, { "y", (5, 4) }, { "z", (5, 5) },
        };

        public bool Encrypt(string text)
        {
            StringBuilder newText = new StringBuilder();
            (int, int) value;
            foreach (char symbol in text.ToLower())
            {
                if (char.IsDigit(symbol))
                    throw new ArgumentException("Текст не должен содержать числа", nameof(text));

                try
                {
                    value = Keywords.First(k => k.Key.Contains(symbol.ToString())).Value;
                    newText.Append($"{value.Item1}{value.Item2}");
                }
                catch (InvalidOperationException)
                {
                    newText.Append(symbol);
                }
            }

            return _serializer.Serialize(newText.ToString());
        }

        public bool Decrypt(out string text)
        {
            bool result = _serializer.Deserialize(out text);
            StringBuilder newText = new StringBuilder(text);

            List<(int, int)> pairs = new List<(int, int)>();
            List<int> pairsIndexes = new List<int>();

            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    if (i + 1 < text.Length)
                    {
                        if (char.IsDigit(text[i + 1]))
                        {
                            pairs.Add((int.Parse(text[i].ToString()), int.Parse(text[i + 1].ToString())));
                            pairsIndexes.Add(i);
                            ++i;
                        }
                    }
                }
            }

            int index = 0;
            int ijs = 0;
            for (int i = 0; i < pairs.Count; i++)
            {
                newText.Remove(pairsIndexes[i] - index, 1);
                string value = Keywords.First(k => k.Value == pairs[i]).Key;
                newText.Insert(pairsIndexes[i] - index, value);
                if (value.Contains("ij"))
                {
                    newText.Remove((pairsIndexes[i] + 1) - i + 1 + ijs, 1);
                    ++ijs;
                    continue;
                }
                newText.Remove((pairsIndexes[i] + 1) - i + ijs, 1);
                ++index;
            }

            text = newText.ToString();

            return result;
        }
    }
}