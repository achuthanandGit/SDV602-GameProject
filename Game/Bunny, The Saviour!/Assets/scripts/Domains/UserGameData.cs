using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.Domains
{
    /// <summary>Domain class to store game related user data</summary>
    [Serializable]
    public class UserGameData

    { 
        private string _Id;
        private int _GameId;
        private string _Username;
        private int _Health;
        private Double _TimeTaken;
        private int _CurrentLevel;
        private int _IsFinished;
        private int _IsWon;

        [PrimaryKey]
        public string Id { get => _Id; set => _Id = value; }
        public int GameId { get => _GameId; set => _GameId = value; }
        public string Username { get => _Username; set => _Username = value; }
        public int Health { get => _Health; set => _Health = value; }
        public double TimeTaken { get => _TimeTaken; set => _TimeTaken = value; }
        public int CurrentLevel { get => _CurrentLevel; set => _CurrentLevel = value; }
        public int IsFinished { get => _IsFinished; set => _IsFinished = value; }
        public int IsWon { get => _IsWon; set => _IsWon = value; }
    }
}
