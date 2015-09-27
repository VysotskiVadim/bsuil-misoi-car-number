using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface IImageFactory
    {
		IImage CreateImage(string name, Stream data);
    }
}
