using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class Utilities
    {
        public static string[] Cut(string s)
        {
            s = s.ToLower();
            s = Regex.Replace(s, @"[^\sa-zA-Z0-9áéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]", " ");
            string[] textico = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return textico;
        }

        public static int LevenshteinDistance( string word1 , string word2)
        {
            int cost = 0;
            int a = word1.Length;
            int b = word2.Length;
            int[,] m = new int[a+1,b+1];

            for (int i = 1; i <= a; m[i,0] = i++) ;
            for (int i = 1; i <= b; m[0, i] = i++) ;

            for( int i = 1; i <= a; i++)
            {
                for( int j = 1; j <= b; j++)
                {
                    cost = (word1[i-1] == word2[j-1]) ? 0 : 1;
                    m[ i , j ] = Math.Min( Math.Min( m[ i - 1 , j ] + 1 , m[ i , j - 1 ] + 1) , m[ i - 1 , j - 1 ] + cost );
                }
            }

            return m[a, b];
        }


    }



}
