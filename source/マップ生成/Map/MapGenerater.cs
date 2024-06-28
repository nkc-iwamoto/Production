using UnityEngine;

namespace MapGenerater
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Init = -999,
    }

    public class DirectionBools
    {
        public readonly bool[] directionBools = new bool[4];


        public DirectionBools()
        {
            for (int i = 0; i < 4; ++i)
            {
                int random = Random.Range(0, 2);
                if (random == 0) directionBools[i] = true;
                else directionBools[i] = false;
            }
        }
        public DirectionBools(bool[] directionBools)
        {
            this.directionBools = directionBools;
        }
        public DirectionBools(bool[] directionBools, Direction fromDirection)
        {
            int tmp = fromDirection switch
            {
                Direction.Up => (int)Direction.Down,
                Direction.Down => (int)Direction.Up,
                Direction.Left => (int)Direction.Right,
                Direction.Right => (int)Direction.Left,
                Direction.Init => -999,
                _ => throw new System.NotImplementedException(),
            };

            this.directionBools = directionBools;
            if (tmp < 0) { return; }
            this.directionBools[tmp] = true;
        }
    }

    public class RoomInfo
    {
        public readonly int pos_x;
        public readonly int pos_y;
        public readonly DirectionBools bools;
        public readonly int roomData;
        public readonly bool isClear;
        public bool isGenerated;
        public int generateNumber;


        public bool IsPlayerEnter { get; private set; }

        public RoomInfo(int pos_x, int pos_y, int roomData, bool isPlayerEnter, bool isClear = false, bool isGenerated = false, DirectionBools bools = null)
        {
            this.pos_x = pos_x;
            this.pos_y = pos_y;
            this.roomData = roomData;
            this.IsPlayerEnter = isPlayerEnter;
            this.isGenerated = isGenerated;
            this.bools = bools;
            this.isClear = isClear;
            generateNumber = -999;

        }
    }

    public class MapGenerater
    {
        public readonly int maxRoomCount;
        public readonly int minRoomCount;

        public RoomInfo[,] MapData { get; private set; }

        public readonly int size;

        private int generatedCount;
        private int roomDataMaxCount;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="max">�����̍ő吶����</param>
        /// <param name="min">�����̍ŏ�������</param>
        /// <exception cref="System.Exception">�ŏ��l���ő�l���傫��</exception>
        public MapGenerater(int max, int min, int roomDataMaxCount)
        {
            if (min > max) { throw new System.Exception("�ŏ��l���ő�l���傫���ł��B"); }
            maxRoomCount = max;
            minRoomCount = min;
            size = max * 2;
            this.roomDataMaxCount = roomDataMaxCount;
            Init();
        }

        /// <summary>
        /// ������
        /// </summary>
        private void Init()
        {
            generatedCount = 0;
            MapData = new RoomInfo[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int roomData = Random.Range(0, roomDataMaxCount);
                    MapData[i, j] = new RoomInfo(i, j, roomData, false);
                }
            }
        }

        /// <summary>
        /// �}�b�v�̔z�񐶐�
        /// </summary>
        public void BuildStart()
        {
            int pos_x = size / 2;
            int pos_y = size / 2;
            while (generatedCount <= minRoomCount)
            {
                Init();
                Build(pos_x, pos_y, 0);
            }
            int roomData = Random.Range(0, roomDataMaxCount);
            InitPlayerSet(pos_x, pos_y, roomData);
        }

        /// <summary>
        /// �w�肵���}�b�v�̍��W�Ƀv���C���[����������Z�b�g
        /// </summary>
        /// <param name="x">����</param>
        /// <param name="y">�c��</param>
        public void InitPlayerSet(int x, int y, int roomData)
        {
            MapData[x, y] = new RoomInfo(x, y, roomData, true, false, true, MapData[x, y].bools);
        }

        /// <summary>
        /// �X�e�[�W���ړ�����Ƃ��v���C���[����������ړ������ɕύX
        /// </summary>
        /// <param name="stageChangeDirection">�ړ������̕���</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void PlayerInStageChange(Direction stageChangeDirection)
        {
            // �v���C���[�̏�񂪂��镔���̍��W���擾
            Vector2Int playerInStagePos = PlayerInStagePos();

            // �ړ���������̕����̍��W���擾
            Vector2Int next_PlayerInStagePos = stageChangeDirection switch
            {
                Direction.Up => new Vector2Int(playerInStagePos.x, playerInStagePos.y - 1),
                Direction.Down => new Vector2Int(playerInStagePos.x, playerInStagePos.y + 1),
                Direction.Left => new Vector2Int(playerInStagePos.x - 1, playerInStagePos.y),
                Direction.Right => new Vector2Int(playerInStagePos.x + 1, playerInStagePos.y),
                _ => throw new System.NotImplementedException(),
            };

            // �v���C���[�̏��̈ړ�
            int keepCount = MapData[playerInStagePos.x, playerInStagePos.y].generateNumber;
            keepCount = keepCount < 0 ? 0 : keepCount;

            MapData[playerInStagePos.x, playerInStagePos.y] = new RoomInfo
                                                                (
                                                                playerInStagePos.x,
                                                                playerInStagePos.y,
                                                                MapData[playerInStagePos.x, playerInStagePos.y].roomData,
                                                                false,
                                                                 MapData[playerInStagePos.x, playerInStagePos.y].isClear,
                                                                true,
                                                                MapData[playerInStagePos.x, playerInStagePos.y].bools
                                                                );
            MapData[playerInStagePos.x, playerInStagePos.y].generateNumber = keepCount;

            keepCount = MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y].generateNumber;

            MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y] = new RoomInfo
                                                                          (
                                                                          next_PlayerInStagePos.x,
                                                                          next_PlayerInStagePos.y,
                                                                          MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y].roomData,
                                                                          true,
                                                                          MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y].isClear,
                                                                          true,
                                                                          MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y].bools
                                                                          );

            MapData[next_PlayerInStagePos.x, next_PlayerInStagePos.y].generateNumber = keepCount;
        }

        /// <summary>
        /// �v���C���[��������������Ă��镔�����}�b�v�̂ǂ��ɂ��邩
        /// </summary>
        /// <returns>�v���C���[�̏��������������̃}�b�v���W</returns>
        public Vector2Int PlayerInStagePos()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (!MapData[i, k].IsPlayerEnter) { continue; }
                    return new Vector2Int(i, k);
                }
            }
            return Vector2Int.zero;
        }

        /// <summary>
        /// �v���C���[�����镔���̏��
        /// </summary>
        /// <returns></returns>
        public int GetRoomData_EnterPlayer()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (!MapData[i, k].IsPlayerEnter) { continue; }
                    return MapData[i, k].roomData;
                }
            }
            return -999;
        }

        public bool IsRoomClear_EnterPlayer()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (!MapData[i, k].IsPlayerEnter) { continue; }
                    return MapData[i, k].isClear;
                }
            }
            return false;
        }

        /// <summary>
        /// �w�肵���}�b�v���W�̕������ړ��ł������
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public DirectionBools CanStageChangeDirection(Vector2Int pos)
        {
            return MapData[pos.x, pos.y].bools;
        }

        /// <summary>
        /// �v���C���[�����镔�����ړ��ł������
        /// </summary>
        /// <returns>����</returns>
        /// <exception cref="System.NullReferenceException">�v���C���[���ǂ̕����ɂ����Ȃ�����</exception>
        public DirectionBools CanStageChageDirection_EnterPlayer()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (!MapData[i, k].IsPlayerEnter) { continue; }
                    return MapData[i, k].bools;
                }
            }
            throw new System.NullReferenceException("�v���C���[�̏�񂪂Ȃ�����");
        }

        public void RoomClear()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (!MapData[i, k].IsPlayerEnter) { continue; }
                    var roomInfo = MapData[i, k];
                    MapData[i, k] = new RoomInfo
                        (
                        roomInfo.pos_x,
                        roomInfo.pos_y,
                        roomInfo.roomData,
                        roomInfo.IsPlayerEnter,
                        true, roomInfo.isGenerated, 
                        roomInfo.bools
                        );
                }
            }
        }

        public RoomInfo[,] GetMapData()
        {
            return MapData;
        }

        /// <summary>
        /// �}�b�v�̐����i�ċA�����j
        /// </summary>
        /// <param name="x">����</param>
        /// <param name="y">�c��</param>
        /// <param name="count">�A�����Đ������Ă��鐔</param>
        /// <param name="fromDirection">�O��ǂ���̕������痈����</param>
        /// <returns>�����ł�����</returns>
        private bool Build(int x, int y, int count, Direction fromDirection = Direction.Init)
        {
            if (count > 6) { return false; }
            if (MapData[x, y].isGenerated) { return false; }
            if (generatedCount >= maxRoomCount) { return false; }
            if (x < 0 || x >= size || y < 0 || y >= size) { return false; }

            // ���ňړ��ł�������𐶐�
            DirectionBools bools = MapData[x, y].bools;

            while (IsGenerateRoom(bools, count))
            {
                bools = new DirectionBools();
            }

            // �����𐶐���������n��
            MapData[x, y].isGenerated = true;
            // �����̘A��
            MapData[x, y].generateNumber = count;
            // �ꎞ�ۑ�
            int keepGenerateNumber = MapData[x, y].generateNumber;

            count++;
            generatedCount++;

            // ���ʂ̈ړ��ł������
            bool[] resultBools = new bool[4];

            if (bools.directionBools[(int)Direction.Up])
            {
                resultBools[(int)Direction.Up] = Build(x, y - 1, count, Direction.Up);
            }
            if (bools.directionBools[(int)Direction.Down])
            {
                resultBools[(int)Direction.Down] = Build(x, y + 1, count, Direction.Down);
            }
            if (bools.directionBools[(int)Direction.Right])
            {
                resultBools[(int)Direction.Right] = Build(x + 1, y, count, Direction.Right);
            }
            if (bools.directionBools[(int)Direction.Left])
            {
                resultBools[(int)Direction.Left] = Build(x - 1, y, count, Direction.Left);
            }

            // �ŏI�I�ȕ����̏��𐶐�
            MapData[x, y] = new RoomInfo(x, y, MapData[x, y].roomData, false, false, true, new DirectionBools(resultBools, fromDirection));
            MapData[x, y].generateNumber = keepGenerateNumber;

            return true;
        }



        /// <summary>
        /// �z���\�����邽�߂̕�����
        /// </summary>
        /// <returns>�z��̕�����</returns>
        public string DebugString()
        {
            string debugStr = "";
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (MapData[k, i].IsPlayerEnter) { debugStr += "P"; }
                    else if (MapData[k, i].isGenerated) { debugStr += MapData[k, i].generateNumber; }
                    else { debugStr += "�~"; }
                }
                debugStr += '\n';
            }
            return debugStr;
        }

        /// <summary>
        /// �ړ��ł�����������邩
        /// </summary>
        /// <param name="bools">�ړ��ł������</param>
        /// <param name="count">���A���Ő������Ă��鐔</param>
        /// <returns>�������邩</returns>
        private bool IsGenerateRoom(DirectionBools bools, int count)
        {
            int falseCount = 0;
            if (bools == null) { return true; }
            for (int i = 0; i < bools.directionBools.Length; i++)
            {
                if (count > 1) { return false; }
                if (bools.directionBools[i]) { continue; }
                falseCount++;
            }

            if (falseCount == bools.directionBools.Length) { return true; }
            return false;
        }
    }
}