﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment_System
{
    public class Mosaic
    {
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        string name;
        string amount;
    }
}
