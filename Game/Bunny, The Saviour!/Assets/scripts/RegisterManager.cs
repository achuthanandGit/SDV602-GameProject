using Assets.scripts;
using Assets.scripts.Domains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.scripts
{
    /// <summary>Class used to manage the new player registration</summary>
    public class RegisterManager : MonoBehaviour
    {
        // UsernameText to get username data
        public static InputField UsernameText;

        // PasswordText to get password data
        public static InputField PasswordText;

        // EmailText to get email data
        public static InputField EmailText;

        // MessagePanel to show error/warning/info messages
        public static GameObject MessagePanel;

        // MessagePanelText to set the error/warning/info messages in MessagePanel
        public static Text MessagePanelText;

        // RegisterSuccess is used to know whether the user register is successfull or not
        private static bool IsRegisterSuccess;


        /// <summary>Start is used to load the GameObjects or actions when the scene gets loaded.</summary>
        void Start()
        {
            IsRegisterSuccess = false;
            GetAllComponents();
            ClearAllTextField();
        }

        /// <summary>
        /// To execute code for each frame updates. Update method will be called for each frame update.
        /// </summary>
        private void Update()
        {
            ListenBackButtonEvent();
        }

        /// <summary>
        /// Listens for native phone back button click event
        /// </summary>
        private void ListenBackButtonEvent()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseRegisterScene();
            }
        }

        /// <summary>
        /// Gets all components required for the register process.
        /// </summary>
        private void GetAllComponents()
        {
            UsernameText = GameObject.Find("UsernameInput").GetComponent<InputField>();
            PasswordText = GameObject.Find("PasswordInput").GetComponent<InputField>();
            EmailText = GameObject.Find("EmailInput").GetComponent<InputField>();
            MessagePanel = GameObject.Find("GameMessagePanel");
            MessagePanelText = MessagePanel.GetComponentInChildren<Text>();
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
            if (string.IsNullOrWhiteSpace(UsernameText.text) ||
                string.IsNullOrWhiteSpace(PasswordText.text) ||
                string.IsNullOrWhiteSpace(EmailText.text))
            {
                Debug.Log("Error in input");
                MessagePanelText.text = "Fill all required fields.";
                MessagePanel.SetActive(true);
            }
            else if (!IsEmailValid(EmailText.text))
            {
                Debug.Log("Invlid email");
                MessagePanelText.text = "Email is invalid.";
                MessagePanel.SetActive(true);
            }
            else
            {
                GameModel.CheckDuplicateUser(UsernameText.text);
            }

        }

        /// <summary>
        /// Success reciever delegate for checking duplicate user
        /// </summary>
        /// <param name="pReceivedList">The p received list.</param>
        public static void CheckDuplicateRecieverDel(List<User> pReceivedList)
        {
            Debug.Log("Duplicate user exists");
            MessagePanelText.text = "Username is already taken. Please use another one.";
            MessagePanel.SetActive(true);
        }

        /// <summary>
        /// Error reciever delegate for checking duplicate user
        /// </summary>
        /// <param name="pReceived">The p received.</param>
        public static void CheckDuplicateErrorDel(JsnReceiver pReceived)
        {
            Debug.Log("Setting data for save");
            User objUser = new User();
            objUser.Username = UsernameText.text;
            objUser.Password = PasswordText.text;
            objUser.Email = EmailText.text;
            objUser.LoginStatus = "inactive";
            // Saving new user
            GameModel.SaveNewUser(objUser);

            if (AddProfilePic.PictureDetailsBytes.Count() > 0)
            {
                #region Get FIle Path


#if UNITY_EDITOR
                var path = string.Format(@"Assets/StreamingAssets/{0}", UsernameText.text);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, UsernameText.text);

        if (!File.Exists(filepath))
        {
           
#if UNITY_ANDROID
            var loadPic= new WWW("jar:file://" + Application.dataPath + "!/assets/" + UsernameText.text);  // this is the path to your StreamingAssets in android
            while (!loadPic.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadPic.bytes);
#elif UNITY_IOS
                 var loadPic = Application.dataPath + "/Raw/" + UsernameText.text;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadPic, filepath);
#elif UNITY_WP8
                var loadPic = Application.dataPath + "/StreamingAssets/" + UsernameText.text;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadPic, filepath);

#elif UNITY_WINRT
		var loadPic = Application.dataPath + "/StreamingAssets/" + UsernameText.text;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadPic, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadPic = Application.dataPath + "/Resources/Data/StreamingAssets/" + UsernameText.text;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadPic, filepath);
#else
	var loadPic = Application.dataPath + "/StreamingAssets/" + UsernameText.text;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadPic, filepath);

#endif

            Debug.Log("Database written");
        }

        var path = filepath;
#endif
                #endregion

                Debug.Log(path);
                // Saving profile picture in name of username. The picture will be saved locally
                File.WriteAllBytes(path + ".png", AddProfilePic.PictureDetailsBytes);
            }

            Debug.Log("Successfull registration");
            IsRegisterSuccess = true;
            MessagePanelText.text = "User has beed added successfully";
            MessagePanel.SetActive(true);
        }

        /// <summary>Determines whether [is email valid] [the specified emailaddress].</summary>
        /// <param name="pEmailaddress">The emailaddress.</param>
        /// <returns>
        ///   <c>true</c> if [is email valid] [the specified emailaddress]; otherwise, <c>false</c>.
        ///  </returns>
        ///  <exception cref="FormatException">
        ///     If the email is not in correct format
        /// </exception>
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
}