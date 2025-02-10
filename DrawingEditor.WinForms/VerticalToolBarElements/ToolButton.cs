namespace DrawingEditor.WinForms.ToolBarElements;


internal class ToolButton : Button, ISelectableButton
{
    public ToolButton(string toolName, Color backColor)
    {
        this.Text = toolName;
        this.BackColor = backColor;
        this.Size = new Size(90, 40);
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
    }

    public virtual void Select()
    {
        this.FlatAppearance.BorderSize = 2;
        this.FlatAppearance.BorderColor = Color.Black;
    }

    public virtual void Deselect()
    {
        this.FlatAppearance.BorderSize = 0;
    }
}


