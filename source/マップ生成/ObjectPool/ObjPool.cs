using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;


public class ObjPool 
{
        private readonly ObjectPool<GameObjectForPool> objectPool;
        private List<GameObjectForPool> onObjectList = new List<GameObjectForPool>();

        public ObjPool(ObjectPool<GameObjectForPool> objectPool)
        {
            this.objectPool = objectPool;
        }

        /// <summary>
        /// オブジェクトを取得
        /// </summary>
        /// <returns></returns>
        public GameObjectForPool Get()
        {
            var onObject = objectPool.Get();
            onObject.Init();
            // 現在アクティブになっているオブジェクトを記憶
            onObjectList.Add(onObject);
            return onObject;
        }

        public void Release(GameObjectForPool releaseObject)
        {
            onObjectList.Remove(releaseObject);
            objectPool.Release(releaseObject);
        }

        public List<GameObjectForPool> GetOnObjectList()
        {
            return onObjectList;
        }


        /// <summary>
        /// オブジェクトプールの中身をすべて返還
        /// </summary>
        public void AllRelease()
        {
            // アクティブになっているオブジェクトを非アクティブにする
            foreach (var obj in onObjectList.ToArray())
            {
                onObjectList.Remove(obj);
                objectPool.Release(obj);
            }
        }
}
