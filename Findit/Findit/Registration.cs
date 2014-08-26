/*
 * this class has the support methods for:
 * generating keys (NewKey)
 * validating keys (ValidKey)
 * tell whether the product is registered (PaidFor)
 * tell how much longer is left in the trial period (DaysLeftInTrialPeriod)
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Findit
{
    class Registration
    {
        private const string c_Registered = "RegisteredKey";
        private const string c_DateOfFirstUse = "DateOfFirstUse";
        private const int c_TrialDays = 30;  //how many days in the trial period?

        public int DaysLeftInTrialPeriod()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SearchParameters.c_RegKeyName, true);
            string strDate = reg.GetValue(c_DateOfFirstUse, "").ToString();
            if (0 == strDate.Length)
            {
                string defaultDate = DateTime.Now.ToShortDateString();
                reg.SetValue(c_DateOfFirstUse, defaultDate);
                strDate = defaultDate;
            }
            DateTime dteDate = Convert.ToDateTime(strDate);
            TimeSpan ts  = DateTime.Now - dteDate;
            return c_TrialDays - (int)Math.Round(ts.TotalDays, 0);
        }

        public Boolean WithinTrialPeriod()
        {
            return 0 < DaysLeftInTrialPeriod();
        }

        public Boolean PaidFor()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SearchParameters.c_RegKeyName, true);
            return ValidKey(reg.GetValue(c_Registered, "").ToString());
        }

        public Boolean ValidKey(string key)
        {
            //see NewKey() for the valid key recipe
            if (key.Length == 64)
            {
                //extract the guid part and the text part separately
                string ExtractedGuid = "";
                string ExtractedWords = "";
                for (int i = 0; i < key.Length; ++i)
                {
                    if (i % 2 == 0)
                    {
                        ExtractedGuid += key[i];
                    }
                    else
                    {
                        ExtractedWords += key[i];
                    }
                }

                if (Util.IsValidGuid(ExtractedGuid))
                {
                    string word1 = Util.ReverseString(ExtractedWords.Substring(0, 8));
                    string word2 = Util.ReverseString(ExtractedWords.Substring(8, 8));
                    string word3 = Util.ReverseString(ExtractedWords.Substring(16, 8));
                    string word4 = Util.ReverseString(ExtractedWords.Substring(24, 8));

                    //if all four words exist in our list of keyable words, then we are good
                    return Util.IsStringInStrArry(word1, ref KeyableWords)
                        && Util.IsStringInStrArry(word2, ref KeyableWords)
                        && Util.IsStringInStrArry(word3, ref KeyableWords)
                        && Util.IsStringInStrArry(word4, ref KeyableWords);
                }
                else
                {
                    //not a valid guid?  tsk, tsk
                    return false;
                }
            }
            else
            {
                //wrong # of characters?  that's an easy one.
                return false;
            }
        }

        //here are 95 8-letter words
        private const int c_WordCount = 95;
        private string[] KeyableWords = new string[c_WordCount] {
            "sentence","together","children","mountain","chipmunk",
            "crashing","drinking","insisted","insulted","invented",
            "squinted","standing","swishing","talented","whiplash",
            "complain","granddad","sprinkle","surprise","umbrella",
            "anything","anywhere","baseball","birthday","bluebird",
            "cheerful","colorful","daylight","doghouse","driveway",
            "everyone","faithful","flagpole","graceful","grateful",
            "grown-up","homemade","homework","housefly","kickball",
            "kingfish","knockout","knothole","lipstick","lunchbox",
            "newscast","nickname","peaceful","sailboat","Saturday",
            "shameful","sidewalk","snowball","splendid","suitcase",
            "sunblock","sunshine","swimming","thankful","thinnest",
            "Thursday","whatever","whenever","windmill","American",
            "possible","suddenly","good-bye","airplane","alphabet",
            "bathroom","favorite","medicine","December","dinosaur",
            "elephant","February","football","forehead","headache",
            "hospital","lollipop","November","outdoors","question",
            "railroad","remember","sandwich","scissors","shoulder",
            "softball","tomorrow","upstairs","vacation","restroom"
        };

        public string NewKey(){
            /*Recipe for a valid key:
             *Ingredients:
             *1 - A Guid with all punctuation (hyphens, braces) stripped away, and all letters capitalized
             *ex: 45C1A33D670D4335837C52BF9A54A333
             *
             *2 - Add four words, randomly chosen from the list of 8-character words in the KeyableWords array
             *ex: alphabet
             * +  veracity
             * +  mongoose
             * +  terrific
             *
             *Reverse the words and interpose them with every other character of the Guid.
             *tebahplayticarevesoognomcifirret
             *gets interposed with the guid, leaving us with:
             *4t5eCb1aAh3p3lDa6y7t0iDc4a3r3e5v8e3s7oCo5g2nBoFm9cAi5f4iAr3r3e3t
             */
            string guid = Guid.NewGuid().ToString().Replace("{","").Replace("}","").Replace("-","").ToUpper();
            Random rnd = new Random();
            string word1 = KeyableWords[rnd.Next(0, c_WordCount - 1)];
            string word2 = KeyableWords[rnd.Next(0, c_WordCount - 1)];
            string word3 = KeyableWords[rnd.Next(0, c_WordCount - 1)];
            string word4 = KeyableWords[rnd.Next(0, c_WordCount - 1)];
            word1 = Util.ReverseString(word1);
            word2 = Util.ReverseString(word2);
            word3 = Util.ReverseString(word3);
            word4 = Util.ReverseString(word4);
            string combinedword = word1 + word2 + word3 + word4;

            string result = "";
            for (int i = 0; i < guid.Length; ++i)
            {
                result = result + guid[i] + combinedword[i];
            }

            return result;
        }

        public void StoreValidation(string key)
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SearchParameters.c_RegKeyName, true);
            reg.SetValue(c_Registered, key);
        }
    }
}