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
        private int _Id;
        private int _GameId;
        private string _Username;
        private int _Health;
        private Double _TimeTaken;
        private int _CurrentLevel;
        private bool _IsFinished;
        private bool _IsWon;
        private DateTime _StartDateTime;
        private DateTime _EndDateTime;

        [PrimaryKey, AutoIncrement]
        public int Id { get => _Id; set => _Id = value; }
        public int GameId { get => _GameId; set => _GameId = value; }
        public string Username { get => _Username; set => _Username = value; }
        public int Health { get => _Health; set => _Health = value; }
        public double TimeTaken { get => _TimeTaken; set => _TimeTaken = value; }
        public int CurrentLevel { get => _CurrentLevel; set => _CurrentLevel = value; }
        public bool IsFinished { get => _IsFinished; set => _IsFinished = value; }
        public bool IsWon { get => _IsWon; set => _IsWon = value; }
        public DateTime StartDateTime { get => _StartDateTime; set => _StartDateTime = value; }
        public DateTime EndDateTime { get => _EndDateTime; set => _EndDateTime = value; }
    }
}
