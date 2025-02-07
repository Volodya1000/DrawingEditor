namespace DrawingEditor.WinForms.FormElements;

public class ZoomablePanel : Panel
{
    private readonly List<Point> points;
    private float zoomFactor = 1.0f;
    private PointF offset = new PointF(0, 0);
    private const float ZoomStep = 1.1f;
    private const int PointSize = 10; // Размер пикселя на сетке

    private bool isPanning = false;
    private Point lastMouseLocation;

    public ZoomablePanel()
    {
        this.points = new List<Point>();
        this.MouseWheel += ZoomablePanel_MouseWheel;
        this.MouseDown += ZoomablePanel_MouseDown;
        this.MouseMove += ZoomablePanel_MouseMove;
        this.MouseUp += ZoomablePanel_MouseUp;
        this.DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Рисуем сетку
        DrawGrid(e.Graphics);

        // Рисуем точки
        foreach (var point in points)
        {
            var transformedPoint = TransformPoint(point);
            e.Graphics.FillRectangle(Brushes.Red, transformedPoint.X, transformedPoint.Y, PointSize * zoomFactor, PointSize * zoomFactor);
        }
    }

    private void DrawGrid(Graphics graphics)
    {
        Pen gridPen = Pens.LightGray;
        float gridSpacing = PointSize * zoomFactor;

        for (float x = offset.X % gridSpacing; x < Width; x += gridSpacing)
        {
            graphics.DrawLine(gridPen, x, 0, x, Height);
        }
        for (float y = offset.Y % gridSpacing; y < Height; y += gridSpacing)
        {
            graphics.DrawLine(gridPen, 0, y, Width, y);
        }
    }

    private void ZoomablePanel_MouseWheel(object sender, MouseEventArgs e)
    {
        float oldZoom = zoomFactor;
        PointF cursorPosition = new PointF(
            (e.Location.X - offset.X) / zoomFactor,
            (e.Location.Y - offset.Y) / zoomFactor
        );

        if (e.Delta > 0)
            zoomFactor *= ZoomStep;
        else if (e.Delta < 0)
            zoomFactor /= ZoomStep;

        zoomFactor = Math.Max(0.1f, Math.Min(zoomFactor, 10.0f));

        offset.X = e.Location.X - cursorPosition.X * zoomFactor;
        offset.Y = e.Location.Y - cursorPosition.Y * zoomFactor;

        Invalidate();
    }

    private void ZoomablePanel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            // Привязка точки к сетке
            int gridX = (int)Math.Round((e.X - offset.X) / (PointSize * zoomFactor));
            int gridY = (int)Math.Round((e.Y - offset.Y) / (PointSize * zoomFactor));

            var point = new Point(gridX, gridY);
            if (!points.Contains(point))
            {
                points.Add(point);
                Invalidate();
            }
        }
        else if (e.Button == MouseButtons.Right)
        {
            isPanning = true;
            lastMouseLocation = e.Location;
            Cursor = Cursors.Hand;
        }
    }

    private void ZoomablePanel_MouseMove(object sender, MouseEventArgs e)
    {
        if (isPanning)
        {
            offset.X += e.X - lastMouseLocation.X;
            offset.Y += e.Y - lastMouseLocation.Y;
            lastMouseLocation = e.Location;
            Invalidate();
        }
    }

    private void ZoomablePanel_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            isPanning = false;
            Cursor = Cursors.Default;
        }
    }

    private PointF TransformPoint(Point point)
    {
        return new PointF(
            point.X * PointSize * zoomFactor + offset.X,
            point.Y * PointSize * zoomFactor + offset.Y
        );
    }
}
