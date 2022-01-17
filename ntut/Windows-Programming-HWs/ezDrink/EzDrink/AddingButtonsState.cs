using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    // 用於加入的按鈕會有的狀態
    public enum AddingButtonsState
    {
        Initial,
        ClickAddFromTextBox,
        ClickAddFromFile,
        ClickOk,
        ClickCancel
    }
}
