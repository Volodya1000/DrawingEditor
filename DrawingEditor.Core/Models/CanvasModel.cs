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
        currentState = new DrawingState(new List<IDrwaingGraphicObject>());
    }

    public void AddObject(IDrwaingGraphicObject obj)
    {
        // Сохраняем текущее состояние перед изменением
        history.SaveState(new DrawingState(new List<IDrwaingGraphicObject>(currentState.GraphicObjects)));
        // Применяем изменение: создаём новое состояние с добавленным объектом
        List<IDrwaingGraphicObject> newObjects = new List<IDrwaingGraphicObject>(currentState.GraphicObjects)
        {
            obj
        };
        currentState = new DrawingState(newObjects);
    }

    public void RemoveObject(IDrwaingGraphicObject obj)
    {
        history.SaveState(new DrawingState(new List<IDrwaingGraphicObject>(currentState.GraphicObjects)));
        List<IDrwaingGraphicObject> newObjects = new List<IDrwaingGraphicObject>(currentState.GraphicObjects);
        newObjects.Remove(obj);
        currentState = new DrawingState(newObjects);
    }

    public IEnumerable<Point> GetPoints() =>
        currentState.GraphicObjects.SelectMany(x => x.GetPoints());

    public IEnumerable<IDrwaingGraphicObject> GetGraphicObjects() =>
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