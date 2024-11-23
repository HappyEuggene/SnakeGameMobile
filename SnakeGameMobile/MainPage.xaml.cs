namespace SnakeGameMobile;

public partial class MainPage : ContentPage
{
    private GameDrawable GameDrawable; // Відповідає за візуалізацію
    private bool GameRunning = false; // Чи запущена гра
    private PointF Direction = new PointF(20, 0); // Напрямок руху (вправо)
    private int Score = 0; // Поточний рахунок

    public MainPage()
    {
        InitializeComponent();

        // Ініціалізація графіки
        GameDrawable = new GameDrawable
        {

            SnakeParts = new List<PointF>(),
            Apple = new PointF()

        };
        GameCanvas.Drawable = GameDrawable;

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        InitializeGame(); // Ініціалізуємо гру після завантаження сторінки
    }

    private void InitializeGame()
    {
        if (GameCanvas.Width <= 0 || GameCanvas.Height <= 0)
        {
            // Відкладаємо ініціалізацію, якщо розміри недоступні
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), InitializeGame);
            return;
        }

        // Встановлюємо початкову позицію змійки
        float centerX = (float)GameCanvas.Width / 2;
        float centerY = (float)GameCanvas.Height / 2;

        GameDrawable.SnakeParts = new List<PointF>
        {
            new PointF(centerX, centerY) // Початкова позиція змійки
        };

        SpawnApple(); // Генеруємо перше яблуко
        Score = 0; // Скидаємо рахунок
        ScoreLabel.Text = "Score: 0";
        GameCanvas.Invalidate(); // Оновлюємо поле
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        if (!GameRunning)
        {
            GameRunning = true;
            StartGameLoop(); // Запускаємо ігровий цикл
        }
    }

    private void StartGameLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            if (GameRunning)
            {
                UpdateGame();
                return true;
            }
            return false;
        });
    }

    private void UpdateGame()
    {
        var head = GameDrawable.SnakeParts.Last();
        var newHead = new PointF(head.X + Direction.X, head.Y + Direction.Y);

        // Перевірка виходу за межі поля
        if (newHead.X < 0 || newHead.Y < 0 || newHead.X > GameCanvas.Width || newHead.Y > GameCanvas.Height)
        {
            GameRunning = false;
            DisplayAlert("Game Over", "You hit the wall!", "OK");
            InitializeGame(); // Перезапуск гри
            return;
        }

        // Перевірка зіткнення з яблуком
        if (Math.Abs(newHead.X - GameDrawable.Apple.X) < 20 && Math.Abs(newHead.Y - GameDrawable.Apple.Y) < 20)
        {
            Score++;
            ScoreLabel.Text = $"Score: {Score}";
            SpawnApple(); // Генеруємо нове яблуко
        }
        else
        {
            GameDrawable.SnakeParts.RemoveAt(0); // Видаляємо хвіст, якщо яблуко не з'їдено
        }

        GameDrawable.SnakeParts.Add(newHead); // Додаємо нову голову
        GameCanvas.Invalidate(); // Оновлюємо графіку
    }

    private void SpawnApple()
    {
        if (GameCanvas.Width <= 0 || GameCanvas.Height <= 0)
        {
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), SpawnApple);
            return;
        }

        Random random = new Random();
        GameDrawable.Apple = new PointF(
            random.Next(20, (int)GameCanvas.Width - 20),
            random.Next(20, (int)GameCanvas.Height - 20)
        );

        GameCanvas.Invalidate(); // Оновлюємо поле
    }

    private void OnUpButtonClicked(object sender, EventArgs e)
    {
        if (Direction.Y == 0) Direction = new PointF(0, -20); // Вгору
    }

    private void OnDownButtonClicked(object sender, EventArgs e)
    {
        if (Direction.Y == 0) Direction = new PointF(0, 20); // Вниз
    }

    private void OnLeftButtonClicked(object sender, EventArgs e)
    {
        if (Direction.X == 0) Direction = new PointF(-20, 0); // Вліво
    }

    private void OnRightButtonClicked(object sender, EventArgs e)
    {
        if (Direction.X == 0) Direction = new PointF(20, 0); // Вправо
    }
}
