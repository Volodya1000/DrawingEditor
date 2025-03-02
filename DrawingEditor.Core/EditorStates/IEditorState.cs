using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

// Интерфейс для состояний графического редактора
public interface IEditorState
{
    void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point);
    void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point);
    IEnumerable<IDrwaingGraphicObject> GetAdditionalRenderingObjects();
}

