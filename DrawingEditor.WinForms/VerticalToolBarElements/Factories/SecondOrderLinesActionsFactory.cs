using DrawingEditor.Core.Algorithms.LineAlgorithms;
using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core;

namespace DrawingEditor.WinForms.ToolBarElements;

internal class SecondOrderLinesActionsFactory
{
    public void AddSubMenu(Toolbar toolbar)
    {
        toolbar.AddToolGroupButton("2-порядок", Color.LightYellow, CreateSecondOrderLinesActions());
    }

    private Dictionary<string, Action> CreateSecondOrderLinesActions()
    {
        return new Dictionary<string, Action>
                    {
                        { "Окружность", () => SetCircle() },
                        { "Элипс", () => SetElipse() },
                        { "Парабола", () => SetParabola() },
                        { "Гипербола", () => SetHyperbola() }
                    };
    }

    private void SetCircle()
    {
        CircleCreator circleCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(circleCreator);
    }

    private void SetElipse()
    {
        ElipceCreator elipceCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(elipceCreator);
    }

    private void SetParabola()
    {
        ParabolaCreator parabolaCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(parabolaCreator);
    }

    private void SetHyperbola()
    {
        HyperbolaCreator hyperbolaCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(hyperbolaCreator);
    }


}
