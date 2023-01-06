namespace CIR.Common.CustomResponse
{
    public enum HttpStatusCodes
    {
        Success = 200,

        CreatedOrUpdated = 201,

        NoContent = 204,

        BadRequest = 400,

        Unauthorized = 401,

        Forbidden = 403,

        NotFound = 404,

        MethodNotAllowed = 405,

        UnprocessableEntity = 422,

        InternalServerError = 500
    }
}
