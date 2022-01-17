using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    /*
     * 配料的資料型別
     */
    public class Ingredient : ICloneable, ISame<Ingredient> , IMerchandise
    {
        private int _price;
        private String _title;

        public Ingredient(int price, String title)
        {
            _price = price;
            _title = title;
        }

        // 價錢
        public int Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
            }
        }

        // 名字
        public String Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        // 判斷兩個topping是否相等
        public bool IsSame(Ingredient top)
        {
            //return top.Title == Title && top.Price == Price;
            return top.Title == Title;
        }

        // 實作 ICloneable
        public Object Clone()
        {
            //return new Ingredient(Price, Title);
            return this;
        }
    }
}
