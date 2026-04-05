using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherHub.Application.Common.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data) =>
            new() { Success = true, Data = data };

        public static ApiResponse<T> Fail(IEnumerable<string> errors) =>
            new() { Success = false, Errors = errors };
    }
}
