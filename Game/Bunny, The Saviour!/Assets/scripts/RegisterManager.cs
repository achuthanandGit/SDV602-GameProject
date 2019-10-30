using Assets.scripts;
using Assets.scripts.Domains;
using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    // UsernameText to get username data
    public InputField UsernameText;

    // PasswordText to get password data
    public InputField PasswordText;

    // EmailText to get email data
    public InputField EmailText;

    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;

    // RegisterSuccess is used to know whether the user register is successfull or not
    private bool IsRegisterSuccess;


    /// <summary>Start is used to load the GameObjects or actions when the scene gets loaded.</summary>
    void Start()
    {
        IsRegisterSuccess = false; 
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
        if (IsRegisterSuccess)
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
            Debug.Log("Error in input");
            MessagePanelText.text = "Fill all required fields.";
            MessagePanel.SetActive(true);
        } else if (!IsEmailValid(EmailText.text))
        {
            Debug.Log("Invlid email");
            MessagePanelText.text = "Email is invalid.";
            MessagePanel.SetActive(true);
        } else if(GameModel.CheckDuplicateUser(UsernameText.text))
        {
            Debug.Log("Duplicate user");
            MessagePanelText.text = "Username is already taken. Please use another one.";
            MessagePanel.SetActive(true);
        } else
        {
            Debug.Log("Setting data for save");
            User objUser = new User();
            objUser.Username = UsernameText.text;
            objUser.Password = PasswordText.text;
            objUser.Email = EmailText.text;
            objUser.LoginStatus = "inactive";
            if (GameModel.SaveNewUser(objUser))
            {
                Debug.Log("Successfull registration");
                IsRegisterSuccess = true;
                MessagePanelText.text = "User has beed added successfully";
            } else
            {
                Debug.Log("Unsuccessfull registration");
                MessagePanelText.text = "Unexpected error occurs. Please try again.";
            }
            // GameModel.UserLoginDetails.Add(UsernameText.text, objUser);
            MessagePanel.SetActive(true);
            
        }        
    }


    /// <summary>Determines whether [is email valid] [the specified emailaddress].</summary>
    /// <param name="pEmailaddress">The emailaddress.</param>
    /// <returns>
    ///   <c>true</c> if [is email valid] [the specified emailaddress]; otherwise, <c>false</c>.
    ///  </returns>
    private bool IsEmailValid(string pEmailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(pEmailaddress);
            return true;
        }
        catch (FormatException exception)
        {
            Debug.Log("FormatException happens when checking email is valid or not: " + exception);
            return false;
        }
    }
    
}
