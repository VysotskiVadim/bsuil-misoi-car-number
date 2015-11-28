using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface IImageProcessor
    {
        string Name { get; }

        void ProcessImage(IImage image);
    }
}
