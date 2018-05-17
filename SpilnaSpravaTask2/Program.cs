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
        static void Main(string[] args)
        {
            //Указываем путь папки с файлами
            string setting = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\Files\";

            string[] files = Directory.GetFiles(setting);
            foreach (string s in files)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }

        public static void IdentityLanguage()
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load(@"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LanguageModels\Core14.profile.xml");

            var languages = identifier.Identify("Larin es el mejor");

            var mostCertainLanguage = languages.FirstOrDefault();
            if (mostCertainLanguage != null)
            {
                string LarMan = mostCertainLanguage.Item1.Iso639_3;

                Console.WriteLine(LarMan);
            }
            else
            {
                Console.WriteLine("The language couldn’t be identified with an acceptable degree of certainty");

            }
        }
    }
}
