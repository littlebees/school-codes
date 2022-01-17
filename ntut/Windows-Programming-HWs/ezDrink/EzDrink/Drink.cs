using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    /*
     * 飲料的資料型別
     */
    public class Drink : ISame<Drink>, IMerchandise
    {
        private int _price;
        private String _title;

        public Drink(int price, String title)
        {
            _price = price;
            _title = title;
        }

        // 飲料價格
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

        // 飲料名字
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

        // 兩個Drink是不是一樣
        public bool IsSame(Drink drink)
        {
            //return _price == drink.Price && _title.Equals(drink.Title);
            return _title.Equals(drink.Title);
        }
    }
}
