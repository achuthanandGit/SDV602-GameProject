using Assets.scripts;
using Assets.scripts.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Class used to manage the login process of user who has already registered</summary>
public class LoginManager : MonoBehaviour
{

    public Text directionText;
    // MessagePanel to show error/warning/info messages
    //public static GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    //public static Text MessagePanelText;

    // UsernameText to get username data
    public static InputField UsernameText;

    // PasswordText to get password data
    public static InputField PasswordText;

    // Errortext to show error messages
    public static Text Errortext;

    private void Update()
    {
        float xdeg = Input.acceleration.x;
        float ydeg = Input.acceleration.y;
        if(xdeg > 0.5)
        {
            directionText.text = "east";
        } else if(xdeg < -0.5)
        {
            directionText.text = "west";
        }
        else if(ydeg > 0.4)
        {
            directionText.text = "north";
        }
        else if(ydeg < -0.5)
        {
            directionText.text = "south";
        }
    }

    /// <summary>Start is used to load the GameObjects or actions when the scene gets loaded.</summary>
    public void Start()
    {
        GetAllLoginComponents();
        directionText = GameObject.Find("DirectionText").GetComponent<Text>();
    }

    /// <summary>
    /// Gets all components required for the login process.
    /// </summary>
    private void GetAllLoginComponents()
    {
        UsernameText = GameObject.Find("UsernameInput").GetComponent<InputField>();
        PasswordText = GameObject.Find("PasswordInput").GetComponent<InputField>();
        Errortext = GameObject.Find("ErrorText").GetComponent<Text>();   
    }



    /// <summary>
    ///   Handles Login button.
    ///   If valid then navigates to GameHomeScene.
    ///   If not valid, will throw error saying so.
    ///   If all fields are filled but, username doesn't exists or username password combination is not valid, will throw error saying so.
    /// </summary>
    public void HandleLoginButton()
    {
        Errortext.text = String.Empty;
        if (string.IsNullOrWhiteSpace(UsernameText.text) ||
            string.IsNullOrWhiteSpace(PasswordText.text))
        {
            Debug.Log("Input data error");
            Errortext.text = "Fill all required fields.";
          //  MessagePanelText.text = "Fill all required fields.";
           // MessagePanel.SetActive(true);
        } else
        {
            GameModel.CheckLogin(UsernameText.text, PasswordText.text);
        }
    }

    public static void JsnUserListReceiverDel(List<User> pReceivedList)
    {
        Debug.Log("Received items " + pReceivedList.Count());
        if (pReceivedList.Count == 1)
        {
            Debug.Log("Successfull Login");
            User currentUser = pReceivedList[0];
            currentUser.LoginStatus = "active";
            currentUser.Lastlogin = DateTime.Now.ToString();
            GameModel.UpdateUserStatus(currentUser);
            GameModel.Username = currentUser.Username;
            GameModel.CurrentUser = currentUser;
            SceneManager.LoadScene("GameHome");
        }
    }


    public static void JsnUserListErrorDel(JsnReceiver pReceived)
    {
        Debug.Log("Login Error");
        Errortext.text = "Username/Password is wrong. please try again with valid data.";
        //MessagePanelText.text = "Username/Password is wrong. please try again with valid data.";
        //MessagePanel.SetActive(true);
    }

    /// <summary>
    /// Handles the message panel button.
    /// Close the message panel when OK button is clicked
    /// </summary>
    public void HandleMessagePanelButton()
    {
    //    MessagePanel.SetActive(false);
    }

    /// <summary>Loads the register scene to register new user</summary>
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }
    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }


    #region testing JSN DROP

    public void jsnReceiverDel(JsnReceiver pReceived)
    {
        Debug.Log(pReceived.JsnMsg + " ..." + pReceived.Msg);
        // To do: parse and produce an appropriate response
    }

    public void jsnListReceiverDel(List<User> pReceivedList)
    {
        Debug.Log("Received items " + pReceivedList.Count());
        foreach (User lcReceived in pReceivedList)
        {
            string ss = lcReceived.Username + "," + lcReceived.Password + "," + lcReceived.BestHealth.ToString();
            Debug.Log(ss);
        }// for

        // To do: produce an appropriate response
    }

    public void clickButton()
    {

        //JSONDropService jsDrop = new JSONDropService { Token = "da308d33-ba58-413d-9abf-b987c9eac791" };
        //CreateTables(jsDrop);

        // // Create table person
        // jsDrop.Create<User, JsnReceiver>(new User
        // {
        //     Username = "achu",
        //     Password = "achu",
        //     BestHealth = 10,
        //     BestTime = 20.00,
        //     Email = "achu@gmail.com",
        //     LoginStatus = "active",
        //     GamesWon = 1,
        //     Lastlogin = DateTime.Now.ToString()
        // }, jsnReceiverDel);
        // Debug.Log(DateTime.Now);





        // // Store people records
        // jsDrop.Store<User, JsnReceiver>(new List<User>
        // {
        //     new User{
        //     Username = "achu",
        //     Password = "achu",
        //     BestHealth = 10,
        //     BestTime = 20.00,
        //     Email = "achu@gmail.com",
        //     LoginStatus = "active",
        //     GamesWon = 1,
        //     Lastlogin = DateTime.Now.ToString()
        //     },
        //     new User{
        //     Username = "manu",
        //     Password = "manu",
        //     BestHealth = 10,
        //     BestTime = 20.00,
        //     Email = "manu@gmail.com",
        //     LoginStatus = "active",
        //     GamesWon = 1,
        //     Lastlogin = DateTime.Now.ToString()
        //     },
        //     new User{
        //     Username = "vasu",
        //     Password = "vasu",
        //     BestHealth = 10,
        //     BestTime = 20.00,
        //     Email = "vasu@gmail.com",
        //     LoginStatus = "active",
        //     GamesWon = 1,
        //     Lastlogin = DateTime.Now.ToString()
        //     },
        //  }, jsnReceiverDel);


        // jsDrop.All<User, JsnReceiver>(jsnListReceiverDel, jsnReceiverDel);

        // // Retreive all people records
        // jsDrop.All<User, JsnReceiver>(jsnListReceiverDel, jsnReceiverDel);

        // jsDrop.Select<User, JsnReceiver>("HighScore > 200",jsnListReceiverDel, jsnReceiverDel);

        // jsDrop.Delete<User, JsnReceiver>("PersonID = 'Jonny'", jsnReceiverDel);
        //jsDrop.Drop<User, JsnReceiver>(jsnReceiverDel);
        //jsDrop.Drop<GameData, JsnReceiver>(jsnReceiverDel);
        //jsDrop.Drop<UserGameData, JsnReceiver>(jsnReceiverDel);
        //jsDrop.Drop<SceneData, JsnReceiver>(jsnReceiverDel);



    }

    private void CreateTables(JSONDropService jsDrop)
    {

        //jsDrop.Create<User, JsnReceiver>(new User
        //{
        //    Username = "achutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt",
        //    Password = "achutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt",
        //    BestHealth = 100,
        //    BestTime = 999.999,
        //    Email = "achuachutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt@gmail.com",
        //    LoginStatus = "inactiveinactiveinactive",
        //    GamesWon = 100,
        //    Lastlogin = DateTime.Now.ToString()
        //}, jsnReceiverDel);
        System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
        int gameId = rnd.Next();
        //jsDrop.Create<GameData, JsnReceiver>(new GameData
        //{
        //    GameId = gameId,
        //    BestHealth = 100,
        //    BestTime = 999.999,
        //    CreateDateTime = DateTime.Now.ToString(),
        //    GameStatus = "inactiveinactiveinactive",
        //    Winner = "achutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt"
        //}, jsnReceiverDel);
        jsDrop.Create<UserGameData, JsnReceiver>(new UserGameData
        {
            Id = gameId + "achutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt",
            GameId = gameId,
            Username = "achutttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt",
            Health = 100,
            TimeTaken = 999.999,
            CurrentLevel = 10,
            IsFinished = 0,
            IsWon = 0
        }, jsnReceiverDel);
    }
    #endregion
}
