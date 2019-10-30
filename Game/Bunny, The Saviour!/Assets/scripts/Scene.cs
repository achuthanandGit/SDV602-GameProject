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

        /// <summary>Initializes a new instance of the <see cref="Scene"/> class.</summary>
        /// <param name="pStory">The story.</param>
        public Scene(string pStory)
        {
            Story = pStory;
        }


        /// <summary>Initializes a new instance of the <see cref="Scene"/> class.</summary>
        /// <param name="pQuestion">The question.</param>
        /// <param name="pAnswer">The answer.</param>
        public Scene(string pQuestion, string pAnswer)
        {
            Question = pQuestion;
            Answer = pAnswer;
        }
    }
}
