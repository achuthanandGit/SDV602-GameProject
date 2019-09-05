using Assets.scripts;
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

    // Used to know whether the user is trying to exit from the game
    private bool IsTryingToExit = false;

    // Used to define the answer of shown question
    private string Answer = string.Empty;

    // To set the question in game room
    public Text QuestionText;

    // To define the input field for answering
    public InputField AnswerInputField;

    // To count the questions
    private int QuestionCount = 1;

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


   
    /// <summary>
    /// Start method is used to initialize or assign values or actions to required 
    /// variable or components before the first frame update.
    /// </summary>
    void Start()
    {
        MessagePanel.SetActive(false);
        CancelButton.SetActive(false);
        PlayerDetailsPanel.SetActive(false);
        SetQuestionOne();
        StartTime = Time.time;
    }

    /// <summary>Updates this instance for each frame update</summary>
    private void Update()
    {
        GameTimerText.text = (Time.time - StartTime).ToString("0.0");
    }



    /// <summary>Validates the answer.</summary>
    public void ValidateAnswer()
    {
        string messagePanelText = string.Empty;
        if((!string.IsNullOrWhiteSpace(AnswerInputField.text)) &&
            AnswerInputField.text.ToLower() == Answer.ToLower())
        {
            MessagePanelText.text = "It's Correct.";
            MessagePanel.SetActive(true);
            IsCorrect = true;
        } else
        {
            MessagePanelText.text = "It's wrong.";
            MessagePanel.SetActive(true);
            IsCorrect = false;
        }
    }

   
    /// <summary>Sets the question one when the game room is loaded.</summary>
    private void SetQuestionOne()
    {
        QuestionText.text = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Question;
        Answer = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Answer;
        QuestionCount++;
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

   
    /// <summary>Handles the ok button in message panel.
    /// If the message is 'wrong answer' or 'correct answer', Closes the message panel and loads next question.
    /// If the messsage is about leaving the game room, then the game data will be saved and load the game home scene
    /// </summary>
    public void HandleOkButton()
    {
        CancelButton.SetActive(false);
        MessagePanel.SetActive(false);
        if (IsTryingToExit)
        {
            GameManager.GameManagerInstance.GameModelInstance = new GameModel();
            SceneManager.LoadScene("GameHome");
        }
        else if (IsCorrect)
            GetAndShowNextQuestion();
        else if (!IsCorrect)
            ReducePlayerHealth();

    }

    /// <summary>Reduces the player health by 2 hearts.</summary>
    private void ReducePlayerHealth()
    {
        PlayerHealth = PlayerHealth - 2;
        PlayerHealthText.text = PlayerHealth.ToString();
        if(PlayerHealth == 0)
        {
            IsTryingToExit = true;
            MessagePanelText.text = "Game Over! \n You lost all your health.";
            MessagePanel.SetActive(true);
        }
    }


    /// <summary>Get and show next question when previous is answered correctly.</summary>
    private void GetAndShowNextQuestion()
    {
        AnswerInputField.text = string.Empty;
        CommandProcessor aCommandProcessor = new CommandProcessor();
        List<string> resultList = new List<string>();
        switch(QuestionCount)
        {
            case 2:
                resultList.AddRange(aCommandProcessor.GetNext("QuestionTwo"));
                break;
            case 3:
                resultList.AddRange(aCommandProcessor.GetNext("QuestionThree"));
                break;
        }
        if (resultList.Count > 0)
        {
            QuestionText.text = resultList[0];
            Answer = resultList[1];
            QuestionCount++;
        }
    }

    
    /// <summary>Handles the game menu button. To show co-player details</summary>
    public void HandleGameMenuButton()
    {
        PlayerDetailsPanel.SetActive(true);
        if(GameManager.GameManagerInstance.GameMode == "random")
        {
            // need to implement later
        } else
        {
            // need to implement later
        }
    }

   
    /// <summary>Handles the player ok button. Closes the player details panel.</summary>
    public void HandlePlayerOkButton()
    {
        PlayerDetailsPanel.SetActive(false);
    }
}
