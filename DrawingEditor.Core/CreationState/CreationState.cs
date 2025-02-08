using System.Drawing;

namespace DrawingEditor.Core;

public class CreationState : ICreationState
{
    private readonly int requiredPoints;
    private readonly List<Point> points;

    public CreationState(int requiredPoints)
    {
        this.requiredPoints = requiredPoints;
        points = new List<Point>();
    }

    public bool IsReadyToCreate() => points.Count >= requiredPoints;

    public void AddPoint(Point point)
    {
        if (points.Count < requiredPoints)
        {
            points.Add(point);
        }
    }

    public List<Point> GetPoints() => points.ToList();
}