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
            n1 = 100;
            n2 = 1000;
            step = 100;
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
        private void isN_Valid()
        {
            if (n1 < 0)
            {
                errors["N1"] = "Не може бути від'ємним";
            }
            else if (n2 < 0)
            {
                errors["N2"] = "Не може бути від'ємним";
            }
            else if (n1 > n2)
            {
                errors["N1"] = "Ліва частина не може бути більше правої";
                errors["N2"] = "Права частина не може бути менше лівої";
            }
            else
            {
                errors["N1"] = null;
                errors["N2"] = null;
            }
            OnPropertyChanged("N1");
            OnPropertyChanged("N2");
        }
        public int N1
        {
            get { return n1; }
            set
            {
                n1 = value;
                isN_Valid();
            }
        }
        public int N2
        {
            get { return n2; }
            set
            {
                n2 = value;
                isN_Valid();
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
        private void isC_Valid()
        {
            if (c1 < 0)
            {
                errors["C1"] = "Не може бути від'ємним";
            }
            else if (c2 < 0)
            {
                errors["C2"] = "Не може бути від'ємним";
            }
            else if (c1 > c2)
            {
                errors["C1"] = "Ліва частина не може бути більше правої";
                errors["C2"] = "Права частина не може бути менше лівої";
            }
            else
            {
                errors["C1"] = null;
                errors["C2"] = null;
            }
            OnPropertyChanged("C1");
            OnPropertyChanged("C2");
        }
        public int C1
        {
            get { return c1; }
            set
            {
                c1 = value;
                isC_Valid();
            }
        }
        public int C2
        {
            get { return c2; }
            set
            {
                c2 = value;
                isC_Valid();
            }
        }
        private void isDC_Valid()
        {
            if (dC1 < 0 || dC1 > 1)
            {
                errors["DC1"] = "Можливі значення від 0 до 1";
            }
            else if (dC2 < 0 || dC2 > 1)
            {
                errors["DC2"] = "Можливі значення від 0 до 1";
            }
            else if (dC1 > dC2)
            {
                errors["DC1"] = "Ліва частина не може бути більше правої";
                errors["DC2"] = "Права частина не може бути менше лівої";
            }
            else
            {
                errors["DC1"] = null;
                errors["DC2"] = null;
            }
            OnPropertyChanged("DC1");
            OnPropertyChanged("DC2");
        }
        public float DC1
        {
            get { return dC1; }
            set
            {
                dC1 = value;
                isDC_Valid();
            }
        }
        public float DC2
        {
            get { return dC2; }
            set
            {
                dC2 = value;
                isDC_Valid();
            }
        }
        private void isB_Valid()
        {
            if (b1 < 0)
            {
                errors["B1"] = "Не може бути від'ємним";
            }
            else if (b2 < 0)
            {
                errors["B2"] = "Не може бути від'ємним";
            }
            else if (b1 > b2)
            {
                errors["B1"] = "Ліва частина не може бути більше правої";
                errors["B2"] = "Права частина не може бути менше лівої";
            }
            else
            {
                errors["B1"] = null;
                errors["B2"] = null;
            }
            OnPropertyChanged("B1");
            OnPropertyChanged("B2");
        }
        public int B1
        {
            get { return b1; }
            set
            {
                b1 = value;
                isB_Valid();
                OnPropertyChanged("B1");
            }
        }
        public int B2
        {
            get { return b2; }
            set
            {
                b2 = value;
                isB_Valid();
                OnPropertyChanged("B2");
            }
        }
        private void isDB_Valid()
        {
            if (dB1 < 0 || dB1 > 1)
            {
                errors["DB1"] = "Можливі значення від 0 до 1";
            }
            else if (dB2 < 0 || dB2 > 1)
            {
                errors["DB2"] = "Можливі значення від 0 до 1";
            }
            else if (dB1 > dB2)
            {
                errors["DB1"] = "Ліва частина не може бути більше правої";
                errors["DB2"] = "Права частина не може бути менше лівої";
            }
            else
            {
                errors["DB1"] = null;
                errors["DB2"] = null;
            }
            OnPropertyChanged("DB1");
            OnPropertyChanged("DB2");
        }
        public float DB1
        {
            get { return dB1; }
            set
            {
                dB1 = value;
                isDB_Valid();
            }
        }
        public float DB2
        {
            get { return dB2; }
            set
            {
                dB2 = value;
                isDB_Valid();
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