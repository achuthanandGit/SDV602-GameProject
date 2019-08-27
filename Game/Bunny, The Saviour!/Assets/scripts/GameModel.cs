using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.scripts
{
    public class GameModel
    {
        // To store user login details
        private static string FileName = "D:\\achuthanandGit\\SDV602\\SDV602-GameProject\\Game\\Bunny, The Saviour!\\Assets\\UserDetails.dat";

        // To store user details
        public static Dictionary<string, Dictionary<string, string>> UserLoginDetails = new Dictionary<string, Dictionary<string, string>>();

        // To define current scene
        public Scene CurrentScene;

        // To define the story description
        public Scene DescriptionScene;

        // To define first scene
        public Scene FirstScene;

        // To define Question one in game room;
        public Scene QuestionOne;

        // To define current question
        public Scene CurrentQuestion;

        /**
         * Constructore to initailize the MakeDialogue method
         */
        public GameModel()
        {
            MakeStoryDescription();
            MakeDialogue();
            MakeGameRoomInteraction();
        }

        /**
         * 
         */
        private void MakeStoryDescription()
        {
            DescriptionScene = new Scene("\t\t\t Once there lived a bunny, Jack with his parents Will and Pink in a village close to a furious jungle. In the deep jungle " +
            "there also lived a fearsome monster in the castle who preyed on innocent animals. One day, Will and Pink along with their neighbour Tinku Duck " +
            "went to the jungle in search of fruits, vegetables and firewood. They were not able to find enough fruits in the outer jungle, so they went deep " +
            "into the forest.As they went deep, there was a garden full of fruits and vegetables. They ran into the garden without thinking twice to pick the fruits. " +
            "They are not aware of that it is monster's garden. " + "\n" +
            "\t\t\t They plucked enough fruits and were relaxing in the meadow. By that time the monster came back from his hunting. He saw these three uninvited animals" +
            " trespassed in his garden and found that they took his fruits without his permission. He got angry, rushed towards them caught Will and Pink Somehow Tinku" +
            " managed to escape from the monster and rushed to the village.");
        }

        /**
         * MakeDialogue is used to set the scene when an instance of GameModel is initialized
         */
        private void MakeDialogue()
        {
            FirstScene = new Scene("Jack, Jack...!!");
            FirstScene.DialogueJackFirst = new Scene("Hey Tinku. \n What happened? \n You seems to be so terrified.");
            FirstScene.DialogueJackFirst.DialogueTinkuSecond =
                new Scene("That monster!! \n He got your parents.");
            FirstScene.DialogueJackFirst.DialogueTinkuSecond.DialogueJackSecond =
                new Scene("What, What happened?");
            FirstScene.DialogueJackFirst.DialogueTinkuSecond.DialogueJackSecond.DialogueFinal =
                new Scene("I am gonna kill that monster and save them. $ Tinku explained what actually happened back in the jungle.");
            FirstScene.DialogueJackFirst.DialogueTinkuSecond.DialogueJackSecond.DialogueFinal.GatherInfo =
                new Scene("Jack learned from village elders that the monster won't kill his parent but will enslave them till they die. He realized that he can actually save his parents by " +
                    "completing certain tasks which will let him inside the castle. So he marched to the monster's castle.");

            // setting the first scene as current scene in the dialogue delivery session
            CurrentScene = FirstScene;
        }

        /**
         * MakeGameRoomInteraction is used to set the question and answer session for the player to play game.
         */
        private void MakeGameRoomInteraction()
        {
            QuestionOne = new Scene("What starts with a T, ends with a T, and has T in it.", "teapot");
            QuestionOne.QuestionTwo = new Scene("I stand when I'm sitting, and jumo when I'm walking. Who am I?.", "kangaroo");
            QuestionOne.QuestionTwo.QuestionThree = new Scene("If you drop a yellow hat in the Red Sea what will it become?", "wet");

            CurrentQuestion = QuestionOne;
        }

        /**
         * SaveData is used to save the user login details in a file
         */
        public static void SaveData()
        {
            using (FileStream lcFileStream = new FileStream(FileName, FileMode.Create))
            {
                BinaryFormatter lcFormatter = new BinaryFormatter();
                lcFormatter.Serialize(lcFileStream, UserLoginDetails);
            }
        }

        /**
         * RetrieveData is used to retrieve saved user login details from a file
         */
        public static void RetrieveData()
        {
            using (FileStream lcFileStream = new FileStream(FileName, FileMode.Open))
            {
                BinaryFormatter lcFormatter = new BinaryFormatter();
                UserLoginDetails = (Dictionary<string, Dictionary<string, string>>)lcFormatter.Deserialize(lcFileStream);
            }
        }
    }
}
