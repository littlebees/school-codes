using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EzDrink
{
    public partial class EzDrinkForm : Form
    {
        private const int EMPTY_INDEX = -1;
        private RepresentationModel _model;
        private AddingButtonsStateMachine<Drink> _addingDrink;
        private AddingButtonsStateMachine<IngredientWhichCanBeAdded> _addingIngredient;

        public EzDrinkForm()
        {
            InitializeComponent();

            EzDrinkModel model = new EzDrinkModel();
            InitialData.EstablishAdapter(model, _sumLabel, _sumButton, _normalSweet, _halfSweet, _fewSweet, _noSweet, _normalIce, _halfIce, _fewIce, _noIce);
            _addingDrink = new AddingButtonsStateMachine<Drink>(_drinkFromBoxButton, _drinkFromFileButton, _addingDrinkNameTextBox, _addingDrinkPriceTextBox, TryAddDrink, model);
            _addingIngredient = new AddingButtonsStateMachine<IngredientWhichCanBeAdded>(_ingredientFromBoxButton, _ingredientFromFileButton, _addingIngredientNameTextBox, _addingIngredientPriceTextBox, TryAddIngredient, model);
            InitialData.EstablishDataGridView(model, _drinkDataGridView, _drinkBackGrid, _ingredientDataGridView, _orderDataGridView, _ingredientBackGrid);
            InitialData.MakeHistoryTab(model, _historyDataGridView, _detailDataGridView, _amountLabel);
            _model = new RepresentationModel(model);
        }

        // 選了飲料的event
        private void SelectDrink(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.SelectDrink(e))
                _model.Model.GetAdapterCollection().UpdateForMakeOrder(_model.Model.MakeAnOrder(e.RowIndex));
        }

        // 選了配料的event
        private void SelectIngredient(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.SelectIngredient(e, GetNowSelectOrderIndex(_orderDataGridView)))
            {
                _model.Model.GetAdapterCollection().UpdateOrder(_model.Model.GetAllOrder(), GetNowSelectOrderIndex(_orderDataGridView));
                _model.Model.GetAdapterCollection().UpdateSum(_model.Model.Price);
                _model.Model.GetAdapterCollection().UpdateIngredient(_model.Model.GetAllOrder()[GetNowSelectOrderIndex(_orderDataGridView)].Ingredients, e.RowIndex);
            }
        }

        // 刪掉點單項目的event
        private void RemoveOrderClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.RemoveOrderClick(e))
            {
                _model.Model.GetAdapterCollection().UpdateOrderAndIngredientAndSumAndSumButton(null);
                _model.Model.GetAdapterCollection().UpdateDrinkState();
            }
        }

        // 點了 正常 甜度的事件
        private void ClickNormalSweet(object sender, EventArgs e)
        {
            if (_model.ClickNormalSweet(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 半糖 甜度的事件
        private void ClickHalfSweet(object sender, EventArgs e)
        {
            if (_model.ClickHalfSweet(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 無糖 甜度的事件
        private void ClickNoSweet(object sender, EventArgs e)
        {
            if (_model.ClickNoSweet(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 少糖 甜度的事件
        private void ClickFewSweet(object sender, EventArgs e)
        {
            if (_model.ClickFewSweet(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 正常 溫度的事件
        private void ClickNormalIce(object sender, EventArgs e)
        {
            if (_model.ClickNormalIce(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 溫熱 溫度的事件(Half的理由是要與甜度的命名一致)
        private void ClickHalfIce(object sender, EventArgs e)
        {
            if (_model.ClickHalfIce(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 去冰 溫度的事件
        private void ClickNoIce(object sender, EventArgs e)
        {
            if (_model.ClickNoIce(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了 少冰 溫度的事件
        private void ClickFewIce(object sender, EventArgs e)
        {
            if (_model.ClickFewIce(GetNowSelectOrderIndex(_orderDataGridView)))
                _model.Model.GetAdapterCollection().UpdateForDrinkState(GetNowSelectOrderIndex(_orderDataGridView));
        }

        // 點了結帳的事件
        private void SaveOrderToModel(object sender, EventArgs e)
        {
            _model.SaveOrderToModel();
            _model.Model.GetAdapterCollection().UpdateOrderAndIngredientAndSumAndSumButton(null);
            _model.Model.GetAdapterCollection().UpdateDrinkState();
            _model.Model.GetAdapterCollection().UpdateHistory(_model.Model.GetAllHistory());
        }

        // order的select index發生改變的事件
        private void OrderSelectChange(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (_model.OrderSelectChange(e))
            {
                List<IngredientWhichCanBeAdded> nowIngredient = GetNowSelectOrderIndex(_orderDataGridView) == EMPTY_INDEX ? null : _model.Model.GetAllOrder()[GetNowSelectOrderIndex(_orderDataGridView)].Ingredients;
                _model.Model.GetAdapterCollection().UpdateIngredient(nowIngredient);
            }
        }

        // 點擊Exit的事件
        private void ClickExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // 點擊About的事件
        private void ClickAbout(object sender, EventArgs e)
        {
            MessageBox.Show(_model.GetAboutString());
        }

        // 點擊從檔案加入飲料的button
        private void ClickDrinkFileButton(object sender, EventArgs e)
        {
            _addingDrink.ClickFileButton();
        }

        // 點擊從檔案加入配料的button
        private void ClickIngredientFileButton(object sender, EventArgs e)
        {
            _addingIngredient.ClickFileButton();
        }

        // 試著加入飲料
        public void TryAddDrink(String name, int price)
        {
            try
            {
                _model.TryAddDrink(name, price);
                _model.Model.GetAdapterCollection().UpdateDrink(_model.Model.GetAllDrink());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // 試著加入配料
        public void TryAddIngredient(String name, int price)
        {
            try
            {
                _model.Model.GetAdapterCollection().UpdateForAddNewIngredient(_model.TryAddIngredient(name, price));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // 點擊新增配料的按鈕
        private void ClickIngredientTextButton(object sender, EventArgs e)
        {
            //_model.ClickIngredientTextButton(sender, e);
            _addingIngredient.ClickTextBoxButton();
        }

        // 當按下刪除飲料時
        private void ClickDeleteDrink(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.ClickDeleteDrink(e))
                _model.Model.GetAdapterCollection().UpdateDrink(_model.Model.GetAllDrink());
        }

        // 當按下刪除配料時
        private void ClickDeleteIngredient(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.ClickDeleteIngredient(e))
                _model.Model.GetAdapterCollection().UpdateOrderAndIngredientAndSumAndSumButton(_model.Model.GetIngredient().Container);
        }

        // 當cell的值變了的話
        private void ChangeCellValueOfDrinkGrid(object sender, DataGridViewCellEventArgs e)
        {
            if (_model != null)
                if (_model.ChangeCellValueOfDrinkGrid(e, _drinkBackGrid))
                {
                    _model.Model.GetAdapterCollection().UpdateDrink(_model.Model.GetAllDrink());
                    _model.Model.GetAdapterCollection().UpdateOrder(_model.Model.GetAllOrder());
                }
                else
                {
                    _model.Model.GetAdapterCollection().UpdateDrink(_model.Model.GetAllDrink());
                    _model.Model.GetAdapterCollection().UpdateSumAndOKOfSum();
                    _model.Model.GetAdapterCollection().UpdateOrder(_model.Model.GetAllOrder());
                }
        }

        // 當cell的值變了的話
        private void ChangeCellValueOfIngredientGrid(object sender, DataGridViewCellEventArgs e)
        {
            if (_model != null)
                if (_model.ChangeCellValueOfIngredientGrid(e, _ingredientBackGrid))
                {
                    _model.Model.GetAdapterCollection().UpdateOrderAndIngredientAndSumAndSumButton(_model.Model.GetIngredient().Container);
                    _model.Model.GetAdapterCollection().UpdateOrder(_model.Model.GetAllOrder());
                }
                else
                {
                    _model.Model.GetAdapterCollection().UpdateOrderAndIngredientAndSumAndSumButton(_model.Model.GetIngredient().Container);
                    _model.Model.GetAdapterCollection().UpdateOrder(_model.Model.GetAllOrder());
                }
        }

        // 現在選到的點單項目的index是什麼
        private int GetNowSelectOrderIndex(DataGridView orderDataGridView)
        {
            for (int i = 0, len = orderDataGridView.Rows.Count; i < len; i++)
                if (orderDataGridView.Rows[i].Selected)
                    return i;
            return InitialData.EMPTY_INDEX;
        }

        // 當點擊新增飲料的按鈕時
        private void ClickDrinkTextButton(object sender, EventArgs e)
        {
            _addingDrink.ClickTextBoxButton();
        }

        // 點了history的item
        private void SelectHistoryItem(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            List<OrderItem> data = _model.SelectHistoryItem(GetNowSelectOrderIndex(_historyDataGridView));
            if (data != null)
            {
                _model.Model.GetAdapterCollection().UpdateDetail(data);
                _model.Model.GetAdapterCollection().UpdateHistorySum(_model.Model.GetAllHistory()[GetNowSelectOrderIndex(_historyDataGridView)].Price);
            }
        }
    }
}
