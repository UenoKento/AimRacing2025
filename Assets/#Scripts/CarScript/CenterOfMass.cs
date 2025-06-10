/**
 * @file    CenterOfMass.cs
 * @brief   Rigidbody�̏d�S�ʒu(CenterOfMass)���X�V����
 * @author  22CU0219 ��ؗF��
 * @date    2024/08/07
 */
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_vehicleRigidbody;
    
    // AddComponent/Reset �����Ƃ��̐ݒ�
    void Reset()
    {
        m_vehicleRigidbody = GetComponentInParent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_vehicleRigidbody == null)
            Debug.LogError("Rigidbody���ݒ肳��Ă��܂���[CenterOfMass]");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_vehicleRigidbody.centerOfMass = gameObject.transform.localPosition;
    }

}
