using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface IPixel
    {
		byte R { get; }
		byte G { get; }
		byte B { get; }
    }
}
