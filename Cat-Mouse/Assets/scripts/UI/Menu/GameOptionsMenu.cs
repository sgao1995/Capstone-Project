using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GameOptionsMenu : MonoBehaviour {
    public Dropdown drpScreenResolution;  // Represents the Screen Resolution dropdown menu
    public Toggle tglFullscreen;  // Stores the current Full Screen status of the game
    public Dropdown drpGraphicsQuality;  // Represents the Graphics Quality dropdown menu

    private Resolution[] supportedScreenResolutions;  // Stores all the resolutions supported by the display device
    private string[] supportedGraphicsQuality;  // Stores all the Graphics Quality settings supported by the game

    /* Stores all settings selected by the user */
    private Resolution setResolution;  // Stores the selected Screen Resolution option
    private bool setFullScreen;  // Stores the selected Full Screen option
    private int setGraphicsQuality;  // Stores the index of selected Graphics Quality option

    /* Options menu actions */

    /* Use this for initialisation */
    void Start()
    {
        /* Retrieves and populates Screen Resolution dropdown with all supported screen resolutions */
        supportedScreenResolutions = Screen.resolutions;

        for (int i = 0; i < supportedScreenResolutions.Length; i++)
        {
            drpScreenResolution.options.Add(new Dropdown.OptionData(resolutionToString(supportedScreenResolutions[i])));
            drpScreenResolution.RefreshShownValue();  // Refreshes the selected resolution

            /* Updates the Screen Resolution dropdown with the current screen resolution */
            if (Screen.currentResolution.width == supportedScreenResolutions[i].width && Screen.currentResolution.height == supportedScreenResolutions[i].height)
            {
                drpScreenResolution.value = i;
                setResolution = supportedScreenResolutions[i];
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

        /* Retrieves and populates Graphics Quality dropdown with all supported graphic quality settings */
        supportedGraphicsQuality = QualitySettings.names;

        for (int i = 0; i < supportedGraphicsQuality.Length; i++)
        {
            drpGraphicsQuality.options.Add(new Dropdown.OptionData(supportedGraphicsQuality[i]));
            drpGraphicsQuality.RefreshShownValue();
        }
        drpGraphicsQuality.value = QualitySettings.GetQualityLevel();  // Updates dropdown with current setting
    }

    /* Updates selected Screen Resolution option (on change of selection) */
    public void drpScreenResolution_OnValueChanged(int index)
    {
        setResolution = supportedScreenResolutions[drpScreenResolution.value];
    }

    /* Updates selected Full Screen status (on toggle)*/
    public void tglFullscreen_OnValueChanged(bool status)
    {
        setFullScreen = tglFullscreen.isOn;
    }

    /* Updates selected Graphics Quality option (on change of selection) */
    public void drpGraphicsQuality_OnValueChanged(int index)
    {
        setGraphicsQuality = index;
    }

    /* 'Apply' button - Applies any changes made to game options and opens the Main Menu */
    public void btnPnlGameOptionsApply_Click()
    {
        /* Applies the selected Screen Resolution and Full Screen settings */
        Screen.SetResolution(setResolution.width, setResolution.height, setFullScreen);

        /* Applies the selected Graphics Quality settings */
        QualitySettings.SetQualityLevel(setGraphicsQuality, true);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Returns to the Main Menu
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
