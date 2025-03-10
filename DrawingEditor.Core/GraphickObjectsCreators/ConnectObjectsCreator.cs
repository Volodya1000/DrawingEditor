using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;
namespace DrawingEditor.Core;

public class ConnectObjectsCreator: IGraphicObjectCreator
{
    public int GetRequiredPointsCount() => int.MaxValue;

    public IDrawingGraphicObject? CreateGraphicObject(Color color, int lineThickness, IEnumerable<Point> points)
    {
        return null;
    }
}
