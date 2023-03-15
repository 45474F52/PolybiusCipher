using System;
using System.IO;

namespace PolybiusCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string answer = Question("Укажите путь до директории:\n(напишите false, если хотите использовать путь до рабочего стола)\n");
                string path;

                if (answer.ToLower() == "false")
                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                else
                    path = File.Exists(answer) ? answer : throw new ArgumentException("Не верный путь", nameof(answer));

                Cryptographer cryptographer = new Cryptographer(Path.Combine(path, "cipher.txt"));

                answer = Question("Зашифровать\t1\nРасшифровать\t2\n", false);

                if (answer == "1")
                {
                    answer = Question("Напишите текст:\n");

                    if (cryptographer.Encrypt(answer))
                        _ = Question("Текст зашифрован", false);
                }
                else if (answer == "2")
                {
                    if (cryptographer.Decrypt(out string newText))
                        _ = Question($"Расшифрованный текст:\n{newText}", false);
                }
                else
                    throw new ArgumentException("Не верное значение", nameof(answer));
            }
            catch (Exception ex)
            {
                ShowException(ex);
                Console.ReadKey(false);
            }
        }

        private static string Question(string text, bool checkAnswer = true)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
            string answer = Console.ReadLine();
            if (checkAnswer)
                CheckAnswer(answer);
            Console.WriteLine();
            return answer;
        }

        private static void CheckAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new ArgumentException("Текст не может иметь пустое значение", nameof(answer));
        }

        private static void ShowException(Exception ex)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex.Message}\n");
            Console.ForegroundColor = oldColor;
        }
    }
}