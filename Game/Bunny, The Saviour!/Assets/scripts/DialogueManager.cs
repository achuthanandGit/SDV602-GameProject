using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class DialogueManager : MonoBehaviour
    {
        // To set the dialogue of Tinku
        public Text TinkuText;

        // To set the dialogue of Jack
        public Text JackText;

        // To update the image of Jack
        public Image Jackbunny;

        // To update the dialogue box of Jack
        public Image JackDlgImage;

        // To update the dialogue box of Tinku
        public Image TinkuDialogBox;

        // To update the image of Tinku as rushing
        public Image TinkuRushImage;

        // To update the image of Tinku as sad
        public Image TinkuSadImage;

        // To update the image of Jack as he came
        public Image JackCameImage;

        // Tp update the image of Jack as angry
        public Image JackAngryImage;

        // To update the text area visibilty
        public Image ScrollArea;

        // To update the text area text content
        public Text SituationExplaText;

        // To record the click count of next button to deliver the required dialogue
        private int ClickCount = 0;

        /**
         * Start method is used to initialize or assign values or actions to required 
         * variable or components before the first frame update
         */
        private void Start()
        {
            HideObjects();
            ShowNarattionArea(false);
            SetInitialTinkuDialogue();
        }

        /**
         * HideObjects is used to hide the gameObjects which is not required when the screen is loaded
         */
        private void HideObjects()
        {
            Jackbunny.enabled = false;
            JackDlgImage.enabled = false;
            TinkuSadImage.enabled = false;
            JackAngryImage.enabled = false;
        }

        /**
         * ShowNarattionArea is used to show/hide the text area which is used to narrate the story
         */
        private void ShowNarattionArea(bool enable)
        {
            ScrollArea.enabled = enable;
            SituationExplaText.enabled = enable;
        }

        /**
         * SetInitialTinkuDialogue is used to set the initial dialogue of Tinku when the screen is loaded
         */
        private void SetInitialTinkuDialogue()
        {
            TinkuText.text = GameManager.GameManagerInstance.GameModelInstance.CurrentScene.Story;
            ClickCount++;
        }

        /**
         * HandleNextButton is used to handle the next button and will update the dialogue
         */
        public void HandleNextButton()
        {
            CommandProcessor aCommandProcessor = new CommandProcessor();
            switch (ClickCount)
            {
                case 1:
                    JackText.text = aCommandProcessor.ProcessAction("DialogueJackFirst");
                    Jackbunny.enabled = true;
                    JackDlgImage.enabled = true;
                    ClickCount++;
                    break;
                case 2:
                    TinkuText.text = aCommandProcessor.ProcessAction("DialogueTinkuSecond");
                    ClickCount++;
                    break;
                case 3:
                    JackCameImage.enabled = false;
                    JackAngryImage.enabled = true;
                    JackText.text = aCommandProcessor.ProcessAction("DialogueJackSecond");
                    ClickCount++;
                    break;
                case 4:
                    TinkuRushImage.enabled = false;
                    TinkuSadImage.enabled = true;
                    TinkuDialogBox.enabled = false;
                    TinkuText.enabled = false;
                    UpdateFinalDialogue(aCommandProcessor.ProcessAction("DialogueFinal"));
                    ClickCount++;
                    ShowNarattionArea(true);
                    break;
                case 5:
                    HideObjects();
                    JackText.enabled = false;
                    ScrollArea.rectTransform.sizeDelta = new Vector2(300, 300);
                    SituationExplaText.rectTransform.sizeDelta = new Vector2(300, 300);
                    SituationExplaText.text = aCommandProcessor.ProcessAction("GatherInfo");
                    ClickCount++;
                    break;
                case 6:
                    SceneManager.LoadScene("GameRoom");
                    break;
            }
        }

        /**
         * UpdateFinalDialogue is used to update the final dialogue of Tinku and Jack
         * 
         * input {textToUpdate (type-string) text to update}
         */
        private void UpdateFinalDialogue(string textToUpdate)
        {
            string[] textArray = textToUpdate.Split('$');
            JackText.text = textArray[0];
            SituationExplaText.text = textArray[1];
        }

    }
}
