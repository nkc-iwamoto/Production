using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LaserColorData")]
public class LaserColorData : ScriptableObject
{
    [ColorUsage(true,true),Header("レーザーの色")]
    public Color[] colors = new Color[7];

    [ColorUsage(true, true),Header("レーザーのエフェクトの色")]
    public Color[] effectColors = new Color[7];
}
