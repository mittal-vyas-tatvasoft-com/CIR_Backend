using CIR.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIR.Core.Entities;
using System.Drawing.Imaging;

namespace CIR.Data.Data
{
    public class ThumbnailRepository : IThumbnailRepository
    {
        public bool MakeThumbnailImage(ImageData imageData)
        {
            try
            {
                var memoryStream = new MemoryStream();
                imageData.ActualImage.CopyTo(memoryStream);

                var image = Image.FromStream(memoryStream);

                var thumbnailHeight = 1 * image.Height;
                var thumbnailWidth = 1 * image.Width;

                var thumbnailImage = image.GetThumbnailImage(thumbnailHeight, thumbnailWidth, () => false, IntPtr.Zero);

                string path = Path.Combine(@"C:\\Users\\tatva\\Source\\Repos\\mittalvyas1\\CIR_Backend\\CIR.Common\\Thumbnails\\", imageData.ActualImage.FileName);

                thumbnailImage.Save(path);

                if(thumbnailImage != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            { 
                return false;
            }
            

        }
    }
}
