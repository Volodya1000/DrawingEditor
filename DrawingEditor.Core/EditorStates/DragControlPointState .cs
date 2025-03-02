
using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

// состояние для перетаскивания опорной точки
internal class DragControlPointState : IEditorState
{
    private readonly IEditableGraphicObject selectedObject;
    private readonly int controlPointIndex;
    private readonly int clickRadius;
    private readonly List<Point> originalControlPoints;

    public DragControlPointState(IEditableGraphicObject selectedObject, int controlPointIndex, int clickRadius)
    {
        this.selectedObject = selectedObject;
        this.controlPointIndex = controlPointIndex;
        this.clickRadius = clickRadius;
        originalControlPoints = selectedObject.GetControlPoints().ToList();
    }

    // Позволяем получить редактируемый объект для фильтрации при отрисовке
    public IEditableGraphicObject EditingObject => selectedObject;

    // При перемещении мыши создаём preview-объект с изменённой опорной точкой
    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        var previewPoints = new List<Point>(originalControlPoints);
        previewPoints[controlPointIndex] = point;
        var previewObject = editor.CurrentCreator.CreateGraphicObject(color, lineThickness, previewPoints);
        editor.SetPreviewGraphicObject(previewObject);
    }

    // По клику фиксируем изменение в оригинальном объекте
    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        selectedObject.UpdateControlPoint(controlPointIndex, point);
        editor.SetPreviewGraphicObject(null);
        editor.SetEditorState(new IdleState());
    }
}
