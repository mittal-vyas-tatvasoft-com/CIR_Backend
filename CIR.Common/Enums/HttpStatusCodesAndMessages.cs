using System.ComponentModel;

namespace CIR.Common.Enums
{
    public class HttpStatusCodesAndMessages
    {
        public enum HttpStatus
        {
            [Description("Success")]
            Success = 200,

            [Description("Data Created or Updated Successfully")]
            Saved = 201,

            [Description("Data Deleted Successfully")]
            Deleted = 201,

            [Description("No Content")]
            NoContent = 204,

            [Description("Bad Request, issue in client request")]
            BadRequest = 400,

            [Description("Unauthorized, invalid authentication credentials")]
            Unauthorized = 401,

            [Description("Forbidden, do not have access rights")]
            Forbidden = 403,

            [Description("NotFound, server cannot find the requested resource")]
            NotFound = 404,

            [Description("Method not allowed")]
            MethodNotAllowed = 405,

            [Description("The request was well-formed but was unable to be followed due to semantic errors")]
            UnprocessableEntity = 422,

            [Description("Internal server error")]
            InternalServerError = 500

        }
    }
}
