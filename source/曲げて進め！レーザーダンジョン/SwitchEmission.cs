using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEmission : MonoBehaviour
{
    // エミッションをかける
    public void ObjectEmission(GameObject obj, Color color,int intensity)
    {
        // メッシュレンダラーを取得
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        
        // エミッションをかける
        meshRenderer.material.EnableKeyword("_EMISSION");

        // HDRをつける
        float factor = Mathf.Pow(2, intensity);

        // 色を変更
        meshRenderer.material.SetColor("_EmissionColor", new Color(color.r * factor, color.g * factor, color.b * factor));
    }

    // 色を初期化する
    public void ObjectEmissionExit(GameObject obj,Color initColor)
    {
        // メッシュレンダラーを取得
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

        // エミッションを消す
        meshRenderer.material.DisableKeyword("_EMISSION");

        // 色を初期化する
        meshRenderer.material.SetColor("_EmissionColor", initColor * 0.5f);
    }
}
