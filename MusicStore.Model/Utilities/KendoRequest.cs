using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Models.Utilities
{
    public class KendoRequest
    {
        public int take { get; set; }
        public int skip { get; set; }
        public int page { get; set; }

        public int pageSize { get; set; }
        public List<KendoSort> sort { get; set; }

        public KendoFilters filter { get; set; }
    }

    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }

    public class KendoFilter
    {
        public string Operator { get; set; }
        public string field { get; set; }
        public string value { get; set; }
        //public bool ignoreCase { get; set; }
    }

    public class KendoFilters
    {
        public List<KendoFilter> filters { get; set; }
        public string logic { get; set; }
    }
}
