using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core;
using DrawingEditor.WinForms.ToolBarElements;

namespace DrawingEditor.WinForms.VerticalToolBarElements.Factories;

internal class _3dActionsFactory
{
    public void AddSubMenu(Toolbar toolbar)
    {
        toolbar.AddToolGroupButton("3д", Color.LightYellow, CreateSecondOrderLinesActions());
    }

    private Dictionary<string, Action> CreateSecondOrderLinesActions()
    {
        return new Dictionary<string, Action>
                    {
                        { "Куб", () => SetCube() }
                    };
    }

    private void SetCube()
    {
        CubeCreator cubeCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(cubeCreator);
    }
}
