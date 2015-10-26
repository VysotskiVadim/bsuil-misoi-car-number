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
        private int[,] AccumulatorPoints;
        private int[,] Accumulator;

        /// <summary>
        /// Discrete distance from the line to 0,0
        /// </summary>
        private int DiscreteDistance { get; set; }

        /// <summary>
        /// Discrete Angle between lines and perpendicular to line from the 0,0
        /// </summary>
        private int ThetaAngle { get; set; }

        private int[] Distance { get; set; }

        private int[] Theta { get; set; }

        private readonly IFilter _binarizationFilter;

        public HoughLinesFindingAlgorithm(IFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        public string Name => "Hough Lines Finding";

        public int[,] GradientThreshold { get; private set; }

        public IEnumerable<IFindResult> Find(IImage image)
        {
            

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {

                    image.GetPixel(i, j);
                    if (GetGradient(image.R, image.G, image.B) > GradientThreshold)
                    {
                        Theta = Atan2(GetRowGradient(image.R, image.G, image.B), GetColumnGradient(image.R, image.G, image.B));
                        ThetaAngle = QuantizeAngle(Theta);
                        Distance = CalculateEquation(image, ThetaAngle);
                        DiscreteDistance = QuantizeDistance(DiscreteDistance);
                        Accumulator[DiscreteDistance, ThetaAngle] = Accumulator[DiscreteDistance, ThetaAngle] + GetGradient(image.R, image.G, image.B);
                        //PTList(DiscreteDistance, ThetaAngle) = append(PTList(DW, ThetaAngle, [image.GetPixel(0,y), image.GetPixel(x,0)]); ебал я в рот что это вообще нахуй

                    }

                }
            }


            _binarizationFilter.Filter(image);

            yield return new FindResult(10, 10, 50, 50);
        }

        private int QuantizeDistance(int discreteDistance)
        {
            throw new NotImplementedException();
        }

        private int[] CalculateEquation(IImage image, int thetaAngle)
        {
            return image.GetPixel(0, y) * Math.Cos(thetaAngle) - image.GetPixel(x, 0) * Math.Sin(thetaAngle);
        }

        private int QuantizeAngle(int[] theta)
        {
            throw new NotImplementedException();
        }

        private int GetRowGradient(int r, int g, int b)
        {
            throw new NotImplementedException();
            //return 0;
        }

        private int GetColumnGradient(int r, int g, int b)
        {
            throw new NotImplementedException();

            //return 0;
        }

        private int[,] GetGradient(int r, int g, int b)
        {
            int rowGradient = GetRowGradient(r, g, b);
            int columnGradient = GetColumnGradient(r, g, b);
            int[,] result = { { rowGradient }, { columnGradient } };
            return result;
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
        private int PickGreatestElementFromAccumulator(Dictionary<int, int> accumulator, bool biggest = true)
        {
            if (biggest)
            {
                if (accumulator.Count == 0)
                {
                    throw new InvalidOperationException("Accumulator is empty, nothing to return");
                }
                //DiscreteDistance = accumulator
                //ThetaAngle = 
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
