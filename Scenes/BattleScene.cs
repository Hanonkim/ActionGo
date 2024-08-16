using ActionGo;
using ActionGo.Scenes;
using ActionGo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ActionGO.Scenes
{
    public class BattleScene : Scene
    {
        private int score;
        private Mob mob;
        private int nowIndex;
        private ConsoleKey inputKey;
        private const int playerAttackPower = 50; // 플레이어의 고정 공격력. 게임기획이 없기 때문에...

        public BattleScene(GameManager gameManager) : base(gameManager)
        {
            mob = new Mob("돌의 원혼", 100, 50);
            nowIndex = 0;
        }

        public override void SetScore(int score)
        {
            this.score = score;
        }

        public override void Enter()
        {
            Console.Clear();
            Console.WriteLine("============================");
            Console.WriteLine("=                          =");
            if (score < 3)
            {
                mob.Health = 200;
                mob.AttackPower = 50;
                Console.WriteLine("=LV0돌의 원혼이 출현합니다.=");
            }
            else if (score >= 3 && score < 7)
            {
                mob.Health = 200;
                mob.AttackPower = 55;
                Console.WriteLine("=LV1돌의 원혼이 출현합니다.=");
            }
            else if (score >= 7 && score < 11)
            {
                mob.Health = 300;
                mob.AttackPower = 60;
                Console.WriteLine("=LV2돌의 원혼이 출현합니다.=");
            }
            else if (score >= 12)
            {
                mob.Health = 500;
                mob.AttackPower = 70;
                Console.WriteLine("=LV3돌의 원혼이 출현합니다.=");
            }
            Console.WriteLine("=                          =");
            Console.WriteLine("============================");
            Thread.Sleep(2000);
            Console.Clear();
        }

        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("=       <역경돌파: 돌의 원혼>      =");
            Console.WriteLine("=                                  =");
            Console.WriteLine("====================================");
            Console.WriteLine("=                                  =");

            char cursor0 = (nowIndex == 0) ? '>' : ' ';
            char cursor1 = (nowIndex == 1) ? '>' : ' ';

            Console.WriteLine($"=           {cursor0} 공격하기             =");
            Console.WriteLine($"=           {cursor1} 도망치기             =");
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
                    if (nowIndex < 0) nowIndex = 1;
                    break;

                case ConsoleKey.DownArrow:
                    nowIndex++;
                    if (nowIndex > 1) nowIndex = 0;
                    break;

                case ConsoleKey.Enter:
                    if (nowIndex == 0)
                    {
                        Battle();
                    }
                    else if (nowIndex == 1)
                    {
                        Console.Clear();
                        Console.WriteLine($"역경돌파에 실패했습니다! 점수가 {score}만큼 하락합니다.");
                        Thread.Sleep(2000);
                        gameManager.ChangeScene(SceneType.Main, score);
                    }
                    break;

                default:
                    break;
            }
        }

        public override void Exit()
        {
        }
        private void Battle()
        {
            int playerHealth = 400; // 플레이어의 초기 체력
            int turn = score; // 행동 가능 횟수만큼 전투를 진행
            Random random = new Random();

            Console.Clear();
            Console.WriteLine("전투 시작!");

            for (int i = 0; i < turn; i++)
            {
                float maxAttackModifier = 1.2f;
                float minAttackModifier = 0.8f;

                //따로 기획이 없기 때문에 전투경험은 고려하지 않고 기능만 구현
                //base + delta * (0.0 ~ 1.0) -> base로부터 delta만큼의 범위 내에서 무작위 실수값
                double damageMultiplier = minAttackModifier + (maxAttackModifier - minAttackModifier) * random.NextDouble(); // 80% ~ 120%

                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("=       <역경돌파: 돌의 원혼>      =");
                Console.WriteLine("=                                  =");
                Console.WriteLine("====================================");
                Console.WriteLine($"               턴 {i + 1}/{turn}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"플레이어 체력: {playerHealth}");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"돌의 원혼 체력: {mob.Health}");

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                int playerDmg = (int)(playerAttackPower * damageMultiplier);
                mob.Health -= playerDmg;
                Console.WriteLine($"플레이어가 {playerDmg}의 피해를 입혔습니다.");

                if (mob.Health <= 0)
                {
                    Console.WriteLine("돌의 원혼을 물리쳤습니다!");
                    Console.ResetColor();
                    EndBattle(true);
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                int MobDmg = (int)(mob.AttackPower * damageMultiplier);
                playerHealth -= MobDmg;
                Console.WriteLine($"돌의 원혼이 {MobDmg}의 피해를 입혔습니다.");

                if (playerHealth <= 0)
                {
                    Console.WriteLine("플레이어가 사망했습니다...");
                    Console.ResetColor();
                    EndBattle(false);
                    return;
                }

                Console.ResetColor();
                Thread.Sleep(2000);
            }

            //무승부
            Console.WriteLine("무승부! 아무도 죽지 않았습니다.");
            EndBattle(null);
        }

        private void EndBattle(bool? playerWon)
        {
            if (playerWon != false) //승리 or 무승부면 점수가 감소하지 않고 정상 획득
            {
                score = 0;
            }

            Console.ResetColor();
            Thread.Sleep(2000);
            gameManager.ChangeScene(SceneType.Main, score);
        }
    }
}
