using Assets.scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // used to know whether the game is running or not
    private bool IsRunning;

    // Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager GameManagerInstance;

    // Instance of GameModel
    public GameModel GameModelInstance;

    // To know which user using this instance. Will set when the user successfull login and will be removed when logout
    public string Username;

    // To define whether the user start a new game or join some random game
    public string GameMode;

    
    /// <summary>
    /// Awakes this instance.
    /// Awake is used to initialize any variables or game state before the game starts.
    /// Called only once during the lifetime of the script instance.
    /// </summary>
    private void Awake()
    {
        if (GameManagerInstance is null)
        {
            GameManagerInstance = this;
            IsRunning = true;
            GameModelInstance = new GameModel();
            GameModel.RetrieveData();
        }
        else
        {
            GameModel.SaveData();
            Destroy(gameObject);
            IsRunning = false;
        }
    }

   
    /// <summary>Determines whether [is game running].</summary>
    /// <returns>
    ///   <c>true</c> if [is game running]; otherwise, <c>false</c>.</returns>
    public bool IsGameRunning()
    {
        return IsRunning;
    }
}
