using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    public class Order
    {
        private List<OrderItem> _list = new List<OrderItem>();
        private DateTime _time;

        public Order(List<OrderItem> order, DateTime now)
        {
            _list = order;
            _time = now;
        }
        /*
        public void SetTime(DateTime now)
        {
            _time = now;
        }

        public void AddItem(OrderItem item)
        {
            _list.Add(item);
        }

        public void SetOrder(List<OrderItem> order)
        {
            _list = order;
        }

        public OrderItem GetItem(int index)
        {
            return _list[ index ];
        }

        public void SetItem(int index, OrderItem item)
        {
            _list[ index ] = item;
        }
        */

        // 時間的getter
        public DateTime Time
        {
            get
            {
                return _time;
            }
        }

        // order的getter
        public List<OrderItem> GetOrder()
        {
            return _list;
        }

        // 加order進來
        public void AddOrderItem(OrderItem item)
        {
            _list.Add(item);
        }

        //price的getter
        public int Price
        {
            get
            {
                int ans = 0;
                foreach (var order in _list)
                    ans += order.Price;
                return ans;
            }
        }
    }
}
