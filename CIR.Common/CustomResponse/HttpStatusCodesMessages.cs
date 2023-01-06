namespace CIR.Common.CustomResponse
{
	public class HttpStatusCodesMessages
	{
		public const string Success = "Success";

		public const string CreatedOrUpdated = "Data Created or Updated Successfully";

		public const string NoContent = "No Content";

		public const string BadRequest = "Bad Request, issue in client request";

		public const string Unauthorized = "Unauthorized, invalid authentication credentials";

		public const string Forbidden = "Forbidden, do not have access rights";

		public const string NotFound = "NotFound, server cannot find the requested resource";

		public const string MethodNotAllowed = "Method not allowed";

		public const string UnprocessableEntity = "The request was well-formed but was unable to be followed due to semantic errors";

		public const string InternalServerError = "Internal server error";
	}
}
