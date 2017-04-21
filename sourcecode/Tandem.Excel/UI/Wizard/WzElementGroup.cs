using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    public class WzElementGroup : IWzElement, INotifyPropertyChanged
    {
        private bool _isSelected;
        private bool _isExpanded;
        private List<IWzElement> _wzElements = new List<IWzElement>();
        private WzElementType _objectType;

        public WzElementType ElementType { get { return _objectType; } }

        public string Name { get; set; }
        public string Key { get; set; }
        public object[,] Value { get; set; }
        
        public bool IsLinked
        {
            get { return true; }
        }

        public List<IWzElement> WzElements
        {
            get { return _wzElements; } 
            set {_wzElements = value;}
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

        public DataTable ValueToGrid
        {
            get
            {
                return WzDataTypes.ConvertToTable(this);
            }
        }

        public string ValueToString
        {
            get { return _convertValueToString(); }
        }

        public void CopyElement(ITdmElement element)
        { 
            this.WzElements.Clear();
            this.Key = element.Key;

            if (element.IsTable) _objectType = WzElementType.Table;

            int maxRows = 99;
            int rowCounter = 0;
            foreach (var item in element.Group)
            {
                if (rowCounter > maxRows) break;

                IWzElement wzElement = WzElementFactory.GetElementClass(item.GetType());
                wzElement.CopyElement(item);
                _wzElements.Add(wzElement);
                rowCounter++;
            }
        }

        private string _convertValueToString()
        {
            string displValue = null;
            switch (_objectType)
            {
                case WzElementType.Dictionary:
                    displValue = "dict [" + _wzElements.Count + "]";
                    break;
                case WzElementType.Table:
                    displValue = "table [" + _wzElements[0].WzElements.Count + "x" + _wzElements.Count + "]";
                    break;
                default:
                    break;
            }
            return displValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
