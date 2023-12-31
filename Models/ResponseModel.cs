using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVMCrudDemo.Models
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public T data { get; set; }
        public int CODE { get; set; }
    }
}
