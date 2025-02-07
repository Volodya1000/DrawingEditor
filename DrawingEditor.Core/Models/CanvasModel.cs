using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class CanvasModel: IDrwaingGraphicObject
{
    private  readonly List<IDrwaingGraphicObject> _objects = new ();

    public void AddObject(IDrwaingGraphicObject obj) => _objects.Add(obj);

    public void RemoveObject(IDrwaingGraphicObject obj) => _objects.Remove(obj);

    public IEnumerable<IDrwaingGraphicObject> GetObjects() => _objects;

    public IEnumerable<Point> GetPoints() => _objects.SelectMany(x => x.GetPoints());
}
