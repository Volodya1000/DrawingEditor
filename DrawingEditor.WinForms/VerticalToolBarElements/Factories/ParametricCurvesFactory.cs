﻿
using DrawingEditor.Core.GraphickObjectsCreators;
using DrawingEditor.Core;
using DrawingEditor.WinForms.ToolBarElements;

namespace DrawingEditor.WinForms.VerticalToolBarElements.Factories;

internal class ParametricCurvesFactory
{

    public void AddSubMenu(Toolbar toolbar)
    {
        toolbar.AddToolGroupButton("Кривые", Color.LightYellow, CreateSecondOrderLinesActions());
    }

    private Dictionary<string, Action> CreateSecondOrderLinesActions()
    {
        return new Dictionary<string, Action>
                    {
                        { "Эрмит", () => SetHermite() },
                        { "Безье", () => SetBezie() },
                        { "B-Сплайн",()=>SetSpline()}
                    };
    }

    private void SetHermite()
    {
        HermiteCreator hermiteCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(hermiteCreator);
    }

    private void SetBezie()
    {
        BezieCreator bezieCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(bezieCreator);
    }

    private void SetSpline()
    {
        BSplineCreator splineCreator = new();
        GraphicsEditorFacade.GetInstance().SetCreator(splineCreator);
    }
}
