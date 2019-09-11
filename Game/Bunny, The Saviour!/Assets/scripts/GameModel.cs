using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.scripts
{
    [Serializable]
    public class GameModel
    {
        // Path to store user login details
        private static string FileName;

        // To store the user details
        public static Dictionary<string, User> UserLoginDetails = new Dictionary<string, User>();

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

        /// <summary>Initializes a new instance of the <see cref="GameModel"/> class.</summary>
        public GameModel()
        {
            GetAndSetGameData();
        }

        /// <summary>Initializes a new instance of the <see cref="GameModel"/> class.</summary>
        /// <param name="pFileName">path of the file.</param>
        public GameModel(string pFileName)
        {
            FileName = pFileName;
            GetAndSetGameData();
        }

        /// <summary>Gets the and set game data.</summary>
        private void GetAndSetGameData()
        {
            MakeStoryDescription();
            MakeDialogue();
            MakeGameRoomInteraction();
        }


        /// <summary>Makes the story description for Game Home scene.</summary>
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

        
        /// <summary>Makes the dialogue. For the character interaction.</summary>
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

        
        /// <summary>Makes the game room interaction. Question & answer session.</summary>
        private void MakeGameRoomInteraction()
        {
            QuestionOne = new Scene("What starts with a T, ends with a T, and has T in it.", "teapot");
            QuestionOne.QuestionTwo = new Scene("I stand when I'm sitting, and jump when I'm walking. Who am I?.", "kangaroo");
            QuestionOne.QuestionTwo.QuestionThree = new Scene("If you drop a yellow hat in the Red Sea what will it become?", "wet");

            CurrentQuestion = QuestionOne;
        }



        /// <summary>Saves the data.</summary>
        public static void SaveData()
        {
            try
            {
                BinaryFormatter lcFormatter = new BinaryFormatter();
                FileStream lcFileStream = File.Create(FileName);
                lcFormatter.Serialize(lcFileStream, UserLoginDetails);
            } catch(IOException exception)
            {
                Debug.Log("IOException occurs when store game data: " + exception);
            }
        }



        /// <summary>Retrieves the data.</summary>
        public static void RetrieveData()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    using (FileStream lcFileStream = new FileStream(FileName, FileMode.Open))
                    {
                        BinaryFormatter lcFormatter = new BinaryFormatter();
                        UserLoginDetails = (Dictionary<string, User>)lcFormatter.Deserialize(lcFileStream);
                    }
                }
            }catch (IOException exception)
            {
                Debug.Log("IOException occurs when load game data: " + exception);
            }
        }

    }
}
