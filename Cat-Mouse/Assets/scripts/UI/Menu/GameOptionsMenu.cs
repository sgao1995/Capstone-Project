using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GameOptionsMenu : MonoBehaviour {
    public Dropdown drpScreenResolution;  // Represents the Screen Resolution dropdown menu
    public Toggle tglFullscreen;  // Represents the Full Screen toggle
    public Dropdown drpGraphicsQuality;  // Represents the Graphics Quality dropdown menu
    public Toggle tglVolumeLevelMute;  // Represents the Volume Level Mute toggle
    public Slider slidVolumeLevel;  // Represents the Volume Level slider

    private Resolution[] supportedScreenResolutions;  // Stores all the resolutions supported by the display device
    private string[] supportedGraphicsQuality;  // Stores all the Graphics Quality settings supported by the game

    /* Stores all settings selected by the user */
    private Resolution setResolution;  // Stores the selected Screen Resolution option
    private bool setFullScreen;  // Stores the selected Full Screen option
    private int setGraphicsQuality;  // Stores the index of selected Graphics Quality option
    private bool setVolumeLevelMute;  // Stores the selected Volume Level Mute option
    private float setVolumeLevel;  // Stores the selected Volume Level option

    /* Options menu actions */

    /* Use this for initialisation */
    void Start()
    {
        /* Retrieves and populates Screen Resolution dropdown with all supported screen resolutions of game */
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

        /* Retrieves and populates Fullscreen toggle with current full screen status of game */
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

        /* Retrieves and populates Graphics Quality dropdown with all supported graphic quality settings of game */
        supportedGraphicsQuality = QualitySettings.names;

        for (int i = 0; i < supportedGraphicsQuality.Length; i++)
        {
            drpGraphicsQuality.options.Add(new Dropdown.OptionData(supportedGraphicsQuality[i]));
            drpGraphicsQuality.RefreshShownValue();
        }
        drpGraphicsQuality.value = QualitySettings.GetQualityLevel();  // Updates dropdown with current setting

        /* Retrieves and adjusts Volume Level slider with current sound volume of game */
        slidVolumeLevel.value = AudioListener.volume;
        setVolumeLevel = slidVolumeLevel.value;
        
        /* Retrieves current volume level mute status of game */
        if (AudioListener.pause == true)
        {
            tglVolumeLevelMute.isOn = true;
            setVolumeLevelMute = tglVolumeLevelMute.isOn;
        }
        else
        {
            tglVolumeLevelMute.isOn = false;
            setVolumeLevelMute = tglVolumeLevelMute.isOn;
        }
    }

    /* Updates selected Screen Resolution option (on change of selection) */
    public void drpScreenResolution_OnValueChanged(int index)
    {
        setResolution = supportedScreenResolutions[drpScreenResolution.value];
    }

    /* Updates selected Full Screen option (on toggle) */
    public void tglFullscreen_OnValueChanged(bool status)
    {
        setFullScreen = tglFullscreen.isOn;   // Retrieves current On/Off setting of toggle
    }

    /* Updates selected Graphics Quality option (on change of selection) */
    public void drpGraphicsQuality_OnValueChanged(int index)
    {
        setGraphicsQuality = index;
    }

    /* Updates selected Volume Level option (on change of slider value) */
    public void slidVolumeLevel_OnValueChanged(float sliderValue)
    {
        setVolumeLevel = sliderValue;
    }

    /* Updates selected Volume Level Mute option (on toggle) */
    public void tglVolumeLevelMute_onValueChanged(bool status)
    {
        setVolumeLevelMute = tglVolumeLevelMute.isOn;
    }

    /* Apply button - Applies any changes made to game options and opens the Main Menu */
    public void btnPnlGameOptionsApply_Click()
    {
        /* Applies the selected Screen Resolution and Full Screen settings */
        Screen.SetResolution(setResolution.width, setResolution.height, setFullScreen);

        /* Applies the selected Graphics Quality settings */
        QualitySettings.SetQualityLevel(setGraphicsQuality, true);

        /* Applies the selected Volume Level settings */

        /* Volume Level setting */
        AudioListener.volume = setVolumeLevel;

        /* Volume Level Mute setting */

        if (setVolumeLevelMute == true)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Returns to the Main Menu
    }

    /* Cancel button - Discards any changes made and opens the Main Menu */
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
