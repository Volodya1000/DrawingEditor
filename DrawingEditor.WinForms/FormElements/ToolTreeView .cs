namespace DrawingEditor.WinForms;

using System;
using System.Windows.Forms;

using System;
using System.Windows.Forms;

public class ToolTreeView : TreeView
{
    public event EventHandler<TreeNode> NodeSelected;

    public ToolTreeView()
    {
        InitializeTree();
        this.AfterSelect += (s, e) => {
            if (e.Node.Tag is EventHandler handler)
            {
                handler.Invoke(this, EventArgs.Empty);
                NodeSelected?.Invoke(this, e.Node);
            }
        };
    }
    private readonly (string Name, object Content)[] structure = new (string, object)[]
    {
        ("Отрезки", new[] { ("ЦДА", nameof(OnDDAAlgorithm)), ("Брезенхема", nameof(OnBresenhamAlgorithm)), ("Ву", nameof(OnWuAlgorithm)) }),
        ("Линии 2-го порядка", new[] { ("Окружность", nameof(OnCircle)), ("Эллипс", nameof(OnEllipse)), ("Гипербола", nameof(OnHyperbola)), ("Парабола", nameof(OnParabola)) }),
        ("Параметрические кривые", new[] { ("Эрмита", nameof(OnHermiteCurve)), ("Безье", nameof(OnBezierCurve)), ("B-сплайн", nameof(OnBSplineCurve)) }),
        ("Геом. преобразования", new[] { ("Перемещение", nameof(OnTranslation)), ("Поворот", nameof(OnRotation)), ("Масштабирование", nameof(OnScaling)) }),
        ("Полигоны", nameof(OnPolygonsAndConvexHulls)),
        ("Заполнение", nameof(OnPolygonFilling))
    };


    private void InitializeTree()
    {
        

        foreach (var item in structure)
        {
            if (item.Item2 is string methodName)
            {
                AddNode(this.Nodes, item.Item1, GetEventHandler(methodName));
            }
            else if (item.Item2 is (string, string)[])
            {
                var parentNode = AddNode(this.Nodes, item.Item1, null);
                foreach (var subItem in (ValueTuple<string, string>[])item.Item2)
                {
                    AddNode(parentNode.Nodes, subItem.Item1, GetEventHandler(subItem.Item2));
                }
            }
        }

        this.ExpandAll();
    }

    private TreeNode AddNode(TreeNodeCollection parent, string text, EventHandler handler)
    {
        var node = new TreeNode(text) { Tag = handler };
        parent.Add(node);
        return node;
    }

    private EventHandler GetEventHandler(string methodName) =>
        (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, methodName);

    // Пустые методы для каждого пункта меню
    private void OnDDAAlgorithm(object sender, EventArgs e) { }
    private void OnBresenhamAlgorithm(object sender, EventArgs e) { }
    private void OnWuAlgorithm(object sender, EventArgs e) { }
    private void OnCircle(object sender, EventArgs e) { }
    private void OnEllipse(object sender, EventArgs e) { }
    private void OnHyperbola(object sender, EventArgs e) { }
    private void OnParabola(object sender, EventArgs e) { }
    private void OnHermiteCurve(object sender, EventArgs e) { }
    private void OnBezierCurve(object sender, EventArgs e) { }
    private void OnBSplineCurve(object sender, EventArgs e) { }
    private void OnTranslation(object sender, EventArgs e) { }
    private void OnRotation(object sender, EventArgs e) { }
    private void OnScaling(object sender, EventArgs e) { }
    private void OnPolygonsAndConvexHulls(object sender, EventArgs e) { }
    private void OnPolygonFilling(object sender, EventArgs e) { }

    public int CalculateTreeViewHeight()
    {
        int CountNodes(TreeNodeCollection nodes)
        {
            return nodes.Cast<TreeNode>().Sum(node => 1 + (node.IsExpanded ? CountNodes(node.Nodes) : 0));
        }

        return this.ItemHeight * CountNodes(this.Nodes);
    }

}
