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
    public static GameObject MessagePanel;

    // Used to set the message inside Message panel
    public static Text MessagePanelText;

    // Used to define the player details panel
    public static GameObject PlayerDetailsPanel;

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
    private static int PlayerHealth = 10;

    // To define the player health
    public Text PlayerHealthText;

    // To track the time taken to finsish the game
    private float StartTime;

    // To define the timer for game
    public static Text GameTimerText;

    // To define each game level
    public Text LevelText;

    // To define whether the game starts or not
    private bool IsStartGame = false;

    // To definde whether the game is finished or not
    private bool IsGameFinished = false;


    // To define the score template container
    public static Transform ScoreTemplateContainer;

    // To define Score template
    public static Transform ScoreTemplate;

    // To define the score template container
    public static Transform MessageContainer;

    // To define Score template
    public static Transform MessageTemplate;

    // To define game objects to destroy
    private static List<GameObject> DestroyList = new List<GameObject>();
    


    /// <summary>
    /// Start method is used to initialize or assign values or actions to required 
    /// variable or components before the first frame update.
    /// </summary>
    void Start()
    {
        GetAllComponents();
        MessagePanel.SetActive(false);
        CancelButton.SetActive(false);
        PlayerDetailsPanel.SetActive(false);
        GameModel.CurrentLevel = 1;
        ShowGameStartDialog();
    }

    /// <summary>
    /// Gets all components.
    /// </summary>
    private void GetAllComponents()
    {
        GameTimerText = GameObject.Find("GameTimerText").GetComponent<Text>();
        ChatRoomPanel = GameObject.Find("GameChatRoomPanel");
        MessageContainer = GameObject.Find("MessageContainer").GetComponent<Transform>();
        MessageTemplate = GameObject.Find("MesssageTemplate").GetComponent<Transform>();
        ChatRoomPanel.SetActive(false);
        MessagePanel = GameObject.Find("GameMessagePanel");
        MessagePanelText = MessagePanel.GetComponentInChildren<Text>();
        MessagePanel.SetActive(false);
        PlayerDetailsPanel = GameObject.Find("PlayerDetailsPanel");
        ScoreTemplateContainer = GameObject.Find("ScoreTemplateContainer").GetComponent<Transform>();
        ScoreTemplate = GameObject.Find("ScoreTemplate").GetComponent<Transform>();
        PlayerDetailsPanel.SetActive(false);
    }


    /// <summary>Updates this instance for each frame update</summary>
    private void Update()
    {
        CheckTiltForAnsweringDirections();
        if (IsStartGame)
            GameTimerText.text = (Time.time - StartTime).ToString("0.0");
    }

    /// <summary>
    /// Checks the tilt for answering directions.
    /// </summary>
    private void CheckTiltForAnsweringDirections()
    {
        if(GameModel.CurrentLevel == 2)
        {
            float xdeg = Input.acceleration.x;
            float ydeg = Input.acceleration.y;
            if (xdeg > 0.5)
            {
                AnswerInputField.text = "east";
            }
            else if (xdeg < -0.5)
            {
                AnswerInputField.text = "west";
            }
            else if (ydeg > 0.5)
            {
                AnswerInputField.text = "north";
            }
            else if (ydeg < -0.5)
            {
                AnswerInputField.text = "south";
            }
        }
    }

    /// <summary>Shows the first message dialog box when game starts</summary>
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

        if (IsGameFinished)
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
        else if (GameModel.CurrentLevel == 1 && QuestionCount == 0)
            SetQuestionOne();
        else if (IsCorrect)
            CheckLevelAndGetNext();
        else if (!IsCorrect)
            ReducePlayerHealth();
    }


    /// <summary>Checks the level and get next scene and updates the game related player information in the database.</summary>
    private void CheckLevelAndGetNext()
    {
        // checking whether the current level is completed and ready to load next level
        if (GameModel.CurrentLevelTotalCount == QuestionCount)
        {
            // loading message to show when each level successfully completes
            MessagePanelText.text = GetMessageForLevelPass();
            // Checks whether the user passes current level
            if (UpdateGameSceneDataLists())
            {
                // updates the game related user informaion
                UpdateUserGameData();
                QuestionCount = 0;
                MessagePanel.SetActive(true);
                // updating UI for level
                LevelText.text = "Level " + GameModel.CurrentLevel;
                AnswerInputField.text = string.Empty;
                IsCorrect = true;
            }
            else
            {
                // when the player finish the game successfully
                MessagePanelText.text = "You have successfully completed all tasks and Saved the Parents. You are brave.";
                MessagePanel.SetActive(true);
                IsStartGame = false;
                IsGameFinished = true;
                // updates the game related user informaion
                UpdateUserGameData();
            }
        }
        else
        {
            // loading the next scene in the current level
            SceneData sceneData = GameModel.CurrentLevelSceneList[QuestionCount];
            QuestionText.text = sceneData.Question;
            Answer = sceneData.Answer;
            QuestionCount++;
            // updates the game related user informaion
            UpdateUserGameData();
        }
    }

    /// <summary>Updates the user game data.</summary>
    private void UpdateUserGameData()
    {
        // checking whether the game is running or not
        if (!IsStartGame)
        {
            // Checking the current player health is better than the previous game
            if (PlayerHealth > GameModel.CurrentUser.BestHealth)
                GameModel.CurrentUser.BestHealth = PlayerHealth;
            // Checking the current time took is better than the previous game
            if (Convert.ToDouble(GameTimerText.text) > GameModel.CurrentUser.BestTime)
                GameModel.CurrentUser.BestTime = Convert.ToDouble(GameTimerText.text);
            // check whether the game is finishe or not
            if (IsGameFinished)
            {
                // updating the user and game related user data
                GameModel.CurrentUser.GamesWon = GameModel.CurrentUser.GamesWon + 1;
                GameModel.UserGameData.IsWon = 1;
                GameModel.UserGameData.IsFinished = 1;
                // updating game related data when the game is finished
                UpdateGameData();
            }
        }
        else
        {
            GameModel.UserGameData.IsFinished = 1;
        }
        // Updating the game realted user data 
        GameModel.UserGameData.Health = PlayerHealth;
        GameModel.UserGameData.TimeTaken = Convert.ToDouble(GameTimerText.text);
        GameModel.UserGameData.CurrentLevel = GameModel.CurrentLevel;
        GameModel.UpdateUserGameData(GameModel.CurrentUser, GameModel.UserGameData);

    }

    /// <summary>Updates the game data when a game is finished.</summary>
    private void UpdateGameData()
    {
        GameModel.GetUpdatedGameData(GameModel.GameData.GameId);
    }

    /// <summary>
    /// Rcieved game object will be updated when a user finish the game
    /// </summary>
    /// <param name="pGameDataList">The Game Data List.</param>
    public static void JsnUpdateGameDataSuccessReciverDel(List<GameData> pGameDataList)
    {
        GameData gameData = pGameDataList[0];
        if (string.IsNullOrWhiteSpace(gameData.Winner))
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
        switch (GameModel.CurrentLevel)
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
        GameModel.CurrentLevel = GameModel.CurrentLevel + 1;
        if (GameModel.CurrentLevel > GameModel.PlaySceneMap.Count)
            return false;
        GameModel.CurrentLevelSceneList.Clear();
        GameModel.CurrentLevelSceneList = GameModel.PlaySceneMap[GameModel.CurrentLevel];
        GameModel.CurrentLevelTotalCount = GameModel.CurrentLevelSceneList.Count;
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
        GameModel.CurrentLevelSceneList = GameModel.PlaySceneMap[GameModel.CurrentLevel];
        GameModel.CurrentLevelTotalCount = GameModel.CurrentLevelSceneList.Count;
        SceneData sceneData = GameModel.CurrentLevelSceneList[QuestionCount];
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

    /// <summary>Reduces the player health by 2 hearts when inputs wrong answer.</summary>
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
        // updating game related user data
        UpdateUserGameData();
    }



    /// <summary>Handles the game menu button. To show co-player details</summary>
    public void HandleGameMenuButton()
    {
        // getting palyer list of the game currently running
        GameModel.GetGamePayerlist();
    }
    
    /// <summary>
    /// This will display the score list of current game
    /// </summary>
    /// <param name="pUserList">The p user game list.</param>
    public static void JsnUserListSuccessRecieverDel(List<UserGameData> pUserGameList)
    {
        float templateHeight = 30f;
        int index = 0;
        // duplicating the score template for showing each user details
        pUserGameList.ForEach(user => {
            Transform scoreTransform = Instantiate(ScoreTemplate, ScoreTemplateContainer);
            RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
            scoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * index);
            DestroyList.Add(scoreTransform.gameObject);
            scoreTransform.Find("NameText").GetComponent<Text>().text = pUserGameList[index].Username;
            scoreTransform.Find("HealthText").GetComponent<Text>().text = pUserGameList[index].Health.ToString();
            scoreTransform.Find("TimeText").GetComponent<Text>().text = pUserGameList[index].TimeTaken.ToString();
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
    public void ShowChatMsgs()
    {
        ChatRoomPanel.SetActive(true);
        float templateHeight = 49f;
        List<string> userList = new List<string> { "achu", "jhon", "achu", "Mike" };
        List<string> healthList = new List<string> { "Hi Guys", "Hello Achu. Hows your game?", "Hi Jhon, It going really intersting", "Hi Guys" };
        for (int i = 0; i < 4; i++)
        {
            Transform scoreTransform = Instantiate(MessageTemplate, MessageContainer);
            RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
            scoreRectTransform.anchoredPosition = new Vector2(-240, -templateHeight * (i + 1));
            scoreTransform.Find("UsernameText").GetComponent<Text>().text = userList[i];
            scoreTransform.Find("MessageText").GetComponent<Text>().text = healthList[i];
            scoreTransform.Find("Splitter").GetComponent<Text>().text = "----------------------------------------------------------------------------------";
        }
    }
}
