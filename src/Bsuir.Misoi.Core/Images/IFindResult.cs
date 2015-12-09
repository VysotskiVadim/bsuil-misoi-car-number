﻿using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindResult
    {
        IList<Point> Points { get; }

        float Angle { get; }
    }
}
