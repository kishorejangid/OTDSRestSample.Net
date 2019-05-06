using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTDSRestSample
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public string ErrorDetails { get; set; }
    }
}
