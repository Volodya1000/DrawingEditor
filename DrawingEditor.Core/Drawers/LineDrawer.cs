using DrawingEditor.Core.Algorithms.SegmentAlgorithms.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class LineDrawer
{
    private ILineDrawingAlgorithm _algorithm;

    public LineDrawer(ILineDrawingAlgorithm algorithm)
    {
        _algorithm = algorithm;
    }

    public void SetAlgorithm(ILineDrawingAlgorithm algorithm)
    {
        _algorithm = algorithm;
    }

    public Line DrawLine(Point start, Point end)
    {
        if (_algorithm == null)
            throw new InvalidOperationException("Алгоритм не установлен.");

        return _algorithm.DrawLine(start, end); // Теперь возвращаем Line
    }
}
