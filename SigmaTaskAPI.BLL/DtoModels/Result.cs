using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTaskAPI.BLL.DtoModels
{
    public class Result
    {
        public Result()
        {
            Errors = new();
        }
        public bool Succeeded { get; set; }
        public string Note { get; set; }
        public List<Error> Errors { get; set; }
    }
    public class Result<T> : Result
    {
        public T Data { get; set; }

    }
    public class Error
    {
        public Error()
        {

        }
        public string Message { get; set; }
        public string Code { get; set; }
    }

}
