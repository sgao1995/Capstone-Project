using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MainMenu : MonoBehaviour {

    /* Main Menu behaviours */

    /* 'New Game' Button - Creates a new game */
    public void btnNewGame_Click()
    {
        SceneManager.LoadScene("catmousegame", LoadSceneMode.Single);   // Loads the demonstration map
    }

    /* 'Options' button - Opens the 'Options' Menu */
    public void btnOptions_Click()
    {
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Single);  // Loads the 'Options' Menu
    }

    /* 'Quit' button - Exits the current game */
    public void btnQuit_Click()
    {
        Application.Quit();  // Quits the player application
    }
}
