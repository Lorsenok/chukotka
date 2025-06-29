using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    //Currect game setup
    public static bool IsEnemiesTriggered { get; set; } = false;


    //Settings
    public static float Sound { get; set; } = 1.0f;
    public static float Music { get; set; } = 1.0f;

    public static float PostProcessingPower { get; set; } = 1.0f;

    public static float FOV { get; set; } = 60f;


    //Default Settings
    public static float SoundDefault { get; set; } = 1.0f;
    public static float MusicDefault { get; set; } = 1.0f;

    public static float PostProcessingPowerDefault { get; set; } = 1.0f;

    public static float FOVDefault { get; set; } = 60f;
}
