using System;
using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class FindResultDrawer : IFindResultDrawer
    {
        public void DrawFindResults(IImage image, IEnumerable<IFindResult> results)
        {
            foreach (var selectedArea in results)
            {
                if (selectedArea.Points.Count >= 2)
                {
                    var rand = new Random();
                    var pixel = new Pixel
                    {
                        R = (byte) rand.Next(255),
                        G = (byte) rand.Next(255),
                        B = (byte) rand.Next(255)
                    };
                    DrawLineAlgorithm.PlotFunction plotFunction = (x, y) =>
                    {
                        
                        if (x < image.Width && y < image.Height && x >= 0 && y >= 0)
                        {
                            image.SetPixel(x, y, pixel);
                        }
                        return true;
                    };
                    var firstPoint = selectedArea.Points[0];
                    var previousPoint = firstPoint;
                    for (int i = 1; i < selectedArea.Points.Count; i++)
                    {
                        var currentPoint = selectedArea.Points[i];
                        DrawLineAlgorithm.Line(previousPoint.X, previousPoint.Y, currentPoint.X, currentPoint.Y, plotFunction);
                        previousPoint = currentPoint;
                    }
                    if (selectedArea.Points.Count > 2)
                    {
                        var lastPoint = selectedArea.Points[selectedArea.Points.Count - 1];
                        DrawLineAlgorithm.Line(lastPoint.X, lastPoint.Y, firstPoint.X, firstPoint.Y, plotFunction);
                    }
                }
            }
        }
    }
}
