using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Filtering;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class HoughLinesFindingAlgorithm : IFindAlgorithm
    {

        private int Angle { get; set; }
        Dictionary<int, int> AccumulatorPoints = new Dictionary<int, int>();
        Dictionary<int, int> Accumulator = new Dictionary<int, int>();
        

        private readonly IFilter _binarizationFilter;

        public HoughLinesFindingAlgorithm(IFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        public string Name => "Hough Lines Finding";

        


        public IEnumerable<IFindResult> Find(IImage image)
        {
            var equation = image.GetPixel(0, y) * Math.Cos(Angle) - image.GetPixel(x, 0) * Math.Sin(Angle);
            //Angle = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    image.GetPixel(i,j);

                }
            }


            _binarizationFilter.Filter(image);

            yield return new FindResult(10, 10, 50, 50);
        }

        public void RowGradient()
        {

        }

        public void ColumnGradient()
        {

        }
        /// <summary>
        /// Sorts the dot elements in array by coordinates 
        /// row -  if (f < 45) or (f > 135)
        /// column - if (45 <= f <= 135)
        /// </summary>
        private void Reader()
        {
            if (Angle < 45 || Angle > 135)
            {
                //Array smth sort by row
            }
            else if (Angle >= 45 || Angle < 135)
            {
                //Array sort by column
            }
        }


        public void CreateSegments()
        {

        }

        /// <summary>
        /// Returns the biggest Key of the Dictionary of Accumulator.
        /// </summary>
        /// <param name="accumulator"> Accumulator with elements</param>
        /// <param name="biggest"> Check if looking for biggest value</param>
        /// <returns>Key of the biggest element in the accumulator</returns>
        private int PickGreatestElementFromAccumulator(Dictionary<int,int> accumulator, bool biggest = true)
        {
            if (biggest)
            {
                if (accumulator.Count == 0)
                {
                    throw new InvalidOperationException("Accumulator is empty, nothing to return");
                }

                return accumulator.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            }
            else
            {
                return -42;
            }
        }


        /// <summary>
        /// Returns angle of value by components of the following quadrant
        /// </summary>
        /// <param name="quadrant">Qaudrant from where to return angle</param>
        /// <returns>Angle by components</returns>
        private int Atan2(int quadrant)
        {
            int result = 0;
            return result;
        }
    }
}
