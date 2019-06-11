using System;
using System.Runtime.Serialization;

[Serializable]
internal class PilhaVaziaException : Exception
{
    public PilhaVaziaException()
    {
    }

    public PilhaVaziaException(string message) : base(message)
    {
    }

    public PilhaVaziaException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PilhaVaziaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}