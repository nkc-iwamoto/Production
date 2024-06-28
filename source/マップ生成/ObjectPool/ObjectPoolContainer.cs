using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolContainer : MonoBehaviour
{
    [SerializeField]
    private List<CreateObjectPoolData> poolDataList = new List<CreateObjectPoolData>();
    private Dictionary<string, ObjPool> objectPoolDictionary = new Dictionary<string, ObjPool>();

    private void Awake()
    {   
        int count = 0;
        foreach (var poolData in poolDataList)
        {
            ++count;
            foreach (var pool in poolData.GetPools())
            {
                var setObejctPool = pool.CreatePool();
                var poolName = pool.PoolName;
                ObjPool objectPool = new ObjPool(setObejctPool);
                objectPoolDictionary.Add(poolName, objectPool);
            }
        }
    }

    private void OnApplicationQuit()
    {
        poolDataList.Clear();
    }
    /// <summary>
    /// ���O���������ăq�b�g���邩
    /// </summary>
    /// <param name="poolName">�����������v�[���̃I�u�W�F�N�g</param>
    /// <param name="objectPool">�q�b�g�����I�u�W�F�N�g�v�[��</param>
    /// <returns>�q�b�g:true</returns>
    public bool GetObjectPool(string poolName, out ObjPool objectPool)
    {
        objectPool = null;
        if (!objectPoolDictionary.ContainsKey(poolName)) { return false; }
        objectPool = objectPoolDictionary[poolName];
        return true;
    }
    /// <summary>
    /// �ێ����Ă���I�u�W�F�N�g�v�[���̒��g�������[�X����i�I�u�W�F�N�g�v�[���ɕԊҁj
    /// </summary>
    public void AllObjectPoolRelease()
    {
        foreach (var pool in objectPoolDictionary.Values)
        {
            pool.AllRelease();
        }
    }


    public GameObjectForPool GetGameOjbectForPool(string poolName)
    {
        foreach (var poolData in poolDataList)
        {
            foreach (var pool in poolData.GetPools())
            {
                if (pool.PoolName != poolName) { continue; }
                return pool.GameObjectForPool;
            }
        }
        throw new System.NullReferenceException("�Ȃ�����");
    }
    public ObjPool[] GetAllObjectPool()
    {
        return objectPoolDictionary.Values.ToArray();
    }
}
