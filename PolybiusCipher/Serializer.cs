using System.IO;
using System.Text;

namespace PolybiusCipher
{
    internal sealed class Serializer
    {
        private readonly string _path;

        public Serializer(string path) => _path = path;

        public bool Serialize(string text)
        {
            bool result = false;

            using (FileStream fs = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                fs.Write(buffer, 0, text.Length);
                result = true;
            }

            return result;
        }

        public bool Deserialize(out string text)
        {
            bool result = false;
            text = string.Empty;

            using (FileStream fs = File.OpenRead(_path))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                text = Encoding.UTF8.GetString(buffer);
                result = true;
            }

            return result;
        }
    }
}