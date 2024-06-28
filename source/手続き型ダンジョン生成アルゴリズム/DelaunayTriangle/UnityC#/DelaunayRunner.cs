using C__Graphics;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayRunner : MonoBehaviour
{
    private float width = 1000.0f;
    private float hegith = 1000.0f;
    Triangle[] triangles = null;

    private void Start()
    {
       SetUp();
    }

    private void SetUp()
    {

        // ランダムに頂点のリストを作成
        List<Vector3> pointList = new List<Vector3>();
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(0.0f, width);
            float y = Random.Range(0.0f, hegith);
            Vector3 point = new Vector3(x, y, 0);
            pointList.Add(point);
        }



        //delaunay = new DelaunayTriangles();
        //triangles = DelaunayTriangles.Generate(pointList);

        foreach (var item in triangles)
        {
            for (int i = 0; i < item.GetVertex().Length; ++i)
            {
                int next = i + 1;
                //Debug.Log(i);
                //Debug.Log(next);
                if (next >= item.GetVertex().Length) { next = 0; }

                Vector2 start = item.GetVertex()[i];
                Vector2 end = item.GetVertex()[next];
                Debug.DrawLine(start, end, Color.red);
            }
        }
    }



    private void Update()
    {
       
    }


}
