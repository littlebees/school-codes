using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    /*
     * Label的Adapter
     */
    public class LabelAdapter : IAdapter<int>
    {
        public delegate String ReceiveDataEventHandler(int sum);
        private ReceiveDataEventHandler _makeDataToString;
        private Label _view;

        public LabelAdapter(Label label, ReceiveDataEventHandler function)
        {
            _view = label;
            _makeDataToString = function;
        }

        // 實作IAdapter
        public void ReceiveDataToApply(List<int> datas, Object otherData = null)
        {
            _view.Text = _makeDataToString(datas[ 0 ]);
        }
    }
}
