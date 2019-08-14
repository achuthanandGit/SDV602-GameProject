using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackTinkuDialogueSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text tinkuDlgCallingJackText;
    public Text jackCameText;
    public Image jackbunny;
    public Image jackDlgImage;
    public Button jacknextBtn;

    void Start()
    {
        HideJackObjects();
        SetInitialTinkuDialogue();
    }

    /**
     * this method is used to hide the character jack and the related game objects
     */
    private void HideJackObjects()
    {
        jackbunny.enabled = false;
        jackDlgImage.enabled = false;
        jacknextBtn.enabled = false;
    }

    /**
     * this method is used to load the dialogue of Tinku duck when the scene is loaded
     */
    private void SetInitialTinkuDialogue()
    {
        tinkuDlgCallingJackText.text = "Jack, Jack....!!";
    }

    /**
     * this method is used to show the jack and realted game objects when the next button inside of Tinku's 1st dialogue is clicked
     */
    public void ShowJack()
    {
        jackbunny.enabled = true;
        jackDlgImage.enabled = true;
        jacknextBtn.enabled = true;
        ShowInitialJackDialog();
    }

    private void ShowInitialJackDialog()
    {
        jackCameText.text = "Hey, Tinku. \n What happened? \n You feel so terrified.";
    }

    /**
     * this method starts the dialog of Tinku explaining what happened to Jack parents
     * this method will show the first message telling that the monster got the parents
     */
    public void StartTinkuDialog()
    {
        tinkuDlgCallingJackText.fontSize = 15;
        tinkuDlgCallingJackText.text = "That monster!! \n He got your parents.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
