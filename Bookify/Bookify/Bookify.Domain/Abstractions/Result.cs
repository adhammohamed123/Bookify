using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Bookify.Domain.Abstractions
{
    public record Result
    {
        [JsonConstructor]
        protected internal Result(bool isSuccess, Error error)
        {
            if(isSuccess && error != Error.NoError)
                throw new InvalidOperationException("A successful result cannot have an error.");
            if(!isSuccess && error == Error.NoError)
                throw new InvalidOperationException("A failure result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get;}
        public Error Error { get; }
        public bool IsFaliure => !IsSuccess;

        public static Result Success() => new (true, Error.NoError);
        public static Result Failure(Error error) => new (false, error);

        public static Result<T> Success<T>(T value) => new (value, true, Error.NoError);
        public static Result<T> Failure<T>(Error error) => new (default, false, error);

        public static Result<T> Create<T>(T value)
            => value is null ? Failure<T>(Error.NullError) : Success(value);

    }

    public record Result<T> : Result
    {
        private readonly T? _value;
        [JsonConstructor]
        protected internal Result(T? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public T Value => IsSuccess ? _value! : throw new InvalidOperationException("can not access value of faliure Result");

        public static implicit operator Result<T>(T value) => Create(value);
        public static implicit operator T(Result<T> result) => result.Value;
    }
}
