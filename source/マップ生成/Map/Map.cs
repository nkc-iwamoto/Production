using MapGenerater;
using Rion;
using RoomEdiorScriptable.Building.Placement.Scriptable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    [SerializeField, Header("敵の情報を管理しているスクリプト")]
    private EnemyRegister enemyRegister;

    // マップ生成
    private MapGenerater.MapGenerater mapGenerate;

    [SerializeField, Header("部屋の配置の情報")]
    private RoomData[] roomDatas;

    [SerializeField, Header("生成する部屋の最低数")]
    private int generateMin;

    [SerializeField, Header("生成する部屋の最大数")]
    private int generateMax;

    private readonly ReactiveProperty<CreateRoomInfo> createRoomInfo = new ReactiveProperty<CreateRoomInfo>();
    public IReadOnlyReactiveProperty<CreateRoomInfo> CreateRoomInfo => createRoomInfo;


    [SerializeField]
    private List<Spawner> spawners = new List<Spawner>();

    [SerializeField]
    private ObjectPoolContainer objectPoolContainer;

    [SerializeField]
    private bool[] canStageChangeDirectionBools = new bool[4];

    [SerializeField]
    private Text text;

    private void Start()
    {
        // マップ生成
        mapGenerate = new MapGenerater.MapGenerater(generateMax, generateMin, roomDatas.Count());
        mapGenerate.BuildStart();

        createRoomInfo.AddTo(this);

        SumSpawer();
        //　全部屋のスポナーの数を渡す
        enemyRegister.Initialize(spawners.Count());

        // 部屋の種類を取得
        int roomDataNumber = mapGenerate.GetRoomData_EnterPlayer();
        bool isClear = mapGenerate.IsRoomClear_EnterPlayer();
        createRoomInfo.Value = new CreateRoomInfo(roomDatas[roomDataNumber], isClear);
    }

    private void Update()
    {
        string str = mapGenerate.DebugString();
        text.text = str;
    }

    public void RoomChange(Direction direction)
    {
        mapGenerate.PlayerInStageChange(direction);

        // 部屋の種類を取得
        int roomDataNumber = mapGenerate.GetRoomData_EnterPlayer();
        bool isClear = mapGenerate.IsRoomClear_EnterPlayer();
        createRoomInfo.Value = new CreateRoomInfo(roomDatas[roomDataNumber], isClear);
        createRoomInfo.Value = null;
    }

    public bool[] GetCanStageChangeDirection()
    {
        return mapGenerate.CanStageChageDirection_EnterPlayer().directionBools;
    }

    public void RoomClear()
    {
        mapGenerate.RoomClear();
    }

    private void SumSpawer()
    {
        var mapData = mapGenerate.GetMapData();
        for (int i = 0; i < mapGenerate.size; i++)
        {
            for (int k = 0; k < mapGenerate.size; k++)
            {
                if (!mapData[i, k].isGenerated) { continue; }
                int roomDataNumber = mapData[i, k].roomData;
                var roomData = roomDatas[roomDataNumber];
                CreateRoomObject(roomData);
            }
        }
    }
    private void CreateRoomObject(RoomData roomData)
    {
        for (int k = 0; k < roomData.ArraySizeZ(); k++)
        {
            for (int i = 0; i < roomData.ArraySizeX(); i++)
            {
                // 部屋の配置情報をひとつづつ取得
                var texture2D = roomData.TestGetTexture2D(k * roomData.ArraySizeX() + i);
                if (texture2D == null) { continue; }

                string texture2DName = texture2D.name;
                // 同じ名前のプールされているオブジェクトを取得
                var gameObjectForPool = objectPoolContainer.GetGameOjbectForPool(texture2DName);
                // そのオブジェクトがスポナーだったら追加
                if (!gameObjectForPool.gameObject.TryGetComponent(out Spawner spawner)) { continue; }
                spawners.Add(spawner);
            }
        }
    }
}
public class CreateRoomInfo
{
    public readonly RoomData roomData;
    public readonly bool isClear;

    public CreateRoomInfo(RoomData roomData, bool isClear)
    {
        this.roomData = roomData;
        this.isClear = isClear;
    }
}
