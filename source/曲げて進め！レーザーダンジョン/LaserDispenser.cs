using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDispenser : MonoBehaviour
{
    public enum COLOR
    {
        Red = 1,
        Blue,
        Green,
        Purple,
        Yellow,
        Cyan,
        white,
    }


    [SerializeField, Header("レーザーの最大屈折回数")]
    int maxConvertCount;

    [SerializeField, Header("レーザーの色")]
    COLOR color;

    [SerializeField, Header("描画するレーザーのオブジェクト")]
    GameObject laserObject;

    [SerializeField, Header("レーザーが撃ちだすときのエフェクト")]
    GameObject shotEffect;

    [SerializeField, Header("レーザーが着弾した時のエフェクト")]
    GameObject hitEffect;

    [SerializeField, Header("レーザーを描画するスクリプト")]
    LaserRendering laserRendering;

    [SerializeField, Header("レーザーのエフェクトを描画するスクリプト")]
    LaserEffectRendering laserEffectRendering;

    // レーザーの当たり判定を行うスクリプト
    RayShoter rayShot;

    // レーザーのデータ
    Laser laserData;

    // レーザーの色（int型にしたもの）
    int value;

    private void Start()
    {
        rayShot = new RayShoter();

    }


  
    private void LateUpdate()
    {
        value = (int)color;
        Vector3 direction = transform.up;
        Vector3 startPos = transform.position;

        laserData = new Laser(value, startPos, direction, gameObject);
        Vector3 endPos = rayShot.ShotRay(laserData, 0, maxConvertCount, out GameObject hitObject);

        if (endPos == Vector3.zero)
        {
            EffectInit();
        }
        EffectActive();
       
        LaserRendering(endPos);
        EffectRendering(endPos);
        laserEffectRendering.LaserHitEffect_SetColor(value, endPos,hitEffect);

        if (hitObject == null)
        { return; }    
        if (hitObject.CompareTag("Gimmick"))
        {
            laserEffectRendering.LaserHitEffect_SetActive(false,hitEffect);
        }
    }

    private void EffectInit()
    {
        laserRendering.LaserSetActive(laserObject,false);
        laserEffectRendering.LaserShotEffect_SetActive(false,shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(false,hitEffect);
    }

    private void EffectActive()
    {
        laserRendering.LaserSetActive(laserObject,true);
        laserEffectRendering.LaserShotEffect_SetActive(true,shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(true,hitEffect);
    }

    private void LaserRendering(Vector3 endPos)
    {
        laserRendering.LaserTransform(laserData, endPos, laserObject);
        laserRendering.RenderingLaserColor(laserObject,value);
    }

    private void EffectRendering(Vector3 endPos)
    {
        laserEffectRendering.LaserShotEffect_SetColor(value,shotEffect);
        laserEffectRendering.LaserHitEffect_SetColor(value, endPos,hitEffect);
    }
}