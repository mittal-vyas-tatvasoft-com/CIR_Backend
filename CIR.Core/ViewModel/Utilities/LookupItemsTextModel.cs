using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CIR.Core.ViewModel.Utilities
{
    public class LookupItemsTextModel
    {
        public long Id { get; set; }
        public long SystemCodeId { get; set; }
        public long LookupItemId { get; set; }
        public long CultureId { get; set; }
        public int DisplayOrderId { get; set; }
        public string Text { get; set; } = null!;
        public bool Active { get; set; }
        public string Code { get; set; } = null!;

    }
}
