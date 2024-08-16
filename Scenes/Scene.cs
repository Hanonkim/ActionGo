using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGo.Scenes
{
    public abstract class Scene
    {
        protected GameManager gameManager;

        public Scene(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        public abstract void Enter();
        public abstract void Render();
        public abstract void Input();
        public abstract void Update();
        public abstract void Exit();

        public virtual void SetScore(int score)
        {

        }
    }
}
