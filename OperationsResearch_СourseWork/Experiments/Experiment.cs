using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsResearch_СourseWork
{
    static class Experiment
    {
        public static void Experiment1(ExperimentalData data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(@$"ExperimentResult/Experiment1.xlsx");
            if (file.Exists)
                file.Delete();

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Exp");
                sheet.Cells[2, 1].Value = "Розмірність";
                sheet.Cells[2, 2].Value = "Пн-Зх кут";
                sheet.Cells[2, 3].Value = "Фогель";
                int iteration = 3;
                for (int n = data.N1; n <= data.N2; n += data.Step)
                {
                    
                    double[] time1 = new double[10];
                    double[] time2 = new double[10];
                    for (int k = 0; k < 10; k++)
                    {
                        Random random = new Random();
                        int c = random.Next(data.C1, data.C2 + 1);
                        double dC;
                        do
                        {
                            dC = random.NextDouble();
                        } while (dC < data.DC1 || dC > data.DC2);

                        float[] vPrices = new float[n];
                        double delta = dC * c;
                        int left = (int)Math.Floor(c - delta);
                        int right = (int)Math.Floor(c + delta);
                        for (int i = 0; i < n; i++)
                            vPrices[i] = random.Next(left, right + 1);

                        int b = random.Next(data.B1, data.B2 + 1);
                        double dB;
                        do
                        {
                            dB = random.NextDouble();
                        } while (dB < data.DB1 || dB > data.DB2);

                        float[] vProjects = new float[n];
                        delta = dB * b;
                        left = (int)Math.Floor(b - delta);
                        right = (int)Math.Floor(b + delta);
                        for (int i = 0; i < n; i++)
                            vProjects[i] = random.Next(left, right + 1);

                        Stopwatch stopWatch = new Stopwatch();
                        var solution = new TRPZSolution((float[])vProjects.Clone(), (float[])vPrices.Clone());
                        stopWatch.Start();
                        solution.FindSolution(TRPZSolution.Method.NordWestAngel);
                        stopWatch.Stop();     
                        TimeSpan ts = stopWatch.Elapsed;
                        time1[k] = ts.TotalMilliseconds;

                        stopWatch.Reset();                   
                        solution = new TRPZSolution((float[])vProjects.Clone(), (float[])vPrices.Clone());
                        stopWatch.Start();
                        solution.FindSolution(TRPZSolution.Method.Fogel);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time2[k] = ts.TotalMilliseconds;
                    }
                    sheet.Cells[iteration, 1].Value = n;
                    sheet.Cells[iteration, 2].Value = time1.GetMedian();
                    sheet.Cells[iteration, 3].Value = time2.GetMedian();
                    iteration++;
                }
                package.Save();
            }
        }
        public static void Experiment3(ExperimentalData data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(@$"ExperimentResult/Experiment3.xlsx");
            if (file.Exists)
                file.Delete();

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Exp");
                sheet.Cells[2, 1].Value = "Розмірність";
                sheet.Cells[2, 2].Value = "Метод потенціалів";
                sheet.Cells[2, 3].Value = "Жадібний алгоритм";
                int iteration = 3;
                for (int n = data.N1; n <= data.N2; n += data.Step)
                {

                    double[] time1 = new double[10];
                    double[] time2 = new double[10];
                    for (int k = 0; k < 10; k++)
                    {
                        Random random = new Random();
                        int c = random.Next(data.C1, data.C2 + 1);
                        double dC;
                        do
                        {
                            dC = random.NextDouble();
                        } while (dC < data.DC1 || dC > data.DC2);

                        float[] vPrices = new float[n];
                        double delta = dC * c;
                        int left = (int)Math.Floor(c - delta);
                        int right = (int)Math.Floor(c + delta);
                        for (int i = 0; i < n; i++)
                            vPrices[i] = random.Next(left, right + 1);

                        int b = random.Next(data.B1, data.B2 + 1);
                        double dB;
                        do
                        {
                            dB = random.NextDouble();
                        } while (dB < data.DB1 || dB > data.DB2);

                        float[] vProjects = new float[n];
                        delta = dB * b;
                        left = (int)Math.Floor(b - delta);
                        right = (int)Math.Floor(b + delta);
                        for (int i = 0; i < n; i++)
                            vProjects[i] = random.Next(left, right + 1);

                        Stopwatch stopWatch = new Stopwatch();
                        var solution = new TRPZSolution((float[])vProjects.Clone(), (float[])vPrices.Clone());
                        stopWatch.Start();
                        solution.FindSolution(TRPZSolution.Method.NordWestAngel);
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        time1[k] = ts.TotalMilliseconds;

                        stopWatch.Reset();
    
                        stopWatch.Start();
                        GreedySolution.DoGreedy(vProjects, vPrices);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time2[k] = ts.TotalMilliseconds;
                    }
                    sheet.Cells[iteration, 1].Value = n;
                    sheet.Cells[iteration, 2].Value = time1.GetMedian();
                    sheet.Cells[iteration, 3].Value = time2.GetMedian();
                    iteration++;
                }
                package.Save();
            }

        }

        public static void Experiment2(ExperimentalData data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(@$"ExperimentResult/Experiment2.xlsx");
            if (file.Exists)
                file.Delete();

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Exp");
                sheet.Cells[2, 1].Value = "Ітерації/Розмірність";
                int iteration = 2;
                for (int n = data.N1; n <= data.N2; n += data.Step)
                {
                    Random random = new Random();
                    int c = random.Next(data.C1, data.C2 + 1);
                    double dC;
                    do
                    {
                        dC = random.NextDouble();
                    } while (dC < data.DC1 || dC > data.DC2);

                    float[] vPrices = new float[n];
                    double delta = dC * c;
                    int left = (int)Math.Floor(c - delta);
                    int right = (int)Math.Floor(c + delta);
                    for (int i = 0; i < n; i++)
                        vPrices[i] = random.Next(left, right + 1);

                    int b = random.Next(data.B1, data.B2 + 1);
                    double dB;
                    do
                    {
                        dB = random.NextDouble();
                    } while (dB < data.DB1 || dB > data.DB2);

                    float[] vProjects = new float[n];
                    delta = dB * b;
                    left = (int)Math.Floor(b - delta);
                    right = (int)Math.Floor(b + delta);
                    for (int i = 0; i < n; i++)
                        vProjects[i] = random.Next(left, right + 1);
   
                    var solution = new TRPZSolution(vProjects, vPrices);       
                    var result = solution.FindSolutionForExperiment(TRPZSolution.Method.NordWestAngel);

                    for (int i = 0; i < result.Length; i++)
                    {
                        sheet.Cells[i + 3, iteration].Value = result[i];
                        sheet.Cells[i + 3, 1].Value = i + 1;
                    }    
                        
                    sheet.Cells[2, iteration].Value = n;
                    iteration++;
                }
                package.Save();
            }

        }
        public static double GetMedian(this double[] source)
        {
            // Create a copy of the input, and sort the copy
            double[] temp = source.ToArray();
            Array.Sort(temp);
            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                double a = temp[count / 2 - 1];
                double b = temp[count / 2];
                return (a + b) / 2d;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }

    }
}
