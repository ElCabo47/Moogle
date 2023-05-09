using System;
using System.IO;

namespace MoogleEngine
{
    public class Reader
    {

        public static string path = Directory.GetCurrentDirectory() + "..\\..\\Content";
        public static string[] archivos = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);
        static int cantArchivos = archivos.Length;
        public string[] textos { get; private set; } = new string[cantArchivos];
        public string[] realTexts { get; private set; } = new string[cantArchivos];

        public Reader()
        {


            for (int i = 0; i < cantArchivos; i++)
            {
                realTexts[i] = File.ReadAllText(archivos[i]);
                textos[i] = realTexts[i].Replace("\r\n", " ");
            }

        }



    }
}
