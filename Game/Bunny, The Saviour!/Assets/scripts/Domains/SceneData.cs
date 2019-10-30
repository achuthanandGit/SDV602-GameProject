using SQLite4Unity3d;
using System;

namespace Assets.scripts.Domains
{

    /// <summary>Domain class used to store Scene data required for running the game</summary>
    [Serializable]
    public class SceneData
    {
        private int _SceneId;
        private string _Question;
        private string _Answer;
        private int _Level;

        [PrimaryKey]
        public int SceneId { get => _SceneId; set => _SceneId = value; }
        public string Question { get => _Question; set => _Question = value; }
        public string Answer { get => _Answer; set => _Answer = value; }
        public int Level { get => _Level; set => _Level = value; }
    }
}
