using Normal2048.Models;
using System.Collections;
using System.Collections.Generic;

namespace Normal2048.Helper
{
    public static class Extension
    {
        public static IEnumerable<T> MatrixToList<T>(this T[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
                for (int column = 0; column < matrix.GetLength(1); column++)
                    yield return matrix[row, column];
        }
    }
}
