using DrawingEditor.Core;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingEditor.WinForms
{
    public class CanvasPanelHandler
    {
        private readonly GraphicsEditorFacade _graphicsEditorFacade;
        private Panel _canvasPanel;
        private Point? _startPoint = null;

        public CanvasPanelHandler(Panel canvasPanel, GraphicsEditorFacade graphicsEditorFacade)
        {
            _canvasPanel = canvasPanel;
            _graphicsEditorFacade = graphicsEditorFacade;

            // Привязываем обработчики событий
            _canvasPanel.Paint += CanvasPanel_Paint;
            _canvasPanel.MouseClick += CanvasPanel_MouseClick;
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var point in _graphicsEditorFacade.GetPoints())
                e.Graphics.FillRectangle(Brushes.Black, point.X, point.Y, 1, 1);
        }

        private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_startPoint.HasValue)
            {
                // Если начальная точка еще не выбрана, сохраняем первую точку
                _startPoint = e.Location;
            }
            else // Если начальная точка уже выбрана, рисуем линию
            {
                var endPoint = e.Location;
                _graphicsEditorFacade.AddLine(_startPoint.Value, endPoint);

                // Перерисовываем панель, чтобы отобразить линию
                _canvasPanel.Invalidate();

                // Сбрасываем начальную точку для следующей линии
                _startPoint = null;
            }
        }
    }
}
