using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    public class Scene
    {
        // To define the story
        public string Story;

        // To define  the question during game play
        public string Question;

        // To define the answer during game play
        public string Answer;

        /**
         * Constructor to assign values to Story variable
         */
        public Scene(string pStory) {
            Story = pStory;
        }

        /**
         * Constructor to assign values to Question and Answer variables
         */
        public Scene(string pQuestion, string pAnswer)
        {
            Question = pQuestion;
            Answer = pAnswer;
        }
                

        // for the first dialogue for Jack
        public Scene DialogueJackFirst;

        // for the second dialogue of Tinku
        public Scene DialogueTinkuSecond;

        // for the second dialogue Jack
        public Scene DialogueJackSecond;

        // for the final dialogue
        public Scene DialogueFinal;

        // for the final information 
        public Scene GatherInfo;

        // for level 1 2nd question
        public Scene QuestionTwo;

        // for level 1 3rd question
        public Scene QuestionThree;

        // for level 2 4th question
        public Scene QuestionFour;
    }
}
