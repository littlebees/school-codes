using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    /*
     * 這裡的code來自
     * https://msdn.microsoft.com/en-us/library/ms171619.aspx
     * 因為原本的buttonRow沒有enabled可以用
     */
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
            this.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
    }
}
