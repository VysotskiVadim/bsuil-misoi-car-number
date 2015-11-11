using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public interface ISegment
    {
        int Id { get; }

        int Square { get; }
    }
}
