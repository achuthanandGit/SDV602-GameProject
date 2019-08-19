using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackTinkuDialogueSceneController : MonoBehaviour
{
   
    public Text tinkuText;
    public Text jackText;
    public Image jackbunny;
    public Image jackDlgImage;
    public Image tinkuDialogBox;
    public Image tinkuRushImage;
    public Image tinkuSadImage;
    public Image jackCameImage;
    public Image jackAngryImage;
    public Image scrollArea;
    public Text situationExplaText;

    private int clickCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        HideObjects();
        ShowNarattionArea(false);
        SetInitialTinkuDialogue();
    }
    
    /**
     * this method is used to identify the action to perform when the nextBtn is clicked in the scene
     */
    public void SetButtonAction()
    {
        switch(clickCount)
        {
            case 0:
                ShowJack();
                clickCount++;
                break;
            case 1:
                StartTinkuDialog();
                clickCount++;
                break;
            case 2:
                jackText.text = "What, What happened?";
                jackCameImage.enabled = false;
                jackAngryImage.enabled = true;
                clickCount++;
                break;
            case 3:
                tinkuRushImage.enabled = false;
                tinkuSadImage.enabled = true;
                tinkuDialogBox.enabled = false;
                tinkuText.enabled = false;
                ShowWhatHappened();
                ShowBunnyAngryReply();
                clickCount++;
                break;
            case 4:
                HideAllObjects();
                situationExplaText.text = "Jack learned from village elders that the monster won't kill his parent but will enslave them till they die. He realized that he can actually save his parents by " +
                    "completing certain tasks which will let him inside the castle. So he marched to the monster's castle.";
                scrollArea.rectTransform.sizeDelta = new Vector2(300, 300);
                situationExplaText.rectTransform.sizeDelta = new Vector2(300, 300);
                break;
            default:
                break;

        }
    }

    /**
     * this method is used to hide all the objects in the scene
     */
    private void HideAllObjects()
    {
        HideObjects();
        jackText.enabled = false;
    }

    /**
     * this methos is used to show the angry reply of bunny 
     */
    private void ShowBunnyAngryReply()
    {
        jackText.text = "I am gonna kill that monster and save them.";
    }

    /**
     * this method is use to show tinku explaining what happened in the jungle
     */
    private void ShowWhatHappened()
    {
        situationExplaText.text = "Tinku explained what actually happened back in the jungle.";
        ShowNarattionArea(true);
    }

    /**
     * this method is used to hide the game objects which is not required initially 
     */
    private void HideObjects()
    {
        jackbunny.enabled = false;
        jackDlgImage.enabled = false;
        tinkuSadImage.enabled = false;
        jackAngryImage.enabled = false;
    }

    /**
     * this method is used to hide/show the narratin area
     */
    private void ShowNarattionArea(bool enable)
    {
        scrollArea.enabled = enable;
        situationExplaText.enabled = enable;
    }

    /**
     * this method is used to load the dialogue of Tinku duck when the scene is loaded
     */
    private void SetInitialTinkuDialogue()
    {
        tinkuText.text = StoryDescriptionTexts.TinkuStartDialog;
    }

    /**
     * this method is used to show the jack and realted game objects when the next button inside of Tinku's 1st dialogue is clicked
     */
    public void ShowJack()
    {
        jackbunny.enabled = true;
        jackDlgImage.enabled = true;
        ShowInitialJackDialog();
    }

    private void ShowInitialJackDialog()
    {
        jackText.text = StoryDescriptionTexts.JackStartDialog;
    }

    /**
     * this method starts the dialog of Tinku explaining what happened to Jack parents
     * this method will show the first message telling that the monster got the parents
     */
    public void StartTinkuDialog()
    {
        tinkuText.fontSize = 15;
        tinkuText.text = "That monster!! \n He got your parents.";
    }

    /**
     * this method is used to show the explanation of what happened in the jungle to jack
     */
    public void StartExplaining()
    {
        tinkuDialogBox.rectTransform.sizeDelta = new Vector2(200, 200);
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
