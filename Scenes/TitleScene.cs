using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionGo.Utils;

namespace ActionGo.Scenes
{
    public class TitleScene : Scene
    {
        private int nowIndex;
        private ConsoleKey inputKey;
        public TitleScene(GameManager gameManager) : base(gameManager)
        {
            nowIndex = 0;
        }
        public override void Enter()
        {
        }
        private void PrintSubtitle()
        {
            var charArr = new (ConsoleColor color, string text)[]
            {
                (ConsoleColor.Red, "대"),
                (ConsoleColor.Green, "!"),
                (ConsoleColor.Yellow, "마"),
                (ConsoleColor.Cyan, "!"),
                (ConsoleColor.Magenta, "불"),
                (ConsoleColor.Blue, "!"),
                (ConsoleColor.Gray, "사")
            };

            Console.Write("=         부제 : ");
            foreach (var (color, text) in charArr)
            {
                Console.ForegroundColor = color;
                Console.Write(text);
            }
            Console.WriteLine("       =");
        }
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("=                                  =");
            Console.WriteLine("=            <ACTION GO>           =");
            PrintSubtitle();
            Console.WriteLine("====================================");

            for (int i = 0; i < 3; i++)
            {
                // 현재 항목을 선택된 항목에 맞게 색상 설정
                if (i == nowIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // 선택된 항목을 초록색으로
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White; // 나머지 항목은 기본 색상으로
                }

                char cursor = (i == nowIndex) ? '>' : ' ';
                string text = i == 0 ? "1. 게임시작" : (i == 1 ? "2. 튜토리얼" : "3. 게임종료");
                Console.WriteLine($"=           {cursor}{text}           =");
            }

            Console.ResetColor();
            Console.WriteLine("====================================");
        }

        public override void Input()
        {
            inputKey = Console.ReadKey(true).Key;
        }

        public override void Update()
        {
            switch (inputKey)
            {
                case ConsoleKey.UpArrow:
                    nowIndex--;
                    if (nowIndex < 0) nowIndex = 2;
                    break;

                case ConsoleKey.DownArrow:
                    nowIndex++;
                    if (nowIndex > 2) nowIndex = 0;
                    break;

                case ConsoleKey.Enter:
                    if (nowIndex == 0)
                    {
                        gameManager.ChangeScene(SceneType.Main);
                    }
                    else if (nowIndex == 1)
                    {
                        gameManager.ChangeScene(SceneType.Tutorial);
                    }
                    else if (nowIndex == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("게임을 종료합니다. 감사합니다.");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                    }
                    break;

                default:
                    break;
            }
        }

        public override void Exit()
        {
        }
    }
}