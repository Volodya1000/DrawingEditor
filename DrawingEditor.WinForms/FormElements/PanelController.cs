using DrawingEditor.Core;
using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.Models.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace DrawingEditor.WinForms;
public class PanelController
{
    private readonly BufferedPanel panel;
    private readonly PanelInputHandler inputHandler;
    private readonly GridRenderer gridRenderer;

    private readonly int cellSize;

    public PanelController(BufferedPanel panel, int gridWidth, int gridHeight, int cellSize)
    {
        this.panel = panel;

        this.cellSize = cellSize;
        gridRenderer = new GridRenderer(gridWidth, gridHeight, cellSize);
        inputHandler = new PanelInputHandler(panel, gridWidth, gridHeight, cellSize);

        panel.Paint += Panel_Paint;
    }

    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        graphics.Clear(Color.White);
        graphics.Transform = inputHandler.GetTransformationMatrix();

        if (CurentDrawingSettings.GetInstance().GridEnable)
            gridRenderer.DrawGrid(graphics);

        //IEnumerable<Point> points = GraphicsEditorFacade.GetInstance().GetPoints();

        DrawObjectsEnumerable(graphics, GraphicsEditorFacade.GetInstance().GetGraphicObjects());

        IDrwaingGraphicObject previewObject = GraphicsEditorFacade.GetInstance().GetPreviewObject();
        
        if(previewObject!=null) DrawObject(graphics,previewObject);

        graphics.ResetTransform();
    }

    private void DrawObjectsEnumerable(Graphics graphics, IEnumerable<IDrwaingGraphicObject> graphicObjects)
    {
        foreach (var graphicObject in graphicObjects)
            DrawObject(graphics,graphicObject);

    }

    private void DrawObject(Graphics graphics, IDrwaingGraphicObject graphicObject)
    {
        if (graphicObject == null) return;
        foreach ((Point point, double intensity) item in graphicObject.GetPointsWithIntensity())
        {
            int alpha = (int)(item.intensity * 255);

            Color color = graphicObject.LineColor;

            Color transparentColor = Color.FromArgb(alpha, color.R, color.G, color.B);

            Brush brush = new SolidBrush(transparentColor);

            graphics.FillRectangle(brush,
            item.point.X* cellSize,
            item.point.Y* cellSize,
            cellSize, cellSize);
        }
    }
}
