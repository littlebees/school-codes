using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    public class AdapterCollection : IAdapter<Ingredient>
    {
        private const int EMPTY_INDEX = -1;
        private List<IAdapter<Drink>> _drinkAdapter = new List<IAdapter<Drink>>();
        private IAdapter<OrderItem> _orderAdapter;
        private IAdapter<IngredientWhichCanBeAdded> _ingredientAdapter;
        private List<IAdapter<IngredientWhichCanBeAdded>> _forGridNeedsIngredient = new List<IAdapter<IngredientWhichCanBeAdded>>();
        private IAdapter<bool> _okAdapter;
        private IAdapter<int> _sumAdapter;
        private List<IAdapter<bool>> _drinkStateAdapter = new List<IAdapter<bool>>();
        private EzDrinkModel _model;
        private List<IngredientWhichCanBeAdded> _disabledIngredients;
        private List<IAdapter<Ingredient>> _forThoseNeedToKnowNewIngredientAdapter = new List<IAdapter<Ingredient>>();
        private IAdapter<Order> _ordersAdapter;
        private IAdapter<OrderItem> _detailAdapter;
        private IAdapter<int> _historySumAdapter;

        public AdapterCollection(EzDrinkModel model, List<IngredientWhichCanBeAdded> ingredients)
        {
            _model = model;
            _disabledIngredients = ingredients.Select(item => (IngredientWhichCanBeAdded)item.Clone()).ToList();
            for (int i = 0, len = _disabledIngredients.Count; i < len; i++)
                _disabledIngredients[i].IsAdded = true;
            _forThoseNeedToKnowNewIngredientAdapter.Add(this);
        }

        // 加入想知道與管理飲料狀態(溫度與甜度)的資料有關的adapter
        public void AddDrinkStateAdapter(IAdapter<bool> adapter)
        {
            _drinkStateAdapter.Add(adapter);
            adapter.ReceiveDataToApply(new List<bool>()
            {
                !_model.IsOrderEmpty
            });
        }

        // 拿到IngredientAdapter
        public IAdapter<IngredientWhichCanBeAdded> GetIngredientAdapter()
        {
            return _ingredientAdapter;
        }

        // 接收與Drink資料有關的adapter的加入點
        public void AddDrinkAdapter(IAdapter<Drink> adapter)
        {
            _drinkAdapter.Add(adapter);
            adapter.ReceiveDataToApply(_model.GetAllDrink());
        }

        // 與被選到的點單項目的配料資料有關的adapter
        public IAdapter<OrderItem> OrderAdapter
        {
            set
            {
                _orderAdapter = value;
                UpdateOrder(_model.GetAllOrder());
            }
        }

        // DetailAdapter
        public IAdapter<OrderItem> DetailAdapter
        {
            set
            {
                _detailAdapter = value;
            }
        }

        // HistorySumAdapter
        public IAdapter<int> HistorySumAdapter
        {
            set
            {
                _historySumAdapter = value;
            }
        }

        // detail的更新
        public void UpdateDetail(List<OrderItem> list)
        {
            _detailAdapter.ReceiveDataToApply(list);
        }

        // 歷史的總價
        public void UpdateHistorySum(int sum)
        {
            List<int> list = new List<int>();
            list.Add(sum);
            _historySumAdapter.ReceiveDataToApply(list);
        }

        // OrdersAdapter
        public IAdapter<Order> OrdersAdapter
        {
            set
            {
                _ordersAdapter = value;
                UpdateHistory(_model.GetAllHistory());
            }
        }

        // 更新歷史
        public void UpdateHistory(List<Order> list)
        {
            _ordersAdapter.ReceiveDataToApply(list);
        }

        // 與點單是否合法的資料有關的adapter的setter
        public IAdapter<IngredientWhichCanBeAdded> IngredientAdapter
        {
            set
            {
                _ingredientAdapter = value;
                UpdateIngredient(_disabledIngredients);
            }
        }

        // 與點單是否合法的資料有關的adapter的setter
        public IAdapter<bool> OkAdapter
        {
            set
            {
                _okAdapter = value;
                UpdateOKOfSum(_model.IsAllOrderOK);
            }
        }

        // 與點單總價的資料有關的adapter的setter
        public IAdapter<int> SumAdapter
        {
            set
            {
                _sumAdapter = value;
                UpdateSum(_model.Price);
            }
        }

        // 讓與飲料狀態有關的adapter接收新的資料
        public void UpdateDrinkState()
        {
            foreach (IAdapter<bool> adapter in _drinkStateAdapter)
                adapter.ReceiveDataToApply(new List<bool>()
                {
                    !_model.IsOrderEmpty
                });
        }

        // 讓與飲料資料有關的adapter接收新的資料
        public void UpdateDrink(List<Drink> drinks)
        {
            foreach (var adapter in _drinkAdapter)
                adapter.ReceiveDataToApply(drinks);
        }

        // 讓與點單資料有關的adapter接收新的資料 
        public void UpdateOrder(List<OrderItem> datas, int index = EMPTY_INDEX)
        {
            if (index != EMPTY_INDEX)
                _orderAdapter.ReceiveDataToApply(datas, index);
            else
                _orderAdapter.ReceiveDataToApply(datas);
        }

        // 讓與點單是否合法的資料有關的adapter接收新的資料
        public void UpdateOKOfSum(bool ok)
        {
            List<bool> list = new List<bool>();
            list.Add(ok);
            _okAdapter.ReceiveDataToApply(list);
        }

        // 讓與點單總價格的資料有關的adapter接收新的資料
        public void UpdateSum(int sum)
        {
            List<int> list = new List<int>();
            list.Add(sum);
            _sumAdapter.ReceiveDataToApply(list);
        }

        // 讓被選到的點單項目的配料資料有關的adapter接收新的資料
        public void UpdateIngredient(List<IngredientWhichCanBeAdded> datas, int index = EMPTY_INDEX)
        {
            List<IAdapter<IngredientWhichCanBeAdded>> allAdapter = _forGridNeedsIngredient.ToList();
            allAdapter.Add(_ingredientAdapter);
            if (datas == null)
                foreach (var adapter in allAdapter)
                    adapter.ReceiveDataToApply(_disabledIngredients);
            else if (index != EMPTY_INDEX)
                foreach (var adapter in allAdapter)
                    adapter.ReceiveDataToApply(datas, index);
            else
                foreach (var adapter in allAdapter)
                    adapter.ReceiveDataToApply(datas);
        }

        // 常用的update的集合
        public void UpdateOrderAndIngredientAndSumAndSumButton(List<IngredientWhichCanBeAdded> datas)
        {
            UpdateIngredient(datas);
            UpdateOrder(_model.GetAllOrder());
            UpdateSum(_model.Price);
            UpdateOKOfSum(_model.IsAllOrderOK);
        }

        // 接收Ingredient的adapter的加入點
        public void AddIngredientAdapter(IAdapter<Ingredient> adapter)
        {
            _forThoseNeedToKnowNewIngredientAdapter.Add(adapter);
        }

        // apply到所有有關的adapter去
        public void UpdateForAddNewIngredient(List<Ingredient> datas)
        {
            foreach (IAdapter<Ingredient> adapter in _forThoseNeedToKnowNewIngredientAdapter)
                adapter.ReceiveDataToApply(datas);
        }

        // 接收新的配料加到List中
        public void ReceiveDataToApply(List<Ingredient> datas, object otherData = null)
        {
            foreach (Ingredient item in datas)
            {
                IngredientWhichCanBeAdded food = new IngredientWhichCanBeAdded(item);
                food.IsAdded = true;
                _disabledIngredients.Add(food);
            }
            UpdateIngredient(_disabledIngredients);
        }

        // 專為DGV設置的adapter加入點
        public void AddAdapterForGridNeedsIngredient(IAdapter<IngredientWhichCanBeAdded> adapter)
        {
            _forGridNeedsIngredient.Add(adapter);
            adapter.ReceiveDataToApply(_disabledIngredients);
        }

        // 同時更新總價與結帳按鈕的enable
        public void UpdateSumAndOKOfSum()
        {
            UpdateSum(_model.Price);
            UpdateOKOfSum(_model.IsAllOrderOK);
        }

        // 把MakeAOrder的update部分抽出來的東西
        public void UpdateForMakeOrder(OrderItem order)
        {
            AddIngredientAdapter(order);
            UpdateOrder(_model.GetAllOrder(), _model.GetAllOrder().Count - 1);
            order.IngredientAdapter = GetIngredientAdapter();
            UpdateSumAndOKOfSum();
            UpdateDrinkState();
            AddAdapterForGridNeedsIngredient(order);
        }

        // DrinkState的update
        public void UpdateForDrinkState(int index)
        {
            UpdateOrder(_model.GetAllOrder(), index);
            UpdateOKOfSum(_model.IsAllOrderOK);
        }
    }
}
