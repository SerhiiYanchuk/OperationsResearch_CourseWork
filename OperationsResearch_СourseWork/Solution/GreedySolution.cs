using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsResearch_СourseWork
{
    public static class GreedySolution
    {
        
        public static (float[,] mOpt, float result) DoGreedy(int[] b, float[] c)
        {
            if (b == null || c == null)
                throw new ArgumentNullException(nameof(c), "Argument equal to null");
            int min = 0;
            int n = b.Length;
            var x = new float[n, n];
            x[0, 0] = 1;
            float result = b[0] * c[0];

            for (int i = 1; i < n; i++)
            {
                if (c[i] < c[min])
                    min = i;
                x[i, min] = 1;
                result += b[i] * c[min];
            }

            return (x, result);
        }
    }
}
