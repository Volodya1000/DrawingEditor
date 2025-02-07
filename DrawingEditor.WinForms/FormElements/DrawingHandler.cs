namespace DrawingEditor.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;



public class DrawingHandler
{
    private readonly BufferedPanel panel;
    private readonly TransformationHandler transformationHandler;
    private readonly LineManager lineManager;
    private readonly GridDrawer gridDrawer;
    private Point? startPoint = null;

    public DrawingHandler(BufferedPanel panel)
    {
        this.panel = panel;
        transformationHandler = new TransformationHandler();
        lineManager = new LineManager();
        gridDrawer = new GridDrawer();

        this.panel.Paint += Panel_Paint;
        this.panel.MouseWheel += Panel_MouseWheel;
        this.panel.MouseDown += Panel_MouseDown;
    }

    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Transform = new Matrix(
            transformationHandler.ZoomFactor, 0, 0,
            transformationHandler.ZoomFactor,
            transformationHandler.Translation.X,
            transformationHandler.Translation.Y);

        gridDrawer.DrawGrid(g);
        gridDrawer.DrawLines(g, lineManager.GetLines());
        gridDrawer.DrawCurrentLine(g, startPoint, null);
    }

    private void Panel_MouseWheel(object sender, MouseEventArgs e)
    {
        transformationHandler.Zoom(e.Location, e.Delta);
        panel.Invalidate();
    }

    private void Panel_MouseDown(object sender, MouseEventArgs e)
    {
        if (startPoint == null)
        {
            startPoint = Point.Round(ScreenToWorld(e.Location));
        }
        else
        {
            var endPoint = ScreenToWorld(e.Location);
            lineManager.AddLine(Point.Round(startPoint.Value), Point.Round(endPoint));
            startPoint = null;
            panel.Invalidate();
        }
    }

    private PointF ScreenToWorld(Point point)
    {
        return new PointF(
            (point.X - transformationHandler.Translation.X) / transformationHandler.ZoomFactor,
            (point.Y - transformationHandler.Translation.Y) / transformationHandler.ZoomFactor
        );
    }
}



//public class DrawingHandler
//{
//    private bool isDrawing = false;
//    private Point startPoint;
//    private Panel panel;

//    public DrawingHandler(Panel panel)
//    {
//        this.panel = panel;
//        this.panel.MouseDown += Panel_MouseDown;
//        this.panel.MouseMove += Panel_MouseMove;
//        this.panel.MouseUp += Panel_MouseUp;
//        this.panel.Paint += Panel_Paint;
//    }

//    private void Panel_MouseDown(object sender, MouseEventArgs e)
//    {
//        isDrawing = true;
//        startPoint = e.Location;
//    }

//    private void Panel_MouseMove(object sender, MouseEventArgs e)
//    {
//        if (isDrawing)
//        {
//            using (Graphics g = panel.CreateGraphics())
//            {
//                g.DrawLine(Pens.Black, startPoint, e.Location);
//            }
//            startPoint = e.Location;
//        }
//    }

//    private void Panel_MouseUp(object sender, MouseEventArgs e)
//    {
//        isDrawing = false;
//    }

//    private void Panel_Paint(object sender, PaintEventArgs e)
//    {
//        // Рисование на панели при перерисовке (можно добавить дополнительные фигуры)
//        Graphics g = e.Graphics;
//        // Пример рисования: гистограмма, линии или другие объекты
//    }

//    // Дополнительные методы для рисования
//    public void Clear()
//    {
//        panel.Invalidate(); // Очистить панель
//    }
//}