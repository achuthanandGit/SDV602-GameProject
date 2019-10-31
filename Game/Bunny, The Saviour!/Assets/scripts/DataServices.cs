using SQLite4Unity3d;
using UnityEngine;
using System.Linq;
using System;
using Assets.scripts.Domains;
using Assets.scripts;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{

    private static SQLiteConnection _Connection;

    /// <summary>
    ///  Initializes a new instance of the <see cref="DataService"/> class and creates database in the application path
    /// </summary>
    /// <param name="pDatabaseName">Name of the database.</param>
    public DataService(string pDatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", pDatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, pDatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + pDatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + pDatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + pDatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + pDatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + pDatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + pDatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _Connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);
    }


    /// <summary>Creates the required tables for the game to run and maintain successfully.</summary>
    public void CreateRequiredTables()
    {
        _Connection.CreateTable<User>();
        _Connection.CreateTable<GameData>();
        _Connection.CreateTable<UserGameData>();
        _Connection.CreateTable<SceneData>();
        _Connection.CreateTable<ChatDetails>();
        Debug.Log("tables created for the game");
    }

    /// <summary> Inserts the game data required for the successfull running of game </summary>
    public void InsertBaseGameData()
    {
        SetSceneData setSceneDataObj = new SetSceneData();
        InsertSceneData(setSceneDataObj.LocalSceneList);
    }

    /// <summary>Inserts the game scene data.</summary>
    /// <param name="pLocalSceneList">The local scene list.</param>
    ///  <exception cref="SQLiteException">
    ///     If any error happens while inserting game data
    /// </exception>
    private void InsertSceneData(List<SceneData> pLocalSceneList)
    {
        try
        {
            _Connection.InsertAll(pLocalSceneList);
            Debug.Log("Inserted game data");
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when inserting game data: " + exception);
        }
    }

    /// <summary>Gets the whole game scene data.</summary>
    /// <returns>
    ///     returns list of SceneData if no exceptions happens
    ///     returns null of exception happens
    /// </returns>
    ///  <exception cref="SQLiteException">
    ///     If any error happens while inserting game data
    /// </exception>
    public static List<SceneData> GetGameSceneData()
    {
        try
        {
            Debug.Log("returning game data");
            return _Connection.Table<SceneData>().ToList();
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when getting Game data: " + exception);
            return null;
        }
    }


    /// <summary>Saves the new user.</summary>
    /// <param name="pObjUser">The object user.</param>
    /// <returns>
    ///     true: if user data saved successfully
    ///     false: if user data fail to save or occurs SQLiteException
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while saving data in database
    /// </exception>
    public static bool SaveNewUser(User pObjUser)
    {
        try
        {
            _Connection.Insert(pObjUser);
            return true;
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when saving new user: " + exception);
            return false;
        }
    }

    /// <summary>Checks the duplicate user.</summary>
    /// <param name="pUsername">The username.</param>
    /// <returns>
    ///     true: if username is already taken or occurs SQLiteException
    ///     false: if username is available
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while checking duplicate data
    /// </exception>
    internal static bool CheckDuplicateUser(string pUsername)
    {
        try
        {
            List<User> userDetailList = _Connection.Table<User>().ToList();
            if (userDetailList.Count == 0)
                return false;
            else
                return (userDetailList.Where(x => x.Username == pUsername).ToList().Count != 0);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when checking username is already taken or not: " + exception);
            return true;
        }
    }

    /// <summary>Validates the username and password combination.</summary>
    /// <param name="pUsername">The username.</param>
    /// <param name="pPassword">The password.</param>
    /// <returns>
    ///    User: If the given values are valid
    ///    null: if the given values are not valid
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while checking login credentials
    /// </exception>
    public static User CheckLogin(string pUsername, string pPassword)
    {
        try
        {
            List<User> userDetailList = _Connection.Table<User>().ToList();
            if (userDetailList.Count() != 0 && userDetailList.Where(x => x.Username == pUsername && x.Password == pPassword).ToList().Count != 0)
            {
                User currentUser = userDetailList.Where(x => x.Username == pUsername && x.Password == pPassword).ToList().FirstOrDefault();
                UpdateUserStatus(pUsername, pPassword);
                return currentUser;
            }
            else
                return null;
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when checking login credentials: " + exception);
            return null;
        }
    }
 

    /// <summary>Updates the login status.</summary>
    /// <param name="pUsername">The username.</param>
    /// <param name="pPassword">The password.</param>
    /// <exception cref="SQLiteException">
    ///     If any error happens while updating login status
    /// </exception>
    public static void UpdateUserStatus(string pUsername, string pPassword )
    {
        try
        {
            User loginUserObj = _Connection.Table<User>().Where(x => x.Username == pUsername && x.Password == pPassword).ToList().FirstOrDefault();
            loginUserObj.LoginStatus = "active";
            loginUserObj.Lastlogin = DateTime.Now;
            GameModel.CurrentUser = loginUserObj;
            _Connection.Update(loginUserObj);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating login status: " + exception);
        }
    }

    /// <summary>Updates the login status.</summary>
    /// <param name="pUsername">The username.</param>
    /// <exception cref="SQLiteException">
    ///     If any error happens while updating login status
    /// </exception>
    public static void LogoutUser(string pUsername)
    {
        try
        {
            User loginUserObj = _Connection.Table<User>().Where(x => x.Username == pUsername).FirstOrDefault();
            loginUserObj.LoginStatus = "inactive";
            _Connection.Update(loginUserObj);
            GameModel.CurrentUser = null;
            GameModel.gameData = null;
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating login status: " + exception);
        }
    }

    /// <summary>Starts the new game.</summary>
    /// <param name="pGameId">The game Id.</param>
    /// <returns>
    ///    GameData: If the game is created successfully
    ///    null: if fails to create new game
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while creating new game
    /// </exception>
    internal static GameData StartNewGame(int pGameId)
    {
        try
        {
            GameData gameData = new GameData();
            gameData.GameId = pGameId;
            gameData.BestHealth = 0;
            gameData.BestTime = 0;
            gameData.CreateDateTime = DateTime.Now;
            gameData.GameStatus = "active";
            gameData.Winner = string.Empty;
            _Connection.Insert(gameData);
            UpdateUserGameData(pGameId);
            return gameData;
        } catch (SQLiteException  exception)
        {
            Debug.Log("SQLiteException occurs when starting new game: " + exception);
            return null;
        }
    }

    /// <summary>
    ///  Updates the user game data when a user join or starts a new game.
    /// </summary>
    /// <param name="pGameId">The game Id.</param>
    /// <exception cref="SQLiteException">
    ///     If any error happens while updating user game data
    /// </exception>
    public static void UpdateUserGameData(int pGameId)
    {
        try
        {
            UserGameData userGameData = new UserGameData();
            userGameData.GameId = pGameId;
            userGameData.Username = GameModel.Username;
            userGameData.Health = 10;
            userGameData.TimeTaken = 0.0;
            userGameData.CurrentLevel = 1;
            userGameData.IsFinished = false;
            userGameData.IsWon = false;
            userGameData.StartDateTime = DateTime.Now;
            GameModel.UserGameData = userGameData;
            _Connection.Insert(userGameData);

        }catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating user data in game: " + exception);
        }
    }

    /// <summary>  Gets the available game list with active state.</summary>
    /// <returns>
    ///     List<GameData>: If no issues happens while getting data
    ///     null: If any exceptions when getting data
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static List<GameData> GetAvailableGameList()
    {
        try
        {
            return _Connection.Table<GameData>().Where(x => x.GameStatus == "active").ToList();
        } catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when getting avaialable game list to join: " + exception);
            return null;
        }
    }

    /// <summary>  Updates the user game data.</summary>
    /// <param name="pCurrentUser">The current user.</param>
    /// <param name="pUserGameData">The user game data.</param>
    /// <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static void UpdateUserGameData(User pCurrentUser, UserGameData pUserGameData)
    {
        try
        {
            _Connection.Update(pCurrentUser);
            _Connection.Update(pUserGameData);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating user game data: " + exception);
        }
    }

    /// <summary> Retrieves the user list related to the game.</summary>
    /// <param name="pGameId">The game Id.</param>
    /// <returns>
    ///     List<UserGameData>: If no issues happens while getting data
    ///     null: If any exceptions when getting data
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static List<UserGameData> GetGamePlayerList(int pGameId)
    {
        try
        {
            return _Connection.Table<UserGameData>().Where(x => x.GameId == pGameId).ToList();
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when getting user data in a game: " + exception);
            return null;
        }
    }

    /// <summary>Gets the updated game data.</summary>
    /// <param name="pGameId">The game identifier.</param>
    /// <returns>
    ///     GameData obj: if data retrieved successfully
    ///     null : if error happens
    /// </returns>
    /// <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static GameData GetUpdatedGameData(int pGameId)
    {
        try
        {
            return _Connection.Table<GameData>().Where(x => x.GameId == pGameId).FirstOrDefault();
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when getting game data: " + exception);
            return null;
        }
    }

    /// <summary>Updates the game data.</summary>
    /// <param name="pGameDate">The game date.</param>
    ///  <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static void UpdateGameData(GameData pGameDate)
    {
        try
        {
            _Connection.Update(pGameDate);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating game data: " + exception);
        }
    }
}
