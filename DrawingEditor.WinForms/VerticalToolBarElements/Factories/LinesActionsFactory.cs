using DrawingEditor.Core;
using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.GraphickObjectsCreators;

namespace DrawingEditor.WinForms.ToolBarElements
{
    internal class LinesActionsFactory
    {
        public void AddLinesSubMenu(Toolbar toolbar)
        {
            toolbar.AddToolGroupButton("Shapes", Color.LightYellow, CreateLinesActions());
        }

        private Dictionary<string, Action> CreateLinesActions()
        {
            return new Dictionary<string, Action>
                        {
                            { "ЦДА", () => SetCDAAlgorithm() },
                            { "Брезенхем", () => SetBresenhamAlgorithm() },
                            { "Ву", () => SetWuAlgorithm() }
                        };
        }

        private void SetCDAAlgorithm()
        {
            LineCreator lineCreator = new (LineDrawingAlgorithms.CDADraw);
            GraphicsEditorFacade.GetInstance().SetCreator(lineCreator);
        }

        private void SetBresenhamAlgorithm()
        {
            LineCreator lineCreator = new(LineDrawingAlgorithms.BresenhamDraw);
            GraphicsEditorFacade.GetInstance().SetCreator(lineCreator);
        }

        private void SetWuAlgorithm()
        {
            WuLineCreator lineCreator = new(LineDrawingAlgorithms.WuDraw);
            GraphicsEditorFacade.GetInstance().SetCreator(lineCreator);
        }


    }
}
