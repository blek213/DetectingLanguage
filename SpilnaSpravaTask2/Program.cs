using NTextCat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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



            int countemail = CountEmails(lines);
            int countnumbers = CountNumbers(lines);


            Console.ReadKey();
        }

        public static int CountWords(string sentense)
        {
            int words = 0;

            for(int i = 0; i < sentense.Length; i++)
            {

            }

            var text = sentense;

            int wordCount = 0, index = 0;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;

            }

            Console.WriteLine(wordCount);

            //string[] textMass;

            //textMass = sentense.Split(' ');
            //Console.WriteLine("Количество слов:");
            //Console.WriteLine(textMass.Length);



            return words;
        }

        public static int CountEmails(string sentense)
        {
            int EmailCount = 0;

            const string MatchEmailPattern =
    @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(sentense);

            EmailCount = matches.Count;

            return EmailCount;
        }

        public static int CountNumbers(string sentense)
        {
            int NumbersCount = 0;

            return NumbersCount;
        }
        public static void IdentitySentense(string lines)
        {

            int StartPoint = 0;
            int EndPoint = 0;
            int valInteger = 0;

            foreach (var b in lines)
            {
                valInteger++;
                if (b == '!' || b == '.' || b == ',' || b == '?')
                {
                    EndPoint = valInteger;

                    string sentense = "";

                    for (int i = StartPoint; i < EndPoint; i++)
                    {
                        sentense += lines[i];

                    }

                    string language = IdentityLanguage(sentense);

                    if (language == "System error: The language couldn’t be identified with an acceptable degree of certainty")
                    {

                    }

                    //Console.WriteLine(language);

                    //CountWords(sentense);
                    CountEmails(sentense);


                    StartPoint = valInteger;
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
