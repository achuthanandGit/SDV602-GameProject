using Assets.scripts;
using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    // UsernameText to get username data
    public Text UsernameText;

    // PasswordText to get password data
    public Text PasswordText;

    // EmailText to get email data
    public Text EmailText;

    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;

    // RegisterSuccess is used to know whether the user register is successfull or not
    private bool RegisterSuccess;


    /// <summary>Start is used to load the GameObjects or actions when the scene gets loaded.</summary>
    void Start()
    {
        RegisterSuccess = false; 
        ClearAllTextField();
        MessagePanel.SetActive(false);
    }


    /// <summary>Clears all text field.</summary>
    private void ClearAllTextField()
    {
        UsernameText.text = string.Empty;
        PasswordText.text = string.Empty;
        EmailText.text = string.Empty;
    }


    /// <summary>Closes the register scene when clicks Cancel button.</summary>
    public void CloseRegisterScene()
    {
        ClearAllTextField();
        SceneManager.LoadScene("LoginScene");
    }


    /// <summary>CloseMessagePanel is used to close the message panel when clicks OK button.</summary>
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
        if (RegisterSuccess)
            SceneManager.LoadScene("LoginScene");
    }

    
    /// <summary>
    /// Register the new user data.
    /// If any field is empty will throw error saying so.
    /// If email is not valid throw error saying so
    /// If the username is already taken throw error saying so
    /// If all data is valid, new user details will be added to GameModel and will throw info saying so
    /// </summary>
    public void RegisterNewUserData()
    {
        if(string.IsNullOrWhiteSpace(UsernameText.text) ||
            string.IsNullOrWhiteSpace(PasswordText.text) ||
            string.IsNullOrWhiteSpace(EmailText.text))
        {
            MessagePanelText.text = "Fill all required fields.";
            MessagePanel.SetActive(true);
        } else if (!IsEmailValid(EmailText.text))
        {
            MessagePanelText.text = "Email is invalid.";
            MessagePanel.SetActive(true);
        } else if(GameModel.UserLoginDetails.ContainsKey(UsernameText.text))
        {
            MessagePanelText.text = "Username is already taken. Please use another one.";
            MessagePanel.SetActive(true);
        } else
        {
            User objUser = new User();
            objUser.Usser = UsernameText.text;
            objUser.Pwd = PasswordText.text;
            objUser.Mail = EmailText.text;
            objUser.UserStatus = "inactive";
            GameModel.UserLoginDetails.Add(UsernameText.text, objUser);
            MessagePanelText.text = "User has beed added successfully";
            MessagePanel.SetActive(true);
            RegisterSuccess = true;
        }

        
    }


    /// <summary>Determines whether [is email valid] [the specified emailaddress].</summary>
    /// <param name="pEmailaddress">The emailaddress.</param>
    /// <returns>
    ///   <c>true</c> if [is email valid] [the specified emailaddress]; otherwise, <c>false</c>.</returns>
    private bool IsEmailValid(string pEmailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(pEmailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    
}
