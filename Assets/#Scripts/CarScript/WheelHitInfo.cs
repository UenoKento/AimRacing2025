using UnityEngine;
using System.Collections;

struct WheelHitInfo
{
    public Vector3 Point;       // レイのヒット位置
    public Vector3 Normal;      // ヒットした座標の垂線
    public Vector3 ForwardDir;  // ホイールの前方ベクトル
    public Vector3 RightDir;    // ホイールの右方向のベクトル
    public Vector3 Force;       // The magnitude of the force being applied for the contact.

    public void Update(in RaycastHit _raycastHit)
    {
        Point  = _raycastHit.point;
        Normal = _raycastHit.normal;
    }
}

