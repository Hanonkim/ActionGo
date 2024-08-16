using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGo.Utils
{
    public enum SceneType { Title, Main, Result, Tutorial, Battle, Size } //Size로 길이 계산
    public enum StoneState
    {
        Block,
        Team,
        Clear,
        Error,
    }
    public static class Constants
    {
        public const int MAP_SIZE = 10;
        public const int MIN_POINT = 10;
        public const char BACKGROUND = '□';
        public const char TARGET = '■';
        public const char POINT = '$';
        public const char STONE_WHITE = '\u25CB';
        public const char STONE_BLACK = '\u25CF';
        public const char MONSTER = 'X';
        public const int scatterInterval = 1000;
    }
}
