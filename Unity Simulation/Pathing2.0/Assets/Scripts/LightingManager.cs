/**
	@file LightingManager.cs
*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Mirror;

/**
	This class is to implement day-night cycle
*/
[ExecuteAlways]
public class LightingManager : NetworkBehaviour
{
    //Scene References
    [SerializeField]
    public Light DirectionalLight;
    [SerializeField]
    public LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)]
    [SyncVar]
    public float timeOfDay;
    private float tValue = 0.0f;

    [SerializeField]
    public GameObject carSpawner;

    private spawning spawningScript;

	/// Called upon initialization of object
    private void Start()
    {
        spawningScript = carSpawner.GetComponent<spawning>();
    }

	/// Called once per frame, updates the time and the spawning rate accordingiy
    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            timeOfDay += (Time.deltaTime * 0.05f);
            timeOfDay %= 24; //Modulus to ensure always between 0-24
            if (timeOfDay > 6 && timeOfDay < 9)
            {
                tValue += Time.deltaTime * 0.05f;
                spawningScript.speed = Mathf.Lerp(60.0f, 90.0f, tValue);
            }
            /*Yes, the branching is stupid and there is probably a better way to code this but it is 1am*/
            /*The spawn rate does jump from 90->60->90 with this is implementation and the time values
            are not set low enough to hide this but it is safer and tested with these so leave the code as is :)*/
            else if (timeOfDay > 9.01 && timeOfDay < 9.1)
            {
                tValue = 0.0f;
            }
            else if (timeOfDay > 9.1 && timeOfDay < 12.9)
            {
                tValue += Time.deltaTime * 0.05f;
                spawningScript.speed = Mathf.Lerp(90.0f, 60.0f, tValue);
            }
            else if (timeOfDay > 12.9 && timeOfDay < 15)
            {
                tValue = 0.0f;
            }
            else if(timeOfDay > 15 && timeOfDay < 18)
            {
                
                tValue += Time.deltaTime * 0.05f;
                spawningScript.speed = Mathf.Lerp(60.0f, 90.0f, tValue);
            }
            else if (timeOfDay > 18 && timeOfDay < 18.1)
            {
                tValue = 0.0f;
            }
            else if (timeOfDay > 5.9 && timeOfDay < 6.00)
            {
                tValue = 0.0f;
            }
            else
            {
                tValue += Time.deltaTime * 0.05f;
                spawningScript.speed = Mathf.Lerp(90.0f, 60.0f, tValue);
            }
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            spawningScript.speed = 60;
            UpdateLighting(timeOfDay / 24f);
        }
    }

	/// Updates the actual lighting in the scene
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

    /*
    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
    */
}
