using DrawingEditor.Core.History;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;


// <<Memento>>
public class CanvasModel
{
    private DrawingState currentState;
    private DrawingHistory history = new DrawingHistory();

    public CanvasModel()
    {
        currentState = new DrawingState(new List<IDrawingGraphicObject>());
    }

    public void AddObject(IDrawingGraphicObject obj)
    {
        // Сохраняем текущее состояние перед изменением
        history.SaveState(new DrawingState(new List<IDrawingGraphicObject>(currentState.GraphicObjects)));
        // Применяем изменение: создаём новое состояние с добавленным объектом
        List<IDrawingGraphicObject> newObjects = new List<IDrawingGraphicObject>(currentState.GraphicObjects)
        {
            obj
        };
        currentState = new DrawingState(newObjects);
    }

    public void RemoveObject(IDrawingGraphicObject obj)
    {
        history.SaveState(new DrawingState(new List<IDrawingGraphicObject>(currentState.GraphicObjects)));
        List<IDrawingGraphicObject> newObjects = new List<IDrawingGraphicObject>(currentState.GraphicObjects);
        newObjects.Remove(obj);
        currentState = new DrawingState(newObjects);
    }

    public IEnumerable<Point> GetPoints() =>
        currentState.GraphicObjects.SelectMany(x => x.GetPoints());

    public IEnumerable<IDrawingGraphicObject> GetGraphicObjects() =>
        currentState.GraphicObjects;

    public bool Undo()
    {
        var previousState = history.Undo(currentState);
        if (previousState != null)
        {
            currentState = previousState;
            return true;
        }
        return false;
    }

    public bool Redo()
    {
        var nextState = history.Redo(currentState);
        if (nextState != null)
        {
            currentState = nextState;
            return true;
        }
        return false;
    }
}