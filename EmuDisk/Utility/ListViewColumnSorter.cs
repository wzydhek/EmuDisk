using System;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;

namespace EmuDisk
{
    internal enum ColumnSortType
    {
        Alphanumeric,
        Numeric,
        Date
    }

    internal class ListViewColumnSorter : IComparer
    {
        #region Private Methods

        private int ColumnToSort;
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;
        private ColumnSortType sortType;

        #endregion

        #region Public Properties

        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public ColumnSortType SortType
        {
            set
            {
                sortType = value;
            }
            get
            {
                return sortType;
            }
        }
        
        #endregion

        #region Public Methods

        public ListViewColumnSorter()
        {
            OrderOfSort = SortOrder.None;
            sortType = ColumnSortType.Alphanumeric;

            ObjectCompare = new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
        }

        public int Compare(object x, object y)
        {
            int compareResult = 0;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            switch (sortType)
            {
                case ColumnSortType.Alphanumeric:
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                    break;
                case ColumnSortType.Numeric:
                    int i1 = int.Parse(listviewX.SubItems[ColumnToSort].Text);
                    int i2 = int.Parse(listviewY.SubItems[ColumnToSort].Text);
                    compareResult = i1.CompareTo(i2);
                    break;
                case ColumnSortType.Date:
                    DateTime d1 = DateTime.Parse(listviewX.SubItems[ColumnToSort].Text);
                    DateTime d2 = DateTime.Parse(listviewY.SubItems[ColumnToSort].Text);
                    compareResult = d1.CompareTo(d2);
                    break;
            }


            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
