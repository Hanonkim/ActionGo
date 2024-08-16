using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ActionGo.Utils;
using static System.Formats.Asn1.AsnWriter;
using System.Threading;

namespace ActionGo.Scenes
{
    public class MainScene : Scene
    {
        private int remains;
        private Map map;
        private ConsoleKey inputKey;
        private int nowIndex = 0;
        //2개의 데이터를 묶을 때? 튜플, 벡터. 연산이 지원되는건 벡터. 튜플로 해도 상관없을 듯?
        private readonly Vector2[] m_dir = new Vector2[] { new(0, -1), new(0, 1), new(-1, 0), new(1, 0) };
        private Timer scatteringTimer = null;
        private const int scatterInterval = Constants.scatterInterval;
        public int tScore { get; set; }
        public int Score { get; set; }
        public int GetStone { get; private set; }

        public MainScene(GameManager gameManager, Map map) : base(gameManager)
        {
            this.map = map;
            this.nowIndex = 0;
            this.tScore = 0;
            this.GetStone = 0;
            this.remains = Constants.MAP_SIZE * Constants.MAP_SIZE;

        }
        public override void SetScore(int score)
        {
            this.tScore = score;
        }
        private void ScatterStones(object state)
        {
            if (map.HasEmptyCells())
            {
                map.Scattering(1);
                Render();
            }
            else
            {
                Render();
                Console.WriteLine("$$$$$$$$$게임 종료$$$$$$$$$$");
                Thread.Sleep(2000);
                // 더 이상 착수할 지점이 없으면 결과 씬으로 전환
                gameManager.ChangeScene(SceneType.Result);
            }

        }

        public override void Enter()
        {
            //Enter할때마다 타이머를 설정시켜야 유저입장에서 맞을 듯. 이전 타이머가 유지되면 안되니까. 
            scatteringTimer = new Timer(ScatterStones, null, scatterInterval, scatterInterval);
            Score -= tScore; //Battle 결과에 따라 패배시 점수 하락
            tScore = 0; //이후 초기화
        }

        public override void Render()
        {
            Console.Clear();
            var displayMap = map.GetMap();

            int cursorX = nowIndex / map.Size;
            int cursorY = nowIndex % map.Size;
            
            Console.WriteLine("========<ACTION GO>=========");
            for (int i = 0; i < map.Size; i++)
            {
                Console.Write("        ");
                for (int j = 0; j < map.Size; j++)
                {
                    char cell = displayMap[i, j];

                    //커서 위치 빨간색
                    if (i == cursorX && j == cursorY)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('■');
                    }
                    else
                    {
                        //나머지 타입의 셀 색상 설정
                        switch (cell)
                        {
                            case Constants.BACKGROUND:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case Constants.TARGET:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case Constants.POINT:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }
                        Console.Write(cell);
                    }
                    Console.ResetColor(); //색상 리셋
                }
                Console.WriteLine();
            }
            Console.WriteLine("============================");
            if (GetStone != 0)
            {
                Console.WriteLine($"=  > {GetStone}개의 돌을 따냅니다!  =");
            }
            else
            {
                Console.WriteLine("=     좋은 하루 되세요.    =");
            }
            Console.WriteLine("=                          =");
            Console.WriteLine($"=         점수 = {Score}         =");
            Console.WriteLine("=                          =");
            Console.WriteLine("============================");
        }

        public override void Input()
        {
            inputKey = Console.ReadKey(true).Key;
        }

        public override void Update()
        {
            int x = nowIndex / map.Size;
            int y = nowIndex % map.Size;

            switch (inputKey)
            {
                case ConsoleKey.UpArrow:
                    if (nowIndex - map.Size >= 0)
                        nowIndex -= map.Size;
                    break;

                case ConsoleKey.DownArrow:
                    if (nowIndex + map.Size < map.Size * map.Size)
                        nowIndex += map.Size;
                    break;

                case ConsoleKey.LeftArrow:
                    if (nowIndex % map.Size > 0)
                        nowIndex--;
                    break;

                case ConsoleKey.RightArrow:
                    if (nowIndex % map.Size < map.Size - 1)
                        nowIndex++;
                    break;

                case ConsoleKey.Enter:
                    if (map.GetMap()[x, y] != Constants.BACKGROUND)
                    {
                        break; // 착수를 무시하고 탈출
                    }
                    map.PlaceStone(x, y, Constants.STONE_WHITE); //내 착수
                    char[,] tmpMap = map.GetMap();
                    FindTargetStone(x, y, tmpMap);
                    map.SetMap(tmpMap);
                    break;

                default:
                    break;
            }

            if (!map.HasEmptyCells())
            {
                Render();
                Console.WriteLine("$$$$$$$$$게임 종료$$$$$$$$$$");
                Thread.Sleep(2000);
                gameManager.ChangeScene(SceneType.Result);
            }
        }

        public override void Exit()
        {
            scatteringTimer.Dispose();
        }

        private void FindTargetStone(int _x, int _y, char[,] _map)
        {
            Queue<bool[,]> queue_DeadList = new(); //큐든 리스트든 뭐든 상관없음. 큐가 편해서 씀
            foreach (var dir in m_dir)
            {
                bool[,] visitedList = new bool[_map.GetLength(0), _map.GetLength(1)]; //혹시 몰라서 버퍼 비워주기
                if (StoneState.Clear == CheckStone(_x + (int)dir.X, _y + (int)dir.Y, _map, visitedList)) //해당 돌들은 살아있어서 체크할 필요없어서 continue
                    continue;

                //죽어있다고 판정
                queue_DeadList.Enqueue(visitedList);
            }

            //상하좌우 돌며 중복발생하는 걸 대비하여 해쉬 사용
            HashSet<Vector2> scoreList = new HashSet<Vector2>();

            while (queue_DeadList.Count > 0)
            {
                var index = queue_DeadList.Dequeue();

                for (int x = 0; x < index.GetLength(0); x++)
                {
                    for (int y = 0; y < index.GetLength(1); y++)
                    {
                        if (index[x, y] == true)
                        {
                            _map[x, y] = Constants.POINT; //죽어있는 것들을 POINT로 대체
                            scoreList.Add(new Vector2(x, y));
                        }
                    }
                }
            }

            int tmpScore = 0;
            int tmpScore2 = 0;
            GetStone = scoreList.Count;
            foreach (var score in scoreList) //scoreList 길이만큼 점수 가산
            {
                tmpScore++; //대마를 잡을수록 점수 가중. 추후 밸런스를 고려하여 연산식 수정 필요
                tmpScore2 += tmpScore;
                Score += tmpScore;
            }

            if(GetStone > 0)
            {
                gameManager.ChangeScene(SceneType.Battle, tmpScore2);
            }
        }
        private StoneState CheckStone(int _x, int _y, char[,] _map, bool[,] _visitedList)
        {
            if (_x < 0 || _y < 0) //경계 밖 벗어나면
                return StoneState.Block;
            if (_x >= _map.GetLength(0) || _y >= _map.GetLength(1)) //경계 밖 벗어나면
                return StoneState.Block;

            //방문한적있거나
            if (_visitedList[_x, _y] == true) //TARGET일 때에만 visited값이 TRUE로 설정되므로 TRUE면 TEAM임
                return StoneState.Team;

            switch (_map[_x, _y])
            {
                case Constants.STONE_WHITE: //내 돌로 막혀있음
                    return StoneState.Block;
                case Constants.TARGET: //자기 돌로 뚫려있듬
                    _visitedList[_x, _y] = true;
                    bool isClear = false;
                    foreach (var dir in m_dir) //상하좌우 체크
                    {
                        if (StoneState.Clear == CheckStone(_x + (int)dir.X, _y + (int)dir.Y, _map, _visitedList))
                            isClear = true;
                    }
                    if (isClear)
                        return StoneState.Clear;
                    else
                        return StoneState.Team;

                case Constants.POINT: //이 경우는 정의상 발생할 수 없음. 인접한 돌이 따먹힌 돌일 수가 없음
                    break;
                case Constants.BACKGROUND:
                    return StoneState.Clear; //BACKGROUND이면 Clear
                default:
                    break;
            }
            return StoneState.Error;
        }

    }
}

