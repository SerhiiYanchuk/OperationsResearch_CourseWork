using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OperationsResearch_СourseWork
{
    public enum Algorithm: byte
    {
        Potential = 0,
        Greedy
    }
    public class SolutionData : INotifyPropertyChanged, IDataErrorInfo
    {
        private string pathInputData;
        private string pathOutputData;
        private Algorithm chosenAlgorithm;

        public SolutionData()
        {
            pathInputData = string.Empty;
            pathOutputData = string.Empty;
            chosenAlgorithm = Algorithm.Potential;

            errors = new Dictionary<string, string>();
        }
        public string PathOutputData
        {
            get { return pathOutputData; }
            set
            {
                pathOutputData = value;
                errors["PathOutputData"] = null;
                OnPropertyChanged("PathOutputData");
            }
        }
        public string PathInputData
        {
            get { return pathInputData; }
            set
            {
                pathInputData = value;
                FileInfo file = new FileInfo(@$"InputData/{pathInputData}");
                if (!file.Exists)
                    errors["PathInputData"] = "Файл не існує";
                else
                    errors["PathInputData"] = null;
                OnPropertyChanged("PathInputData");
            }
        }
        public Algorithm ChosenAlgorithm
        {
            get { return chosenAlgorithm; }
            set
            {
                chosenAlgorithm = value;
                errors["ChosenAlgorithm"] = null;
                OnPropertyChanged("ChosenAlgorithm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        // Валидация 
        Dictionary<string, string> errors;
        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;
        
        // Если все тексты ошибок null - данные валидные
        public bool IsValid => !errors.Values.Any(x => x != null);
        public string Error
        {
            get
            {
                return null;
            }
        }
    }
}