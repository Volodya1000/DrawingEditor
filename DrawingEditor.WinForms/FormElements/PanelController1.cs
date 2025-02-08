namespace DrawingEditor.WinForms;

using System.Drawing.Drawing2D;

public class PanelController1
{
    private readonly BufferedPanel panel;
    private readonly int gridWidth;
    private readonly int gridHeight;
    private readonly int cellSize; // Размер ячейки сетки
    private float scale = 1.0f;
    private Point offset = Point.Empty;
    private readonly List<Point> leftClickHistory = new List<Point>();
    private bool isDragging;
    private Point previousMousePosition;
    private const float MinScale = 0.2f;
    private const float MaxScale = 10.0f;
    public bool GridEnable { get; set; } = false;

    public PanelController1(BufferedPanel panel, int gridWidth, int gridHeight, int cellSize)
    {
        this.panel = panel;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.cellSize = cellSize; // Сохраняем размер ячейки

        panel.MouseWheel += Panel_MouseWheel;
        panel.MouseDown += Panel_MouseDown;
        panel.MouseMove += Panel_MouseMove;
        panel.MouseUp += Panel_MouseUp;
        panel.Paint += Panel_Paint;
    }

    private void Panel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            HandleLeftMouseClick(e.Location);
        }
        else if (e.Button == MouseButtons.Right)
        {
            StartDrag(e.Location);
        }
    }

    private void Panel_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            Drag(e.Location);
        }
    }

    private void Panel_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            StopDrag();
        }
    }

    private void Panel_MouseWheel(object sender, MouseEventArgs e)
    {
        HandleMouseWheel(e.Location, e.Delta);
    }

    public void HandleLeftMouseClick(Point location)
    {
        var gridPoint = ConvertToGridCoordinates(location);
        leftClickHistory.Add(gridPoint);
        panel.Invalidate();
    }

    private void StartDrag(Point location)
    {
        isDragging = true;
        previousMousePosition = location;
    }

    private void Drag(Point currentLocation)
    {
        var deltaX = currentLocation.X - previousMousePosition.X;
        var deltaY = currentLocation.Y - previousMousePosition.Y;
        offset.X += deltaX;
        offset.Y += deltaY;
        previousMousePosition = currentLocation;
        panel.Invalidate();
    }

    private void StopDrag()
    {
        isDragging = false;
    }

    public void HandleMouseWheel(Point location, int delta)
    {
        float scaleFactor = delta > 0 ? 1.1f : 0.9f;
        float newScale = scale * scaleFactor;

        newScale = Math.Clamp(newScale, MinScale, MaxScale);
        scaleFactor = newScale / scale;
        scale = newScale;

        offset.X = (int)(location.X - (location.X - offset.X) * scaleFactor);
        offset.Y = (int)(location.Y - (location.Y - offset.Y) * scaleFactor);

        panel.Invalidate();
    }

    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        graphics.Clear(Color.White);
        graphics.Transform = GetTransformationMatrix();

        if (GridEnable)
            DrawGrid(graphics);


        DrawPoints(graphics);

        graphics.ResetTransform();
    }

    private Matrix GetTransformationMatrix()
    {
        Matrix matrix = new Matrix();
        matrix.Translate(offset.X, offset.Y);
        matrix.Scale(scale, scale);
        return matrix;
    }

    private void DrawGrid(Graphics graphics)
    {
        using (Pen gridPen = new Pen(Color.LightGray))
        {
            for (int x = 0; x <= gridWidth; x++)
            {
                graphics.DrawLine(gridPen, x * cellSize, 0, x * cellSize, gridHeight * cellSize);
            }
            for (int y = 0; y <= gridHeight; y++)
            {
                graphics.DrawLine(gridPen, 0, y * cellSize, gridWidth * cellSize, y * cellSize);
            }
        }
    }

    private void DrawPoints(Graphics graphics)
    {
        foreach (var point in leftClickHistory)
        {
            graphics.FillRectangle(Brushes.Red, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
        }
    }

    private Point ConvertToGridCoordinates(Point screenPoint)
    {
        float worldX = (screenPoint.X - offset.X) / scale;
        float worldY = (screenPoint.Y - offset.Y) / scale;

        int gridX = (int)(worldX / cellSize);
        int gridY = (int)(worldY / cellSize);

        return new Point(gridX, gridY);
    }
}