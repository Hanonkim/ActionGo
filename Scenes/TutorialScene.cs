using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionGo.Utils;

namespace ActionGo.Scenes
{
    public class TutorialScene : Scene
    {
        private ConsoleKey inputKey;
        public TutorialScene(GameManager gameManager) : base(gameManager) { }
        public override void Enter()
        {

        }
        public override void Render()
        {
            ShowTutorial();
        }
        public override void Input()
        {
            inputKey = Console.ReadKey(true).Key;
        }
        public override void Update()
        {
            if (inputKey != ConsoleKey.None)
            {
                gameManager.ChangeScene(SceneType.Title);
            }
        }
        private void ShowTutorial()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("=             튜토리얼             =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("= 1. 게임 목표:                    =");
            Console.WriteLine("=    - 화면가득찰 때까지 진행하여  =");
            Console.WriteLine("=      최대한 많은 돌을 따내라!    =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("= 2. 기본 조작법:                  =");
            Console.WriteLine("=    - 이동 : 화살키               =");
            Console.WriteLine("=    - 착수 : Enter키              =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("= 3. 따내기 규칙                   =");
            Console.WriteLine("=    - 모든 곳을 가로막았을 때     =");
            Console.WriteLine("=      적이 메운 곳이 하나라도     =");
            Console.WriteLine("=      존재하면 인정되지 않습니다  =");
            Console.WriteLine("=      (적의 자살수 방지)          =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("= 4. 점수 획득 규칙                =");
            Console.WriteLine("=    - 한번에 더 많은 돌을 딸수록  =");
            Console.WriteLine("=      점수가 가중됩니다.          =");
            Console.WriteLine("=    - 따낼 때 전투에 진입하며     =");
            Console.WriteLine("=      승리/무승부하면 정상 획득,  =");
            Console.WriteLine("=      패배/포기하면 획득 불가     =");
            Console.WriteLine("=    - 따내려고 시도하는 돌의 수가 =");
            Console.WriteLine("=      많을수록 적은 강해집니다.   =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("= 5. 전투 규칙                     =");
            Console.WriteLine("=    - 둘 중 하나가 죽거나         =");
            Console.WriteLine("=      행동력을 소진할 때까지      =");
            Console.WriteLine("=      전투를 진행합니다           =");
            Console.WriteLine("=    - 나의 스펙은 2단계 수준입니다=");
            Console.WriteLine("=    - 따라서 0, 1단계의 적은      =");
            Console.WriteLine("=      반드시 이깁니다             =");
            Console.WriteLine("====================================");
            Console.WriteLine("아무 키 입력시 타이틀로 돌아갑니다.=");
        }
        public override void Exit()
        {
        }
    }
}
