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
        
        /// <summary>Initializes a new instance of the <see cref="NextCommand"/> class.</summary>
        /// <param name="pAdVerb">The adverb.</param>
        public NextCommand(string pAdVerb)
        {
            adverb = pAdVerb;
        }

        
        /// <summary>Updates the current scene according to user command or action/summary>
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
        /// <summary>Initializes a new instance of the <see cref="NextCommand"/> class.</summary>
        /// <param name="pAdVerb">The adverb.</param>
        public AnswerQuestion(string pAdVerb)
        {
            adverb = pAdVerb;
        }

        
        /// <summary>Updates the current scene according to user command or action/summary>
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
