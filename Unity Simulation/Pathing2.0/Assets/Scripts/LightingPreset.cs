/**
	@file LightingPreset.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class is implemented to hold values concerning the day-night cycle
*/
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
