using UnityEngine;

public class Switch : MonoBehaviour, IRayReceiver
{
    // ゴール出来るレーザーの色
    public enum COLOR
    {
        White,
        Red,
        Blue,
        Green,
        Purple,
        Yellow,
        Cyan,
    }

    [SerializeField, Header("スイッチの光る部位")]
    GameObject[] switchObj;

    [SerializeField, Header("スイッチのカウントアップのスクリプト")]
    SwitchCountUp switchCountUp;

    [SerializeField, Header("光の強さ")]
    int intensity;

    [SerializeField, Header("ゴールできるレーザーの色")]
    COLOR enumGoalColor;

    [SerializeField, Header("レーザーの色データ")]
    LaserColorData laserColorData;

    // 取得したレーザーの色情報
    int laserColorNum;

    // ゴールできる色情報
    int enumGoalColorNum;

    // 
    public int GetGoalColor { get { return (int)enumGoalColor; } }

    // スイッチの初期化(光っていない時)する色
    private Color initColor;

    //　スイッチが光っているか
    bool isOnLight;


    public bool getIsOnLight => isOnLight;

    private void Start()
    {
        // 色情報を元に初期化する色を取得
        initColor = laserColorData.colors[(int)enumGoalColor];
        // スイッチの初期化
        switchCountUp.Init(switchObj, initColor);
        // 光っていない
        isOnLight = false;
    }

    private void Update()
    {
        // 光っていないとき
        if (!isOnLight) { return; }

        // レーザーの色とゴールできる色が同じか
        if (laserColorNum != enumGoalColorNum)
        {
            // スイッチの初期化
            RayExit();
            return;
        }
    }

    // レーザーが当たらなくなったら
    public void RayExit()
    {
        // スイッチの初期化
        switchCountUp.Init(switchObj, initColor);
        // 光っていない
        isOnLight = false;
    }

    // レーザーが当たっているとき
    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {
        // レーザーの色情報を取得
        laserColorNum = laser.ColorNum;

        // ゴールできる色を取得
        enumGoalColorNum = (int)enumGoalColor;

        // レーザーの色とゴールできる色が同じか
        if (laserColorNum != enumGoalColorNum) { return; }

        // レーザーの色情報を元に色を取得
        Color color = laserColorData.colors[laser.ColorNum];

        // スイッチを光らす
        isOnLight = switchCountUp.IsOnSwitch(switchObj, color, intensity);
    }
}
