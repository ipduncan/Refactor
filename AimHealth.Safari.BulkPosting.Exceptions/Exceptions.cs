using System;

namespace AimHealth.Safari.BulkPosting.Exceptions
{
    public class ExceptionHandler
    {
        public static void ThrowException(Exception exception, string message, params object[] list)
        {
            string textToSend = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    string placeHolder = "{" + i + "}";
                    textToSend = message.Replace(placeHolder, list[i].ToString());
                }
            }
            throw new Exception(textToSend);
        }

        public static void ThrowException(Exception exception, Exception innerException, string message, params object[] list)
        {
            string textToSend = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    string placeHolder = "{" + i + "}";
                    textToSend = message.Replace(placeHolder, list[i].ToString());
                }
            }
            throw new Exception(textToSend, innerException);

        }

    }

    public class InvalidPaymentPropertyException : ApplicationException
    {
        public InvalidPaymentPropertyException(string message) : base(message)
        {}
        public InvalidPaymentPropertyException(string message, Exception innerException) : base(message, innerException)
        {}
        public InvalidPaymentPropertyException() : base()
        {}
    }

    public class BatchNotCreatedException : ApplicationException
    {
        public BatchNotCreatedException(string message) : base(message)
        {}
        public BatchNotCreatedException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }

    public class BatchNotFoundException : ApplicationException
    {
        public BatchNotFoundException(string message): base(message)
        {}

        public BatchNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }

    public class InvalidDataInsurerInformationException : ApplicationException
    {
        public InvalidDataInsurerInformationException(string message) :
            base(message)
        {}
        public InvalidDataInsurerInformationException(string message, Exception innerException) :
            base(message, innerException)
        {}
    }

    public class UserInformationInvalidException : ApplicationException
    {
        public UserInformationInvalidException(string message) :
            base(message)
        {}
        public UserInformationInvalidException(string message, Exception innerException) :
            base(message, innerException)
        {}
    }

    public class UserNotAuthorizedException : ApplicationException
    {
        public UserNotAuthorizedException(string message) :
            base(message)
        {}
        public UserNotAuthorizedException(string message, Exception innerException) :
            base(message, innerException)
        {}
    }

}
