using Assets.scripts;
using System;
using System.Collections.Generic;
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

    /**
     * Start is used to load the GameObjects or actions when the scene gets loaded
     */
    void Start()
    {
        RegisterSuccess = false; 
        ClearAllTextField();
        MessagePanel.SetActive(false);
    }

    /**
     * ClearAllTextField is used to clear all the text fields
     */
    private void ClearAllTextField()
    {
        UsernameText.text = string.Empty;
        PasswordText.text = string.Empty;
        EmailText.text = string.Empty;
    }

    /**
     * CloseRegisterScene is used to close the RegisterScene when Cancel button is clicked
     */
    public void CloseRegisterScene()
    {
        ClearAllTextField();
        SceneManager.LoadScene("LoginScene");
    }

    /**
     * CloseMessagePanel is used to close the message panel when ok button is clicked
     */
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
        if (RegisterSuccess)
            SceneManager.LoadScene("LoginScene");
    }

    /**
     * RegisterNewUserData is used to register new data
     * If any field is empty will throw error saying so
     * If email is not valid throw error saying so
     * If the username is already taken throw error saying so
     * If all data is valid, new user details will be added to GameModel and will throw info saying so
     */
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
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();
            userDataDictionary.Add("password", PasswordText.text);
            userDataDictionary.Add("email", EmailText.text);
            userDataDictionary.Add("status", "inactive");
            GameModel.UserLoginDetails.Add(UsernameText.text, userDataDictionary);
            MessagePanelText.text = "User has beed added successfully";
            MessagePanel.SetActive(true);
            RegisterSuccess = true;
        }

        
    }

    /**
     * IsEmailValid is used to check whether the email id is valid or not
     * return true if it is valid
     * return false if it is not valid
     * 
     * emailaddress - emailId to check
     */
    private bool IsEmailValid(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    
}
