using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.WebUI.Storage.Implementation
{
    public class ImageUrlProvider : IImageUrlProvider
    {
        public string GetImageUrl(string imageName)
        {
            return $"/image/{imageName}";
        }
    }
}
