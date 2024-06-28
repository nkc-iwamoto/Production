using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffectRendering : MonoBehaviour
{
    [SerializeField, Header("レーザーの色データ")]
    LaserColorData laserColorData;

    [Header("ポイントライトrangeの加減数値")]
    public float fluctuationNumber;

    [Header("ポイントライトのrangeの最大値")]
    public float maxPointRange;
    [Header("ポイントライトのrangeの最小値")]
    public float minPointRange;

    public void LaserHitEffect_SetActive(bool isActive,GameObject hitEffect)
    {
        hitEffect.SetActive(isActive);
    }
    public void LaserShotEffect_SetActive(bool isActive,GameObject shotEffect)
    {
        shotEffect.SetActive(isActive);
    }

    public void LaserShotEffect_Angle(GameObject shotEffect,float angle)
    {
        shotEffect.transform.localRotation = Quaternion.Euler(0,0,angle + 90);
    }

    public void LaserShotEffect_SetColor(int intColorCode,GameObject shotEffect)
    {
        GameObject effectParticleObject = GetChildObject(shotEffect, 0);

        // エフェクトのベースカラーを変更
        SetColor_BaseColor(effectParticleObject, intColorCode);

        // エフェクトのエミッションを変更
        SetColor_Emission(effectParticleObject, intColorCode);

        // ライトのカラーを変更
        SetLightColor(shotEffect, intColorCode);
    }

    public void LaserHitEffect_SetColor(int intColorCode, Vector3 hitPos,GameObject hitEffect)
    {
        // エフェクトが再生されるポジションを変更
        SetPosition(hitEffect, hitPos);

        // エフェクトのパーティクルを取得
        GameObject effectParticleObject = GetChildObject(hitEffect, 0);

        // エフェクトのベースカラーを変更
        SetColor_BaseColor(effectParticleObject, intColorCode);

        // エフェクトのエミッションを変更
        SetColor_Emission(effectParticleObject, intColorCode);

        // パーティクルのスタートカラーを変更
        SetStartColor(effectParticleObject, intColorCode);

        SetMeshColor(hitEffect, intColorCode);

        GameObject pointLightObject = GetChildObject(hitEffect, 2);
        Light poingtLight = pointLightObject.GetComponent<Light>();


        PointLight_SetColor(poingtLight, intColorCode);

        PointLight_Expand_Shrink(poingtLight);
    }


    private void SetColor_Emission(GameObject effect, int intColorCode)
    {
        // エフェクトのレンダラーを取得
        Renderer renderer = effect.GetComponent<Renderer>();

        // マテリアルにエミッションをかける
        renderer.material.EnableKeyword("_EMISSION");

        // マテリアルに渡すカラーを取得
        Color color = laserColorData.effectColors[intColorCode];

        // Emissionのカラー変更
        renderer.material.SetColor("_EmissionColor", color);
    }
    private void SetColor_BaseColor(GameObject effect, int intColorCode)
    {
        // エフェクトのレンダラーを取得
        Renderer renderer = effect.GetComponent<Renderer>();

        // マテリアルに渡すカラーを取得
        Color color = laserColorData.colors[intColorCode];

        // ベースカラーを変更
        renderer.material.color = color;
    }

    private void SetStartColor(GameObject effect, int intColorCode)
    {
        // スタートカラーに渡すカラーを取得
        Color color = laserColorData.colors[intColorCode];

        // ParticleSystem.MainModuleを取得
        ParticleSystem.MainModule particleSystem = effect.GetComponent<ParticleSystem>().main;

        // スタートカラーを変更
        particleSystem.startColor = color;
    }
    private void SetPosition(GameObject effect, Vector3 pos)
    {
        // ポジションを変更
        effect.transform.position = pos;
    }


    private void SetLightColor(GameObject effect, int intColorCode)
    {
        // エフェクトのライトを取得
        GameObject light = effect.transform.GetChild(1).gameObject;

        // ライトのカラーを変更
        light.GetComponent<Light>().color = laserColorData.colors[intColorCode];
    }

    private void SetMeshColor(GameObject effect,int intColorCode)
    {
        GameObject effectChild = effect.transform.GetChild(1).gameObject;

        MeshRenderer meshRenderer = effectChild.GetComponent<MeshRenderer>();
        meshRenderer.material.color = laserColorData.colors[intColorCode]
;    }


    private void PointLight_SetColor(Light pointLight, int intColorCode)
    {
        pointLight.intensity = 0.05f;
        pointLight.color = laserColorData.colors[intColorCode];
    }

    // ポイントライトのRangeの拡大・縮小
    private void PointLight_Expand_Shrink(Light pointLight)
    {
        float sin = Mathf.Abs(Mathf.Sin(2 * Mathf.PI * fluctuationNumber * Time.time)) * maxPointRange;

        if(sin <= minPointRange)
        {
            sin = minPointRange;
        }

        pointLight.range = sin;
    }

    private GameObject GetChildObject(GameObject gameObject, int getChildNumber)
    {
        return gameObject.transform.GetChild(getChildNumber).gameObject;
    }

}
