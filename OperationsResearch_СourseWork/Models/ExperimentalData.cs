using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OperationsResearch_СourseWork
{
    public class ExperimentalData : INotifyPropertyChanged, IDataErrorInfo
    {
        private int n1;
        private int n2;
        private int step;
        private int c1;
        private int c2;
        private float dC1;
        private float dC2;
        private int b1;
        private int b2;
        private float dB1;
        private float dB2;

        public ExperimentalData()
        {
            c1 = 100;
            c2 = 1000;
            dC1 = 0.1f;
            dC2 = 0.5f;
            b1 = 10;
            b2 = 100;
            dB1 = 0.1f;
            dB2 = 0.5f;

            errors = new Dictionary<string, string>();
        }
        public int N1
        {
            get { return n1; }
            set
            {
                n1 = value;

                if (n1 < 0)
                {
                    errors["N1"] = "Не може бути від'ємним";
                }
                else if (n1 > n2)
                {
                    errors["N1"] = "Ліва частина не може бути більше правої";
                }
                else
                {
                    errors["N1"] = null;
                }
                OnPropertyChanged("N1");
            }
        }
        public int N2
        {
            get { return n2; }
            set
            {
                n2 = value;

                if (n1 > n2)
                {
                    errors["N2"] = "Права частина не може бути менше лівої";
                }
                else
                {
                    errors["N2"] = null;
                }
                OnPropertyChanged("N2");
            }
        }
        public int Step
        {
            get { return step; }
            set
            {
                step = value;

                if (step < 0)
                {
                    errors["Step"] = "Не може бути від'ємним";
                }
                else
                {
                    errors["Step"] = null;
                }
                OnPropertyChanged("Step");
            }
        }
        public int C1
        {
            get { return c1; }
            set
            {
                c1 = value;

                if (c1 < 0)
                {
                    errors["C1"] = "Не може бути від'ємним";
                }
                else if (c1 > c2)
                {
                    errors["C1"] = "Ліва частина не може бути більше правої";
                }
                else
                {
                    errors["C1"] = null;
                }
                OnPropertyChanged("C1");
            }
        }
        public int C2
        {
            get { return c2; }
            set
            {
                c2 = value;

                if (c1 > c2)
                {
                    errors["C2"] = "Права частина не може бути менше лівої";
                }
                else
                {
                    errors["C2"] = null;
                }
                OnPropertyChanged("C2");
            }
        }
        public float DC1
        {
            get { return dC1; }
            set
            {
                dC1 = value;

                if (dC1 < 0)
                {
                    errors["DC1"] = "Не може бути від'ємним";
                }
                else if (dC1 > dC2)
                {
                    errors["DC1"] = "Ліва частина не може бути більше правої";
                }
                else
                {
                    errors["DC1"] = null;
                }
                OnPropertyChanged("DC1");
            }
        }
        public float DC2
        {
            get { return dC2; }
            set
            {
                dC2 = value;

                if (dC1 > dC2)
                {
                    errors["DC2"] = "Права частина не може бути менше лівої";
                }
                else
                {
                    errors["DC2"] = null;
                }
                OnPropertyChanged("DC2");
            }
        }
        public int B1
        {
            get { return b1; }
            set
            {
                b1 = value;

                if (b1 < 0)
                {
                    errors["B1"] = "Не може бути від'ємним";
                }
                else if (b1 > b2)
                {
                    errors["B1"] = "Ліва частина не може бути більше правої";
                }
                else
                {
                    errors["B1"] = null;
                }
                OnPropertyChanged("B1");
            }
        }
        public int B2
        {
            get { return b2; }
            set
            {
                b2 = value;

                if (b1 > b2)
                {
                    errors["B2"] = "Права частина не може бути менше лівої";
                }
                else
                {
                    errors["B2"] = null;
                }
                OnPropertyChanged("B2");
            }
        }
        public float DB1
        {
            get { return dB1; }
            set
            {
                dB1 = value;

                if (dB1 < 0)
                {
                    errors["DB1"] = "Не може бути від'ємним";
                }
                else if (dB1 > dB2)
                {
                    errors["DB1"] = "Ліва частина не може бути більше правої";
                }
                else
                {
                    errors["DB1"] = null;
                }
                OnPropertyChanged("DB1");
            }
        }
        public float DB2
        {
            get { return dB2; }
            set
            {
                dB2 = value;

                if (dB1 > dB2)
                {
                    errors["DB2"] = "Права частина не може бути менше лівої";
                }
                else
                {
                    errors["DB2"] = null;
                }
                OnPropertyChanged("DB2");
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