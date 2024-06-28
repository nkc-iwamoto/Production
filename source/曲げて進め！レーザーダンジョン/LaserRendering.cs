using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRendering : MonoBehaviour
{
    [SerializeField,Header("レーザーの光の強度")]
    float intensity;

    [SerializeField, Range(0, 1), Header("レーザーの太さ")]
    float scaleOffset = 0.25f;

    [SerializeField,Header("レーザーの色データ")]
    LaserColorData laserColorData;

    public void LaserSetActive(GameObject laserObject,bool isSetActive)
    {
        laserObject.SetActive(isSetActive);
    }


    public void RenderingLaserColor(GameObject laserObject,int intColorCode)
    {
        Color color = laserColorData.colors[intColorCode];

        GameObject childLaserObject = laserObject.transform.GetChild(0).gameObject;
        SetObjectEmission(childLaserObject, color);
    }

    private void SetObjectEmission(GameObject gameObject, Color color)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        float factor = Mathf.Pow(2, intensity);
        meshRenderer.material.SetColor("_Color", new Color(color.r * factor, color.g * factor, color.b * factor));
    }

    public void LaserTransform(Laser laser, Vector3 endPos,GameObject laserObject)
    {
        Vector3 startPos = laser.StartPos;

        Vector3 vector = endPos - startPos;
        Vector3 centerPosition = Vector3.Lerp(startPos, endPos, 0.5f);

        Vector3 vectorNormalized = vector.normalized;


        float scale = vector.magnitude;

        laserObject.transform.position = centerPosition;
        laserObject.transform.localScale = new Vector3(scaleOffset, scale, scaleOffset);
    }

    public void LaserAngle(Laser laser , Vector3 endPos,GameObject laserObject)
    {
        Vector3 startPos = laser.StartPos;

        Vector3 vector = endPos - startPos;
        Vector3 vectorNormalized = vector.normalized;
        float radianZ = Mathf.Atan2(vectorNormalized.x, vectorNormalized.y) * Mathf.Rad2Deg;
        laserObject.transform.rotation = Quaternion.Euler(0,0,-radianZ );

    }

}
