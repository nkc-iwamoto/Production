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
        /// �I�u�W�F�N�g���擾
        /// </summary>
        /// <returns></returns>
        public GameObjectForPool Get()
        {
            var onObject = objectPool.Get();
            onObject.Init();
            // ���݃A�N�e�B�u�ɂȂ��Ă���I�u�W�F�N�g���L��
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
        /// �I�u�W�F�N�g�v�[���̒��g�����ׂĕԊ�
        /// </summary>
        public void AllRelease()
        {
            // �A�N�e�B�u�ɂȂ��Ă���I�u�W�F�N�g���A�N�e�B�u�ɂ���
            foreach (var obj in onObjectList.ToArray())
            {
                onObjectList.Remove(obj);
                objectPool.Release(obj);
            }
        }
}
