using DrawingEditor.Core.Models.Interfaces;

namespace DrawingEditor.Core.History;


internal class DrawingState
{
    public List<IDrawingGraphicObject> GraphicObjects { get; set; }

    // Конструктор для создания копии состояния
    public DrawingState(List<IDrawingGraphicObject> objects)
    {
        // Важно создать глубокую копию списка и объектов, чтобы избежать проблем с изменением состояния
        GraphicObjects = new List<IDrawingGraphicObject>(objects);
    }
}