using System.Collections.Generic;

namespace Assets.scripts
{
    class CommandMap
    {
        // used to store the Commands used in the game
        private Dictionary<string, Command> CommandDictionary;

        // used to set the scene after runCommand
        public string Result = string.Empty;

        // used to set the question
        public string Question = string.Empty;

        // used to set the answer
        public string Answer = string.Empty;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMap"/> class.
        /// Store values in CommandDictionary.
        /// </summary>
        public CommandMap () {
            CommandDictionary = new Dictionary<string, Command>();
            CommandDictionary.Add("dialoguejackfirst", new NextCommand("DialogueJackFirst"));
            CommandDictionary.Add("dialoguetinkusecond", new NextCommand("DialogueTinkuSecond"));
            CommandDictionary.Add("dialoguejacksecond", new NextCommand("DialogueJackSecond"));
            CommandDictionary.Add("dialoguefinal", new NextCommand("DialogueFinal"));
            CommandDictionary.Add("gatherinfo", new NextCommand("GatherInfo"));

            CommandDictionary.Add("questiontwo", new AnswerQuestion("QuestionTwo"));
            CommandDictionary.Add("questionthree", new AnswerQuestion("QuestionThree"));
        }

        
        /// <summary>Runs the command.</summary>
        /// <param name="pStrCommand">The string command.</param>
        /// <returns>true/false</returns>
        public bool RunCommand(string pStrCommand)
        {
            Command aCommand;
            if(CommandDictionary.ContainsKey(pStrCommand))
            {
                aCommand = CommandDictionary[pStrCommand];
                aCommand.Do();
                Result = GameManager.GameManagerInstance.GameModelInstance.CurrentScene.Story;
                return true;
            }
            return false;
        }

        
        /// <summary>Gets the next question.</summary>
        /// <param name="pStrCommand">The string command.</param>
        /// <returns>true/false</returns>
        public bool GetNextQuestion(string pStrCommand)
        {
            Command aCommand;
            if (CommandDictionary.ContainsKey(pStrCommand))
            {
                aCommand = CommandDictionary[pStrCommand];
                aCommand.Do();
                Question = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Question;
                Answer = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Answer;
                return true;
            }
            return false;
        }
    }
}
