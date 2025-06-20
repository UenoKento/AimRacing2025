/**
 * @file    CenterOfMass.cs
 * @brief   Rigidbodyの重心位置(CenterOfMass)を更新する
 * @author  22CU0219 鈴木友也
 * @date    2024/08/07
 */
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_vehicleRigidbody;
    
    // AddComponent/Reset したときの設定
    void Reset()
    {
        m_vehicleRigidbody = GetComponentInParent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_vehicleRigidbody == null)
            Debug.LogError("Rigidbodyが設定されていません[CenterOfMass]");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_vehicleRigidbody.centerOfMass = gameObject.transform.localPosition;
    }

}
