using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GameOptionsMenu : MonoBehaviour {
    public Dropdown drpScreenResolution;  // Represents the Screen Resolution dropdown menu
    public Toggle tglFullscreen;  // Stores the current Full Screen status of the game
    private Resolution[] supportedScreenResolutions;  // Stores all the resolutions supported by the display device

    /* Stores all settings selected by the user */
    private Resolution setResolution;  // Stores the selected 'Screen Resolution' option
    private bool setFullScreen = false;  // Stores the selected 'Full Screen' option

    /* 'Options' Menu actions */

    /* Use this for initialisation */
    void Start()
    {
        /* Retrieves and populates Screen Resolution dropdown with all supported screen resolutions */
        supportedScreenResolutions = Screen.resolutions;

        for (int i = 0; i < supportedScreenResolutions.Length; i++)
        {
            drpScreenResolution.options.Add(new Dropdown.OptionData(resolutionToString(supportedScreenResolutions[i])));
            
            /* Updates the dropdown with the current screen resolution */
            if (Screen.currentResolution.width == supportedScreenResolutions[i].width && Screen.currentResolution.height == Screen.currentResolution.height)
            {
                drpScreenResolution.value = i;
            }
        }

        /* Retrieves and populates Fullscreen toggle with game's current full screen status */
        if (Screen.fullScreen == true)
        {
            tglFullscreen.isOn = true;  // Selects toggle
            setFullScreen = tglFullscreen.isOn; // Updates Full Screen status
        }
        else
        {
            tglFullscreen.isOn = false;
            setFullScreen = tglFullscreen.isOn;
        }
    }

    /* Updates selected 'Screen Resolution' option (on change of selection) */
    public void drpScreenResolution_OnValueChanged(int index)
    {
        setResolution = supportedScreenResolutions[drpScreenResolution.value];
    }

    /* Updates selected 'Full Screen' status (on toggle)*/
    public void tglFullscreen_OnValueChanged(bool status)
    {
        setFullScreen = tglFullscreen.isOn;
    }

    /* 'Apply' button - Applies any changes made to game options and opens the Main Menu */
    public void btnPnlGameOptionsApply_Click()
    {
        /* Applies the selected 'Screen Resolution' setting */
        Screen.SetResolution(setResolution.width, setResolution.height, setFullScreen);
    }


    /* 'Cancel' button - Discards any changes made and opens the Main Menu */
    public void btnPnlGameOptionsCancel_Click()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    /* Converts resolution options into strings (helper function) */
    private string resolutionToString(Resolution inputResolution)
    {
        return inputResolution.width + " x " + inputResolution.height;
    }
}
