using Assets.scripts.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.scripts
{

    public static class GameModel
    {
        // used to know whether the game is running or not
        public static bool IsRunning;

        // To know which user using this instance. Will set when the user successfull login and will be removed when logout
        public static string Username;

        // To define whether the user start a new game or join some random game
        public static string GameMode;
        
        // To define the game description
        public static Scene GameHomeDescription;

        // To define the scene details to show the dialogue delivery
        public static List<SceneData> DialogueList = new List<SceneData>();

        // To save the current login user
        public static User CurrentUser;

        // To define user game data
        public static UserGameData UserGameData;

        // To save current game details
        public static GameData gameData;

        // To define the game scene data according to levels
        public static IDictionary<int, List<SceneData>> playSceneMap = new Dictionary<int, List<SceneData>>();

        // To define the current level scene list
        public static List<SceneData> currentLevelSceneList = new List<SceneData>();

        // To define the current level
        public static int currentLevel;

        // To define how many need to pass to complete the current level
        public static int currentLevelTotalCount;


        /// <summary>Gets the and set game data.</summary>
        public static void GetAndSetGameData()
        {
            // creating game database if required
            Debug.Log("Creating database");
            var dataServiceObj = new DataService("gameDB.db");

            // creating game tables if required
            Debug.Log("Creating Tables");
            dataServiceObj.CreateRequiredTables();

            // inserting base game data 
            Debug.Log("Inserting game data");
            dataServiceObj.InsertBaseGameData();

            Debug.Log("Getting game Data");
            MakeGameData();
            // MakeGameRoomInteraction();
        }

        /// <summary>Makes the story description for Game Home scene.</summary>
        public static void MakeGameData()
        {
            List<SceneData>  gameSceneList = DataService.GetGameSceneData();
            if (gameSceneList != null && gameSceneList.Count > 0)
            {
                Debug.Log("Successfully retrievs game data.");
                // getting the game description data from the scene list
                SceneData GameDescData = gameSceneList.First(scene => scene.SceneId == 1111);
                GameHomeDescription = new Scene(GameDescData.Question);
                Debug.Log(GameHomeDescription);
                // getting dialog scene data according to the dialog delivery order
                DialogueList = gameSceneList.Where(scene => scene.SceneId >= 1112)
                    .OrderBy(scene => scene.SceneId).ToList();
                Debug.Log(DialogueList);
                // minimizing the GameSceneList to the data required to activate the game room
                gameSceneList = gameSceneList.Except(DialogueList).ToList();
                gameSceneList.Remove(GameDescData);
                playSceneMap = gameSceneList.GroupBy(scene => scene.Level)
                                            .ToDictionary(scene => scene.Key, scene => scene.ToList());
            }
            else
                Debug.Log("Failed to retrieve game data");
        }

        

        /// <summary>Saves the new user.</summary>
        /// <param name="pObjUser">The object user.</param>
        /// <returns>
        ///     true: if user data saved successfully
        ///     false: if user data fail to save       
        /// </returns>
        public static bool SaveNewUser(User pObjUser)
        {
            return DataService.SaveNewUser(pObjUser);
        }

        /// <summary>Checks the duplicate user.</summary>
        /// <param name="pUsername">The username.</param>
        /// <returns>
        ///    true: if username already exists
        ///    false: if username not taken
        /// </returns>
        public static bool CheckDuplicateUser(string pUsername)
        {
            return DataService.CheckDuplicateUser(pUsername);
        }

        /// <summary>Validates the username and password combination.</summary>
        /// <param name="pUsername">The username.</param>
        /// <param name="pPassword">The password.</param>
        /// <returns>
        ///    User: If the given values are valid
        ///    null: if the given values are not valid
        /// </returns>
        public static User CheckLogin(string pUsername, string pPassword)
        {
            return DataService.CheckLogin(pUsername, pPassword);
        }

        /// <summary>Logouts the user.</summary>
        /// <param name="pUsername">The username.</param>
        public static void LogoutUser(string pUsername)
        {
            DataService.LogoutUser(pUsername);
        }

        /// <summary>Updates the user game data.</summary>
        /// <param name="pCurrentUser">The current user.</param>
        /// <param name="pUserGameData">The user game data.</param>
        public static void UpdateUserGameData(User pCurrentUser, UserGameData pUserGameData)
        {
            DataService.UpdateUserGameData(GameModel.CurrentUser, GameModel.UserGameData);
        }


        /// <summary>Retrievs the payerlist in a game.</summary>
        /// <returns>
        ///     List<UserGameData>: If no issues happens while getting data
        ///     null: If any exceptions when getting data
        /// </returns>
        public static List<UserGameData> GetGamePayerlist()
        {
            return DataService.GetGamePlayerList(gameData.GameId);
        }

        /// <summary>Starts the new game.</summary>
        public static void StartNewGame()
        {
            // generating random gameId for the uniqueness processs
            System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
            int gameId = rnd.Next();
            gameData = DataService.StartNewGame(gameId);
        }

        /// <summary>Gets the updated game data.</summary>
        /// <param name="pGameId">The game identifier.</param>
        /// <returns>
        ///     GameData obj: if data retrieved successfully
        ///     null : if error happens
        /// </returns>
        internal static GameData GetUpdatedGameData(int pGameId)
        {
            return DataService.GetUpdatedGameData(pGameId);
        }
        
        /// <summary>Gets the random game to join.</summary>
        /// <returns>
        ///     true: If able to find random game
        ///     false: If not able to fins random game
        /// </returns>
        public static bool GetRandomGameToJoin()
        {
            List<GameData> gameList = DataService.GetAvailableGameList();
            if (gameList.Count == 0)
                return false;
            else
            {
                // getting random game to join from the list of active games
                System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
                int randomIndex = rnd.Next(gameList.Count);
                gameData = gameList[randomIndex];
                DataService.UpdateUserGameData(gameData.GameId);
                return true;
            }
        }

        /// <summary>Updates the game data.</summary>
        /// <param name="pGameDate">The game date.</param>
        internal static void UpdateGameData(GameData pGameDate)
        {
            DataService.UpdateGameData(pGameDate);
        }
    }
}
