using DrawingEditor.Core;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace DrawingEditor.WinForms;
public class PanelController
{
    private readonly BufferedPanel panel;
    private readonly PanelInputHandler inputHandler;
    private readonly GridRenderer gridRenderer;
    private readonly GraphicsEditorFacade graphicsEditorFacade;

    private readonly int cellSize;

    public bool GridEnable { get; set; } = false;

    public PanelController(BufferedPanel panel, int gridWidth, int gridHeight, int cellSize)
    {
        this.panel = panel;

        this.cellSize = cellSize;
        graphicsEditorFacade = new GraphicsEditorFacade();
        gridRenderer = new GridRenderer(gridWidth, gridHeight, cellSize);
        inputHandler = new PanelInputHandler(panel, graphicsEditorFacade, gridWidth, gridHeight,cellSize);

        panel.Paint += Panel_Paint;
    }

    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        graphics.Clear(Color.White);
        graphics.Transform = inputHandler.GetTransformationMatrix();

        if (GridEnable)
            gridRenderer.DrawGrid(graphics);

        IEnumerable<Point> points = graphicsEditorFacade.GetPoints();

        DrawPoints(graphics,points);

        IEnumerable<Point> previewPoints = graphicsEditorFacade.GetPreviewPoints();

        DrawPoints(graphics, previewPoints);

        graphics.ResetTransform();
    }

    private void DrawPoints(Graphics graphics,IEnumerable<Point> points)
    {
        foreach (var point in points)
        {
            graphics.FillRectangle(Brushes.Red, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
        }
    }
}
