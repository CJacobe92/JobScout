using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.App.Common
{
    public class CommandOrQueryResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Messages { get; set; } = [];
    }
}
