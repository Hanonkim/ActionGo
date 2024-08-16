using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGo.Scenes
{
    public class ResultScene : Scene
    {
        public ResultScene(GameManager gameManager) : base(gameManager) { }
        public override void Enter()
        {

        }
        public override void Render()
        {
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("게임을 종료합니다. 감사합니다.");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
        public override void Input()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

        }
    }
}
