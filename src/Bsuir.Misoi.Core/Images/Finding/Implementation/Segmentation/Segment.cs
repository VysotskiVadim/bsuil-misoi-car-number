using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    internal class Segment : ISegment
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public int MergedWithSegmentId { get; set; }

        public int Square { get; set; }
    }
}
