using NTextCat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpilnaSpravaTask2
{
    class Program
    {
        //Указываем путь папки с файлами
        public static string setting = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\Files\";

        //Указываем путь для Core14.profile.xml
        public static string identifierFactory = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LanguageModels\Core14.profile.xml";

        static void Main(string[] args)
        {

            string[] files = Directory.GetFiles(setting);

            foreach (string s in files)
            {

                //   Console.WriteLine(lines);
            }

            string lines = System.IO.File.ReadAllText(@"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\Files\OnlyRussian.txt");

            IdentitySentense(lines);


            Console.ReadKey();
        }

        public static void IdentitySentense(string lines)
        {
            int StartPoint = 0;
            int EndPoint = 0;
            int val = 0;

            foreach (var b in lines)
            {
                val++;
                if (b == '!' || b == '.' || b == ',' || b == '?')
                {
                    EndPoint = val;

                    string sentense = "";

                    for (int i = StartPoint; i < EndPoint; i++)
                    {
                        sentense += lines[i];

                    }

                    string language = IdentityLanguage(sentense);

                    if (language == "System error: The language couldn’t be identified with an acceptable degree of certainty")
                    {

                    }

                    Console.WriteLine(language);

                    StartPoint = val;
                }
            }
        }
   
        public static string IdentityLanguage(string sentence)
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load(@"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LanguageModels\Core14.profile.xml");

            var languages = identifier.Identify(sentence);

            var mostCertainLanguage = languages.FirstOrDefault();
            if (mostCertainLanguage != null)
            {
                string resultLanguage = mostCertainLanguage.Item1.Iso639_3;

                return resultLanguage;
            }
           
            return "System error: The language couldn’t be identified with an acceptable degree of certainty";
        }
    }
}
