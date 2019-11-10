using System;
using SQLite4Unity3d;

namespace Assets.scripts.Domains
{
    /// <summary>Domain class to save user details</summary>
    [Serializable]
    public class User
    {
        private string _Username;
        private string _Password;
        private string _Email;
        private string _LoginStatus;
        private int _GamesWon;
        private int _BestHealth;
        private double _BestTime;
        private DateTime _Lastlogin;

        [PrimaryKey]
        public string Username { get => _Username; set => _Username = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string Email { get => _Email; set => _Email = value; }
        public string LoginStatus { get => _LoginStatus; set => _LoginStatus = value; }
        public int GamesWon { get => _GamesWon; set => _GamesWon = value; }
        public int BestHealth { get => _BestHealth; set => _BestHealth = value; }
        public double BestTime { get => _BestTime; set => _BestTime = value; }
        public DateTime Lastlogin { get => _Lastlogin; set => _Lastlogin = value; }
        
    }
}
