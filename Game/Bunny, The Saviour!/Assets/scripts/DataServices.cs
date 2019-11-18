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

    public SQLiteConnection Connection;

    public static SQLiteConnection _Connection;

    public static JSONDropService _JsnDrop;

   
    #region SQLite database path
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
        _JsnDrop = new JSONDropService { Token = "da308d33-ba58-413d-9abf-b987c9eac791" };

        Debug.Log("Final PATH: " + dbPath);
    }

    #endregion

    public static void JsnReceiverDel(JsnReceiver pReceived)
    {
        Debug.Log(pReceived.JsnMsg + " ..." + pReceived.Msg);
    }

    /// <summary>Creates the required tables for the game to run and maintain successfully.</summary>
    public void CreateRequiredTables()
    {
        //_Connection.CreateTable<User>();
        //_Connection.CreateTable<GameData>();
        //_Connection.CreateTable<UserGameData>();
        _Connection.CreateTable<SceneData>();
        //_Connection.CreateTable<ChatDetails>();
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
    /// <exception cref="SQLiteException">
    ///     If any error happens while saving data in database
    /// </exception>
    public static void SaveNewUser(User pObjUser)
    {
        try
        {
            _JsnDrop.Store<User,JsnReceiver>(new List<User> { pObjUser }, JsnReceiverDel);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when saving new user: " + exception);
        }
    }

    /// <summary>Checks the duplicate user.</summary>
    /// <param name="pUsername">The username.</param>
    /// <exception cref="Exception">
    ///     If any error happens while checking duplicate data
    /// </exception>
    internal static void CheckDuplicateUser(string pUsername)
    {
        try
        {
            string whereCondition = "Username='" + pUsername + "'";
            _JsnDrop.Select<User, JsnReceiver>(whereCondition, RegisterManager.CheckDuplicateRecieverDel, RegisterManager.CheckDuplicateErrorDel);
        }
        catch (Exception exception)
        {
            Debug.Log("Exception occurs when checking username is already taken or not: " + exception);
        }
    }

    /// <summary>Validates the username and password combination.</summary>
    /// <param name="pUsername">The username.</param>
    /// <param name="pPassword">The password.</param>
    /// <exception cref="Exception">
    ///     If any error happens while checking login credentials
    /// </exception>
    public static void CheckLogin(string pUsername, string pPassword)
    {
        try
        {
            
            string whereCondition = "Username='" + pUsername + "' and Password='" + pPassword + "'";
            _JsnDrop.Select<User, JsnReceiver>(whereCondition, LoginManager.JsnUserListReceiverDel, LoginManager.JsnUserListErrorDel);

        } catch (Exception exception)
        {
            Debug.Log("Exception occurs when checking login credentials: " + exception);
        }
    }

    /// <summary>
    /// Updates the user status and last login time when the user successfully logins
    /// </summary>
    /// <param name="pUser">The User</param>
    /// <exception cref="Exception">
    ///     If any error happens while updating login status
    /// </exception>
    internal static void UpdateUserStatus(User pUser)
    {
       try
        {
            _JsnDrop.Store<User, JsnReceiver>(new List<User> { pUser }, JsnReceiverDel);
        } catch (Exception exception)
        {
            Debug.Log("Exception occurs when updating user status and last login time: " + exception);
        }
    }

    /// <summary>Updates the login status.</summary>
    /// <param name="pUser">The User.</param>
    /// <exception cref="Exception">
    ///     If any error happens while updating login status
    /// </exception>
    public static void LogoutUser(User pUser)
    {
        try
        {
            string whereCondition = "Username='" + pUser.Username + "' and Password='" + pUser.Password + "'";
            _JsnDrop.Select<User, JsnReceiver>(whereCondition, GameHomeManager.JsnLogoutRecieverDel, JsnReceiverDel);
        }
        catch (Exception exception)
        {
            Debug.Log("Exception occurs when updating login status: " + exception);
        }
    }

    /// <summary>Starts the new game.</summary>
    /// <param name="pGameId">The game Id.</param>
    /// <exception cref="Exception">
    ///     If any error happens while creating new game
    /// </exception>
    internal static void StartNewGame(int pGameId)
    {
        try
        {
            GameData gameData = new GameData();
            gameData.GameId = pGameId;
            gameData.BestHealth = 0;
            gameData.BestTime = 0;
            gameData.CreateDateTime = DateTime.Now.Date.ToString();
            gameData.GameStatus = "active";
            gameData.Winner = string.Empty;
            GameModel.GameData = gameData;
            _JsnDrop.Store<GameData, JsnReceiver>(new List<GameData> { gameData }, GameHomeManager.JsnNewGameReceiverDel);
        } catch (Exception  exception)
        {
            Debug.Log("Exception occurs when starting new game: " + exception);
        }
    }

    /// <summary>
    ///  Updates the user game data when a user join or starts a new game.
    /// </summary>
    /// <param name="pGameId">The game Id.</param>
    /// <exception cref="Exception">
    ///     If any error happens while updating user game data
    /// </exception>
    public static void UpdateUserGameData(int pGameId)
    {
        try
        {
            UserGameData userGameData = new UserGameData();
            userGameData.Id = pGameId + GameModel.Username;
            userGameData.GameId = pGameId;
            userGameData.Username = GameModel.Username;
            userGameData.Health = 10;
            userGameData.TimeTaken = 0.0;
            userGameData.CurrentLevel = 1;
            userGameData.IsFinished = 0;
            userGameData.IsWon = 0;
            GameModel.UserGameData = userGameData;
            _JsnDrop.Store<UserGameData, JsnReceiver>(new List<UserGameData> { userGameData }, GameHomeManager.JsnLoadGameReceiverDel);
        }
        catch (Exception exception)
        {
            Debug.Log("Exception occurs when updating user data in game: " + exception);
        }
    }

    /// <summary>  Gets the available game list with active state.</summary>
    /// <exception cref="SQLiteException">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static void GetAvailableGameList()
    {
        try
        {
            string whereCondition = "GameStatus='active'";
            _JsnDrop.Select<GameData, JsnReceiver>(whereCondition, GameHomeManager.JsnRandomGameSuccessRecieverDel, GameHomeManager.JsnRandomGameFailRecieverDel);
        } catch (Exception exception)
        {
            Debug.Log("Exception occurs when getting avaialable game list to join: " + exception);
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
            _JsnDrop.Store<User, JsnReceiver>(new List<User> { pCurrentUser }, JsnReceiverDel);
            _JsnDrop.Store<UserGameData, JsnReceiver>(new List<UserGameData> { pUserGameData }, JsnReceiverDel);
        }
        catch (SQLiteException exception)
        {
            Debug.Log("SQLiteException occurs when updating user game data: " + exception);
        }
    }

    /// <summary> Retrieves the user list related to the game.</summary>
    /// <param name="pGameId">The game Id.</param>
    /// <exception cref="Exception">
    ///     If any error happens while getting game data 
    /// </exception>
    public static void GetGamePlayerList(int pGameId)
    {
        try
        {
            string whereCondition = "GameId=" + pGameId;
            _JsnDrop.Select<UserGameData, JsnReceiver>(whereCondition, GameRoomManager.JsnUserListSuccessRecieverDel, JsnReceiverDel);
        }
        catch (Exception exception)
        {
            Debug.Log("Exception occurs when getting user data in a game: " + exception);
        }
    }

    /// <summary>Gets the updated game data.</summary>
    /// <param name="pGameId">The game identifier.</param>
    /// <exception cref="Exception">
    ///     If any error happens while getting game data 
    /// </exception>
    internal static void GetUpdatedGameData(int pGameId)
    {
        try
        {
            string whereCondition = "GameId="+pGameId;
            _JsnDrop.Select<GameData, JsnReceiver>(whereCondition, GameRoomManager.JsnUpdateGameDataSuccessReciverDel, JsnReceiverDel);
        }
        catch (Exception exception)
        {
            Debug.Log("Exception occurs when getting game data: " + exception);
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
