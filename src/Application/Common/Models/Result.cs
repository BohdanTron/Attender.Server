using System;
using System.Collections.Generic;
using System.Linq;

namespace Attender.Server.Application.Common.Models
{
    public static class Result
    {
        public static Result<T> Succeeded<T>(T data)
        {
            return new(true, data, Array.Empty<string>());
        }

        public static Result<T> Failure<T>(string error)
        {
            return Failure<T>(new[] { error });
        }

        public static Result<T> Failure<T>(IEnumerable<string> errors)
        {
            return new(false, default, errors);
        }
    }

    public class Result<T>
    {
        internal Result(bool succeeded, T? data, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Data = data;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; }
        public T? Data { get; }
        public string[] Errors { get; }
    }
}
