using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LineSegment
{
    public Vector3 Start { get; private set; }
    public Vector3 End { get; private set; }

    public Vector3 Direction { get; private set; } 

    public LineSegment(Vector3 start, Vector3 end)
    {
        this.Start = start;
        this.End = end;
        Direction  = end- start;
    }
}
