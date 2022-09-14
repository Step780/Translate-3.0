using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate3._0
{
    public class History
    {
        public History()
        {
        }

        public History(string to, string from)
        {
            To = to;
            From = from;
        }

        public string To { get; set; }
        public string From { get; set; }


    }
}
