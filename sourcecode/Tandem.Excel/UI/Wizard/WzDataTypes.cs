using System;
using System.Data;

namespace Tandem.Excel.UI.Wizard
{
    public static class WzDataTypes
    {
        public static DataTable ConvertToTable(IWzElement wzElement)
        {
            var dataTable = new DataTable();

            foreach (var item in wzElement.WzElements[0].WzElements)
            {
                dataTable.Columns.Add(item.Key);
            }

            foreach (var items in wzElement.WzElements)
            {
                int i = 0;
                DataRow dataRow = dataTable.NewRow();
                foreach (var item in items.WzElements)
                {
                    object[,] value = (object[,])item.Value;
                    dataRow[i] = item.Value[0,0];
                    i = i + 1;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }


        public static DataView ConvertToArray(IWzElement wzElement)
        {
            return _getBindableArray((object[,])wzElement.Value);
        }


        private class Ref<T>
        {
            private readonly Func<T> getter;
            private readonly Action<T> setter;

            public Ref(Func<T> getter, Action<T> setter)
            {
                this.getter = getter;
                this.setter = setter;
            }
            public T Value { get { return getter(); } set { setter(value); } }
        }

        private static DataView _getBindableArray<T>(T[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            DataTable dataTable = new DataTable();
            for (int i = 0; i < cols; i++)
            {
                dataTable.Columns.Add(i.ToString(), typeof(Ref<T>));
            }
            for (int i = 0; i < rows; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }

            DataView dataView = new DataView(dataTable);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int a = i;
                    int b = j;
                    Ref<T> refT = new Ref<T>(() => array[a, b], z => { array[a, b] = z; });
                    dataView[i][j] = refT;
                }
            }
            return dataView;
        }
    }
}
