using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardSaveFixer
{
    class SelectionClass
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SelectionClass(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
