namespace DrawingEditor.WinForms;

public class GridRenderer
{
    private readonly int gridWidth;
    private readonly int gridHeight;
    private readonly int cellSize;

    public GridRenderer(int gridWidth, int gridHeight, int cellSize)
    {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.cellSize = cellSize;
    }

    public void DrawGrid(Graphics graphics)
    {
        using (Pen gridPen = new Pen(Color.LightGray))
        {
            for (int x = 0; x <= gridWidth; x++)
                graphics.DrawLine(gridPen, x * cellSize, 0, x * cellSize, gridHeight * cellSize);

            for (int y = 0; y <= gridHeight; y++)
                graphics.DrawLine(gridPen, 0, y * cellSize, gridWidth * cellSize, y * cellSize);
        }
    }
}

