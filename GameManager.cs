using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionGo.Scenes;
using ActionGo.Utils;
using ActionGO.Scenes;

namespace ActionGo
{
    public class GameManager
    {
        private static GameManager instance;
        private bool isRunning;
        private Scene[] scenes;
        private Scene curScene;

        private GameManager() { }

        public static GameManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
       }

        public void Start()
        {
            isRunning = true;
            // 맵 생성
            Map map = new Map(Constants.MAP_SIZE);

            // 씬 생성
            scenes = new Scene[(int)SceneType.Size];
            scenes[(int)SceneType.Title] = new TitleScene(this);
            scenes[(int)SceneType.Main] = new MainScene(this, map);
            scenes[(int)SceneType.Result] = new ResultScene(this);
            scenes[(int)SceneType.Tutorial] = new TutorialScene(this);
            scenes[(int)SceneType.Battle] = new BattleScene(this);

            // 최초 씬 설정
            curScene = scenes[(int)SceneType.Title];
            curScene.Enter();
        }

        public void Run()
        {
            Start();
            //Render(); //한번은 호출해야 최초 화면이 그려지는데
            while (isRunning)
            {
                Render(); //input, update보다 render가 우선되면, 현재루프에서는 이전 갱신내용이 출력되는거아닌가?
                Input();
                Update();   
            }
            End();
        }

        private void End()
        {
            curScene.Exit();
            // 기타 종료시 필요한 처리
        }

        private void Render()
        {
            curScene.Render();
        }

        private void Input()
        {
            curScene.Input();
        }

        private void Update()
        {
            curScene.Update();
        }

        public void ChangeScene(SceneType sceneType)
        {
            curScene.Exit();
            curScene = scenes[(int)sceneType];
            curScene.Enter();
        }
        public void ChangeScene(SceneType sceneType, int Score)
        {
            curScene.Exit();
            curScene = scenes[(int)sceneType];
            curScene.SetScore(Score);
            curScene.Enter();
        }

    }
}

