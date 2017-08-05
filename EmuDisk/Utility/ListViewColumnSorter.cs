using System.Collections;
using System.Windows.Forms;
using System.Globalization;

namespace EmuDisk
{
    internal class ListViewColumnSorter : IComparer
    {
        #region Private Methods

        private int ColumnToSort;
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;

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

        #endregion

        #region Public Methods

        public ListViewColumnSorter()
        {
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
        }

        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

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
