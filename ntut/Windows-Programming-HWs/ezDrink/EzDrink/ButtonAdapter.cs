using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    /*
     * button的adapter
     */
    public class ButtonAdapter : IAdapter<bool>
    {
        private Button _view;

        public ButtonAdapter(Button button)
        {
            _view = button;
        }

        // 實作IAdapter
        public void ReceiveDataToApply(List<bool> datas, Object otherData = null)
        {
            _view.Enabled = datas[ 0 ];
        }
    }
}
