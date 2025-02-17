using DrawingEditor.Core;
using DrawingEditor.WinForms;

internal class HorizontalToolPanelFactory
{
    private BufferedPanel bufferedPanel;

    private CheckBox gridCheckBox;
    private Button UndoButton;
    private Button RedoButton;
    private PictureBox color_picker;
    private Button pic_color;

    private NumericUpDown thicknessSelector;
    private Label thicknessLabel;

    public HorizontalToolPanelFactory(BufferedPanel bufferedPanel)
    {
        this.bufferedPanel = bufferedPanel;
    }

    public FlowLayoutPanel CreateToolPanel()
    {
        var toolPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Height = 180,
            Padding = new Padding(1),
            BorderStyle = BorderStyle.Fixed3D
        };

        // Настройка CheckBox
        gridCheckBox = new CheckBox
        {
            Text = "Grid",
            Font = new Font("Arial", 12),
            Checked = true
        };
        gridCheckBox.CheckedChanged += gridCheckBox_CheckedChanged;
        toolPanel.Controls.Add(gridCheckBox);

        // Настройка кнопки "Отменить"
        UndoButton = new Button()
        {
            Text = "Отменить",
            Height = 40,
            Font = new Font("Arial", 12),
            AutoSize = true,
            Padding = new Padding(10)
        };
        UndoButton.Click += UndoButton_Click;
        toolPanel.Controls.Add(UndoButton);

        // Настройка кнопки "Вернуть"
        RedoButton = new Button()
        {
            Text = "Вернуть",
            Height = 40,
            Font = new Font("Arial", 12),
            AutoSize = true,
            Padding = new Padding(10)
        };
        RedoButton.Click += RedoButton_Click;
        toolPanel.Controls.Add(RedoButton);

        // Настройка color_picker
        color_picker = new PictureBox()
        {
            SizeMode = PictureBoxSizeMode.Zoom,
            Width = toolPanel.Width - 20,
            Height = 150,
        };

        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        string imagePath = Path.Combine(projectDirectory, "Images", "color_palette.png");

        if (File.Exists(imagePath))
        {
            color_picker.Image = Image.FromFile(imagePath);
        }

        color_picker.MouseClick += color_picker_MouseClick;
        toolPanel.Controls.Add(color_picker);

        // Настройка pic_color
        pic_color = new Button
        {
            BackColor = Color.White,
            Width = 120,
            Height = 120,
        };

        toolPanel.Controls.Add(pic_color);

        // Настройка выбора толщины линии
        thicknessLabel = new Label
        {
            Text = "Толщина линии:",
            Font = new Font("Arial", 12),
            AutoSize = true
        };

        thicknessSelector = new NumericUpDown
        {
            Minimum = 1, // Минимальная толщина
            Maximum = 20, // Максимальная толщина (можно сделать переменной)
            Value = 1, // Начальное значение
            Increment = 1, // Шаг изменения толщины
            Width = 60 // Ширина элемента управления
        };

        thicknessSelector.ValueChanged += ThicknessSelector_ValueChanged;

        toolPanel.Controls.Add(thicknessLabel);
        toolPanel.Controls.Add(thicknessSelector);

        return toolPanel;
    }

    private void ThicknessSelector_ValueChanged(object sender, EventArgs e)
    {
        int lineThickness = (int)thicknessSelector.Value;
        CurentDrawingSettings.GetInstance().lineThickness = lineThickness;
    }

    private void gridCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CurentDrawingSettings.GetInstance().GridEnable = !CurentDrawingSettings.GetInstance().GridEnable;
        bufferedPanel.Invalidate();
    }

    private void UndoButton_Click(object sender, EventArgs e)
    {
        bool PreviusStateExists = GraphicsEditorFacade.GetInstance().Undo();
        bufferedPanel.Invalidate();
    }

    private void RedoButton_Click(object sender, EventArgs e)
    {
        bool PreviusStateExists = GraphicsEditorFacade.GetInstance().Redo();
        bufferedPanel.Invalidate();
    }

    private void color_picker_MouseClick(object sender, MouseEventArgs e)
    {
        Point point = set_point(color_picker, e.Location);

        if (point.X >= 0 && point.X < color_picker.Image.Width && point.Y >= 0 && point.Y < color_picker.Image.Height)
        {
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            CurentDrawingSettings.GetInstance().SelectedColor = pic_color.BackColor;
        }
    }

    private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
    {
        Color cx = bm.GetPixel(x, y);
        if (cx == old_color)
        {
            sp.Push(new Point(x, y));
            bm.SetPixel(x, y, new_color);
        }
    }

    public void Fill(Bitmap bm, int x, int y, Color new_clr)
    {
        Color old_color = bm.GetPixel(x, y);
        Stack<Point> pixelStack = new Stack<Point>();
        pixelStack.Push(new Point(x, y));
        bm.SetPixel(x, y, new_clr);

        if (old_color == new_clr) return;

        while (pixelStack.Count > 0)
        {
            Point pt = pixelStack.Pop();
            validate(bm, pixelStack, pt.X - 1, pt.Y, old_color, new_clr);
            validate(bm, pixelStack, pt.X, pt.Y - 1, old_color, new_clr);
            validate(bm, pixelStack, pt.X + 1, pt.Y, old_color, new_clr);
            validate(bm, pixelStack, pt.X, pt.Y + 1, old_color, new_clr);
        }
    }

    static Point set_point(PictureBox pb, Point pt)
    {
        float pX = (float)pb.Image.Width / pb.Width;
        float pY = (float)pb.Image.Height / pb.Height;
        return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
    }
}