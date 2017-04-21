using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tandem.Elements.Utilities
{
    internal static class ValueArray
    {
        internal static TOut[][] ToJaggedArray<TOut, TIn>(this TIn[,] array2D)
        {
            int rowsFirstIndex = array2D.GetLowerBound(0);
            int rowsLastIndex = array2D.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = array2D.GetLowerBound(1);
            int columnsLastIndex = array2D.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            TOut[][] arrayJagged = new TOut[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                arrayJagged[i] = new TOut[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    arrayJagged[i][j] = (TOut)Convert.ChangeType(array2D[i, j], typeof(TOut));
                }
            }
            return arrayJagged;
        }

        internal static bool IsSingle<T>(this T[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);
            return (rows == 1 && cols == 1);
        }
    }
}
