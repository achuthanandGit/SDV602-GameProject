using Assets.scripts;
using Assets.scripts.Domains;
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
}
