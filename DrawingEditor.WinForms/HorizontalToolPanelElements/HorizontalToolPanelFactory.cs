using DrawingEditor.Core;

namespace DrawingEditor.WinForms.HorizontalToolPanelElements;

internal class HorizontalToolPanelFactory
{
    private BufferedPanel bufferedPanel;

    private CheckBox gridCheckBox;
    private Button UndoButton;
    private Button RedoButton;
    private PictureBox color_picker;
    private Button pic_color;

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
            AutoSize = true, // Позволяет кнопке автоматически подстраиваться под текст
            Padding = new Padding(10) // Добавляем отступы для улучшения внешнего вида
        };
        UndoButton.Click += UndoButton_Click;
        toolPanel.Controls.Add(UndoButton);

        // Настройка кнопки "Отменить"
        RedoButton = new Button()
        {
            Text = "вернуть",
            Height = 40,
            Font = new Font("Arial", 12),
            AutoSize = true, // Позволяет кнопке автоматически подстраиваться под текст
            Padding = new Padding(10) // Добавляем отступы для улучшения внешнего вида
        };
        RedoButton.Click += RedoButton_Click;
        toolPanel.Controls.Add(RedoButton);

        // Настройка color_picker
        color_picker = new PictureBox()
        {
            SizeMode = PictureBoxSizeMode.Zoom, // Изменяем на Zoom для сохранения пропорций
            Width = toolPanel.Width - 20, // Учитываем отступы
            Height = 150, // Высота элемента
        };

        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        string imagePath = Path.Combine(projectDirectory, "Images", "color_palette.png");

        // Проверяем существование файла перед загрузкой изображения
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

        return toolPanel;
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
        Stack<Point> pixel = new Stack<Point>();
        pixel.Push(new Point(x, y));
        bm.SetPixel(x, y, new_clr);
        if (old_color == new_clr) return;
        while (pixel.Count > 0)
        {
            Point pt = (Point)pixel.Pop();
            validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
            validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
            validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
            validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
        }
    }



    //преобразования координат точки (pt) относительно размеров PictureBox
    static Point set_point(PictureBox pb, Point pt)
    {
        float pX = 1f * ((float)pb.Image.Width / pb.Width);
        float pY = 1f * ((float)pb.Image.Height / pb.Height);
        return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
    }

}
