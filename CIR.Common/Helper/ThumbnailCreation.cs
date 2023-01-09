using CIR.Common.CommonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace CIR.Common.Helper
{
    public class ThumbnailCreation
    {
        private readonly ThumbnailModel _thumbnailModel;
        public ThumbnailCreation(IOptions<ThumbnailModel> thumbnailModel)
        {
            _thumbnailModel = thumbnailModel.Value;
        }

        public void CreateThumbnailImage(IFormFile actualImage)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                actualImage.CopyTo(memoryStream);

                using (Image image = Image.FromStream(memoryStream))
                {
                    int thumbnailImageHeight = Convert.ToInt32(_thumbnailModel.ScalingFactor * image.Height);
                    int thumbnailImageWidth = Convert.ToInt32(_thumbnailModel.ScalingFactor * image.Width);

                    var thumbnailImage = image.GetThumbnailImage(thumbnailImageHeight, thumbnailImageWidth, () => false, IntPtr.Zero);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
