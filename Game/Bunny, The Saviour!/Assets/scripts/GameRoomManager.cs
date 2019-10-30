using Assets.scripts;
using Assets.scripts.Domains;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviour
{
    // Used to define Message panel which will be used to show messages
    public GameObject MessagePanel;

    // Used to set the message inside Message panel
    public Text MessagePanelText;

    // Used to define the player details panel
    public GameObject PlayerDetailsPanel;

    // Used to defince the Chat room
    public GameObject ChatRoomPanel;

    // Used to know whether the user is trying to exit from the game
    private bool IsTryingToExit = false;

    // Used to define the answer of shown question
    private string Answer = string.Empty;

    // To set the question in game room
    public Text QuestionText;

    // To define the input field for answering
    public InputField AnswerInputField;

    // To count the questions
    private int QuestionCount = 0;

    // To define the CancelButton
    public GameObject CancelButton;

    // To define whether the answer is correct or not
    private bool IsCorrect = false;

    // To define the initial health and updates as the game progress
    private int PlayerHealth = 10;

    // To define the player health
    public Text PlayerHealthText;

    // To track the time taken to finsish the game
    private float StartTime;

    // To define the timer for game
    public Text GameTimerText;

    // To define each game level
    public Text LevelText;

    // To define whether the game starts or not
    private bool IsStartGame = false;

    // To definde whether the game is finished or not
    private bool IsGameFinished = false;

    // To define the components in the game room other thane message/chat/payer panels
    public GameObject GameRoomComponents;

    // To define the score template container
    public Transform ScoreTemplateContainer;

    // To define Score template
    public Transform ScoreTemplate;

    // To define the score template container
    public Transform TestContainer;

    // To define Score template
    public Transform TestTemplate;



    // To define game objects to destroy
    private List<GameObject> DestroyList = new List<GameObject>();


    /// <summary>
    /// Start method is used to initialize or assign values or actions to required 
    /// variable or components before the first frame update.
    /// </summary>
    void Start()
    {
        MessagePanel.SetActive(false);
        CancelButton.SetActive(false);
        PlayerDetailsPanel.SetActive(false);
        GameModel.currentLevel = 1;
        ShowGameStartDialog();
    }

  

    /// <summary>Updates this instance for each frame update</summary>
    private void Update()
    {
        if (IsStartGame)
            GameTimerText.text = (Time.time - StartTime).ToString("0.0");
    }

    private void ShowGameStartDialog()
    {
        MessagePanelText.text = "By clicking OK you are ready to start the game. Complete level 1 to enter into the castle.";
        MessagePanel.SetActive(true);
    }

    /// <summary>Handles the ok button in message panel.
    /// If the message is 'wrong answer' or 'correct answer', Closes the message panel and loads next question.
    /// If the messsage is about leaving the game room, then the game data will be saved and load the game home scene
    /// </summary>
    public void HandleOkButton()
    {
        CancelButton.SetActive(false);
        MessagePanel.SetActive(false);
      
        if(IsGameFinished)
        {
            MessagePanel.SetActive(false);
            SceneManager.LoadScene("GameHome");
        }
        else if (IsTryingToExit)
        {
            IsStartGame = false;
            UpdateUserGameData();
            SceneManager.LoadScene("GameHome");
        }
        else if (GameModel.currentLevel == 1 && QuestionCount == 0)
            SetQuestionOne();
        else if (IsCorrect)
            CheckLevelAndGetNext();
        else if (!IsCorrect)
            ReducePlayerHealth();
    }


    /// <summary>Checks the level and get next scene.</summary>
    private void CheckLevelAndGetNext()
    {
        if (GameModel.currentLevelTotalCount == QuestionCount)
        {
            MessagePanelText.text = GetMessageForLevelPass();

            if (UpdateGameSceneDataLists())
            {
                UpdateUserGameData();
                QuestionCount = 0;
                MessagePanel.SetActive(true);
                LevelText.text = "Level " + GameModel.currentLevel;
                AnswerInputField.text = string.Empty;
                IsCorrect = true;
            }
            else
            {
                MessagePanelText.text = "You have successfully completed all tasks and Saved the Parents. You are brave.";
                MessagePanel.SetActive(true);
                IsStartGame = false;
                IsGameFinished = true;
                UpdateUserGameData();
            }
        }
        else
        {
            SceneData sceneData = GameModel.currentLevelSceneList[QuestionCount];
            QuestionText.text = sceneData.Question;
            Answer = sceneData.Answer;
            QuestionCount++;
            UpdateUserGameData();
        }
    }

    /// <summary>Updates the user game data.</summary>
    private void UpdateUserGameData()
    {
    
        if (!IsStartGame)
        {
            if (PlayerHealth > GameModel.CurrentUser.BestHealth)
                GameModel.CurrentUser.BestHealth = PlayerHealth;
            if (Convert.ToDouble(GameTimerText.text) > GameModel.CurrentUser.BestTime)
                GameModel.CurrentUser.BestTime = Convert.ToDouble(GameTimerText.text);
            if(IsGameFinished)
            {
                GameModel.CurrentUser.GamesWon = GameModel.CurrentUser.GamesWon + 1;
                GameModel.UserGameData.IsWon = true;
                GameModel.UserGameData.IsFinished = true;
                GameModel.UserGameData.EndDateTime = DateTime.Now;
                UpdateGameData();
            }
        }
        else
        {
            GameModel.UserGameData.IsFinished = false;
        }

        GameModel.UserGameData.Health = PlayerHealth;
        GameModel.UserGameData.TimeTaken = Convert.ToDouble(GameTimerText.text);
        GameModel.UserGameData.CurrentLevel = GameModel.currentLevel;
        GameModel.UpdateUserGameData(GameModel.CurrentUser, GameModel.UserGameData);

    }

    /// <summary>Updates the game data when a game is finished.</summary>
    private void UpdateGameData()
    {
        GameData gameData = GameModel.GetUpdatedGameData(GameModel.gameData.GameId);
        if(gameData != null && string.IsNullOrWhiteSpace(gameData.Winner))
        {
            gameData.Winner = GameModel.Username;
            gameData.BestHealth = PlayerHealth;
            gameData.BestTime = Convert.ToDouble(GameTimerText.text);
            gameData.GameStatus = "finished";
            GameModel.UpdateGameData(gameData);
        }
    }

    /// <summary>  Returns custom message to show when each level passes</summary>
    /// <returns> success message text </returns>
    private string GetMessageForLevelPass()
    {
        string successMessageText = string.Empty;
        switch (GameModel.currentLevel)
        {
            case 1:
                successMessageText = "You have successfully entered into the castle.";
                break;
            case 2:
                successMessageText = "You got the secret sword to kill the monster. Now pass Level 3 to get the key for unlocking your parents.";
                break;
            case 3:
                successMessageText = "You are a brave warrior. Now you need to get special energy drink to fight with monster. Go and get it.";
                break;
            case 4:
                successMessageText = "You got all the elements to kill the monster and free your parents. Now pass level 5 to make sure you are capable of killing the monster";
                break;
            case 5:
                successMessageText = "You saved your parents using your bravery and brightmess. You finished the game successfully.";
                break;
        }
        return successMessageText;
    }



    /// <summary>Updates the game scene data lists.</summary>
    /// <returns>
    ///     true : If there are more levels to continue
    ///     false : If all levels are complete
    /// </returns>
    private bool UpdateGameSceneDataLists()
    {
        GameModel.currentLevel = GameModel.currentLevel + 1;
        if (GameModel.currentLevel > GameModel.playSceneMap.Count)
            return false;
        GameModel.currentLevelSceneList.Clear();
        GameModel.currentLevelSceneList = GameModel.playSceneMap[GameModel.currentLevel];
        GameModel.currentLevelTotalCount = GameModel.currentLevelSceneList.Count;
        return true;
    }

    /// <summary>Exits the game.</summary>
    public void ExitGame()
    {
        IsTryingToExit = true;
        MessagePanelText.text = "Game data won't be saved. Are you sure you want to leave.";
        CancelButton.SetActive(true);
        MessagePanel.SetActive(true);
    }


    /// <summary>Handles the cancel button in message panel. Lets the user stay in the game.</summary>
    public void HandleCancelButton()
    {
        IsTryingToExit = false;
        CancelButton.SetActive(false);
        MessagePanel.SetActive(false);
    }

    /// <summary>Sets the question one when the game room is loaded.</summary>
    private void SetQuestionOne()
    {
        GameModel.currentLevelSceneList = GameModel.playSceneMap[GameModel.currentLevel];
        GameModel.currentLevelTotalCount = GameModel.currentLevelSceneList.Count;
        SceneData sceneData = GameModel.currentLevelSceneList[QuestionCount];
        QuestionText.text = sceneData.Question;
        Answer = sceneData.Answer;
        QuestionCount++;
        IsStartGame = true;
        StartTime = Time.time;
    }

    /// <summary>Validates the answer.</summary>
    public void ValidateAnswer()
    {
        string messagePanelText = string.Empty;
        if ((!string.IsNullOrWhiteSpace(AnswerInputField.text)) &&
            AnswerInputField.text.ToLower() == Answer.ToLower())
        {
            MessagePanelText.text = "It's Correct.";
            AnswerInputField.text = string.Empty;
            MessagePanel.SetActive(true);
            IsCorrect = true;
        }
        else
        {
            MessagePanelText.text = "It's wrong.";
            MessagePanel.SetActive(true);
            IsCorrect = false;
        }
    }

    /// <summary>Reduces the player health by 2 hearts.</summary>
    private void ReducePlayerHealth()
    {
        PlayerHealth = PlayerHealth - 2;
        PlayerHealthText.text = PlayerHealth.ToString();
        if (PlayerHealth == 0)
        {
            IsTryingToExit = true;
            MessagePanelText.text = "Game Over! \n You lost all your health.";
            MessagePanel.SetActive(true);
        }
        UpdateUserGameData();
    }



    /// <summary>Handles the game menu button. To show co-player details</summary>
    public void HandleGameMenuButton()
    {
        List<UserGameData> userList = GameModel.GetGamePayerlist();
        
        float templateHeight = 30f;
        int index = 0;
        userList.ForEach(user => {

            Transform scoreTransform = Instantiate(ScoreTemplate, ScoreTemplateContainer);
            RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
            scoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * index);
            DestroyList.Add(scoreTransform.gameObject);
            scoreTransform.Find("NameText").GetComponent<Text>().text = userList[index].Username;
            scoreTransform.Find("HealthText").GetComponent<Text>().text = userList[index].Health.ToString();
            scoreTransform.Find("TimeText").GetComponent<Text>().text = userList[index].TimeTaken.ToString();
            index = index + 1;
        });
        PlayerDetailsPanel.SetActive(true);
    }


    /// <summary>Handles the player ok button. Closes the player details panel.</summary>
    public void HandlePlayerOkButton()
    {

        PlayerDetailsPanel.SetActive(false);
        DestroyList.ForEach(gameObject => Destroy(gameObject));
        DestroyList.Clear();
    }


    /// <summary>Closes the chat rooom.</summary>
    public void CloseChatRooom()
    {
        ChatRoomPanel.SetActive(false);
    }


    /// <summary>  Will be used to update the chat room. Under development now. Will be ready for Milestone 3</summary>
    private void ShowChatMsgs()
    {
        float templateHeight = 49f;
        List<string> userList = new List<string> { "achu", "jhon", "achu", "Mike" };
        List<string> healthList = new List<string> { "Hi Guys", "Hello Achu. Hows your game?", "Hi Jhon, It going really intersting", "Hi Guys" };
        for (int i = 0; i < 4; i++)
        {
            Transform scoreTransform = Instantiate(TestTemplate, TestContainer);
            RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
            scoreRectTransform.anchoredPosition = new Vector2(-240, -templateHeight * (i + 1));
            scoreTransform.Find("UsernameText").GetComponent<Text>().text = userList[i];
            scoreTransform.Find("MessageText").GetComponent<Text>().text = healthList[i];
            scoreTransform.Find("Splitter").GetComponent<Text>().text = "----------------------------------------------------------------------------------";
        }
    }
}
