using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LaserColorData")]
public class LaserColorData : ScriptableObject
{
    [ColorUsage(true,true),Header("���[�U�[�̐F")]
    public Color[] colors = new Color[7];

    [ColorUsage(true, true),Header("���[�U�[�̃G�t�F�N�g�̐F")]
    public Color[] effectColors = new Color[7];
}
