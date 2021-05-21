using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsResearch_СourseWork
{
    partial class TRPZSolution
    {
        public enum Method: byte
        {
            NordWestAngel = 0,
            Fogel
        }
        private void EmptyToNaN(float[,] matrix)
        {
            for (int i = 0; i < ASize; i++)
                for (int j = 0; j < BSize; j++)
                    if (matrix[i, j] == 0) matrix[i, j] = float.NaN;
        }
        private bool IsEmpty(int[] vector) => Array.TrueForAll(vector, delegate (int x) { return x == 0; });
        // Метод пн-зх кута
        public float[,] NordWestAngle()
        {
            int[] Ahelp = vA;
            int[] Bhelp = vB;
            int i = 0, j = 0;
            float[,] mDBR = new float[ASize, BSize];
            EmptyToNaN(mDBR);

            while (!(IsEmpty(Ahelp) && IsEmpty(Bhelp)))
            {
                int Dif = Math.Min(Ahelp[i], Bhelp[j]);
                mDBR[i, j] = Dif;
                Ahelp[i] -= Dif; Bhelp[j] -= Dif;
                if ((Ahelp[i] == 0) && (Bhelp[j] == 0) && (j + 1 < BSize)) mDBR[i + 1, j] = 0;
                if (Ahelp[i] == 0) i++;
                if (Bhelp[j] == 0) j++;
            }
            return mDBR;
        }

        private bool IsOneRowOneColumn(bool[] v1, bool[] v2)
        {
            int q1 = v1.Sum(p => p ? 0 : 1);
            int q2 = v2.Sum(p => p ? 0 : 1);
            if (q1 == 1 && q2 == 1)
                return true;
            return false;
        }
        private bool IsOneNotZero(bool[] v1, bool[] v2, int[] d1, int[] d2)
        {
            int q1 = v1.Sum(p => p ? 0 : 1);
            bool b1 = false;
            if (q1 == 1)
            {
                for (int i = 0; i < v1.Length; i++)
                    if (!v1[i])
                    {
                        b1 = d1[i] == 0 ? false : true;
                        break;
                    }
            }

            int q2 = v2.Sum(p => p ? 0 : 1);
            bool b2 = false;
            if (q2 == 1)
            {
                for (int i = 0; i < v2.Length; i++)
                    if (!v2[i])
                    {
                        b2 = d2[i] == 0 ? false : true;
                        break;
                    }
            }

            if ((b1 && !b2) || (!b1 && b2))
                return true;
            return false;
        }
        private bool IsRowsColumsZero(int[] d1, int[] d2)
        {
            bool b1 = Array.TrueForAll(d1, delegate (int x) { return x == 0; });
            bool b2 = Array.TrueForAll(d2, delegate (int x) { return x == 0; });
            if (b1 && b2)
                return true;
            return false;
        }
        // Метод Фогеля
        public float[,] Fogel()
        {
            // IsOneRowOneColumn
            // IsOneNotZero
            // IsRowsColumsZero
            // Булеві вектори r, v - чи знайдений відповідний полтениціа
            bool[] r = new bool[ASize];
            bool[] c = new bool[BSize];
            FogelElement[] rE = new FogelElement[ASize];
            FogelElement[] cE = new FogelElement[BSize];

            int[] Ahelp = vA;
            int[] Bhelp = vB;

            float[,] mDBR = new float[ASize, BSize];
            EmptyToNaN(mDBR);
            int quantity = 0;
            while (!(IsOneRowOneColumn(r, c) || IsOneNotZero(r, c, Ahelp, Bhelp) || IsRowsColumsZero(Ahelp, Bhelp)))
            {
                for (int i = 0; i < ASize; i++)
                {
                    if (Ahelp[i] == 0)
                        continue;
                    float min = float.MaxValue;
                    for (int j = 0; j < BSize; j++)
                    {
                        // но не точно
                        if (Bhelp[j] == 0)
                            continue;
                        if (mC[i, j] < min)
                        {
                            rE[i] = new FogelElement(j, i);
                            min = mC[i, j];
                        }
                    }
                    min = float.MaxValue;
                    for (int j = 0; j < BSize; j++)
                    {
                        // но не точно
                        if (Bhelp[j] == 0)
                            continue;
                        if (mC[i, j] < min && j != rE[i].X)
                        {
                            min = mC[i, j];
                            rE[i].Penalty = min - mC[rE[i].Y, rE[i].X];
                        }
                    }
                }
                for (int j = 0; j < BSize; j++)
                {
                    if (Bhelp[j] == 0)
                        continue;
                    float min = float.MaxValue;
                    for (int i = 0; i < ASize; i++)
                    {
                        // но не точно
                        if (Ahelp[i] == 0)
                            continue;
                        if (mC[i, j] < min)
                        {
                            cE[j] = new FogelElement(j, i);
                            min = mC[i, j];
                        }
                    }
                    min = float.MaxValue;
                    for (int i = 0; i < ASize; i++)
                    {
                        // но не точно
                        if (Ahelp[i] == 0)
                            continue;
                        if (mC[i, j] < min && i != cE[j].Y)
                        {
                            min = mC[i, j];
                            cE[j].Penalty = min - mC[cE[j].Y, cE[j].X];
                        }
                    }
                }

                FogelElement maxPenaltyElem = new FogelElement();
                maxPenaltyElem.Penalty = float.MinValue;
                for (int i = 0; i < ASize; i++)
                {
                    if (r[i] || Ahelp[i] == 0)
                        continue;
                    if (rE[i].Penalty > maxPenaltyElem.Penalty)
                        maxPenaltyElem = rE[i];
                }
                for (int j = 0; j < BSize; j++)
                {
                    if (c[j] || Bhelp[j] == 0)
                        continue;
                    if (cE[j].Penalty > maxPenaltyElem.Penalty)
                        maxPenaltyElem = cE[j];
                }

                int Dif = Math.Min(Ahelp[maxPenaltyElem.Y], Bhelp[maxPenaltyElem.X]);
                mDBR[maxPenaltyElem.Y, maxPenaltyElem.X] = Dif;
                Ahelp[maxPenaltyElem.Y] -= Dif;
                Bhelp[maxPenaltyElem.X] -= Dif;

                if (!r[maxPenaltyElem.Y] && Ahelp[maxPenaltyElem.Y] == 0)
                    r[maxPenaltyElem.Y] = true;
                else if (!c[maxPenaltyElem.X] && Bhelp[maxPenaltyElem.X] == 0)
                    c[maxPenaltyElem.X] = true;
                quantity++;
            }
            if (IsOneNotZero(r, c, Ahelp, Bhelp))
            {
                if (r.Sum(p => p ? 0 : 1) == 1)
                {
                    int row = 0;
                    for (int i = 0; i < r.Length; i++)
                        if (!r[i])
                        {
                            row = i;
                            break;
                        }
                    while (quantity != ASize + BSize - 1)
                    {
                        Element minElemnt = new Element();
                        float min = float.MaxValue;
                        for (int j = 0; j < BSize; j++)
                        {
                            if (!c[j] && mC[row, j] < min)
                            {
                                minElemnt = new Element(j, row);
                                min = mC[row, j];
                            }
                        }

                        int Dif = Math.Min(Ahelp[minElemnt.Y], Bhelp[minElemnt.X]);
                        mDBR[minElemnt.Y, minElemnt.X] = Dif;
                        Ahelp[minElemnt.Y] -= Dif;
                        Bhelp[minElemnt.X] -= Dif;
                        c[minElemnt.X] = true;
                        quantity++;
                    }
                }
                else
                {
                    int col = 0;
                    for (int j = 0; j < c.Length; j++)
                        if (!c[j])
                        {
                            col = j;
                            break;
                        }
                    while (quantity != ASize + BSize - 1)
                    {
                        Element minElemnt = new Element();
                        float min = float.MaxValue;
                        for (int i = 0; i < ASize; i++)
                        {
                            if (!r[i] && mC[i, col] < min)
                            {
                                minElemnt = new Element(col, i);
                                min = mC[col, i];
                            }
                        }

                        int Dif = Math.Min(Ahelp[minElemnt.Y], Bhelp[minElemnt.X]);
                        mDBR[minElemnt.Y, minElemnt.X] = Dif;
                        Ahelp[minElemnt.Y] -= Dif;
                        Bhelp[minElemnt.X] -= Dif;
                        r[minElemnt.Y] = true;
                        quantity++;
                    }
                }
            }
            else if (IsRowsColumsZero(Ahelp, Bhelp))
                while (quantity != ASize + BSize - 1)
                {
                    Element minElemnt = new Element();
                    float min = float.MaxValue;
                    for (int i = 0; i < r.Length; i++)
                        if (!r[i])
                            for (int j = 0; j < BSize; j++)
                            {
                                if (mC[i, j] < min)
                                {
                                    minElemnt = new Element(j, i);
                                    min = mC[i, j];
                                }
                            }
                    for (int j = 0; j < c.Length; j++)
                        if (!c[j])
                            for (int i = 0; i < ASize; i++)
                            {
                                if (mC[i, j] < min)
                                {
                                    minElemnt = new Element(j, i);
                                    min = mC[i, j];
                                }
                            }

                    mDBR[minElemnt.Y, minElemnt.X] = 0;
                    if (!r[minElemnt.Y] && Ahelp[minElemnt.Y] == 0)
                        r[minElemnt.Y] = true;
                    else if (!c[minElemnt.X] && Bhelp[minElemnt.X] == 0)
                        c[minElemnt.X] = true;
                    quantity++;
                }
            return mDBR;
        }
        struct FogelElement
        {
            public float Penalty { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public FogelElement(int x, int y)
            {
                Penalty = 0;
                X = x;
                Y = y;
            }

            public static bool operator ==(FogelElement element1, FogelElement element2)
            {
                return element1.X == element2.X && element1.Y == element2.Y;
            }
            public static bool operator !=(FogelElement element1, FogelElement element2)
            {
                return element1.X != element2.X || element1.Y != element2.Y;
            }
        }
    }
}
