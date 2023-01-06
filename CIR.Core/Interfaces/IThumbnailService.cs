using CIR.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces
{
    public interface IThumbnailService
    {
        bool MakeThumbnailImage(ImageData imageData);
    }
}
