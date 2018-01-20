using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacaMvc.Models
{
    public class OrderViewFrameModel
    {
        public string Address { get; private set; } = string.Empty;

        public OrderViewFrameModel(string address)
        {
            this.Address = address;
        }
    }
}
