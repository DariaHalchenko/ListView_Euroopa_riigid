using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListView_Euroopa_riigid
{
    public class Telefon
    {
        public string Nimetus { get; set; }
        public string Tootja { get; set; }
        public int Hind { get; set; }
        public string Pilt { get; set; }
    }
    public class Ruhm<K, T> : ObservableCollection<T>
    {
        public K Nimetus { get; private set; }
        public Ruhm(K nimetus, IEnumerable<T> items)
        {
            Nimetus = nimetus;
            foreach (T item in items)
                Items.Add(item);
        }
    }
    public class Euroopa
    {
        public string Nimetus { get; set; }
        public string Pealinn { get; set; }
        public int Rahvastiku_suurus { get; set; }
        public string Lipp { get; set; }
        public string Info { get; set; }
        public string Keel { get; set; }
    }
    public class EuroopaRuhm<K, T> : ObservableCollection<T> where T : Euroopa
    {
        public K Nimetus { get; private set; }
        public EuroopaRuhm(K nimetus, IEnumerable<T> items)
        {
            Nimetus = nimetus;
            foreach (T item in items.OrderBy(i => i.Nimetus))
            {
                Items.Add(item);
            }
        }
    }
}