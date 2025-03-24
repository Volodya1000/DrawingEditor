using DrawingEditor.Core.Models.Interfaces;
using System.Drawing;

namespace DrawingEditor.Core;

// Состояние создания объекта
public class CreationState : IEditorState
{
    private readonly List<Point> points;
    private readonly int requiredPoints;

    public CreationState(int requiredPoints)
    {
        this.requiredPoints = requiredPoints;
        points = new List<Point>();
    }

    public void AddPoint(Point point)
    {
        points.Add(point);
    }

    public bool IsReadyToCreate() => points.Count >= requiredPoints;

    public IEnumerable<Point> GetPoints() => points;

    public void HandleMouseMove(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        // Обновляем preview объект только если уже есть хотя бы одна точка
        if (points.Count > 0)
        {
            var previewPoints = new List<Point>(points) { point };
            var previewObject = editor.CurrentCreator.CreateGraphicObject(color, lineThickness, previewPoints);
            editor.SetPreviewGraphicObject(previewObject);
        }
        else
        {
            editor.SetPreviewGraphicObject(null);
        }
    }

    public void HandlePoint(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        AddPoint(point);
        if (IsReadyToCreate())
        {
            var graphicObject = editor.CurrentCreator.CreateGraphicObject(color, lineThickness, points);
            if (graphicObject != null)
            {
                editor.CanvasModel.AddObject(graphicObject);
            }
            // После создания объекта возвращаемся в состояние ожидания
            editor.SetEditorState(new IdleState());
        }
    }

    public void ShouldFinish(GraphicsEditorFacade editor, Color color, int lineThickness, Point point)
    {
        var graphicObject = editor.CurrentCreator.CreateGraphicObject(color, lineThickness, points);
        if (graphicObject != null)
            editor.CanvasModel.AddObject(graphicObject);
        // После создания объекта возвращаемся в состояние ожидания
        editor.SetEditorState(new IdleState());
    }

    public IEnumerable<IDrawingGraphicObject> GetAdditionalRenderingObjects()
    {
        return Enumerable.Empty<IDrawingGraphicObject>();
    }
}