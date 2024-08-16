namespace ActionGo
{
    public class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = GameManager.GetInstance();
            gameManager.Run();
        }
    }
}
