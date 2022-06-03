using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models
{
    public class Paginate
    {
        public int Page { get; set; }
        public int Limit { get; set; }

        public Paginate()
        {}

        public Paginate(int page, int limit)
        {
            Page = page;
            Limit = limit;
        }
    }
}
