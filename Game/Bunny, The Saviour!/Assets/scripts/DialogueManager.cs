using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

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
        private int ClickCount = 1112;

        /// <summary>Starts this instance.
        /// Start method is used to initialize or assign values or actions to required variable or components before the first frame update
        /// </summary>
        private void Start()
        {
            HideObjects();
            ShowNarattionArea(false);
            SetInitialTinkuDialogue();
        }

        
        /// <summary>Hides the specified objects.</summary>
        private void HideObjects()
        {
            Jackbunny.enabled = false;
            JackDlgImage.enabled = false;
            TinkuSadImage.enabled = false;
            JackAngryImage.enabled = false;
        }


        /// <summary>Shows the narattion area.</summary>
        /// <param name="pEnable">if set to <c>true</c> [pEnable].</param>
        private void ShowNarattionArea(bool pEnable)
        {
            ScrollArea.enabled = pEnable;
            SituationExplaText.enabled = pEnable;
        }

        
        /// <summary>Sets the initial tinku dialogue.</summary>
        private void SetInitialTinkuDialogue()
        {
            TinkuText.text = GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question;
            ClickCount++;
        }

        
        /// <summary>Handles the next button and updates dialogue.</summary>
        public void HandleNextButton()
        {
            switch (ClickCount)
            {
                case 1113:
                    JackText.text = GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question;
                    Jackbunny.enabled = true;
                    JackDlgImage.enabled = true;
                    ClickCount++;
                    break;
                case 1114:
                    TinkuText.text = GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question;
                    ClickCount++;
                    break;
                case 1115:
                    JackCameImage.enabled = false;
                    JackAngryImage.enabled = true;
                    JackText.text = GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question;
                    ClickCount++;
                    break;
                case 1116:
                    TinkuRushImage.enabled = false;
                    TinkuSadImage.enabled = true;
                    TinkuDialogBox.enabled = false;
                    TinkuText.enabled = false;
                    UpdateFinalDialogue(GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question);
                    ClickCount++;
                    ShowNarattionArea(true);
                    break;
                case 1117:
                    HideObjects();
                    JackText.enabled = false;
                    ScrollArea.rectTransform.sizeDelta = new Vector2(300, 300);
                    SituationExplaText.rectTransform.sizeDelta = new Vector2(300, 300);
                    SituationExplaText.text = GameModel.DialogueList.First(scene => scene.SceneId == ClickCount).Question;
                    ClickCount++;
                    break;
                case 1118:
                    SceneManager.LoadScene("GameRoom");
                    break;
            }
        }


        /// <summary>Updates the final dialogue.</summary>
        /// <param name="pTextToUpdate">The text to update.</param>
        private void UpdateFinalDialogue(string pTextToUpdate)
        {
            string[] textArray = pTextToUpdate.Split('$');
            JackText.text = textArray[0];
            SituationExplaText.text = textArray[1];
        }

    }
}