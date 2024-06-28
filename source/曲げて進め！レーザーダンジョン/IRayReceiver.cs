using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRayReceiver
{ 
    void RayStay(Laser laser,int f_count,int maxConvertCount);
    void RayExit();
}