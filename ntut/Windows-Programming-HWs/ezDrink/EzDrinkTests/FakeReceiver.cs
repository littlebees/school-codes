using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink.Tests
{
    class FakeReceiver : EzDrink.IAdapter<EzDrink.IngredientWhichCanBeAdded>
    {
        // 假的接收者
        public void ReceiveDataToApply(List<EzDrink.IngredientWhichCanBeAdded> datas, object otherData = null)
        {

        }
    }
}
