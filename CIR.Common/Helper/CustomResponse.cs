namespace CIR.Common.Helper
{
    public class CustomResponse<T>
    {
        public bool Result { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }

    }
}
