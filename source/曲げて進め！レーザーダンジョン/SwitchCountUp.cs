using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCountUp : MonoBehaviour
{
    [SerializeField, Header("光る間隔時間")]
    float intervalTime;
    [SerializeField, Header("スイッチのエミッションをかけるスクリプト")]
    SwitchEmission switchEmission;

    float countDownTime = 0;
    // 光っている数
    int switchOnLightCount = 0;
    // 光っているスイッチの最大値
    int maxOnLightCount = 4;

    // スイッチの判定を取る
    public bool IsOnSwitch(GameObject[] objs,Color color,int intensity)
    {
        // スイッチが全部光ったら
        if (switchOnLightCount >= objs.Length)
        {
            // 時間を最大値にさせる
            countDownTime = intervalTime * 3;

            // 当たっている判定を出す
            return true;
        }

        // 時間を測定
        countDownTime += Time.deltaTime;

        // カウントアップ　スイッチを光らせる
        if (countDownTime > intervalTime * 3)
        {
            switchEmission.ObjectEmission(objs[3], color,intensity);
            // 光っている数を最大にする
            switchOnLightCount = maxOnLightCount;
            
        }
        else if (countDownTime > intervalTime * 2)
        {
            switchEmission.ObjectEmission(objs[2], color,intensity);
        }
        else if (countDownTime > intervalTime)
        {
            switchEmission.ObjectEmission(objs[1], color,intensity);
        }
        else if (countDownTime > 0)
        {
            switchEmission.ObjectEmission(objs[0], color,intensity);
        }

        return false;
    }

    // スイッチの情報を初期化
    public void Init(GameObject[] objects,Color initColor)
    {
        // スイッチの光を消す
        foreach (var item in objects)
        {
            switchEmission.ObjectEmissionExit(item, initColor);
        }
        // 測定した時間を初期化
        countDownTime = 0;
        // 光っている数を初期化
        switchOnLightCount = 0;


    }
}