using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    public class WzElementItem : IWzElement, INotifyPropertyChanged
    {
        private string _key;
        private object[,] _value;
        private string _valueToString;
        private bool _isSelected;
        private bool _isExpanded;
        private List<IWzElement> _wzElements = new List<IWzElement>();


        public bool IsLinked
        {
            get 
            {
                return (_value.GetLength(0) == 1 
                            && _value.GetLength(1) == 1) ? false : true;
            }
        }

        public string Name { get; set; }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public object[,] Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string ValueToString
        {
            get { return _valueToString; }
        }

        public DataView ValueToGrid
        {
            get 
            {
                return WzDataTypes.ConvertToArray(this);
            }
        }

        public List<IWzElement> WzElements
        {
            get { return _wzElements; }
            set { _wzElements = value; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; OnPropertyChanged("IsExpanded"); }
        }

        public WzElementType ElementType { get { return WzElementType.Array; } }

        public void CopyElement(ITdmElement element)
        {
            _key = element.Key;
            _value = element.GetValue<object>();
            _valueToString = _convertValueToString(_value);
        }

        private string _convertValueToString(object[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            if (value[0, 0] == null)
                    value[0, 0] = "null";

            return (rows == 1 && cols == 1) ?
                         value[0, 0].ToString() : "array [" + rows + "x" + cols + "]";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
