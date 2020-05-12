using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator
{
    class ComboItem<T>
    {
        public T Item { get; set; } = default(T);

        public ComboItem(T item)
        {
            Item = item;
        }

        public override string ToString()
        {
            if (this.Item is String || this.Item is int) return Item.ToString();
            return Item.GetType().GetProperties()[0].GetValue(this.Item).ToString();
        }
    }
}
