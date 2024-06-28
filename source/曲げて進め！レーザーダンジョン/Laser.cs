using UnityEngine;

// レーザーの色判定
enum COLOR_CODE
{
    White,
    Red,
    Blue,
    Green,
    Purple,
    Yellow,
    Cyan,
}

public class Laser
{
    private  int colorNum;
    private  Vector3 startPos;
    private  Vector3 direction;
    private GameObject startGameObject;


    public Laser(int colorNum, Vector3 startPos, Vector3 direction,GameObject startGameObject)
    {
        this.colorNum = colorNum;
        this.startPos = startPos;
        this.direction = direction;
        this.startGameObject = startGameObject;
    }

    public int ColorNum { get { return colorNum; } set { colorNum = value; } }
    public Vector3 StartPos { get { return startPos; } set { startPos = value; } }
    public Vector3 Direction { get { return direction; } set { direction = value; } }
    public GameObject StartGameObject { get { return startGameObject; }set { startGameObject = value; } }
};
