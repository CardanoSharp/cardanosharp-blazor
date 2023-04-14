namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class DeserializationException : Exception
    {
        public string? SerializedData { get; set; }

        public DeserializationException()
        {
        }

        public DeserializationException(string message)
            : base(message)
        {
        }

        public DeserializationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DeserializationException(string serializedData, string message, Exception inner)
            : base(message, inner)
        {
            SerializedData = serializedData;
        }
    }
}