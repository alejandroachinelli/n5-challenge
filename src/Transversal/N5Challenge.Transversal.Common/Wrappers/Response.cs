using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Transversal.Common.Wrappers;

public class Response<T>
{
    public Response() { }

    public Response(T data, string message = null)
    {
        Data = data;
        Message = message;
        IsSuccess = true;
    }

    public T Data { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = [];
}