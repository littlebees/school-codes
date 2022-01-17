using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    /*
     * DataGridView的Adapter
     */
    public class DataGridViewAdapter<T> : IAdapter<T>
    {
        public delegate void ModifyCellsEventHandler(DataGridViewRow row, T data);
        private ModifyCellsEventHandler _writeCells;
        private const int EMPTY_INDEX = -1;
        private DataGridView _view;
        private int _lengthOfDataGridView = 0;

        public DataGridViewAdapter(DataGridView dataGridView, ModifyCellsEventHandler function)
        {
            _view = dataGridView;
            _writeCells = function;
        }

        // 實作 IAdapter 的函式
        public void ReceiveDataToApply(List<T> datas, Object otherData = null)
        {
            int lengthOfDatas = datas.Count;
            if (_lengthOfDataGridView != 0)
            {
                if (lengthOfDatas < _lengthOfDataGridView)
                    UpdateWhenSomeDataBeenDeleted(datas);
                else if (lengthOfDatas == _lengthOfDataGridView)
                    UpdateWhenSomeDataBeenAdded(datas);
                else if (lengthOfDatas > _lengthOfDataGridView)
                    UpdateWhenSomeDataBeenAdded(datas);
                else
                    throw new Exception();
            } 
            else
                UpdateWhenSomeDataBeenAdded(datas);
            ProcessOtherData(otherData);
            _lengthOfDataGridView = lengthOfDatas;
        }

        // 當有些資料被刪掉了要怎麼更新
        // (先刷掉再從頭塞新資料，但是會把原本的select取消掉)
        private void UpdateWhenSomeDataBeenDeleted(List<T> datas)
        {
            _view.Rows.Clear();

            foreach (T data in datas)
                AddDataToDataGridView(data);

            ClearAllSelections();
        }

        // 處理otherDataˇ的資料
        private void ProcessOtherData(Object otherData)
        {
            if (otherData != null)
            {
                int index = (int)otherData;
                ClearAllSelections();
                _view.Rows[ index ].Selected = true;
            }
        }

        // 當有些資料被修改了要怎麼更新
        // (就是直接更新DGV的資料。但是改Enabled的參數，不會馬上反映到畫面上去所以棄用它了)
        private void UpdateWhenSomeDataBeenModified(List<T> datas)
        {
            int index = 0;
            foreach (T data in datas)
                ModifySpecificRow(index++, data);
        }

        // 當加入新資料的時候怎麼更新
        // (先刷掉再從頭塞新資料)
        private void UpdateWhenSomeDataBeenAdded(List<T> datas)
        {
            _view.Rows.Clear();

            foreach (T data in datas)
                AddDataToDataGridView(data);
        }

        // 如何新增資料到DGV去
        // (先new Row再用_reWriteCells來處理)
        private void AddDataToDataGridView(T data)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(_view);
            _writeCells(row, data);
            _view.Rows.Add(row);
        }

        // 當要更新指定的row的函式
        private void ModifySpecificRow(int index, T data)
        {
            DataGridViewRow row = _view.Rows[ index ];
            _writeCells(row, data);
        }

        // 把所有selection清掉
        private void ClearAllSelections()
        {
            for (int i = 0, len = _view.Rows.Count; i < len; i++)
                _view.Rows[ i ].Selected = false;
        }
    }
}
