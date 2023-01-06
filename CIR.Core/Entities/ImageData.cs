using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities
{
    public class ImageData
    {
        //public string ImageName { get; set; }
        public IFormFile ActualImage { get; set; }
    }
}
