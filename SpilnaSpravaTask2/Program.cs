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

        //Path to logFile
        public static string logFile = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LogFile.txt";

        //Path to Core14.profile.xml
        public static string identifierFactory = @"C:\Users\Andrew Romanuk\source\Projects\SpilnaSpravaTask2\SpilnaSpravaTask2\LanguageModels\Core14.profile.xml";


        static void Main(string[] args)
        {

            string[] files = Directory.GetFiles(setting);


            foreach (string s in files)
            {
              
                Dictionary<string, int> dictionary = new Dictionary<string, int>();

                string lines = System.IO.File.ReadAllText(s);

                var text= DeleteAllDashes(lines);

                var emailTuple = CountEmails(text); //Tuple with output email and sentense

                var countNumberTuple = CountNumbers(emailTuple.Item2);

                IdentitySentense(countNumberTuple.Item2, dictionary);

                DeleteDanZero(dictionary);

                using (StreamWriter sw = new StreamWriter(logFile, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("");

                    sw.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

                    sw.WriteLine("");

                    sw.WriteLine("For: " + s);

                    foreach (var b in dictionary)
                    {
                        sw.WriteLine(b.Key + ":" + b.Value);
                    }

                    sw.WriteLine("~");

                    sw.WriteLine("Count emails: " + emailTuple.Item1);

                    sw.WriteLine("Count numbers:" + countNumberTuple.Item1);

                    sw.WriteLine("~");

                    sw.WriteLine("");

                    sw.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

                }
            }

        }

        public static void CountWords(string sentense, string language, Dictionary<string,int> dictionary)
        {

            if (language == "jpn" || language == "kor" || language == "zho")
            {
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


        //This is function for deleting one unnecessary language that creates through the bug. Костыль. 
        public static void DeleteDanZero(Dictionary<string, int> dictionary)
        {
            foreach (var b in dictionary)
            {
                if (b.Key == "dan" && b.Value == 0)
                {
                    dictionary.Remove(b.Key);
                    break;
                }
            }
        }

        public static string DeleteAllDashes(string text)
        {
            object[] array = new object[text.Length];

            //Convert string to object[]
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = text[i];
            }

            foreach (var b in text)
            {
                if(b == '-')
                {
                    int StartIndex = text.IndexOf('-');
                    int EndIndex = StartIndex + 1;

                    //Truncate needed element
                    for (var i = StartIndex; i < EndIndex; i++)
                    {
                        array = array.Where(w => w != array[StartIndex]).ToArray();
                    }

                }
                if(b == '—')
                {
                    int StartIndex = text.IndexOf('—');
                    int EndIndex = StartIndex + 1;

                    //Truncate needed element
                    for (var i = StartIndex; i < EndIndex; i++)
                    {
                        array = array.Where(w => w != array[StartIndex]).ToArray();
                    }
                }
            }

            string resultValue = "";

            //Convert object[] to string
            for (var i = 0; i < array.Length; i++)
            {
                resultValue += array[i];
            }

            return resultValue;

        }


    }
}
