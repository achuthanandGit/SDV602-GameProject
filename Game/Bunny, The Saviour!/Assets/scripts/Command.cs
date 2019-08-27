namespace Assets.scripts
{
    public class Command
    {
        // used to track which command type need to process
        protected string adverb;

        public virtual void Do() {  }
    }

    public class NextCommand : Command
    {
        /**
         * Constructor to assign value to adverb
         * 
         * input {pAdverb (type-string)}
         */
        public NextCommand(string pAdVerb)
        {
            adverb = pAdVerb;
        }

        /**
         * Do is used to update the current scene of game instance 
         * according to the user command or action
         */
        public override void Do()
        {
            Scene aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene;
           switch(adverb)
            {
                case "DialogueJackFirst":
                    if (aScene.DialogueJackFirst != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene = aScene.DialogueJackFirst;
                    break;
                case "DialogueTinkuSecond":
                    if (aScene.DialogueTinkuSecond != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene = aScene.DialogueTinkuSecond;
                    break;
                case "DialogueJackSecond":
                    if (aScene.DialogueJackSecond != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene = aScene.DialogueJackSecond;
                    break;
                case "DialogueFinal":
                    if (aScene.DialogueFinal != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene = aScene.DialogueFinal;
                    break;
                case "GatherInfo":
                    if (aScene.GatherInfo != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentScene = aScene.GatherInfo;
                    break;
            }
        }
    }


    public class AnswerQuestion : Command
    {
        /**
        * Constructor to assign value to adverb
        * 
        * input {pAdverb (type-string)}
        */
        public AnswerQuestion(string pAdVerb)
        {
            adverb = pAdVerb;
        }

        /**
        * Do is used to update the current scene of game instance 
        * according to the user command or action
        */
        public override void Do()
        {
            Scene aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion;
            switch (adverb)
            {
                case "QuestionTwo":
                    if (aScene.QuestionTwo != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion = aScene.QuestionTwo;
                    break;
                case "QuestionThree":
                    if (aScene.QuestionThree != null)
                        aScene = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion = aScene.QuestionThree;
                    break;
            }
        }
    }
}
