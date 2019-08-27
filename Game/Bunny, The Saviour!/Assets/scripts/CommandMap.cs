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

        /**
         * Constructor used to store values in CommandDictionary
         */
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

        /**
         * RunCommand is used to run the command given by the user
         * 
         * input {pStrCommand (type-string} - command given by the user}
         * output {result (type-bool) - true if command is valid else false}
         */
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

        /**
        * GetNextQuestion is used to get the next question when current question answer is correct
        * 
        * input {pStrCommand (type-string} - command given by the user}
        * output {result (type-bool) - true if command is valid else false}
        */
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
