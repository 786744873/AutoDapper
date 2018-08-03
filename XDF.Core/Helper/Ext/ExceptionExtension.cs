using System;

namespace System
{
        public static class ExceptionExtension
        {
            public static Exception GetInnestException(this Exception ex)
            {
                Exception innerException = ex.InnerException;
                Exception exception = ex;
                for (; innerException != null; innerException = innerException.InnerException)
                    exception = innerException;
                return exception;
            }

        }
}