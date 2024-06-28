using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RayShoter
{
    // 最後に当たったオブジェクトを保存する変数
    GameObject lastHitObj;

    // レーザーが当たっている時
    public Vector3 ShotRay(Laser laser, int f_count, int maxConvertCount, out GameObject hitObject)
    {
        // レーザーが当たっている座標の変数を初期化
        Vector3 laserHitPos = Vector3.zero;
        // 当たっているオブジェクトの初期化
        hitObject = null;

        // レイを飛ばす
        if (!Physics.Raycast(laser.StartPos, laser.Direction, out RaycastHit hit)) { return laserHitPos; }

        // 当たった物がIRayreceiverを持っているか-------------------------------------------------------

        if (IsExit(hit ,out IRayReceiver rayReceiver))
        {
            // 持っていなければ
            // 最後に当たったオブジェクトに当たっていない処理を呼び出す
            ExitRay();
        }
        else
        {
            // 持っていれば
            // レーザーを曲げる回数が最大を超えたか
            if (f_count <= maxConvertCount)
            {
                // 当たっている処理を呼び出す
                rayReceiver.RayStay(laser, f_count, maxConvertCount);
            }
            // 最後に当たったオブジェクトを記憶する
            lastHitObj = hit.collider.gameObject;
        }
        // ---------------------------------------------------------------------------------------------

        // プレイヤーに当たったら
        if (hit.collider.TryGetComponent(out PlayerDeath playerDeath))
        {
            // プレイヤーを死亡させるメソッドを呼び出す
            playerDeath.Death().Preserve();
        }

        // 当たっているオブジェクトに代入
        hitObject = lastHitObj;

        // レーザーが当たった座標を代入
        laserHitPos = hit.point;

        // レーザーが当たった座標を返す
        return laserHitPos;
    }

    public void ExitRay()
    {
        // 最後に当たったオブジェクトがなかったら return
        if (lastHitObj == null) { return; }
        // 最後に当たったオブジェクトがIRayReceiverを持っているか
        // 持っていなければ return
        if (!lastHitObj.TryGetComponent(out IRayReceiver lastHitRecevier)) { return; }
        // 最後に当たったオブジェクトを初期化する
        lastHitObj = null;
        // 持っていれば当たっていない処理を呼び出す
        lastHitRecevier.RayExit();
    }

    // レーザーが当たっていない処理をしていいか
    private bool IsExit(RaycastHit hit, out IRayReceiver rayReceiver)
    {
        // IRayReceiverを持っていない時
        // リターン
        if (!hit.collider.TryGetComponent(out rayReceiver)) { return true; }
        // 前フレームレーザーが当たったオブジェクトが今フレーム当たっているオブジェクトと同じか
        // かつ
        // 前フレームレーザーが当たったオブジェクトがnullではない時
        // リターン
        if(lastHitObj != hit.collider.gameObject　&& lastHitObj != null) { return true; }


        return false;
    }
}