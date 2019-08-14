using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHomeController : MonoBehaviour
{
 
    public Text GameDescription;
    // This method is called when the script is activated. Here we can include the code to update the game objects when the scene is loading.
    void Start()
    {
        LoadGameDescription();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is used to update the game description when the GameHome Scene is loaded
    private void LoadGameDescription()
    {
        GameDescription.text = "\t\t\t Once there lived a bunny, Jack with his parents Will and Pink in a village close to a furious jungle. In the deep jungle " +
            "there also lived a fearsome monster in the castle who preyed on innocent animals. One day, Will and Pink along with their neighbour Tinku Duck " +
            "went to the jungle in search of fruits, vegetables and firewood. They were not able to find enough fruits in the outer jungle, so they went deep " +
            "into the forest.As they went deep, there was a garden full of fruits and vegetables. They ran into the garden without thinking twice to pick the fruits. " +
            "They are not aware of that it is monster's garden. " + "\n"  +
            "\t\t\t They plucked enough fruits and were relaxing in the meadow. By that time the monster came back from his hunting. He saw these three uninvited animals" +
            " trespassed in his garden and found that they took his fruits without his permission. He got angry, rushed towards them caught Will and Pink Somehow Tinku" +
            " managed to escape from the monster and rushed to the village.";
    }

    public void LoadJackTinkuDialogScene()
    {
        SceneManager.LoadScene("JackTinkuDialog");
    }
}
