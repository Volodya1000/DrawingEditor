namespace DrawingEditor.WinForms;

public class LineManager
{
    private List<(Point, Point)> lines = new List<(Point, Point)>();

    public void AddLine(Point start, Point end)
    {
        lines.Add((start, end));
    }

    public IEnumerable<(Point, Point)> GetLines() => lines;
}
