using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

// Состояние ожидания – пользователь пока не начал создавать объект
public class IdleState : IEditorState
{
    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        // В состоянии ожидания не обновляем preview объект
        editor.SetPreviewGraphicObject(null);
    }

    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        // При первом клике запускаем процесс создания объекта
        var creationState = new CreationState(editor.CurrentCreator.GetRequiredPointsCount());
        creationState.AddPoint(point);
        editor.SetEditorState(creationState);
    }

    public IEnumerable<IDrwaingGraphicObject> GetAdditionalRenderingObjects()
    {
        return Enumerable.Empty<IDrwaingGraphicObject>();
    }
}

