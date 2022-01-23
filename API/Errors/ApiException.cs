using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException
    {
        
        public ApiException(int statusCode, string message = null, string details = null) 
        {
            StatusCode = statusCode;
            Details = details;
            Messgae =message;
        }
        public int StatusCode { get; set; }
        public string Messgae { get; set; }
        public string Details { get; set; }
    }
}