namespace Attender.Server.Application.Common.Models
{
    public static class Result
    {
        public static Result<T> Success<T>(T data)
        {
            return new(true, data, default);
        }

        public static Result<T> Failure<T>(string errorType, string errorMessage)
        {
            return Failure<T>(new Error(errorType, errorMessage));
        }

        public static Result<T> Failure<T>(Error error)
        {
            return new(false, default, error);
        }
    }

    public class Result<T>
    {
        internal Result(bool succeeded, T? data, Error? error)
        {
            Succeeded = succeeded;
            Data = data;
            Error = error;
        }

        public bool Succeeded { get; }
        public T? Data { get; }
        public Error? Error { get; }
    }
}
