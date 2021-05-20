using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsResearch_СourseWork
{
    partial class TRPZSolution
    {
        // вектор к-стей програмних продуктів i-ого виду
        public float[] vA;
        // вектор к-стей спеціалістів j-ої категорії
        public float[] vB;
        public int ASize;
        public int BSize;
        // матриця зарплат спецілістів j-ої категорії, що працюють над i-тим програмним продуктом
        public float[,] mC;

        // Компенсаторний цикл
        private Element[] Allowed;// хранит координаты клеток, в которых есть груз
        // Конструкторы
        public TRPZSolution(float[] vProjects, float[] vPrices)
        {
            PrepareData(vProjects, vPrices);
        }

        private void PrepareData(float[] vProjects, float[] vPrices)
        {
            int n = vProjects.Length;
            mC = new float[n + 1, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (j <= i)
                        mC[i, j] = vPrices[j];
                    else
                        mC[i, j] = 100000;

            float projectCount = vProjects.Sum();
            vB = new float[n];
            BSize = n;
            vB[0] = projectCount;
            for (int i = 0; i < n - 1; i++)
            {
                projectCount -= vProjects[i];
                vB[i + 1] = projectCount;
            }

            vA = new float[n + 1];
            ASize = n + 1;
            vProjects.CopyTo(vA, 0);
            vA[n] = vB.Sum() - vProjects.Sum();
        }
        private float CalcCF(float[,] dbr)
        {
            float val = 0;
            for (int i = 0; i <= dbr.GetUpperBound(0) - 1; i++)
                for (int j = 0; j <= dbr.GetUpperBound(1); j++)
                    if (!float.IsNaN(dbr[i, j]))
                        val = val + mC[i, j] * dbr[i, j];
            return val;
        }
        private bool IsAllTrue(bool[] vector) => Array.TrueForAll(vector, delegate (bool x) { return x; });
        private bool IsAllNegative(float[,] matrix)
        {
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                    if (matrix[i, j] > 0) return false;
            return true;
        }

        // обчислювання потенціалів
        private void CalcPotentials(float[] U, float[] V, float[,] mHelp)
        {
            // Булеві вектори U1, V1 - чи знайдений відповідний потениціал
            //                U2, V2 - чи пройдений відповідний рядок/стовпець
            bool[] U1 = new bool[ASize];
            bool[] U2 = new bool[ASize];
            bool[] V1 = new bool[BSize];
            bool[] V2 = new bool[BSize];
            V[BSize - 1] = 0;
            V1[BSize - 1] = true;
            // доки всі елементи векторів V1 и U1 не будуть дорівнювати true
            while (!(IsAllTrue(V1) && IsAllTrue(U1)))
            {
                int i = -1;
                int j = -1;
                for (int j1 = BSize - 1; j1 >= 0; j1--)
                    if (V1[j1] && !V2[j1]) j = j1;
                for (int i1 = ASize - 1; i1 >= 0; i1--)
                    if (U1[i1] && !U2[i1]) i = i1;

                if (j != -1)
                {
                    for (int i1 = 0; i1 < ASize; i1++)
                    {
                        if (!float.IsNaN(mHelp[i1, j]) && !U1[i1])
                        {
                            U[i1] = mHelp[i1, j] - V[j];
                            U1[i1] = true;
                        }
                    }
                    V2[j] = true;
                }

                if (i != -1)
                {
                    for (int j1 = 0; j1 < BSize; j1++)
                    {
                        if (!float.IsNaN(mHelp[i, j1]) && !V1[j1])
                        {
                            V[j1] = mHelp[i, j1] - U[i];
                            V1[j1] = true;
                        }
                    }
                    U2[i] = true;
                }

            }
        }
        // обчислювання відносинх оцінок
        private float[,] CalcRelativeValues(float[,] mDBR, float[] U, float[] V)
        {

            float[,] mRelativeValue = new float[ASize, BSize];
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                {
                    if (float.IsNaN(mDBR[i, j]))
                        mRelativeValue[i, j] = (U[i] + V[j]) - mC[i, j];
                    else
                        mRelativeValue[i, j] = 0;
                }
            return mRelativeValue;
        }

        private Element[] GetLoop(int x, int y)
        {
            Element Beg = new Element(x, y);
            FindLoop fw = new FindLoop(x, y, true, Allowed, Beg, null);
            fw.BuildTree();
            Element[] Way = Array.FindAll<Element>(Allowed, delegate (Element p) { return (p.X != -1) && (p.Y != -1); });
            return Way;
        }
        class FindLoop
        {
            FindLoop Father;
            Element Root;
            FindLoop[] Childrens;
            Element[] mAllowed;
            Element Begining;
            //true - вниз/вгору
            //false - вліво/вправо
            bool flag;
            public FindLoop(int x, int y, bool _flag, Element[] _mAllowed, Element _Beg, FindLoop _Father)
            {
                Begining = _Beg;
                flag = _flag;
                Root = new Element(x, y);
                mAllowed = _mAllowed;
                Father = _Father;
            }
            public bool BuildTree()
            {
                Element[] ps = new Element[mAllowed.Length];
                int Count = 0;
                for (int i = 0; i < mAllowed.Length; i++)
                    if (flag)
                    {
                        if (Root.Y == mAllowed[i].Y)
                        {
                            Count++;
                            ps[Count - 1] = mAllowed[i];
                        }
                    }
                    else
                        if (Root.X == mAllowed[i].X)
                    {
                        Count++;
                        ps[Count - 1] = mAllowed[i];
                    }

                FindLoop fwu = this;
                Childrens = new FindLoop[Count];

                int k = 0;
                for (int i = 0; i < Count; i++)
                {
                    if (ps[i] == Root) continue;
                    if (ps[i] == Begining)
                    {
                        while (fwu != null)
                        {
                            mAllowed[k] = fwu.Root;
                            fwu = fwu.Father;
                            k++;
                        };
                        for (; k < mAllowed.Length; k++) mAllowed[k] = new Element(-1, -1);
                        return true;
                    }

                    if (!Array.TrueForAll<Element>(ps, p => ((p.X == 0) && (p.Y == 0))))
                    {
                        Childrens[i] = new FindLoop(ps[i].X, ps[i].Y, !flag, mAllowed, Begining, this);
                        bool result = Childrens[i].BuildTree();
                        if (result) return true;
                    }
                }
                return false;
            }

        }

        private void CalcNewBasicSolution(float[,] mDBR, float[,] mRelativeValue)
        {
            Element maxInd = new Element();
            float max = float.MinValue;
            int k = 0;
            // координати базисних змінних
            Allowed = new Element[ASize + BSize];
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                {
                    if (!float.IsNaN(mDBR[i, j]))
                    {
                        Allowed[k].X = i;
                        Allowed[k].Y = j;
                        k++;
                    }
                    // шукаємо максимальну додатню оцінку
                    if (mRelativeValue[i, j] > max)
                    {
                        max = mRelativeValue[i, j];
                        maxInd.X = i;
                        maxInd.Y = j;
                    }
                }
            // Шукаємо компенсаторний цикл
            Allowed[Allowed.Length - 1] = maxInd;
            // останній елемент вектору - елемент, що вводимо у базис
            Element[] Cycle = GetLoop(maxInd.X, maxInd.Y);

            float min = float.MaxValue;

            Element temp = new Element();
            // пошук мінімального
            for (int i = 0; i < Cycle.Length; i++)
            {
                if (i % 2 == 0 && mDBR[Cycle[i].X, Cycle[i].Y] < min)
                {
                    min = mDBR[Cycle[i].X, Cycle[i].Y];
                    temp = Cycle[i];
                }
            }
            mDBR[maxInd.X, maxInd.Y] = 0;
            // +/-
            for (int i = 0; i < Cycle.Length; i++)
            {
                if (i % 2 == 0)
                    mDBR[Cycle[i].X, Cycle[i].Y] -= min;
                else
                    mDBR[Cycle[i].X, Cycle[i].Y] += min;
            }
            mDBR[temp.X, temp.Y] = float.NaN;
        }
        public (float[,] mOpt, float result) FindSolution(Method method)
        {
            float[,] mDBR = method switch
            {
                Method.NordWestAngel => NordWestAngle(),
                Method.Fogel => Fogel()
            };
            float[,] mHelp = new float[ASize, BSize];
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                    if (!float.IsNaN(mDBR[i, j]))
                        mHelp[i, j] = mC[i, j];
                    else
                        mHelp[i, j] = float.NaN;

            float[] U = new float[ASize];
            float[] V = new float[BSize];
            CalcPotentials(U, V, mHelp);
            float[,] mRelativeValue = CalcRelativeValues(mDBR, U, V);

            while (!IsAllNegative(mRelativeValue))
            {
                CalcNewBasicSolution(mDBR, mRelativeValue);
                for (int i = 0; i < ASize; i++)
                    for (int j = 0; j < BSize; j++)
                    {
                        if (!float.IsNaN(mDBR[i, j]))
                            mHelp[i, j] = mC[i, j];
                        else
                            mHelp[i, j] = float.NaN;
                    }
                CalcPotentials(U, V, mHelp);
                mRelativeValue = CalcRelativeValues(mHelp, U, V);
            }

            float val = CalcCF(mDBR);
            return (mDBR, val);
        }
        public float[] FindSolutionForExperiment(Method method)
        {
            List<float> vOpt = new List<float>();
            float[,] mDBR = method switch
            {
                Method.NordWestAngel => NordWestAngle(),
                Method.Fogel => Fogel()
            };
            vOpt.Add(CalcCF(mDBR));
            float[,] mHelp = new float[ASize, BSize];
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                    if (!float.IsNaN(mDBR[i, j]))
                        mHelp[i, j] = mC[i, j];
                    else
                        mHelp[i, j] = float.NaN;

            float[] U = new float[ASize];
            float[] V = new float[BSize];
            CalcPotentials(U, V, mHelp);
            float[,] mRelativeValue = CalcRelativeValues(mDBR, U, V);

            while (!IsAllNegative(mRelativeValue))
            {
                CalcNewBasicSolution(mDBR, mRelativeValue);
                vOpt.Add(CalcCF(mDBR));
                for (int i = 0; i < ASize; i++)
                    for (int j = 0; j < BSize; j++)
                    {
                        if (!float.IsNaN(mDBR[i, j]))
                            mHelp[i, j] = mC[i, j];
                        else
                            mHelp[i, j] = float.NaN;
                    }
                CalcPotentials(U, V, mHelp);
                mRelativeValue = CalcRelativeValues(mHelp, U, V);
            }

            float val = CalcCF(mDBR);
            return vOpt.ToArray();
        }

    }
    
    struct Element
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Element(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Element element1, Element element2)
        {
            return element1.X == element2.X && element1.Y == element2.Y;
        }
        public static bool operator !=(Element element1, Element element2)
        {
            return element1.X != element2.X || element1.Y != element2.Y;
        }
    }

   
}
