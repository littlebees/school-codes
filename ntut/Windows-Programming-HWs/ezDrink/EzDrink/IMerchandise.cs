using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    public interface IMerchandise
    {
        int Price
        {
            set;
            get;
        }

        String Title
        {
            set;
            get;
        }
    }
}
