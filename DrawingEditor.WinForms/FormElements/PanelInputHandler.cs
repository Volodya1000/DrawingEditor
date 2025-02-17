using DrawingEditor.Core;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace DrawingEditor.WinForms;

public class PanelInputHandler
{
    private readonly BufferedPanel panel;
    private float scale = 1.0f;
    private Point offset = Point.Empty;
    private Point previousMousePosition;
    private bool isDragging;
    private readonly int PointsQueueSize; // Количество хранимых точек

    private readonly int cellSize;

    private readonly int gridWidth;
    private readonly int gridHeight;

    private const float MinScale = 0.01f;
    private const float MaxScale = 10.0f;



    public PanelInputHandler(BufferedPanel panel, int gridWidth, int gridHeight ,int cellSize,int pointsQueueSize = 5)
    {
        this.panel = panel;
        this.PointsQueueSize = pointsQueueSize;
        this.cellSize = cellSize;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;

        panel.MouseWheel += HandleMouseWheel;
        panel.MouseDown += HandleMouseDown;
        panel.MouseMove += HandleMouseMove;
        panel.MouseUp += HandleMouseUp;
    }

    public Matrix GetTransformationMatrix()
    {
        Matrix matrix = new Matrix();
        matrix.Translate(offset.X, offset.Y); // Применение смещения
        matrix.Scale(scale, scale);           // Применение масштаба
        return matrix;
    }

    private void HandleMouseWheel(object sender, MouseEventArgs e)
    {
        // Определение коэффициента масштабирования
        // в зависимости от направления прокрутки мыши
        float scaleFactor = e.Delta > 0 ? 1.1f : 0.9f;  
        float newScale = scale * scaleFactor; // Вычисление нового масштаба

        // Ограничение масштаба минимальным и максимальным значениями
        newScale = Math.Clamp(newScale, MinScale, MaxScale);
        scaleFactor = newScale / scale;
        scale = newScale;

        Point location = e.Location;
        // Обновление смещения по обоим осям с учетом масштаба
        offset.X = (int)(location.X - (location.X - offset.X) * scaleFactor);
        offset.Y = (int)(location.Y - (location.Y - offset.Y) * scaleFactor);

        panel.Invalidate();
    }

    private void HandleMouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            HandleLeftMouseDown(e.Location);
        else if (e.Button == MouseButtons.Right)
            StartDrag(e.Location);
    }

    private void HandleLeftMouseDown(Point location)
    {
        Point gridPoint = ConvertToGridCoordinates(location);

        bool pointIsInGrid = gridPoint.X >= 0 && gridPoint.X < gridWidth && gridPoint.Y >= 0 && gridPoint.Y < gridHeight;
        if (!pointIsInGrid) return;

        GraphicsEditorFacade.GetInstance().HandlePoint(CurentDrawingSettings.GetInstance().SelectedColor, CurentDrawingSettings.GetInstance().lineThickness, gridPoint);
        panel.Invalidate();
    }


    private void HandleMouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging) Drag(e.Location);
        else
        {
            Point gridPoint = ConvertToGridCoordinates(e.Location);
            GraphicsEditorFacade.GetInstance().HandleMouseMove(CurentDrawingSettings.GetInstance().SelectedColor, CurentDrawingSettings.GetInstance().lineThickness, gridPoint);
            panel.Invalidate();
        }
    }

    private void HandleMouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) StopDrag();
    }

    private void StartDrag(Point location)
    {
        isDragging = true;
        previousMousePosition = location;
    }

    private void Drag(Point currentLocation)
    {
        // Обновление смещения по обоим осям при перетаскивании
        offset.X += currentLocation.X - previousMousePosition.X;
        offset.Y += currentLocation.Y - previousMousePosition.Y;
        previousMousePosition = currentLocation;
        panel.Invalidate();
    }

    private void StopDrag() => isDragging = false;

    private Point ConvertToGridCoordinates(Point screenPoint)
    {
        // Преобразование координат экрана
        // в виртуальные координаты сетки с учетом смещения и масштаба
        float worldX = (screenPoint.X - offset.X) / scale;
        float worldY = (screenPoint.Y - offset.Y) / scale;

        // Вычисление номеров виртуальных ячеек учитыва размер одной ячейки
        int gridX = (int)(worldX / cellSize);
        int gridY = (int)(worldY / cellSize);

        return new Point(gridX, gridY);
    }
}

