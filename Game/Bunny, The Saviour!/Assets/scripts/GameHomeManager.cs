using Assets.scripts;
using Assets.scripts.Domains;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

/// <summary>Class used to manage the Home screen of the game where the basic outline of the game is provided and options to join a random game or to start new game.</summary>
public class GameHomeManager : MonoBehaviour
{
    // To set the Game story 
    public Text GameDescription;

    // MessagePanel to show error/warning/info messages
    public static GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public static Text MessagePanelText;

    // To load the profile picture if available
    public Image ProfilePicture;


    /// <summary>Starts this instance.
    /// Start method is used to initialize or assign values or actions to required variable or components before the first frame update.
    /// </summary>
    void Start()
    {
        GetAllComponents();
        LoadGameDescription();
        LoadProfilePicture();
    }

    /// <summary>
    /// Loads the profile picture.
    /// </summary>
    private void LoadProfilePicture()
    {
        Destroy(ProfilePicture.mainTexture);
        #region Get FIle Path


#if UNITY_EDITOR
        var path = string.Format(@"Assets/StreamingAssets/{0}", GameModel.Username);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, GameModel.Username);

        if (!File.Exists(filepath))
        {
           
#if UNITY_ANDROID
            var loadPic= new WWW("jar:file://" + Application.dataPath + "!/assets/" + GameModel.Username);  // this is the path to your StreamingAssets in android
            while (!loadPic.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadPic.bytes);
#elif UNITY_IOS
                 var loadPic = Application.dataPath + "/Raw/" + GameModel.Username;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadPic, filepath);
#elif UNITY_WP8
                var loadPic = Application.dataPath + "/StreamingAssets/" + GameModel.Username;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadPic, filepath);

#elif UNITY_WINRT
		var loadPic = Application.dataPath + "/StreamingAssets/" + GameModel.Username;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadPic, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadPic = Application.dataPath + "/Resources/Data/StreamingAssets/" + GameModel.Username;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadPic, filepath);
#else
	var loadPic = Application.dataPath + "/StreamingAssets/" + GameModel.Username;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadPic, filepath);

#endif

            Debug.Log("Database written");
        }

        var path = filepath;
#endif
        #endregion
        if(File.Exists(path+ ".png")) {
            ProfilePicture.enabled = true;
            byte[] bytes = File.ReadAllBytes(path + ".png");
            Texture2D texture = new Texture2D(33, 31);
            texture.filterMode = FilterMode.Trilinear;
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 33, 31), new Vector2(0.5f, 0.0f), 1.0f);
            ProfilePicture.sprite = sprite;
        } else
        {
            ProfilePicture.enabled = false;
        }
    }

    /// <summary>
    /// Gets all components.
    /// </summary>
    private void GetAllComponents()
    {
        MessagePanel = GameObject.Find("GameMessagePanel");
        MessagePanelText = MessagePanel.GetComponentInChildren<Text>();
        MessagePanel.SetActive(false);
    }


    /// <summary>Loads the game description.</summary>
    private void LoadGameDescription()
    {
        Debug.Log("Putting game description");
        GameDescription.text = GameModel.GameHomeDescription.Story;
    }

    
    /// <summary>Logouts the user session.</summary>
    public void LogoutUser()
    {
        Debug.Log("Logout User");
        string username = GameModel.Username;
        GameModel.LogoutUser(GameModel.CurrentUser);
    }

    /// <summary>
    /// JSNs the logout reciever delete.
    /// </summary>
    /// <param name="pReceivedList">The received list.</param>
    public static void JsnLogoutRecieverDel(List<User> pReceivedList)
    {
        Debug.Log("Successfull Logout");
        User objUser = pReceivedList[0];
        objUser.LoginStatus = "inactive";
        GameModel.UpdateUserStatus(objUser);
        GameModel.CurrentUser = null;
        GameModel.GameData = null;
        Debug.Log("Loading Login Scene");
        SceneManager.LoadScene("LoginScene");
    }


    /// <summary>Starts the new game.</summary>
    public void StartNewGame()
    {
        Debug.Log("Start New Game");
        // setting the game mode to 'new' for future refernece
        GameModel.GameMode = "new";
        // creating new game
        GameModel.StartNewGame();
    }

    /// <summary>
    ///  Reciever delegate for creating new game
    /// </summary>
    /// <param name="pReceived">The received.</param>
    public static void JsnNewGameReceiverDel(JsnReceiver pReceived)
    {
        Debug.Log("Response: " + pReceived);
        GameModel.UpdateUserGameData(GameModel.GameData.GameId);
    }

    /// <summary>
    /// JSNs the new game receiver delete.
    /// </summary>
    /// <param name="pReceived">The p received.</param>
    public static void JsnLoadGameReceiverDel(JsnReceiver pReceived)
    {
        Debug.Log("Response: " + pReceived);
        SceneManager.LoadScene("DialogScene");
    }


    /// <summary>Joins the random game.</summary>
    public void JoinRandomGame()
    {
        Debug.Log("Join Random Game");
        // setting the game mode to 'random' for future refernece
        GameModel.GameMode = "random";
        // finding random game to join
        GameModel.GetAvailableGameList(); 
    }

    /// <summary>
    /// Gets the random game from the recieved active game list
    /// </summary>
    /// <param name="pGameList">The p game list.</param>
    public static void JsnRandomGameSuccessRecieverDel(List<GameData> pGameList)
    {
        System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
        int randomIndex = rnd.Next(pGameList.Count);
        GameData gameData = pGameList[randomIndex];
        GameModel.GameData = gameData;
        GameModel.UpdateUserGameData(gameData.GameId);
    }

    /// <summary>
    /// Will throw error saying no games available to join
    /// </summary>
    /// <param name="pJsnReciever">The p JSN reciever.</param>
    public static void JsnRandomGameFailRecieverDel(JsnReceiver pJsnReciever)
    {
        MessagePanelText.text = "At the moment there is no active game to join. Please create a new game to start.";
        MessagePanel.SetActive(true);
    }



    /// <summary>
    /// Handles the message panel button.
    /// Close the message panel when OK button is clicked
    /// </summary>
    public void HandleMessagePanelButton()
    {
        MessagePanel.SetActive(false);
    }

   
}
