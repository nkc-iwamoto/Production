using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionConverter : MonoBehaviour, IRayReceiver
{
    Vector3 direction;

    private RayShoter rayShoter;

    // int型の色判定
    int intColorCode;
    // 屈折回数
    int count;
    Vector3 endPos;
    Laser laserData;
    bool isConvert = false;
    GameObject hitObject;

    [SerializeField, Header("描画するレーザーのオブジェクト")]
    GameObject laserObject;

    [SerializeField, Header("レーザーが撃ちだすときのエフェクト")]
    GameObject shotEffect;

    [SerializeField, Header("レーザーが着弾した時のエフェクト")]
    GameObject hitEffect;

    [SerializeField, Header("レーザーを描画するスクリプト")]
    LaserRendering laserRendering;

    [SerializeField]
    LaserDataList laserDataList;

    [SerializeField, Header("レーザーのエフェクトを描画するスクリプト")]
    LaserEffectRendering laserEffectRendering;

    private void Start()
    {
        // 屈折させる方向を指定
        direction = transform.up;
        // レイを飛ばすスクリプトを生成
        rayShoter = new RayShoter();
    }

    private void Update()
    {
        // 屈折させる方向を指定
        direction = transform.up;

        if (!isConvert)
        {
            EffectInit();
            return;
        }
        EffectActive();
        LaserRendering(endPos);
        EffectRendering(endPos);
        laserEffectRendering.LaserHitEffect_SetColor(intColorCode, endPos, hitEffect);

        if (hitObject == null) { return; }

        if (hitObject.CompareTag("Gimmick"))
        {
            laserEffectRendering.LaserHitEffect_SetActive(false, hitEffect);
        }

        laserDataList.InitalizeLaserData();
    }
    public void RayExit()
    {
        laserDataList.InitalizeLaserData();
        EffectInit();

        rayShoter.ExitRay();

        intColorCode = 0;
        count = 0;
        
        laserData = null;
        isConvert = false;
    }
    /// <summary>
    /// レーザーが当たっているとき、レーザーを屈折させる
    /// </summary>
    /// <param name="laser">レーザー情報</param>
    /// <param name="f_count">1フレーム内での屈折回数</param>
    /// <param name="maxConvertCount">1フレーム内での最大屈折回数</param>
    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {

        if (laserDataList.IsSameLaserData(laser))
        {
            laserDataList.OverWritingLaserDataList(laser);
        }

        count = laserDataList.AddLaserDataList(laser, intColorCode);

        intColorCode = laserDataList.SetIntColor(count);
        laserData = new Laser(intColorCode, transform.position, direction, gameObject);
        f_count++;
        endPos = rayShoter.ShotRay(laserData, f_count, maxConvertCount, out GameObject hitObject);

        this.hitObject = hitObject;



        isConvert = true;
    }

    /// <summary>
    /// エフェクトの初期化
    /// </summary>
    private void EffectInit()
    {
        laserRendering.LaserSetActive(laserObject, false);
        laserEffectRendering.LaserShotEffect_SetActive(false, shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(false, hitEffect);
    }

    /// <summary>
    /// エフェクトを表示させる
    /// </summary>
    private void EffectActive()
    {
        laserRendering.LaserSetActive(laserObject, true);
        laserEffectRendering.LaserShotEffect_SetActive(true, shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(true, hitEffect);
    }

    /// <summary>
    /// レーザーのtransformと、色の変更
    /// </summary>
    /// <param name="endPos">レーザーが当たっている座標</param>
    private void LaserRendering(Vector3 endPos)
    {
        if (laserData == null)
        { return; }
        laserRendering.LaserTransform(laserData, endPos, laserObject);
        laserRendering.RenderingLaserColor(laserObject, intColorCode);
    }

    /// <summary>
    /// エフェクトの描画
    /// </summary>
    /// <param name="endPos">レーザーが当たっている座標</param>
    private void EffectRendering(Vector3 endPos)
    {
        laserEffectRendering.LaserShotEffect_SetColor(intColorCode, shotEffect);
        laserEffectRendering.LaserHitEffect_SetColor(intColorCode, endPos, hitEffect);
    }


}
