using SQLite4Unity3d;
using System;

namespace Assets.scripts.Domains
{
    /// <summary>Domain class to store game data</summary>
    [Serializable]
    public class GameData
    {
        private int _GameId;
        private int _BestHealth;
        private double _BestTime;
        private DateTime _CreateDateTime;
        private string _GameStatus;
        private string _Winner;

        [PrimaryKey]
        public int GameId { get => _GameId; set => _GameId = value; }
        public int BestHealth { get => _BestHealth; set => _BestHealth = value; }
        public double BestTime { get => _BestTime; set => _BestTime = value; }
        public DateTime CreateDateTime { get => _CreateDateTime; set => _CreateDateTime = value; }
        public string GameStatus { get => _GameStatus; set => _GameStatus = value; }
        public string Winner { get => _Winner; set => _Winner = value; }
    }
}
