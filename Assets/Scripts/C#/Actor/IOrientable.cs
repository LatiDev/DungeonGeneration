using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrientable
{
    void OrientTo(OrientationInfos v); //Global Vector as Vector3.left or Vector3.rigth ect...
}
