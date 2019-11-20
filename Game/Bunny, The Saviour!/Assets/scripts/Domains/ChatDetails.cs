using SQLite4Unity3d;
using System;

namespace Assets.scripts.Domains
{
    /// <summary> Domain class used to store chat data for the communcation between users in a game </summary>
    [Serializable]
    class ChatDetails
    {
        private string _Id;
        private int _GameId;
        private string _Username;
        private string _Message;
        private string _SendTime;


        [PrimaryKey]
        public string Id { get => _Id; set => _Id = value; }
        public int GameId { get => _GameId; set => _GameId = value; }
        public string Username { get => _Username; set => _Username = value; }
        public string Message { get => _Message; set => _Message = value; }
        public string SendTime { get => _SendTime; set => _SendTime = value; }

    }
}
