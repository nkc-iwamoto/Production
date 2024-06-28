using UnityEngine;

public class Room
{
    // 部屋の中心座標
    public Vector3 Center { get; private set; }

    // 部屋の左上座標
    public Vector3 LeftTop { get; private set; }

    // 部屋の右下座標
    public Vector3 RightBottom { get; private set; }

    // 質量（面積）
    public float Mass { get; private set; }

    // 部屋の水平方向の長さ
    public float HorizantalLength { get; private set; }

    // 部屋の垂直方向の長さ
    public float VerticalLength { get; private set; }

    // 同期している部屋のオブジェクト
    public GameObject roomObject { get; private set; }

    // 部屋が生成されて順番
    public int generateNumber { get; private set; }

    public Room(GameObject roomObject, int generateNumber)
    {
        this.roomObject = roomObject;
        this.generateNumber = generateNumber;
        this.Center = Vector3.zero;
        this.LeftTop = Vector3.zero;
        this.RightBottom = Vector3.zero;
        this.Mass = 0;
        this.HorizantalLength = 0;
        this.VerticalLength = 0;
        InformationUpdate();
    }

    public Room(Vector3 leftTop, Vector3 rightBottom)
    {
        this.LeftTop = leftTop;
        this.RightBottom = rightBottom;
        this.Center = new Vector3
            (
               x: leftTop.x + ((rightBottom.x - leftTop.x) * 0.5f),
               y: rightBottom.y + ((leftTop.y - rightBottom.y) * 0.5f),
               z: 0
            );
    }


    /// <summary>
    /// 部屋同士が重複しているか
    /// </summary>
    /// <param name="room">部屋情報</param>
    /// <returns>ture:重複している false:重複していない</returns>
    public bool IsDuplicate(Room room)
    {
        // ２つの矩形の距離を測る
        Vector3 absDistance = new Vector3
            (
                // 小数第3位未満を四捨五入
                x: (float)System.Math.Round(Mathf.Abs(this.Center.x - room.Center.x) * 1000.0f, System.MidpointRounding.AwayFromZero) * 0.001f,
                y: (float)System.Math.Round(Mathf.Abs(this.Center.y - room.Center.y) * 1000.0f, System.MidpointRounding.AwayFromZero) * 0.001f,
                0
            );

        // 各部屋の縦横の辺の長さを取得
        Vector3 roomSize = new Vector3
            (
                x: room.HorizantalLength,
                y: room.VerticalLength,
                0
            );
        Vector3 thisRoomSize = new Vector3
            (
                x: this.HorizantalLength,
                y: this.VerticalLength,
                0
            );

        // 2つのサイズの和を算出し２で割る
        Vector3 roomSumSize = (roomSize + thisRoomSize) * 0.5f;

        // 矩形同士の長さ(中心座標-中心座標)が各部屋の各長さの半分を足した長さより小さければ重なっている
        return (absDistance.x < roomSumSize.x && absDistance.y < roomSumSize.y);
    }

    /// <summary>
    /// 自身と対象が同じ部屋か
    /// </summary>
    /// <param name="room">対象の部屋</param>
    /// <returns>ture:同じ　false:違う</returns>
    public bool IsEqual(Room room)
    {
        return this.generateNumber == room.generateNumber;
    }

    public bool EnterPoint(Vector3 direction)
    {
        return (LeftTop.x <= direction.x && direction.x <= RightBottom.x)
            && (LeftTop.y >= direction.y && direction.y >= RightBottom.y);
    }



    /// <summary>
    /// 重なっている長さを取得
    /// </summary>
    /// <param name="room">重なっている部屋</param>
    /// <returns>Vector3型の長さ</returns>
    public Vector3 DuplicateLength(Room room)
    {
        Vector3 duplicateLength = Vector3.zero;
        Vector3 direction = Direction(room);

        if (direction.x > 0)
        {
            float tmp = Mathf.Abs(room.LeftTop.x - this.RightBottom.x);
            duplicateLength.x = tmp;

        }
        else if (direction.x < 0)
        {
            float tmp = Mathf.Abs(this.LeftTop.x - room.RightBottom.x);
            duplicateLength.x = tmp;
        }

        if (direction.y > 0)
        {
            float tmp = Mathf.Abs(this.LeftTop.y - room.RightBottom.y);
            duplicateLength.y = tmp;
        }
        else if (direction.y < 0)
        {
            float tmp = Mathf.Abs(room.LeftTop.y - this.RightBottom.y);
            duplicateLength.y = tmp;
        }

        return duplicateLength;
    }
    /// <summary>
    /// 自身を基準に重なっている部屋がどちらの方向にあるか取得
    /// </summary>
    /// <param name="room">調べたい部屋</param>
    /// <returns>Vector3型の方向</returns>
    public Vector3 Direction(Room room)
    {
        return room.Center - this.Center;
    }

    /// <summary>
    /// 部屋情報の更新
    /// </summary>
    public void InformationUpdate()
    {
        Center = roomObject.transform.position;

        LeftTop = new Vector3
            (
                -roomObject.transform.localScale.x * 0.5f + Center.x,
                roomObject.transform.localScale.y * 0.5f + Center.y,
                0
            );

        RightBottom = new Vector3
            (
                roomObject.transform.localScale.x * 0.5f + Center.x,
                -roomObject.transform.localScale.y * 0.5f + Center.y,
                0
            );

        Mass = roomObject.transform.localScale.x * roomObject.transform.localScale.y;
        HorizantalLength = roomObject.transform.localScale.x;
        VerticalLength = roomObject.transform.localScale.y;
    }
}
