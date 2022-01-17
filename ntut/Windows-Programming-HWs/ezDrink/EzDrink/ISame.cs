using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    public interface ISame<T>
    {
        // 兩物件是不是一樣
        bool IsSame(T item);
    }
}
