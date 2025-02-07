using DrawingEditor.Core.Models;
using System.Drawing;

namespace DrawingEditor.Core.Algorithms.SegmentAlgorithms.Interfaces;

public interface ILineDrawingAlgorithm
{
    Line DrawLine(Point start, Point end);
}
