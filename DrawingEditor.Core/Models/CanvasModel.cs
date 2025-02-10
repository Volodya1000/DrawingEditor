using DrawingEditor.Core.History;
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

public class CanvasModel
{
    private DrawingState currentState { get; set; }

    private DrawingHistory history = new DrawingHistory();

    public CanvasModel()
    {
        currentState = new DrawingState(new List<IDrwaingGraphicObject>());
    }


    public void AddObject(IDrwaingGraphicObject obj)
    {
        List<IDrwaingGraphicObject> newObjects = new List<IDrwaingGraphicObject>(currentState.GraphicObjects);
        newObjects.Add(obj);
        SetState(new DrawingState(newObjects));
    }

    public void RemoveObject(IDrwaingGraphicObject obj) => currentState.GraphicObjects.Remove(obj);


    public IEnumerable<Point> GetPoints() => currentState.GraphicObjects.SelectMany(x => x.GetPoints());


    public IEnumerable<IDrwaingGraphicObject> GetGraphicObjects() => currentState.GraphicObjects;

    #region WorkWithState

    private void SetState(DrawingState newState)
        {
            SaveState(); // Сохраняем текущее состояние перед изменением
            currentState = newState;
        }

        private void SaveState()
        {
            // Создаем копию текущего состояния и сохраняем в истории
            history.SaveState(new DrawingState(new List<IDrwaingGraphicObject>(currentState.GraphicObjects)));
        }

        public bool Undo()
        {
            DrawingState previousState = history.Undo();
            if (previousState != null)
            {
                currentState = previousState;
                return true;
            }
            else
                return false;
        }
    #endregion
}
