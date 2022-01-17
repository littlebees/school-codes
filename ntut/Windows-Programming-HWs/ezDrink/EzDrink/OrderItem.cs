using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    /*
     * Order的項目的資料型別
     */
    public class OrderItem : IAdapter<Ingredient>, IAdapter<IngredientWhichCanBeAdded>
    {
        private Drink _drink;
        private Sweet _sweet;
        private Ice _ice;
        private List<IngredientWhichCanBeAdded> _ingredients;
        private IAdapter<IngredientWhichCanBeAdded> _ingredientAdapter;

        // 配料的表從model取得，讓每筆項目維護自己有加的配料
        public OrderItem(Drink drink, List<IngredientWhichCanBeAdded> ingredients)
        {
            _drink = drink;
            _ingredients = ingredients.Select(item => (IngredientWhichCanBeAdded)item.Clone()).ToList();
            _sweet = Sweet.Empty;
            _ice = Ice.Empty;
        }

        // 點的飲料叫什麼名字
        public String DrinkName
        {
            get
            {
                return _drink.Title;
            }
        }

        // 甜度是多少
        public Sweet Sugar
        {
            set
            {
                _sweet = value;
            }
        }

        // 多冰
        public Ice Cold
        {
            set
            {
                _ice = value;
            }
        }

        // 這筆項目的價格是
        public int Price
        {
            get
            {
                int ans = _drink.Price;
                foreach (IngredientWhichCanBeAdded top in _ingredients)
                    if (top.IsAdded)
                        ans += top.Top.Price;
                return ans;
            }
        }

        // 取得加入的topping的表(給UpdateTopping用)
        public List<IngredientWhichCanBeAdded> Ingredients
        {
            get
            {
                return _ingredients;
            }
        }

        // 所有有加的配料 in String
        public String Ingredient
        {
            get
            {
                List<String> ans = new List<string>();
                foreach (IngredientWhichCanBeAdded top in _ingredients)
                    if (top.IsAdded)
                        ans.Add(top.Top.Title);
                const String separate = "、";
                return String.Join(separate, ans);
            }
        }

        // 甜度 in String
        public String SweetLevel
        {
            get
            {
                const String NORMAL = "正常";
                const String HALF = "半糖";
                const String FEW = "微糖";
                const String NO = "無糖";
                switch (_sweet)
                {
                    case Sweet.Normal:
                        return NORMAL;
                    case Sweet.Half:
                        return HALF;
                    case Sweet.Few:
                        return FEW;
                    case Sweet.No:
                        return NO;
                    default:
                        return "";
                }
            }
        }

        // 溫度 in String
        public String Temperature
        {
            get
            {
                const String NORMAL = "正常";
                const String HALF = "溫熱";
                const String FEW = "少冰";
                const String NO = "去冰";
                switch (_ice)
                {
                    case (Ice.Normal):
                        return NORMAL;
                    case (Ice.Half):
                        return HALF;
                    case (Ice.Few):
                        return FEW;
                    case (Ice.No):
                        return NO;
                    default:
                        return "";
                }
            }
        }

        // 這筆訂單的描述
        // 0 是 名字
        // 1 是 價格
        // 2 是 甜度
        // 3 是 溫度
        // 4 是 有加的配料
        public List<String> OrderDescription
        {
            get
            {
                List<String> ans = new List<string>();
                OrderItem order = this;
                ans.Add(order.DrinkName);
                ans.Add(order.Price.ToString());
                ans.Add(order.SweetLevel);
                ans.Add(order.Temperature);
                ans.Add(order.Ingredient);
                return ans;
            }
        }

        // 有設定溫度與甜度嗎
        public bool IsOK
        {
            get
            {
                return _sweet != Sweet.Empty && _ice != Ice.Empty;
            }
        }

        // 設定adapter
        public IAdapter<IngredientWhichCanBeAdded> IngredientAdapter
        {
            set
            {
                _ingredientAdapter = value;
                _ingredientAdapter.ReceiveDataToApply(_ingredients);
            }
        }

        // 加入配料
        public void AddIngredient(Ingredient top)
        {
            for (int i = 0; i < _ingredients.Count; i++)
                if (top.IsSame(_ingredients[i].Top))
                {
                    _ingredients[i].IsAdded = true;
                    break;
                }
        }

        // 當有新的配料誕生時
        public void ReceiveDataToApply(List<Ingredient> datas, object otherData = null)
        {
            foreach (Ingredient item in datas)
                _ingredients.Add(new IngredientWhichCanBeAdded(item));
            _ingredientAdapter.ReceiveDataToApply(_ingredients);
        }

        // 當有配料被刪掉時要做的處理
        public void ReceiveDataToApply(List<IngredientWhichCanBeAdded> datas, object otherData = null)
        {
            List<int> checkTable = new List<int>();
            int index = 0;
            foreach (var ing in _ingredients)
            {
                bool inside = false;
                foreach (var data in datas)
                    if (ing.TheIngredient.IsSame(data.TheIngredient))
                    {
                        inside = true;
                        break;
                    }
                if (!inside)
                    checkTable.Add(index);
                index++;
            }
            foreach (var theIndex in checkTable)
                _ingredients.RemoveAt(theIndex);
        }
    }
}
