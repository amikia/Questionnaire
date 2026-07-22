namespace Questionnaire.SharedKernel.Exceptions;

public class PaymentException : Exception
{
    public PaymentException() : base() { }
    public PaymentException(string message) : base(message) { }
}