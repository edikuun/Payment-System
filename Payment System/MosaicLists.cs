using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment_System
{
    public class MosaicLists
    {
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public long Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        string name;
        string amount;
        string nameId;
        long quantity;
    }
}
