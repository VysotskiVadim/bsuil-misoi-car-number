using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public class SegmentsManager : ISegmentManager
    {
        private int _nextSegment;
        private int[,] _segmentMatrix;
        private IDictionary<int, Segment> _segments = new Dictionary<int, Segment>();

        public SegmentsManager(int xSize, int ySize)
        {
            _segmentMatrix = new int[xSize, ySize];
        }

        public int[,] BuildSegmentMatrix()
        {
            for(int x = 0; x < _segmentMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < _segmentMatrix.GetLength(1); y++)
                {
                    _segmentMatrix[x, y] = FindSegmentId(_segmentMatrix[x, y]);
                }
            }
            return _segmentMatrix;
        }

        public int GetSegmentIdFor(int x, int y)
        {
            var result = _segmentMatrix[x, y];
            return this.FindSegmentId(result);
        }

        public int MarkNewSegment(int x, int y)
        {
            int id = GetNextSegmentId();
            this.MarkSegment(x, y, id);
            return id;
        }

        public void MarkSegment(int x, int y, int segmentId)
        {
            _segmentMatrix[x, y] = segmentId;
        }

        public void MergeSegments(int firstSergmentId, int secondSegmentId)
        {
            firstSergmentId = FindSegmentId(firstSergmentId);
            secondSegmentId = FindSegmentId(secondSegmentId);
            if (firstSergmentId != secondSegmentId)
            {
                var firstSegment = _segments[firstSergmentId];
                var secondSegment = _segments[secondSegmentId];
                MergeSegments(firstSegment, secondSegment);
            }
        }

        private void MergeSegments(Segment firstSegment, Segment secondSegment)
        {
            if (firstSegment.Rank == secondSegment.Rank)
            {
                secondSegment.MergedWithSegmentId = firstSegment.Id;
                firstSegment.Rank++;
            }
            if (firstSegment.Rank < secondSegment.Rank)
            {
                firstSegment.MergedWithSegmentId = secondSegment.Id;
            }
            else
            {
                secondSegment.MergedWithSegmentId = firstSegment.Id;
            }
        }

        private int GetNextSegmentId()
        {
            var id = ++_nextSegment;
            _segments.Add(id, new Segment { Id = id });
            return id;
        }

        private int FindSegmentId(int id)
        {
            int result = id;
            if (result != 0)
            {
                var segment = _segments[result];
                while (segment.MergedWithSegmentId != 0)
                {
                    segment = _segments[segment.MergedWithSegmentId];
                }
                result = segment.Id;
            }
            return result;
        }
    }
}
