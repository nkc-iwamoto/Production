using System.Collections.Generic;
using UnityEngine;
public class RoomSeparation
{

    public void Separation(List<Room> roomsList)
    {
        while (GetDuplicateCount(roomsList) > 0)
        {

            foreach (Room room1 in roomsList)
            {
                foreach (Room room2 in roomsList)
                {
                    if (room1.IsEqual(room2)) { continue; }
                    if (!room1.IsDuplicate(room2)) { continue; }

                    // 各部屋情報が持っている部屋オブジェクトの座標を取得
                    Vector3 room1Pos = room1.roomObject.transform.position;
                    Vector3 room2Pos = room2.roomObject.transform.position;

                    // 垂直方向、水平方向 room1を基準に重なっている長さの割合を求める

                    // 重なっている長さを取得
                    Vector3 length = room1.DuplicateLength(room2);

                    // 垂直方向、水平方向の割合を求める
                    float horizontalLengthRaito = length.x / room1.HorizantalLength;
                    float verticalLengthRaito = length.y / room1.VerticalLength;

                    // 割合が低いほうに移動（割合 != 0 の場合）
                    if (horizontalLengthRaito > verticalLengthRaito && verticalLengthRaito != 0)
                    {
                        RoomMove(DIRECTION.Vertical, room1, room2, ref room1Pos, ref room2Pos);
                    }
                    else if (horizontalLengthRaito < verticalLengthRaito && horizontalLengthRaito != 0)
                    {
                        RoomMove(DIRECTION.Horizontal, room1, room2, ref room1Pos, ref room2Pos);
                    }
                    else if (horizontalLengthRaito == 0 && verticalLengthRaito > 0)
                    {
                        RoomMove(DIRECTION.Vertical, room1, room2, ref room1Pos, ref room2Pos);
                    }
                    else if (verticalLengthRaito == 0 && horizontalLengthRaito > 0)
                    {
                        RoomMove(DIRECTION.Horizontal, room1, room2, ref room1Pos, ref room2Pos);
                    }
                    else if (horizontalLengthRaito == 0 && verticalLengthRaito == 0)
                    {
                        int rand = Random.Range(0, 2);
                        DIRECTION dr = rand == 0 ? DIRECTION.Horizontal : DIRECTION.Vertical;
                        RoomMove(dr, room1, room2, ref room1Pos, ref room2Pos);
                    }
                    else
                    {
                        Debug.LogError("dekitenaaaaaaaai");
                    }

                    // 部屋の移動
                    room2.roomObject.transform.position = room2Pos;
                    room1.roomObject.transform.position = room1Pos;

                    // 移動後の情報に更新
                    room1.InformationUpdate();
                    room2.InformationUpdate();
                }
            }
        }
    }


    private int GetDuplicateCount(List<Room> roomsList)
    {
        int duplicateCount = 0;
        for (int i = 0; i < roomsList.Count; i++)
        {
            Room room1 = roomsList[i];
            for (int j = 0; j < roomsList.Count; j++)
            {
                Room room2 = roomsList[j];

                if (room1.generateNumber == room2.generateNumber) { continue; }
                if (!room1.IsDuplicate(room2)) { continue; }
                ++duplicateCount;
            }
        }
        return duplicateCount;
    }

    void RoomMove(DIRECTION dr, Room room1, Room room2, ref Vector3 room1Pos, ref Vector3 room2Pos)
    {
        Vector3 pos1 = room1Pos;
        Vector3 pos2 = room2Pos;
        float movePos1 = 0;
        float movePos2 = 0;
        float direction = 0;
        float length = 0;

        // 水平方向に部屋の移動するための情報を取得
        if (dr == DIRECTION.Horizontal)
        {
            movePos1 = room1Pos.x;
            movePos2 = room2Pos.x;
            direction = room1.Direction(room2).x;
            length = room1.DuplicateLength(room2).x;
            length = length == 0 ? room1.HorizantalLength : length;
        }
        // 垂直方向に部屋の移動するための情報を取得
        else if (dr == DIRECTION.Vertical)
        {
            movePos1 = room1Pos.y;
            movePos2 = room2Pos.y;
            direction = room1.Direction(room2).y;
            length = room1.DuplicateLength(room2).y;
            length = length == 0 ? room1.VerticalLength : length;
        }

        // 質量で移動させる割合を変える
        // 移動させる長さの分割数を取得(部屋の面積を質量とする）
        float raitoNumber = room1.Mass + room2.Mass;
        float raitoedLength = length / raitoNumber;

        float room1MoveLength = room1.Mass * raitoedLength;
        float room2MoveLenght = room2.Mass * raitoedLength;

        if (direction == 0)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                movePos1 -= room1MoveLength;
                movePos2 += room2MoveLenght;
            }
            else if (rand == 1)
            {
                movePos1 += room1MoveLength;
                movePos2 -= room2MoveLenght;
            }
        }
        else if (direction > 0)
        {
            movePos1 -= room1MoveLength;
            movePos2 += room2MoveLenght;
        }
        else if (direction < 0)
        {
            movePos1 += room1MoveLength;
            movePos2 -= room2MoveLenght;
        }

        if (dr == DIRECTION.Horizontal)
        {
            pos1.x = movePos1;
            pos2.x = movePos2;
        }
        else if (dr == DIRECTION.Vertical)
        {
            pos1.y = movePos1;
            pos2.y = movePos2;
        }

        room1Pos = pos1;
        room2Pos = pos2;
    }

}
