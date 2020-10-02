/**
    @file OptionsMenu
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
	This Options class handles all the logic for the options menu
*/
public class OptionsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

	/// Called upon initialization and sets the correct resolution
    void Start(){
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height){
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
	
	/// Setter for setting the resolution
    public void SetResolution (int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
	
	/// Setter for setting the quality of the application
    public void SetQuality (int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

	/// Setter for setting fullscreen or not
    public void SetFullScreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
}
