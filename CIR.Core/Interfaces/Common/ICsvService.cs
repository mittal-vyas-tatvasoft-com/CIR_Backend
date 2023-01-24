namespace CIR.Core.Interfaces.Common
{
    public interface ICsvService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}
