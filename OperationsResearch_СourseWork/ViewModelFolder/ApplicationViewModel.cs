using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace OperationsResearch_СourseWork
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        
        private SolutionData solutionData;
        private ExperimentalData experimentalData;
        private RelayCommand solutionCommand;
        private RelayCommand experimentalCommand1;
        private RelayCommand experimentalCommand2;
        private RelayCommand experimentalCommand3;
        public SolutionData SolutionData
        {
            get { return solutionData; }
            set
            {
                solutionData = value;
                OnPropertyChanged("SolutionData");
            }
        }
        public ExperimentalData ExperimentalData
        {
            get { return experimentalData; }
            set
            {
                experimentalData = value;
                OnPropertyChanged("ExperimentalData");
            }
        }
        public RelayCommand SolutionCommand
        {
            get
            {
                return solutionCommand;
            }
        }
        public RelayCommand ExperimentalCommand1
        {
            get
            {
                return experimentalCommand1;
            }
        }
        public RelayCommand ExperimentalCommand2
        {
            get
            {
                return experimentalCommand2;
            }
        }
        public RelayCommand ExperimentalCommand3
        {
            get
            {
                return experimentalCommand3;
            }
        }
        public ApplicationViewModel()
        {

            solutionData = new SolutionData();
            experimentalData = new ExperimentalData();

            Predicate<object> canExecuted0 = obj => (obj as SolutionData).IsValid;
            Action<object> execute0 = obj =>
            {
                try
                {
                    (int n, int[] vProjects, float[] vPrices) = ReadData(@$"InputData/{solutionData.PathInputData}");
                    var solution = new TRPZSolution(vProjects, vPrices);

                    var result = solutionData.ChosenAlgorithm switch
                    {
                        Algorithm.Potential => solution.FindSolution(TRPZSolution.Method.NordWestAngel),
                        Algorithm.Greedy => GreedySolution.DoGreedy(vProjects, vPrices)
                    };
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    DirectoryInfo directory = new DirectoryInfo("OutputData");
                    if (!directory.Exists)
                        directory.Create();
                    FileInfo file = new FileInfo(@$"OutputData/{solutionData.PathOutputData}.xlsx");
                    if (file.Exists)
                        file.Delete();
                    using (var package = new ExcelPackage(file))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Solution");
                        sheet.Cells[1, 1].Value = "Значення ЦФ:";
                        sheet.Cells[1, 2].Value = result.result;

                        int rows;
                        if (solutionData.ChosenAlgorithm == Algorithm.Potential)
                        {
                            sheet.Cells[1, 4].Value = "Математична модель №1";
                            rows = result.mOpt.GetUpperBound(0) - 1;
                        }                           
                        else
                        {
                            sheet.Cells[1, 4].Value = "Математична модель №2";
                            rows = result.mOpt.GetUpperBound(0);
                        }
                            

                        for (int j = 0; j <= result.mOpt.GetUpperBound(1); j++)
                            sheet.Cells[3, j + 2].Value = $"Спеціаліст {j + 1}";

                        
                        for (int i = 0; i <= rows; i++)
                        {
                            sheet.Cells[i + 4, 1].Value = $"Проект {i+1}";
                            for (int j = 0; j <= result.mOpt.GetUpperBound(1); j++)
                            {
                                if (!float.IsNaN(result.mOpt[i, j]))
                                    sheet.Cells[i + 4, j + 2].Value = result.mOpt[i, j];
                                else
                                    sheet.Cells[i + 4, j + 2].Value = 0;
                            }

                            Console.WriteLine();
                        }
                        package.Save();
                    }
                    solutionData.PathInputData = string.Empty;
                    solutionData.PathOutputData = string.Empty;
                    solutionData.ChosenAlgorithm = Algorithm.Potential;
                }
                catch(ArgumentException)
                {
                    solutionData.PathInputData = "Невалідний файл";
                }              
            };
            solutionCommand = new RelayCommand(execute0, canExecuted0);

            Predicate<object> canExecuted1 = obj => (obj as ExperimentalData).IsValid;
            Action<object> execute1 = obj =>
            {
                Experiment.Experiment1(ExperimentalData);
            };
            experimentalCommand1 = new RelayCommand(execute1, canExecuted1);

            Predicate<object> canExecuted2 = obj => (obj as ExperimentalData).IsValid;
            Action<object> execute2 = obj =>
            {
                Experiment.Experiment2(ExperimentalData);
            };
            experimentalCommand2 = new RelayCommand(execute2, canExecuted2);

            Predicate<object> canExecuted3 = obj => (obj as ExperimentalData).IsValid;
            Action<object> execute3 = obj =>
            {
                Experiment.Experiment3(ExperimentalData);
            };
            experimentalCommand3 = new RelayCommand(execute3, canExecuted3);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        (int n, int[] vProjects, float[] vPrices) ReadData(string pathFile)
        {

            using (StreamReader sr = new StreamReader(pathFile, System.Text.Encoding.Default))
            {
                string line = sr.ReadLine();
                int n = int.Parse(line);

                string[] projects = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] prices = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (projects.Length != prices.Length)
                    throw new ArgumentException("Кількість видів програмних продуктів не співпадають з кількістю категорій спеціалістів");

                int[] vProjects = new int[projects.Length];
                for (int i = 0; i < projects.Length; i++)
                    vProjects[i] = int.Parse(projects[i]);

                
                float[] vPrices = new float[prices.Length];
                for (int i = 0; i < prices.Length; i++)
                    vPrices[i] = float.Parse(prices[i]);

                return (n, vProjects, vPrices);
            }
        }
    }
}