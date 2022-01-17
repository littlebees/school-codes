using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    /*
     * 就是開一洞給資料提供的物件，來給資料以完成更新。
     * 通常是用在model產生資料變動要通知view的時候，
     * 但還有一個用法是在Order的DGV的select改變要通知負責Topping的View要換成該OrderItem的Topping List時，
     * 因為只有DGV知道index是什麼所以由Form來通知model抓出改orderItem來做相對應的更新
     */
    public interface IAdapter<T>
    {
        // 塞資料的洞，資料用List存放因為可能有很多筆
        // otherData是給要放其他不同於T的資料用的，很像JS的arguments
        void ReceiveDataToApply(List<T> datas, Object otherData = null);
    }
}
