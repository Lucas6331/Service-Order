using System.Net;

namespace OsDsII.api.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) :
            base
                (
                "HSO-003", 
                message,
                HttpStatusCode.BadRequest,
                StatusCodes.Status400BadRequest,
                null,
                DateTimeOffset.UtcNow,
                null
            )
        { }
    }
}
