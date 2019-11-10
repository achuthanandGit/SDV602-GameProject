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
    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;

    // UsernameText to get username data
    public InputField UsernameText;

    // PasswordText to get password data
    public InputField PasswordText;

   
    /// <summary>
    ///   Handles Login button.
    ///   If valid then navigates to GameHomeScene.
    ///   If not valid, will throw error saying so.
    ///   If all fields are filled but, username doesn't exists or username password combination is not valid, will throw error saying so.
    /// </summary>
    public void HandleLoginButton()
    {
        if (string.IsNullOrWhiteSpace(UsernameText.text) ||
            string.IsNullOrWhiteSpace(PasswordText.text))
        {
            Debug.Log("Input data error");
            MessagePanelText.text = "Fill all required fields.";
            MessagePanel.SetActive(true);
        } else
        {
            User currentUser = GameModel.CheckLogin(UsernameText.text, PasswordText.text);
            if(currentUser is null) {
                Debug.Log("Login Error");
                MessagePanelText.text = "Username/Password is wrong. please try again with valid data.";
                MessagePanel.SetActive(true);
            }
            else
            {
                Debug.Log("Successfull Login");
                GameModel.Username = UsernameText.text;
                GameModel.CurrentUser = currentUser;
                SceneManager.LoadScene("GameHome");
            }
        }
    }

   
    /// <summary>
    /// Handles the message panel button.
    /// Close the message panel when OK button is clicked
    /// </summary>
    public void HandleMessagePanelButton()
    {
        MessagePanel.SetActive(false);
    }

    /// <summary>Loads the register scene to register new user</summary>
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }


    // testing JSN DROP

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
        
        JSONDropService jsDrop = new JSONDropService { Token = "da308d33-ba58-413d-9abf-b987c9eac791"};

        // Create table person
        jsDrop.Create<User, JsnReceiver>(new User
        {
            Username = "achu",
            Password = "achu",
            BestHealth = 10,
            BestTime = 20.00,
            Email = "achu@gmail.com",
            LoginStatus = "active",
            GamesWon = 1,
            Lastlogin = DateTime.Now
        }, jsnReceiverDel);

       // jsDrop.All<User, JsnReceiver>(jsnListReceiverDel, jsnReceiverDel);

        /*
        // Store people records
        jsDrop.Store<User, JsnReceiver>(new List<User>
        {
            new User{
            Username = "achu",
            Password = "achu",
            BestHealth = 10,
            BestTime = 20.00,
            Email = "achu@gmail.com",
            LoginStatus = "active",
            GamesWon = 1,
            Lastlogin = DateTime.Now
            },
            new User{
            Username = "manu",
            Password = "manu",
            BestHealth = 10,
            BestTime = 20.00,
            Email = "manu@gmail.com",
            LoginStatus = "active",
            GamesWon = 1,
            Lastlogin = DateTime.Now
            },
            new User{
            Username = "vasu",
            Password = "vasu",
            BestHealth = 10,
            BestTime = 20.00,
            Email = "vasu@gmail.com",
            LoginStatus = "active",
            GamesWon = 1,
            Lastlogin = DateTime.Now
            },
         }, jsnReceiverDel);

        /*
        // Retreive all people records
        jsDrop.All<tblPerson, JsnReceiver>(jsnListReceiverDel, jsnReceiverDel);
        
        jsDrop.Select<tblPerson,JsnReceiver>("HighScore > 200",jsnListReceiverDel, jsnReceiverDel);
        
        jsDrop.Delete<tblPerson, JsnReceiver>("PersonID = 'Jonny'", jsnReceiverDel);
        
        jsDrop.Drop<User, JsnReceiver>(jsnReceiverDel);
        */
    }
}
