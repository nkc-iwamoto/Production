using Cysharp.Threading.Tasks;
using MapGenerater;
using RoomEdiorScriptable.Building.Placement.Scriptable;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Rion;
using System.Linq;
using Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private EnemyRegister enemyRegister;

    [SerializeField]
    private MainCharacter mainCharacter;

    [SerializeField]
    private CharacterDataBase characterDataBase;

    [SerializeField]
    private VirtualCameraSetting vcSetting;

    [SerializeField, Header("プレイヤーのレイヤー設定")]
    private int playerLayer;
    [SerializeField, Header("配置物のタグ設定")]
    private string gameObjectTag;

    [SerializeField]
    private ObjectPoolContainer objectPoolContainer;

    [SerializeField]
    private Map map;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private List<Spawner> spawners = new List<Spawner>();

    [SerializeField]
    private List<GameObject> onEnemies = new List<GameObject>();
    public List<GameObject> OnEnemies => onEnemies;

    [SerializeField, Header("上下左右")]
    private BoxCollider[] ruteCollider;

    private List<RuteGameObject> ruteWall = new List<RuteGameObject>();
    private List<RuteCollider> ruteColliderList = new List<RuteCollider>();

    private float[] centerX;
    private float[] centerZ;

    private bool[] bools;

    private RuteGameObject forwardRute = new RuteGameObject();
    private RuteGameObject backRute = new RuteGameObject();
    private RuteGameObject rightRute = new RuteGameObject();
    private RuteGameObject leftRute = new RuteGameObject();

    private Vector3 playerSpownPosition;
    [SerializeField]
    private Direction fromDirection = Direction.Init;

    private bool isClear = false;
    public bool IsClear => isClear;


    private Subject<Unit> roomChangeSubject = new Subject<Unit> ();


    private void Start()
    {
        map.CreateRoomInfo
            .Where(x => x != null)
            .Subscribe(x =>
            {
                isClear = false;
                roomChangeSubject.OnNext(default);
                CreateRoom(x.roomData, x.isClear);
                Spawn();
            })
            .AddTo(this);

        foreach (var r in ruteCollider)
        {
            if (!r.gameObject.TryGetComponent(out RuteCollider component)) { continue; }
            ruteColliderList.Add(component);
        }

        ruteColliderList[(int)Direction.Up].IsHit
              .Subscribe
              (
               x =>
               {
                   fromDirection = Direction.Up;
                   map.RoomChange(Direction.Up);
               }
               )
              .AddTo(this);
        ruteColliderList[(int)Direction.Down].IsHit
             .Subscribe
             (
              x =>
              {
                  fromDirection = Direction.Down;
                  map.RoomChange(Direction.Down);
              }
              )
             .AddTo(this);
        ruteColliderList[(int)Direction.Left].IsHit
             .Subscribe
             (
              x =>
              {
                  fromDirection = Direction.Left;
                  map.RoomChange(Direction.Left);
              }
              )
             .AddTo(this);
        ruteColliderList[(int)Direction.Right].IsHit
             .Subscribe
             (
              x =>
              {
                  fromDirection = Direction.Right;
                  map.RoomChange(Direction.Right);
              }
              )
             .AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomClear();
        }
    }

    public void RoomClear()
    {
        isClear = true;
        map.RoomClear();
        bools = map.GetCanStageChangeDirection();
        OnRute(bools);
    }

    public void CreateRoom(RoomData roomData, bool isClear)
    {

        Init(roomData);

        float arraySizeX = roomData.ArraySizeX() + 2;
        float arraySizeZ = roomData.ArraySizeZ() + 2;

        centerX = GetCenters(arraySizeX);
        centerZ = GetCenters(arraySizeZ);


        objectPoolContainer.AllObjectPoolRelease();
        Vector3 offset = new Vector3
            (
            x: (roomData.ArraySizeX() - 1) * 0.5f,
            y: 0,
            z: (roomData.ArraySizeZ() + 1) * 0.5f
            );

        for (int k = -1; k < roomData.ArraySizeZ() + 1; k++)
        {
            for (int i = -1; i < roomData.ArraySizeX() + 1; i++)
            {
                Vector3 position = new Vector3(i - offset.x, 0, -k + offset.z);
                // 外壁生成
                GenerateWall(i, k, roomData, position);
                // 部屋の中身生成
                GenerateRoomObject(i, k, roomData, position, isClear);
            }
        }

        PlayerSetting();
        SpawerSetting();
        vcSetting.SetCameraMoveControlRange(new Vector3(roomData.ArraySizeX(), 1, roomData.ArraySizeZ()));
        if (isClear) { RoomClear(); }
    }

    private void Spawn()
    {
        foreach (var spawner in spawners)
        {
            var enemy = spawner.Spawn();
            onEnemies.Add(enemy);
        }

        enemyRegister.Subscribe();
        enemyRegister.ClearFlag.Subscribe
            (
            x =>
            {
                RoomClear();
            }
            );
    }

    private float[] GetCenters(float value)
    {
        float harfValue = value / 2;
        if (value % 2 == 0)
        {
            return new float[] { harfValue - 1, harfValue, harfValue - 2, harfValue + 1 };
        }

        return new float[] { (int)harfValue + 1, (int)harfValue + 2, (int)harfValue };
    }

    private void OnRute(bool[] canChangeStageDirection)
    {
        if (canChangeStageDirection[(int)MapGenerater.Direction.Up])
        {
            foreach (var item in ruteWall[(int)MapGenerater.Direction.Up].Rute)
            {
                item.SetActive(false);
            }
        }
        if (canChangeStageDirection[(int)MapGenerater.Direction.Down])
        {
            foreach (var item in ruteWall[(int)MapGenerater.Direction.Down].Rute)
            {
                item.SetActive(false);
            }
        }
        if (canChangeStageDirection[(int)MapGenerater.Direction.Left])
        {
            foreach (var item in ruteWall[(int)MapGenerater.Direction.Left].Rute)
            {
                item.SetActive(false);
            }
        }
        if (canChangeStageDirection[(int)MapGenerater.Direction.Right])
        {
            foreach (var item in ruteWall[(int)MapGenerater.Direction.Right].Rute)
            {
                item.SetActive(false);
            }
        }

    }



    private void SetRuteBlock(int arrayX, int arrayZ, RoomData roomData, GameObject ruteObject)
    {
        foreach (var centerPosX in centerX)
        {
            if (arrayX == centerPosX - 1 && arrayZ == -1)
            {
                leftRute.Rute.Add(ruteObject);
            }
            else if (arrayX == centerPosX - 1 && arrayZ == roomData.ArraySizeZ())
            {
                rightRute.Rute.Add(ruteObject);
            }
        }
        foreach (var centerPosZ in centerZ)
        {
            if (arrayZ == centerPosZ - 1 && arrayX == -1)
            {
                forwardRute.Rute.Add(ruteObject);
            }
            else if (arrayZ == centerPosZ - 1 && arrayX == roomData.ArraySizeX())
            {
                backRute.Rute.Add(ruteObject);
            }
        }
        List<RuteGameObject> list = new List<RuteGameObject>
        {
              leftRute,
               rightRute,
              forwardRute,
             backRute,

        };
        ruteWall = list;
    }

    private void RePositioningRuteCollider(int arrayX, int arrayZ, RoomData roomData, Vector3 position)
    {
        if (arrayZ == -1)
        {
            ruteCollider[0].gameObject.transform.position = new Vector3(0, 0, position.z);
            ruteCollider[0].size = new Vector3(centerX.Count(), 1, 0.75f);
        }
        else if (arrayZ == roomData.ArraySizeZ())
        {
            ruteCollider[1].gameObject.transform.position = new Vector3(0, 0, position.z);
            ruteCollider[1].size = new Vector3(centerX.Count(), 1, 0.75f);
        }
        if (arrayX == -1)
        {
            ruteCollider[2].gameObject.transform.position = new Vector3(position.x, 0, 0);
            ruteCollider[2].size = new Vector3(0.75f, 1, centerX.Count());
        }
        else if (arrayX == roomData.ArraySizeX())
        {
            ruteCollider[3].gameObject.transform.position = new Vector3(position.x, 0, 0);
            ruteCollider[3].size = new Vector3(0.75f, 1, centerX.Count());
        }


    }

    private GameObjectForPool GenerateGameObject(string poolName, Vector3 position)
    {
        objectPoolContainer.GetObjectPool(poolName, out ObjPool objectPool);
        var obj = objectPool.Get();
        obj.transform.position = position;
        obj.transform.parent = this.transform;
        obj.gameObject.tag = gameObjectTag;
        return obj;
    }

    private void GenerateWall(int arrayX, int arrayZ, RoomData roomData, Vector3 position)
    {
        if ((arrayX == -1 || arrayX == roomData.ArraySizeX()) ||
            (arrayZ == -1 || arrayZ == roomData.ArraySizeZ()))
        {
            string name = "wall";
            var obj = GenerateGameObject(name, position);

            SetRuteBlock(arrayX, arrayZ, roomData, obj.gameObject);
            RePositioningRuteCollider(arrayX, arrayZ, roomData, position);
        }
    }
    private void GenerateRoomObject(int arrayX, int arrayZ, RoomData roomData, Vector3 position, bool isClear)
    {
        if (arrayX < 0 || arrayZ < 0) { return; }
        if (arrayX == roomData.ArraySizeX() || arrayZ == roomData.ArraySizeZ()) { return; }

        if (roomData.TestGetTexture2D(arrayZ * roomData.ArraySizeX() + arrayX) == null) { return; }

        string name = roomData.TestGetTexture2D(arrayZ * roomData.ArraySizeX() + arrayX).name;

        var obj = GenerateGameObject(name, position);

        if (isClear) { return; }
        if (obj.TryGetComponent(out Spawner spawner))
        {
            //  Debug.Log(spawner.name);
            spawners.Add(spawner);
        }
    }

    private void PlayerSetting()
    {
        // プレイヤーが憑依している敵の名前を取得
        string playerName = mainCharacter.GetPlayerName();
        // 名前から敵を取得
        if (!objectPoolContainer.GetObjectPool(playerName, out ObjPool playerPool)) { return; }
        var playerGameObject = playerPool.Get();

        // 敵のスクリプトを使用しないようにする
        playerGameObject.gameObject.layer = playerLayer;
        playerGameObject.gameObject.tag = "Player";
        mainCharacter.SetCharacter(playerGameObject.gameObject);
        mainCharacter.SetRigidbody(playerGameObject.GetComponent<Rigidbody>());
        if (playerGameObject.TryGetComponent(out EnemyManager eManager))
        {
            eManager.enabled = false;
        }
        if (playerGameObject.TryGetComponent<EnemyStatus>(out var status))
        {
            for (int i = 0; i < characterDataBase.CharaDataList.Count; i++)
            {
                if (characterDataBase.CharaDataList[i].CharaName == playerGameObject.name.Replace("(Clone)", ""))
                {
                    status.CharacterData = characterDataBase.CharaDataList[i];
                    break;
                }
            }
        }

        vcSetting.SetVirtualCameraFollow(playerGameObject.transform);
        playerGameObject.transform.position = playerSpownPosition;
    }

    private void SpawerSetting()
    {
        // 出現させる敵の種類の数を取得
        int typeNumberOrder = Random.Range(1, spawners.Count());
        // 種類の数を設定
        enemyRegister.SetPoolEnemys(typeNumberOrder);
        // スポナーにレジスターとプールコンテナーを渡す
        foreach (var spawner in spawners)
        {
            spawner.MainCharacter = mainCharacter;
            spawner.EnemyRegister = enemyRegister;
            spawner.Database = characterDataBase;
            spawner.ObjectPoolContainer = objectPoolContainer;
        }
    }

    private void Init(RoomData roomData)
    {
        virtualCamera.Follow = null;
        ruteWall.Clear();
        onEnemies.Clear();
        spawners.Clear();
        centerX = null;
        centerZ = null;
        bools = null;
        forwardRute = new RuteGameObject();
        backRute = new RuteGameObject();
        rightRute = new RuteGameObject();
        leftRute = new RuteGameObject();
        SetPlayerSpownPosition(fromDirection, roomData);
    }

    private void SetPlayerSpownPosition(Direction fromDirection, RoomData roomData)
    {
        playerSpownPosition = fromDirection switch
        {
            Direction.Up => new Vector3(0, 0, -roomData.ArraySizeZ() / 2 + 2),
            Direction.Down => new Vector3(0, 0, roomData.ArraySizeZ() / 2 - 1),
            Direction.Left => new Vector3(roomData.ArraySizeX() / 2 - 1, 0, 0),
            Direction.Right => new Vector3(-roomData.ArraySizeX() / 2 + 1, 0, 0),
            Direction.Init => new Vector3(0, 0, 0),
            _ => throw new System.NotImplementedException(),
        };
    }
    public System.IObservable<Unit> RoomChangeObservable()
    {
        return roomChangeSubject.AsObservable();
    }
}

public class RuteGameObject
{
    private List<GameObject> rute = new List<GameObject>();
    public List<GameObject> Rute => rute;
}