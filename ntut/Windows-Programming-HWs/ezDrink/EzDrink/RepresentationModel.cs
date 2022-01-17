using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    public class RepresentationModel
    {
        private EzDrinkModel _model;
        
        public RepresentationModel(EzDrinkModel model)
        {
            _model = model;
        }

        // EzDrinkModel的getter
        public EzDrinkModel Model
        {
            get
            {
                return _model;
            }
        }

        // 選了飲料的event
        public bool SelectDrink(DataGridViewCellEventArgs e)
        {
            return e.ColumnIndex == 0;
        }

        // 刪掉點單項目的event
        public bool RemoveOrderClick(DataGridViewCellEventArgs e)
        {
            bool answer = e.ColumnIndex == InitialData.DELETE_COLUMN_INDEX;
            if (answer)
                _model.RemoveOrderOf(e.RowIndex);
            return answer;
        }

        // ClickSweet的helper
        private bool ClickForSweet(Sweet sweet, int index)
        {
            if (index != InitialData.EMPTY_INDEX)
            {
                _model.SetSweetOf(index, sweet);
            }
            return index != InitialData.EMPTY_INDEX;
        }

        // 點了 正常 甜度的事件
        public bool ClickNormalSweet(int orderDataGridView)
        {
            return ClickForSweet(Sweet.Normal, orderDataGridView);
        }

        // 點了 半糖 甜度的事件
        public bool ClickHalfSweet(int orderDataGridView)
        {
            return ClickForSweet(Sweet.Half, orderDataGridView);
        }

        // 點了 無糖 甜度的事件
        public bool ClickNoSweet(int orderDataGridView)
        {
            return ClickForSweet(Sweet.No, orderDataGridView);
        }

        // 點了 少糖 甜度的事件
        public bool ClickFewSweet(int orderDataGridView)
        {
            return ClickForSweet(Sweet.Few, orderDataGridView);
        }

        // ClickIce的helper
        private bool ClickForIce(Ice sweet, int index)
        {
            if (index != InitialData.EMPTY_INDEX)
            {
                _model.SetIceOf(index, sweet);
            }
            return index != InitialData.EMPTY_INDEX;
        }

        // 點了 正常 溫度的事件
        public bool ClickNormalIce(int orderDataGridView)
        {
            return ClickForIce(Ice.Normal, orderDataGridView);
        }

        // 點了 溫熱 溫度的事件(Half的理由是要與甜度的命名一致)
        public bool ClickHalfIce(int orderDataGridView)
        {
            return ClickForIce(Ice.Half, orderDataGridView);
        }

        // 點了 去冰 溫度的事件
        public bool ClickNoIce(int orderDataGridView)
        {
            return ClickForIce(Ice.No, orderDataGridView);
        }

        // 點了 少冰 溫度的事件
        public bool ClickFewIce(int orderDataGridView)
        {
            return ClickForIce(Ice.Few, orderDataGridView);
        }

        // 點了結帳的事件
        public void SaveOrderToModel()
        {
            _model.SaveOrder();
        }

        // order的select index發生改變的事件
        public bool OrderSelectChange(DataGridViewRowStateChangedEventArgs e)
        {
            return e.StateChanged != DataGridViewElementStates.Selected;
        }

        // click歷史的item
        public List<OrderItem> SelectHistoryItem(int index)
        {
            List<OrderItem> data = null;
            //if (e.StateChanged != DataGridViewElementStates.Selected)
            //{
            data = index == InitialData.EMPTY_INDEX ? null : _model.GetAllHistory()[index].GetOrder();
            //}
            return data;
        }

        // 點擊About的事件
        public String GetAboutString()
        { 
            return InitialData.GetAboutString();
        }

        // 試著加入飲料
        public void TryAddDrink(String name, int price)
        {
            _model.TryAddDrink(name, price);
        }

        // 試著加入配料
        public List<Ingredient> TryAddIngredient(String name, int price)
        {
            return _model.TryAddIngredient(name, price);
        }

        // 當按下刪除飲料時
        public bool ClickDeleteDrink(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
                _model.RemoveDrinkByIndex(e.RowIndex);
            return e.ColumnIndex == 0;
        }

        // 當按下刪除配料時
        public bool ClickDeleteIngredient(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
                _model.RemoveIngredientByIndex(e.RowIndex);
            return e.ColumnIndex == 0;
        }

        // 當cell的值變了的話
        public bool ChangeCellValueOfDrinkGrid(DataGridViewCellEventArgs e, DataGridView drinkBackGrid)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == InitialData.INDEX_FOR_TITLE)
            {
                _model.SetDrinkName(e.RowIndex, (String)drinkBackGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                return true;
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                _model.SetDrinkPrice(e.RowIndex, int.Parse((String)drinkBackGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
            }
            return false;
        }

        // 當cell的值變了的話
        public bool ChangeCellValueOfIngredientGrid(DataGridViewCellEventArgs e, DataGridView ingredientBackGrid)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 1)
            {
                _model.SetIngredientName(e.RowIndex, (String)ingredientBackGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                return true;
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // if (e.RowIndex >= 0 && e.ColumnIndex != 0 && e.ColumnIndex == SPECIAL_INGREDIENT)
            {
                _model.SetIngredientPrice(e.RowIndex, int.Parse((String)ingredientBackGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
            }
            return false;
        }

        // 選了配料的event
        public bool SelectIngredient(DataGridViewCellEventArgs e, int index)
        {
            if (e.ColumnIndex == 0 && index != InitialData.EMPTY_INDEX)
                _model.AddIngredientOf(index, e.RowIndex);
            return e.ColumnIndex == 0 && index != InitialData.EMPTY_INDEX;
        }
    }
}
