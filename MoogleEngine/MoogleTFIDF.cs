using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public class MoogleTFIDF
    {
        public static MoogleDictionary dictionary;

        public static float Calculartfidf(float ocurrencies, float totalWords,
                                   float textsAmount, float ocursPerTexts)
        {
            float idf = (float)Math.Log10((1 + textsAmount) / (1 + ocursPerTexts) + 0.001);
            return TF(ocurrencies, totalWords) * idf;
        }

        public static float TF(float ocurrencies, float totalWords)
        {
            return ocurrencies / totalWords;

        }





        public static void GetValues()
        {
            dictionary = new MoogleDictionary();

            Dictionary<string, float>.KeyCollection keys = dictionary.MasterDictionary.Keys;
            foreach (string s in keys)
            {
                for (int j = 0; j < dictionary.cantPal.Length; j++)
                {
                    if (dictionary.LocalDictionary[j].ContainsKey(s))
                    {
                        dictionary.LocalDictionary[j][s] = Calculartfidf(dictionary.LocalDictionary[j][s], dictionary.cantPal[j],
                                                               dictionary.cantPal.Length, dictionary.MasterDictionary[s]);
                    }
                }
            }
        }
    }
}
