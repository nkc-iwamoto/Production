using UnityEngine;

public class Room
{
    // �����̒��S���W
    public Vector3 Center { get; private set; }

    // �����̍�����W
    public Vector3 LeftTop { get; private set; }

    // �����̉E�����W
    public Vector3 RightBottom { get; private set; }

    // ���ʁi�ʐρj
    public float Mass { get; private set; }

    // �����̐��������̒���
    public float HorizantalLength { get; private set; }

    // �����̐��������̒���
    public float VerticalLength { get; private set; }

    // �������Ă��镔���̃I�u�W�F�N�g
    public GameObject roomObject { get; private set; }

    // ��������������ď���
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
    /// �������m���d�����Ă��邩
    /// </summary>
    /// <param name="room">�������</param>
    /// <returns>ture:�d�����Ă��� false:�d�����Ă��Ȃ�</returns>
    public bool IsDuplicate(Room room)
    {
        // �Q�̋�`�̋����𑪂�
        Vector3 absDistance = new Vector3
            (
                // ������3�ʖ������l�̌ܓ�
                x: (float)System.Math.Round(Mathf.Abs(this.Center.x - room.Center.x) * 1000.0f, System.MidpointRounding.AwayFromZero) * 0.001f,
                y: (float)System.Math.Round(Mathf.Abs(this.Center.y - room.Center.y) * 1000.0f, System.MidpointRounding.AwayFromZero) * 0.001f,
                0
            );

        // �e�����̏c���̕ӂ̒������擾
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

        // 2�̃T�C�Y�̘a���Z�o���Q�Ŋ���
        Vector3 roomSumSize = (roomSize + thisRoomSize) * 0.5f;

        // ��`���m�̒���(���S���W-���S���W)���e�����̊e�����̔����𑫂���������菬������Ώd�Ȃ��Ă���
        return (absDistance.x < roomSumSize.x && absDistance.y < roomSumSize.y);
    }

    /// <summary>
    /// ���g�ƑΏۂ�����������
    /// </summary>
    /// <param name="room">�Ώۂ̕���</param>
    /// <returns>ture:�����@false:�Ⴄ</returns>
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
    /// �d�Ȃ��Ă��钷�����擾
    /// </summary>
    /// <param name="room">�d�Ȃ��Ă��镔��</param>
    /// <returns>Vector3�^�̒���</returns>
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
    /// ���g����ɏd�Ȃ��Ă��镔�����ǂ���̕����ɂ��邩�擾
    /// </summary>
    /// <param name="room">���ׂ�������</param>
    /// <returns>Vector3�^�̕���</returns>
    public Vector3 Direction(Room room)
    {
        return room.Center - this.Center;
    }

    /// <summary>
    /// �������̍X�V
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
