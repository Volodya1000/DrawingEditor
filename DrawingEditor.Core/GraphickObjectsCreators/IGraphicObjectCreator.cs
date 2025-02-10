using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core.GraphickObjectsCreators;

public interface IGraphicObjectCreator
{
    int GetRequiredPointsCount();
    IDrwaingGraphicObject? CreateGraphicObject(Color color,IEnumerable<Point> points);
}