using System;
using System.Text.RegularExpressions;

namespace MoogleEngine;

public static class Moogle
{
    public static string[] topWord = new string[MoogleTFIDF.dictionary.reader.textos.Length];

    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda
        
        string[] palabras = Utilities.Cut(query);
        List<(SearchItem,int)> list = new List<(SearchItem,int)>();
        Dictionary<string, float> vectorQuery = ContarVector(palabras);
        float squaresSum = 0;

        for (int i = 0; i < vectorQuery.Count; i++)
        {
            KeyValuePair<string, float> entry = vectorQuery.ElementAt(i);
            vectorQuery[entry.Key] = MoogleTFIDF.TF(vectorQuery[entry.Key], palabras.Length);
            squaresSum += vectorQuery[entry.Key] * vectorQuery[entry.Key];
        }

        for (int i = 0; i < Reader.archivos.Length; i++)
        {
            SearchItem a = new SearchItem(Reader.archivos[i].Substring(Reader.path.Length + 1, Reader.archivos[i].Length - Reader.path.Length-5), " ", SimilitudCoseno(vectorQuery, i, squaresSum));
            for (int j = 0; j < 5; j++)
            {
                if (list.Count < 5)
                {
                    if (a.Score > 0)
                    {
                        list.Insert(j, (a,i));
                        break;
                    }
                }
                else if (list[j].Item1.Score < a.Score)
                {
                    list.Insert(j, (a,i));
                    break;
                }
            }
        }

        SearchItem[] items = new SearchItem[Math.Min(5, list.Count)];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SearchItem(list[i].Item1.Title, Snippet(topWord[list[i].Item2], list[i].Item2,vectorQuery),list[i].Item1.Score);
        }

        string suggestion = Suggest(palabras);

        if (items.Length == 0)
        {
            SearchItem a = new SearchItem("No se han encontrado resultados para su búsqueda", " ", 0.11f);
            SearchItem[] items1 = new SearchItem[1];
            for (int i = 0; i < items1.Length; i++)
            {
                items1[0] = a;
            }
            return new SearchResult(items1, suggestion);
        }

        return new SearchResult(items, suggestion);
    }
    


    public static Dictionary<string, float> ContarVector(string[] palabras)
    {

        Dictionary<string, float> dic = new Dictionary<string, float>();
        for (int i = 0; i < palabras.Length; i++)
        {
            if (!dic.ContainsKey(palabras[i]))
            {
                dic.Add(palabras[i], 1);
            }
            else
            {
                dic[palabras[i]]++;
            }
        }
        return dic;
    }

    public static float SimilitudCoseno(Dictionary<string, float> vector, int indice, float sumaRaices)
    {
        float maxi = 0;
        float productoEscalar = 0;
        float textoRaices = 0;
        sumaRaices = (float)Math.Sqrt(sumaRaices);
        foreach (var element in vector)
        {
            if (MoogleTFIDF.dictionary.LocalDictionary[indice].ContainsKey(element.Key))
            {
                productoEscalar += (element.Value * MoogleTFIDF.dictionary.LocalDictionary[indice][element.Key]);

                if(maxi < MoogleTFIDF.dictionary.LocalDictionary[indice][element.Key])
                {
                    maxi = MoogleTFIDF.dictionary.LocalDictionary[indice][element.Key];
                    topWord[indice] = element.Key;
                }
            }
        }
        foreach (var element in MoogleTFIDF.dictionary.LocalDictionary[indice])
        {
            textoRaices += MoogleTFIDF.dictionary.LocalDictionary[indice][element.Key] * MoogleTFIDF.dictionary.LocalDictionary[indice][element.Key];
        }
        textoRaices = (float)Math.Sqrt(textoRaices);


        return productoEscalar / (sumaRaices * textoRaices);
    }

    /*
    public static string Snippet(string word , int indice, Dictionary<string,float> vector)
    {
        string snippy = "can";
        string texto = Regex.Replace(MoogleTFIDF.dictionary.reader.realTexts[indice] , @"[^\sa-zA-Z0-9áéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙ]", " ");
        int textSize = texto.Length;
        int position = 0;
        bool basaur = false;
        int maxAmount = 0;
        position = texto.IndexOf(word);
        while(position != -1)
        {
            int left = Math.Max(0,position-70);
            int right = Math.Min(textSize, position + 70);

            while (left != 0 && (textSize-left < 140 && (texto[left-1] != ' ' && !char.IsLetterOrDigit(texto[left]))))
            {
                left--;
            }

            while (right != textSize-1 && ( right < 140 && (texto[right + 1] != ' ' && !char.IsLetterOrDigit(texto[right]))))
            {
                right++;
            }
            string snippet = texto.Substring(left,right - left);
            string[] words = snippet.Split(" ",StringSplitOptions.RemoveEmptyEntries);
            int amount = 0;
            for( int i = 0; i < words.Length; i++)
            {
                if (vector.ContainsKey(words[i]))
                {
                    amount++;
                }
            }
            if(amount > maxAmount)
            {
                maxAmount = amount;
                snippy = MoogleTFIDF.dictionary.reader.realTexts[indice].Substring(left, right - left);
            }

            texto = texto.Substring(0,position-1) + '*' + texto.Substring(position+1, texto.Length - position - 10);
            position = texto.IndexOf(word);
        }
        return snippy;
    }
    */

    public static string Snippet(string word, int indice, Dictionary<string, float> vector)
    {
        string texto = Regex.Replace(MoogleTFIDF.dictionary.reader.realTexts[indice], @"[^a-zA-Z0-9áéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙ]", " ");
        texto = texto.ToLower();
        int textSize = texto.Length;
        int position = 0;
        double itle = 0;
        bool basaur = false;
        char mander = '*';
        itle = Math.Sqrt(itle);

        if(texto.IndexOf(word + ' ') == 0)
        {
            position = texto.IndexOf(word);
        }
        else if(texto.IndexOf(' ' + word + ' ') != -1)
        {
            position = texto.IndexOf(' ' + word + ' ');
        }
        else 
        {
            position = textSize - 10;
        }
        
        int left = Math.Max(0, position - 70);
        int right = Math.Min(textSize-1, position + 70);

        while (left != 0 && (textSize - left < 140 || (texto[left - 1] != ' ' || !char.IsLetterOrDigit(texto[left]))))
        {
            left--;
        }

        while (right != textSize - 1 && (right < 140 || (texto[right + 1] != ' ' || !char.IsLetterOrDigit(texto[right]))))
        {
            right++;
        }

        string snippy = MoogleTFIDF.dictionary.reader.realTexts[indice].Substring(left, right - left + 1);
        
        return snippy;
    }

    public static string Suggest( string[] vector)
    {
        string suggy = "";
        string actualWord = "";
        bool basaur = false;

        for( int i = 0; i < vector.Length; i++)
        {
            int cost = int.MaxValue;
            if (MoogleTFIDF.dictionary.MasterDictionary.ContainsKey(vector[i]))
            {
                if (i == vector.Length - 1) suggy += vector[i];
                else suggy += vector[i] + ' ';
            }
            else
            {
                basaur = true;
                foreach (string word in MoogleTFIDF.dictionary.MasterDictionary.Keys)
                {
                    int temporaryCost = Utilities.LevenshteinDistance(vector[i], word);
                    if (temporaryCost < cost)
                    {
                        cost = temporaryCost;
                        actualWord = word;
                    }

                }
                if (i == vector.Length - 1) suggy += actualWord;
                else suggy += actualWord + ' ';
            }
        }
        
        return basaur ? suggy : "";
    }

}
