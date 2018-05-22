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
        //Path to folder with files
        public static string setting = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\Files\";

        //Path to Core14.profile.xml
        public static string identifierFactory = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LanguageModels\Core14.profile.xml";

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(setting);

            foreach (string s in files)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();

                string lines = System.IO.File.ReadAllText(s);

                var emailTuple = CountEmails(lines); //Tuple with output email and sentense

                var countNumberTuple = CountNumbers(emailTuple.Item2);

                IdentitySentense(countNumberTuple.Item2, dictionary);

                //Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

                //Console.WriteLine("Count emails: " + emailTuple.Item1);

                //Console.WriteLine("Count numbers:" + countNumberTuple.Item1);

                //Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

                foreach(var b in dictionary)
                {
                    Console.WriteLine("Language: " + b.Key + " Value: " + b.Value);
                }

            }


            Console.ReadKey();
        }

        public static int CountWords(string sentense, string language, Dictionary<string,int> dictionary)
        {

            if (language == "jpn" || language == "kor" || language == "zho")
            {
                return 1;
            }

            int WordCount = 0;

            const string MatchWordPattern = @"([^\s]+)";

            Regex rx = new Regex(MatchWordPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(sentense);

            WordCount = matches.Count;

            //Check if dictionary contains language that exists 

            bool IsLanguageExist = false;

            foreach(var b in dictionary)
            {
                if(b.Key == language)
                {
                    int OldVal = b.Value;
                    dictionary[b.Key] = OldVal + matches.Count;
                    IsLanguageExist = true;
                    break;
                }
            }

            //add count item to dictionary 
            if(IsLanguageExist == false)
            {
                dictionary.Add(language, matches.Count);

            }



            //Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            //Console.WriteLine("The language: " + language);

            //Console.WriteLine("Count words:" + WordCount);

            //Console.WriteLine("The total word count is {0}", counter);


            return 1;
        }

        public static Tuple<int,string> CountEmails(string sentense)
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

            foreach (Match match in matches)
            {
                String value = sentense;

                string val = match.Value;

                int valLength = val.Length;

                int StartIndex = value.IndexOf(val); //10
                int EndIndex = StartIndex + valLength; //15

                object[] array = new object[value.Length];

                //Convert string to object[]
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = value[i];
                }

                //Truncate needed element
                for (var i = StartIndex; i < EndIndex; i++)
                {
                    array = array.Where(w => w != array[StartIndex]).ToArray();
                }

                string resultValue = "";

                //Convert object[] to string
                for (var i = 0; i < array.Length; i++)
                {
                    resultValue += array[i];
                }

                sentense = resultValue;

            }

            return Tuple.Create<int, string>(EmailCount,sentense);
        }

        public static Tuple<int, string> CountNumbers(string sentense)
        {
            int NumberCount = 0;

            const string MatchNumberPattern = @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)";

            Regex rx = new Regex(MatchNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(sentense);

            NumberCount = matches.Count;

            foreach (Match match in matches)
            {
                String value = sentense;

                string val = match.Value;

                int valLength = val.Length;

                int StartIndex = value.IndexOf(val);
                int EndIndex = StartIndex + valLength;

                object[] array = new object[value.Length];

                //Convert string to object[]
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = value[i];
                }

                //Truncate needed element
                for (var i = StartIndex; i < EndIndex; i++)
                {
                    array = array.Where(w => w != array[StartIndex]).ToArray();
                }

                string resultValue = "";

                //Convert object[] to string
                for (var i = 0; i < array.Length; i++)
                {
                    resultValue += array[i];
                }

                sentense = resultValue;

            }

            return Tuple.Create<int, string>(NumberCount, sentense);
        }
        public static void IdentitySentense(string lines, Dictionary<string,int> dictionary )
        {
            int StartPoint = 0;
            int EndPoint = 0;
            int valInteger = 0;
            var checkVal = 0;

            lines += Environment.NewLine;

            foreach (var b in lines)
            {
                valInteger++;
                if ((b == '!' && lines[checkVal] == ' ')  || ( b == '.' && lines[checkVal] == ' ') || (b == ',' && lines[checkVal] == ' ') || ( b == '?' && lines[checkVal] == ' ') || ( b == '(' && lines[checkVal] == ' ' ) || ( b == ')' && lines[checkVal] == ' ' ) || lines[checkVal] == '\r')
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
                        Console.WriteLine("System error: The language couldn’t be identified with an acceptable degree of certainty");
                    }

                    CountWords(sentense, language, dictionary);

                    StartPoint = valInteger;

                }
                checkVal++;
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
