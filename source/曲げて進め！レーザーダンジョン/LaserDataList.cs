using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDataList : MonoBehaviour
{
    // レーザーのデータを公開
    public Laser[] laserData => laserDataList;

    // レーザーのデータ
    Laser[] laserDataList = new Laser[4];

    // enumの要素数
    int enumMaxNum;

    // 当たっているレーザーの数
    int laserCount;

    bool isCanInit = true;

    private void Start()
    {
        // 要素数を取得
        enumMaxNum = Common.GetEnumLength<COLOR_CODE>();
    }

    // レーザーの初期化
    public void InitalizeLaserData()
    { 
        if(!isCanInit)
        {
            return;
        }

        for (int i = 0; i < laserCount; i++)
        {
            laserDataList[i] = null;
        }
        isCanInit = false;
    }

    // 同じレーザーのデータがあるか
    public bool IsSameLaserData(Laser laser)
    {
        // レーザーの発射元が同じではない数
        int count = 0;
        for (int i = 0; i < laserDataList.Length; ++i)
        {
            // レーザーのデータに入っていなければコンテニュー
            if (laserDataList[i] == null)
            { continue; }

            // レーザーの発射元が同じならばコンテニュー
            if (laserDataList[i].StartGameObject == laser.StartGameObject)
            { continue; }

            // 1増やす
            count++;
        }
        // 同じのがあるか
        if (count == laserDataList.Length)
        // ない
        { return false; }

        // ある
        return true;
    }

    // 上書きする
    public void OverWritingLaserDataList(Laser laser)
    {
        // 発射元が同じ配列の添字
        int elementNumber = 0;

        for (int i = 0; i < laserDataList.Length; ++i)
        {
            // レーザーのデータが入っていない場合
            if (laserDataList[i] == null)
            // リターン
            { return; }

            // あれば
            // レーザーの発射元が同じならば
            if (laserDataList[i].StartGameObject == laser.StartGameObject)
            {
                // 配列の添字を取得
                elementNumber = i;
                break;
            }
        }
        // 上書き
        laserDataList[elementNumber] = laser;
    }

    // レーザーの色をセットする
    public int SetIntColor(int arrayNum)
    {
        // 返り値の引数
        int tmp = 0;

        // 当たっているレーザーの数が1つ
        const int SINGLE_COLOR = 1;

        // 配列の添字を取得
        int elementNumber = arrayNum - 1;

        // レーザーのデータが入っているか
        if (laserDataList[0] == null)
        {  return tmp; }

        // ある
        // 当たっている数が1つか
        if (arrayNum == SINGLE_COLOR)
        {
            // 1つなら
            // 色合成はなし
            tmp = laserDataList[elementNumber].ColorNum;
            return tmp;
        }

        for (int i = 0; i < arrayNum; ++i)
        {
            switch (laserDataList[i].ColorNum)
            {
                case (int)COLOR_CODE.Purple:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {
                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Red || laserDataList[k].ColorNum == (int)COLOR_CODE.Blue))
                                continue;
                            
                            tmp = (int)COLOR_CODE.Purple;
                        }
                    }
                    break;
                case (int)COLOR_CODE.Yellow:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {

                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Red || laserDataList[k].ColorNum == (int)COLOR_CODE.Green))                            
                                continue;

                            tmp = (int)COLOR_CODE.Yellow;
                        }
                    }
                    break;
                case (int)COLOR_CODE.Cyan:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {
                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Blue || laserDataList[k].ColorNum == (int)COLOR_CODE.Green))
                                continue;
                            tmp = (int)COLOR_CODE.Cyan;
                        }
                    }
                    break;
                case (int)COLOR_CODE.White:
                    {
                        tmp = (int)COLOR_CODE.White;
                    }
                    break;
                default:
                    continue;
            }
            return tmp;
        }

        // 色合成
        for (int i = 0; i < arrayNum; i++)
        {
            tmp += laserDataList[i].ColorNum;
        }
        tmp += 1;
        
        // enumの要素数より多かったら最大値を代入
        int setColorNumber_int = tmp > enumMaxNum ? 0 : tmp;
        return setColorNumber_int;
    }

    // レーザーのデータを追加する
    public int AddLaserDataList(Laser laser, int nowColorCode)
    {
        // レーザーのデータが入っていなければ
        if (laserDataList[0] == null)
        {
            // 追加
            laserDataList[0] = laser;
            // 当たっている数を１にする
            laserCount = 1;
        }
        // レーザーのデータが入っていなければ
        if (laserDataList[1] == null)
        {
            // 1つ前のデータが今当たっているレーザーの色と同じではない
            // かつ
            // 今のレーザーの色と今当たっているレーザーの色と同じではない
            if (laserDataList[0].ColorNum != laser.ColorNum
                && nowColorCode != laser.ColorNum)
            {
                // 追加
                laserDataList[1] = laser;
                // 当たっている数を２にする
                laserCount = 2;
            }
        }
        // レーザーのデータが入っていなければ
        if (laserDataList[2] == null)
        {
            // 1つ前のデータが今当たっているレーザーの色と同じではない
            // かつ
            // ２つ前のデータが今当たっているレーザーの色と同じではない
            // かつ
            // 今のレーザーの色と今当たっているレーザーの色と同じではない
            if ((laserDataList[0].ColorNum != laser.ColorNum)
                && (laserDataList[1].ColorNum != laser.ColorNum)
                && (nowColorCode != laser.ColorNum))
            {
                // 追加
                laserDataList[2] = laser;
                // 当たっている数を３にする
                laserCount = 3;
            }
        }

        isCanInit = true;

        return laserCount;
    }
    private bool AAA(int intColorCode)
    {
        if (intColorCode > (int)COLOR_CODE.Purple)
            return false;


        return true;
    }
}
