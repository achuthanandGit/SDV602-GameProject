using UnityEngine;
using UnityEngine.UI;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // UsernameText to get username data
    public Text UsernameText;

    // PasswordText to get password data
    public Text PasswordText;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;

    /**
     * Start is used to load the GameObjects or actions when the scene gets loaded
     */
    void Start()
    {
        MessagePanel.SetActive(false);
    }

      /**
     * HandleLoginButton is used to check whether all the input are valid when login button is clicked
     * if valid then navigates to GameHomeScene
     * if not valid will throw error ssaying so
     * if all fields are filled but if username doen't exists or username password combination is not valid, will throw error saying so
     */
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
            GameModel.UserLoginDetails[UsernameText.text]["status"] = "active";
            SceneManager.LoadScene("GameHome");
        }   
        else
        {
            MessagePanelText.text = "Username/Password is wrong. please try again with valid data.";
            MessagePanel.SetActive(true);
        }
    }

    /**
     * CheckLogin will check the username and password
     * if username doesn't exists or username password combination doesn't exists, will return false
     * return true if username and password combination is valid
     */
    public static bool CheckLogin(string username, string password)
    {
        return (GameModel.UserLoginDetails.ContainsKey(username) &&
            GameModel.UserLoginDetails[username]["password"] == password);
    }

    /**
     * HandleMessagePanelButton is used to close the message panel when ok button is clicked
     */
    public void HandleMessagePanelButton()
    {
        MessagePanel.SetActive(false);
    }

    /**
     * LoadRegisterScene is used to load the Register scene to register new user details
     */
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }


    
}
