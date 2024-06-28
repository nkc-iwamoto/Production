using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBranch : MonoBehaviour, IRayReceiver
{
    [SerializeField, Header("描画するレーザーのオブジェクト")]
    GameObject[] laserObjects;

    [SerializeField, Header("描画するレーザーのエフェクト(hit)のオブジェクト")]
    GameObject[] laserHitEffect;

    [SerializeField, Header("描画するレーザーのエフェクト(shot)のオブジェクト")]
    GameObject[] laserShotEffect;

    [SerializeField, Header("回転する角度(Deg)")]
    private float rotationAngle;

    // 例の描画するすくリプト
    private RayShoter[] rayShoters;

    // レーザーのカラー
    private int intColorCode;

    // 分岐しているか
    private bool isBranch;

    // 分岐する数
    private int branchLaserCount = 2;

    // 分岐したレーザーのデータ配列
    private Laser[] branchLaserData;

    // 分岐したレーザーの最後に当たったポジションの配列
    private Vector3[] branchLaser_endPos;

    // 分岐したレーザーの最後に当たったオブジェクトの配列
    private GameObject[] hitObjects;

    // 分岐したレーザーの方向
    private Vector3[] branchDirection;

    // 当たっているレーザーの数
    private int count;

    [SerializeField, Header("レーザーを描画するスクリプト")]
    LaserRendering laserRendering;

    [SerializeField, Header("レーザーのエフェクトを描画するスクリプト")]
    LaserEffectRendering laserEffectRendering;

    [SerializeField,Header("レーザーのデータに関するスクリプト")]
    LaserDataList laserDataList;


    private void Start()
    {
        rayShoters = new RayShoter[branchLaserCount];
        for(int i = 0;i<branchLaserCount;++i)
        {
            rayShoters[i] = new RayShoter();
        }

        hitObjects = new GameObject[branchLaserCount];
        branchLaserData = new Laser[branchLaserCount];
        branchLaser_endPos = new Vector3[branchLaserCount];
        branchDirection = new Vector3[branchLaserCount];
    }

    private void Update()
    {
        // 分岐してるか
        if (!isBranch)
        {
            // していないなら
            // レーザーとエフェクトを見えなくする
            InitRender();

            laserDataList.InitalizeLaserData();

            return;
        }
        // しているなら
        for (int i = 0; i < branchLaserCount; ++i)
        {
            // レーザーを描写
            laserRendering.LaserSetActive(laserObjects[i], true);

            // レーザーのtransformを調整
            laserRendering.LaserTransform(branchLaserData[i], branchLaser_endPos[i], laserObjects[i]);
            laserRendering.LaserAngle(branchLaserData[i], branchLaser_endPos[i], laserObjects[i]);

            // レーザーのカラーを描写する
            laserRendering.RenderingLaserColor(laserObjects[i], intColorCode);

            // レーザーのエフェクトを描写
            laserEffectRendering.LaserHitEffect_SetActive(true, laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetActive(true, laserShotEffect[i]);

            // レーザーのエフェクトのカラーを描写
            laserEffectRendering.LaserHitEffect_SetColor(intColorCode, branchLaser_endPos[i], laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetColor(intColorCode, laserShotEffect[i]);

            // 最後に当たったオブジェクトがあるか
            if (hitObjects[i] == null)
            { continue; }

            // ある場合
            // それがGimmickタグを持っているか
            if (hitObjects[i].CompareTag("Gimmick"))
            {
                // 持っているなら
                // 当たっているエフェクトを見えなくする
                laserEffectRendering.LaserHitEffect_SetActive(false, laserHitEffect[i]);
            }
        }

        // レーザーが撃ちだすエフェクトの角度を調整
        laserEffectRendering.LaserShotEffect_Angle(laserShotEffect[0], rotationAngle);
        laserEffectRendering.LaserShotEffect_Angle(laserShotEffect[1], -rotationAngle);
    }
    public void RayExit()
    {
        for (int i = 0; i < branchLaserCount; ++i)
        {
            rayShoters[i].ExitRay();
            InitRender();            
        }

        laserDataList.InitalizeLaserData();

        intColorCode = 0;
        count = 0;

        // 分岐してるか
        if (!isBranch)
            return;

        // 分岐してないよ
        isBranch = false;
    }

    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {
        // 分岐させる
        Branch(laser, f_count, maxConvertCount);

        // 分岐させているか
        if (isBranch)
        { return; }

        // 分岐してるよ
        isBranch = true;
    }

    private void Branch(Laser laser, int f_count, int maxConvertCount)
    {
        // 今当たっているレーザーの発射元が同じなら
        if (laserDataList.IsSameLaserData(laser))
        {
            // あれば
            // 上書きするよ
            laserDataList.OverWritingLaserDataList(laser);
        }

        // レーザーのデータを追加して、今当たっているレーザーの数を返す
        count = laserDataList.AddLaserDataList(laser, intColorCode);

        // レーザーの色合成
        intColorCode = laserDataList.SetIntColor(count);

        // 方向を指定
        Vector3 direction = transform.right;

        // 方向変換した数を増やす
        f_count++;

        // レーザーを分岐させる
        branchDirection[0] = Quaternion.Euler(0, 0, rotationAngle) * direction;
        branchDirection[1] = Quaternion.Euler(0, 0, -rotationAngle) * direction;

        // レーザーのデータを取得
        branchLaserData[0] = new Laser(intColorCode, transform.position, branchDirection[0], gameObject);
        branchLaserData[1] = new Laser(intColorCode, transform.position, branchDirection[1], gameObject);

        // レーザーを射出して、当たったポジションを取得
        branchLaser_endPos[0] = rayShoters[0].ShotRay(branchLaserData[0], f_count, maxConvertCount, out GameObject hitObject1);
        branchLaser_endPos[1] = rayShoters[1].ShotRay(branchLaserData[1], f_count, maxConvertCount, out GameObject hitObject2);

        // 最後に当たったオブジェクトを取得
        hitObjects[0] = hitObject1;
        hitObjects[1] = hitObject2;
    }

    void InitRender()
    {
        for (int i = 0; i < branchLaserCount; ++i)
        {
            laserRendering.LaserSetActive(laserObjects[i], false);
            laserEffectRendering.LaserHitEffect_SetActive(false, laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetActive(false, laserShotEffect[i]);
        }
    }
}
