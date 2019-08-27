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
    public Button CancelButton;

    // To define whether the answer is correct or not
    public bool IsCorrect = false;

    /**
    * Start method is used to initialize or assign values or actions to required 
    * variable or components before the first frame update
    */
    void Start()
    {
        MessagePanel.SetActive(false);
        CancelButton.enabled = false;
        PlayerDetailsPanel.SetActive(false);
        SetQuestionOne();
    }

   
    /**
     * ValidateAnswer is used to validate the answer
     */
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

    /**
     * SetQuestionOne is used to set the first question when the game room is loaded
     */
    private void SetQuestionOne()
    {
        QuestionText.text = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Question;
        Answer = GameManager.GameManagerInstance.GameModelInstance.CurrentQuestion.Answer;
        QuestionCount++;
    }

    /**
     * ExitGame is used to exit from the game.
     * As of now no game data will be saved, hence leaving game in between, later need to start from level 1
     */
    public void ExitGame()
    {
        IsTryingToExit = true;
        MessagePanelText.text = "Game data won't be saved. Are you sure you want to leave.";
        CancelButton.enabled = true;
        MessagePanel.SetActive(true);
    }

    /**
     * HandleCancelButton is used to handle the click event of cancel button in message panel,
     * if clicked the user will stay in the program
     */
    public void HandleCancelButton()
    {
        CancelButton.enabled = false;
        MessagePanel.SetActive(false);
    }

    /**
     * HandleOkButton is used to handle the click event of ok button in message panel
     */
    public void HandleOkButton()
    {
        CancelButton.enabled = false;
        MessagePanel.SetActive(false);
        if (IsTryingToExit)
            SceneManager.LoadScene("GameHome");
        else if(IsCorrect)
            GetAndShowNextQuestion();

    }

    /**
     * GetAndShowNextQuestion is used to get and show next question when each question is answered correctly
     */
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
        QuestionText.text = resultList[0];
        Answer = resultList[1];
        QuestionCount++;
    }
}
