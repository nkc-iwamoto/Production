using C__Graphics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrimMst
{
    List<Vector3> starts = new List<Vector3>();
    List<Vector3> ends = new List<Vector3>();

    public Dictionary<float,LineSegment> Mst(List<Vector3> pointArray, List<Triangle> triangleArray)
    {
        Dictionary<float, LineSegment> keyValuePairs = new Dictionary<float, LineSegment>();
        Dictionary<float , LineSegment> deletePairs = new Dictionary<float , LineSegment>();    

        // ˆê”Ô‰“‚¢À•W‚ðŽæ“¾
        Vector3 maxPoint = Vector3.zero;
        float maxPointSqrMagnitude = pointArray.Max(x => x.sqrMagnitude);
        foreach (var point in pointArray)
        {
            if (maxPointSqrMagnitude != point.sqrMagnitude) { continue; }
            maxPoint = point;
        }

        int count = 0;  
        while(count < 10)
        {
            count++;
            foreach (var triangle in triangleArray)
            {
                if (!IsEqualPoint(maxPoint, triangle)) { continue; }

                foreach (var item in triangle.GetVertex())
                {
                    Vector3 ts = new Vector3
                       (
                           x: Mathf.Abs(item.x - maxPoint.x),
                           y: Mathf.Abs(item.y - maxPoint.y),
                           z: Mathf.Abs(item.z - maxPoint.z)
                        );

                    float sqrMag = ts.sqrMagnitude;
                    if (sqrMag == 0) { continue; }
                    LineSegment lineSeg = new LineSegment(maxPoint, item);
                    if (keyValuePairs.ContainsKey(sqrMag)) { continue; }
                    if(deletePairs.ContainsKey(sqrMag)) { continue; }
                    keyValuePairs.Add(sqrMag, lineSeg);
                }
            }

            // ˆê”Ô‹——£‚ª’Z‚¢À•W‚ðŽæ“¾‚µ‚Äíœ
            float minKey = keyValuePairs.Keys.Min();
            LineSegment seg = keyValuePairs[minKey];



            keyValuePairs.Remove(minKey);
            deletePairs.Add(minKey, seg);

            maxPoint = seg.End;
            Debug.DrawLine(seg.Start, seg.End, Color.red);
        }
        return deletePairs;

    }

    bool IsEqualPoint(Vector3 checkPoint, Triangle triangle)
    {
        foreach (var item in triangle.GetVertex())
        {
            if (checkPoint.Equals(item))
            {
                return true;
            }
        }
        return false;
    }


}
