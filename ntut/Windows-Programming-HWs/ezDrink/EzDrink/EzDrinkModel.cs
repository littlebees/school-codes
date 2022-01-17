using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EzDrink
{
    public class EzDrinkModel
    {
        private const int EMPTY_INDEX = -1;
        private const string DUPLICATE_INGREDIENT = "Duplicate Ingredient";
        private const string DUPLICATE_DRINK = "Duplicate Drink";
        private List<OrderItem> _order = new List<OrderItem>();
        private List<Order> _orders = new List<Order>();
        private AdapterCollection _allAdapter;
        private DataContainer<Drink> _drinks;
        private DataContainer<IngredientWhichCanBeAdded> _ingredients;

        public EzDrinkModel()
        {
            _drinks = new DataContainer<Drink>(delegate(List<Drink> list)
            {
                foreach (var item in InitialData.GenerateDrinkList())
                    list.Add(item);
            }, DUPLICATE_DRINK);
            _ingredients = new DataContainer<IngredientWhichCanBeAdded>(delegate(List<IngredientWhichCanBeAdded> list)
            {
                foreach (var item in InitialData.GenerateIngredientWhichCanBeAddedList())
                    list.Add(item);
            }, DUPLICATE_INGREDIENT);
            _allAdapter = new AdapterCollection(this, _ingredients.Container);
        }

        //history的setter
        public List<Order> GetAllHistory()
        {
            return _orders;
        }

        // 拿到所有的飲料的List
        public List<Drink> GetAllDrink()
        {
            return _drinks.Container;
        }

        // 拿到目前的點單
        public List<OrderItem> GetAllOrder()
        {
            return _order;
        }

        // 拿到 Adapter的集合物件
        public AdapterCollection GetAdapterCollection()
        {
            return _allAdapter;
        }

        // 創造新的點單項目
        public OrderItem MakeAnOrder(int drinkIndex)
        {
            Drink drink = _drinks.GetItem(drinkIndex);
            OrderItem order = new OrderItem(drink, _ingredients.Container);
            _order.Add(order);
            return order;
        }

        // 設定甜度到特定的項目
        public void SetSweetOf(int index, Sweet sweet)
        {
            _order[index].Sugar = sweet;
        }

        // 設定溫度到特定的項目
        public void SetIceOf(int index, Ice ice)
        {
            _order[index].Cold = ice;
        }

        // 加入配料到特定的項目
        public void AddIngredientOf(int orderIndex, int ingredientIndex)
        {
            _order[orderIndex].AddIngredient(_ingredients.GetItem(ingredientIndex).Top);
        }

        // 把 index 的點單刪掉
        public void RemoveOrderOf(int index)
        {
            _order.RemoveAt(index);
        }

        // 把目前的點單存起來
        public void SaveOrder()
        {
            if (IsAllOrderOK)
            {
                _orders.Add(new Order(_order, DateTime.Now));
                _order = new List<OrderItem>();
            }
        }

        // 沒有任何項目在目前的點單中嗎
        public bool IsOrderEmpty
        {
            get
            {
                return _order.Count == 0;
            }
        }

        // 目前點單的總價格
        public int Price
        {
            get
            {
                int ans = 0;
                foreach (OrderItem order in _order)
                    ans += order.Price;
                return ans;
            }
        }

        // 所有點單都是合法的嗎
        public bool IsAllOrderOK
        {
            get
            {
                if (_order.Count != 0)
                {
                    foreach (OrderItem order in _order)
                        if (!order.IsOK)
                            return false;
                    return true;
                }
                return false;
            }
        }

        // 用stream加入item
        public DataContainer<T> AddNewItem<T>(StreamReader stream) where T : ISame<T>
        {
            DataContainer<T> storeData = GetContainerByType<T>().AddNewItem(stream);
            return storeData;
        }

        // 用type的到對應的dataContainer
        private DataContainer<T> GetContainerByType<T>() where T : ISame<T>
        {
            if (typeof(T) == typeof(Drink))
                return _drinks as DataContainer<T>;
            else //if (typeof(T) == typeof(IngredientWhichCanBeAdded))
                return _ingredients as DataContainer<T>;
        }

        // 設著加入新飲料
        public void TryAddDrink(String name, int price)
        {
            GetContainerByType<Drink>().TryAddItem(new Drink(price, name));
        }

        // 設著加入新配料
        public List<Ingredient> TryAddIngredient(String name, int price)
        {
            GetContainerByType<IngredientWhichCanBeAdded>().TryAddItem(new IngredientWhichCanBeAdded(new Ingredient(price, name)));
            List<Ingredient> list = new List<Ingredient>();
            list.Add(new Ingredient(price, name));
            return list;
        }

        /*
        // IAdapter的實作
        public void ReceiveDataToApply(List<int> datas, Object otherData = null)
        {
            String forHistory = otherData as String;
            if(forHistory == "FOR_HISTORY")
            {
                List<OrderItem> data = datas[ 0 ] == EMPTY_INDEX ? null : _orders[ datas[ 0 ] ].GetOrder();
                if(data != null)
                {
                    _allAdapter.UpdateDetail(data);
                    _allAdapter.UpdateHistorySum(_orders[datas[0]].Price);
                }
            }
            else
            {
                List<IngredientWhichCanBeAdded> nowIngredient = datas[0] == EMPTY_INDEX ? null : _order[datas[0]].Ingredients;
                _allAdapter.UpdateIngredient(nowIngredient);
            }
        }
        */

        // 移除飲料
        public void RemoveDrinkByIndex(int index)
        {
            _drinks.RemoveAt(index);
        }

        // 移除配料
        public void RemoveIngredientByIndex(int index)
        {
            _ingredients.RemoveAt(index);
        }

        // ingredient的getter
        public DataContainer<IngredientWhichCanBeAdded> GetIngredient()
        {
            return _ingredients;
        }

        // 設定飲料名字
        public void SetDrinkName(int index, String name)
        {
            _drinks.GetItem(index).Title = name;
        }

        // 設定飲料價格
        public void SetDrinkPrice(int index, int price)
        {
            _drinks.GetItem(index).Price = price;
        }

        // 設定配料名字
        public void SetIngredientName(int index, String name)
        {
            _ingredients.GetItem(index).TheIngredient.Title = name;
        }

        // 設定配料價格
        public void SetIngredientPrice(int index, int price)
        {
            _ingredients.GetItem(index).TheIngredient.Price = price;
        }
    }
}