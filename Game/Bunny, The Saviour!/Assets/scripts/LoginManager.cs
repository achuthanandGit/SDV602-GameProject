using UnityEngine;
using UnityEngine.UI;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;

    // UsernameText to get username data
    public Text UsernameText;

    // PasswordText to get password data
    public Text PasswordText;

       
    /// <summary>Start is used to load the GameObjects or actions when the scene gets loaded</summary>
    void Start()
    {
        MessagePanel.SetActive(false);
    }

    
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
            MessagePanelText.text = "Fill all required fields.";
            MessagePanel.SetActive(true);
        }
        else if (CheckLogin(UsernameText.text, PasswordText.text))
        {
            GameManager.GameManagerInstance.Username = UsernameText.text;
            GameModel.UserLoginDetails[UsernameText.text].UserStatus = "active";
            SceneManager.LoadScene("GameHome");
        }   
        else
        {
            MessagePanelText.text = "Username/Password is wrong. please try again with valid data.";
            MessagePanel.SetActive(true);
        }
    }

    /// <summary>
    /// CheckLogin will check the username and password if username doesn't exists or username 
    /// password combination doesn't exists, will return false
    /// return true if username and password combination is valid.
    /// </summary>
    /// 
    /// <param name="pUsername">The username.</param>
    /// <param name="pPassword">The password.</param>
    /// <returns>bool</returns>
    public static bool CheckLogin(string pUsername, string pPassword)
    {
        return (GameModel.UserLoginDetails.ContainsKey(pUsername) &&
            GameModel.UserLoginDetails[pUsername].Pwd == pPassword);
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
}
