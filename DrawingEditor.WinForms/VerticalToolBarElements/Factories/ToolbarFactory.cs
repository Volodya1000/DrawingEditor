using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core;
using DrawingEditor.WinForms.VerticalToolBarElements.Factories;

namespace DrawingEditor.WinForms.ToolBarElements;

internal class ToolbarFactory
{
    public Toolbar CreateToolbar()
    {
        Toolbar toolbar = new Toolbar();
        AddTools(toolbar);
        return toolbar;
    }

    private void AddTools(Toolbar toolbar)
    {
        //toolbar.AddToolButton("Pen", Color.LightBlue, () => MessageBox.Show("Pen selected"));
        //toolbar.AddToolButton("Brush", Color.LightGreen, () => MessageBox.Show("Brush selected"));

        LinesActionsFactory linesFactory = new LinesActionsFactory();
        linesFactory.AddSubMenu(toolbar);

        SecondOrderLinesActionsFactory secondOrderLinesActionsFactory = new ();
        secondOrderLinesActionsFactory.AddSubMenu (toolbar);

        ParametricCurvesFactory parametricCurvesFactory = new ();
        parametricCurvesFactory.AddSubMenu(toolbar);

        toolbar.AddToolButton("Соеденить", 
                              Color.LightGreen, 
                              ()=>GraphicsEditorFacade.GetInstance()
                                                        .SetCreator(new ConnectObjectsCreator())
                              );

        _3dActionsFactory _3dFactory = new();
        _3dFactory.AddSubMenu(toolbar);


    }
}