using Microsoft.Maui.Graphics;

public class GameDrawable : IDrawable
{
    public List<PointF> SnakeParts { get; set; } = new List<PointF>();
    public PointF Apple { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Фон
        canvas.FillColor = Colors.LightGray;
        canvas.FillRectangle(dirtyRect);

        // Малювання змійки (зелений колір)
        canvas.FillColor = Colors.Green;
        foreach (var part in SnakeParts)
        {
            canvas.FillCircle(part.X, part.Y, 20); // Радіус частини змійки
        }

        // Малювання яблука (червоний колір)
        canvas.FillColor = Colors.Red;
        canvas.FillCircle(Apple.X, Apple.Y, 20); // Радіус яблука
    }
}
