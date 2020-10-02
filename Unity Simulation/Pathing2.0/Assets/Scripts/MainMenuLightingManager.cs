/**
    @file MainMenuLightingManager
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class handles the lighting for the main menu background
*/
[ExecuteAlways]
public class MainMenuLightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField]
    public Light DirectionalLight;
    [SerializeField]
    public LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)]
    public float timeOfDay;
	
	/// Called every frame and updates the time of day for the main menu
    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            timeOfDay += (Time.deltaTime * 0.05f);
            timeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

	/// Helper function for updating the light in the main menu background
    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }
}
