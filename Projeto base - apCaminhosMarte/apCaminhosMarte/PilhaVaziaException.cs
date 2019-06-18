using System;
using System.Runtime.Serialization;

//Ana Clara Sampaio Pires RA: 18201
//Ariane Paula Barros     RA: 18173
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