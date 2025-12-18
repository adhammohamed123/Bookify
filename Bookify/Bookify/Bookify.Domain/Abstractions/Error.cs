namespace Bookify.Domain.Abstractions
{
    public record Error(string Code, string Message)
    {
        public static Error NoError => new("Error.NoError", "this is Sucess Result that have no Error");
        public static Error NullError => new("Error.NullValue", "Null Value was Provided");
    }
}
