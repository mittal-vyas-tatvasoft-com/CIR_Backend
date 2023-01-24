using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Common
{
    public interface ICsvService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}
