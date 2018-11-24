using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Upload_WordScanner
{
    class Program
    {

        private static readonly char[] delimiters = { ' ', '.', ',', ';', '\'', '-', ':', '!', '?', '(', ')', '<', '>', '=', '*', '/', '[', ']', '{', '}', '\\', '"', '\r', '\n' };

        private static readonly Func<string, string> theWord = Word;

        private static readonly Func<IGrouping<string, string>, KeyValuePair<string, int>> theNewWordCount = NewWordCount;
        private static readonly Func<KeyValuePair<string, int>, int> theCount = Count;

        static void Main(string[] args)
        {

            FileDownloander();



            XmlTextWriter write = new XmlTextWriter(@"C:\ParkNet\deneme.xml", System.Text.UTF8Encoding.UTF8);
            //Daha önce bu isimle oluşturulan bir XML dosyası var ise, eski dosya silinir.
            write.Formatting = Formatting.Indented;
            // Dosya yapısını hiyerarşik olarak oluşturarak okunabilirliği arttırır.


            try
            {

                write.WriteStartDocument(); //Xml dökümanına ait declaration satırını oluşturur. Kısaca yazmaya başlar.
                write.WriteStartElement("words");

                foreach (var wordCount in File.ReadAllText(args.Length > 0 ? args[0] : @"C:\ParkNet\ParkNet.txt")
               .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
               .AsParallel()
               .GroupBy(theWord, StringComparer.OrdinalIgnoreCase)
               .Select(theNewWordCount)
               .OrderByDescending(theCount))
                {
                    Console.WriteLine(
                        "Word: \""
                        + wordCount.Key
                        + "\" Count: "
                        + wordCount.Value);

                    write.WriteStartElement("word");

                    write.WriteAttributeString("text", wordCount.Key);
                    write.WriteAttributeString("Count", wordCount.Value.ToString());
                    write.WriteEndElement();
                }


                write.Close();
           

            }

            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();

        }


        private static void FileDownloander()
        {
            string path = @"C:\ParkNet";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Console.WriteLine("File already exists");

            }



            WebClient client = new WebClient();


            client.DownloadFile("http://www.gutenberg.org/files/2701/2701-0.txt", @"C:\ParkNet\ParkNet.txt");



        }
        private static string Word(string word)
        {
            return word;
        }

        private static KeyValuePair<string, int> NewWordCount(IGrouping<string, string> wordCount)
        {
            return new KeyValuePair<string, int>(wordCount.Key, wordCount.Count());
        }

        private static int Count(KeyValuePair<string, int> wordCount)
        {
            return wordCount.Value;
        }


    }


}









