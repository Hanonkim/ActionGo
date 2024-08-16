using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ActionGo.Utils;
namespace ActionGo
{
    public class Map
    {
        private char[,] grid;
        private readonly Random random = new Random();
        public int Size { get; private set; }
        public Map(int size)
        {
            Size = size;
            grid = new char[Size, Size];

            InitGrid();
            Scattering(Constants.MIN_POINT);
        }
        
        private void InitGrid()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    grid[i, j] = Constants.BACKGROUND;
                }
            }
        }

        public void Scattering(int n)
        {
            while (n > 0)
            {
                int rndX = random.Next(0, Size);
                int rndY = random.Next(0, Size);
                
                //빈 타일에 적돌 배치
                if (grid[rndX, rndY] == Constants.BACKGROUND)
                {
                    grid[rndX, rndY] = Constants.TARGET;
                    n--;
                }
            }
        }
        public void PlaceStone(int x, int y, char stone)
        {
            if (x >= 0 && x < Size && y >= 0 && y < Size)
            {
                grid[x, y] = stone;
            }
            else
            {
                Console.WriteLine("잘못된 좌표입니다.");
            }
        }

        //String이 얕은 참조되지 않도록 깊은 참조로 전달해야함
        public char[,] GetMap()
        {
            char[,] clone = new char[Size, Size];

            //Array.Copy는 1차원 배열만 리턴한다고 함
            //따라서 각 행에 대해 Copy를 실행해야함
            for (int i = 0; i < Size; i++)
            {
                Array.Copy(grid, i * Size, clone, i * Size, Size);
            }

            return clone;
        }

        //Copy와 다르게 Clone은 1차원이든 2차원이든 가능. 편하지만 Copy처럼 디테일하게 복사는 못하고 통째로만 복사
        public void SetMap(char[,] newGrid)
        {
            grid = (char[,])newGrid.Clone();
        }

        public bool HasEmptyCells()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (grid[i, j] == Constants.BACKGROUND)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
