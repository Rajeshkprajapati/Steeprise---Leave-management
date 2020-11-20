using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Helpers
{
    public class RandomGenerator
    {
        public static string GetRandom(int size = 10)
        {
            string possibles =
                Constants.PossiblitiesToRandomGenerator;
            char[] primaryString = new char[size];
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                primaryString[i] =
                    possibles[rd.Next(0, possibles.Length)];
            }
            return new string(primaryString);
        }
    }
}
