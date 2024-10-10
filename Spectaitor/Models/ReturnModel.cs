using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectaitor.Models
{
    public class ReturnModel<T>
    {
        public bool IsSucceeded { get; set; }
        public string Comment { get; set; }
        public T Value { get; set; }
    }
}
