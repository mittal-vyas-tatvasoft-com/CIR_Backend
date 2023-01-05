using CIR.Core.Entities;
using CIR.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly IThumbnailRepository _thumbnailRepository;
        public ThumbnailService(IThumbnailRepository thumbnailRepository)
        {
            _thumbnailRepository= thumbnailRepository;
        }

        public bool MakeThumbnailImage(ImageData imageData)
        {
            var isThumbnailCreated = _thumbnailRepository.MakeThumbnailImage(imageData);
            return isThumbnailCreated;
        }
    }
}
