using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    internal class IdentifyResult : ICanNumberResult
    {
        public IImage ProcessedImage { get; set; }
    }
}
