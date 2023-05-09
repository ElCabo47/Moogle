using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public class MoogleDictionary
    {
        public Reader reader;
        public Dictionary<string, float>[] LocalDictionary { private set; get; }
        public Dictionary<string, float> MasterDictionary { private set; get; }
        public int[] cantPal;

        public MoogleDictionary()
        {
            reader = new Reader();
            MasterDictionary = new Dictionary<string, float>();
            LocalDictionary = new Dictionary<string, float>[reader.textos.Length];
            cantPal = new int[reader.textos.Length];

            for (int i = 0; i < reader.textos.Length; i++)
            {
                LocalDictionary[i] = new Dictionary<string, float>();
                string[] text = Utilities.Cut(reader.textos[i]);
                cantPal[i] = text.Length;
                for (int j = 0; j < text.Length; j++)
                {
                    if (!LocalDictionary[i].ContainsKey(text[j]))
                    {
                        LocalDictionary[i].Add(text[j], 1);

                        if (!MasterDictionary.ContainsKey(text[j]))
                        {
                            MasterDictionary.Add(text[j], 1);
                        }
                        else
                        {
                            MasterDictionary[text[j]]++;
                        }

                    }
                    else
                    {
                        LocalDictionary[i][text[j]]++;
                    }
                }


            }
        }
    }
}

