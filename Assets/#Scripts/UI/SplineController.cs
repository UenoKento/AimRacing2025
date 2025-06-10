/*
 * @file    SplineController.cs
 * @brief   �X�v���C������v���C���[�ɋt����m�点��UI��\�����鏈��
 * @date    2024/07/12  �쐬
 */

using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _spline = null;

    [SerializeField]
    private GameObject _car = null;

    [SerializeField]
    private GameObject _caveatUI = null;

    [SerializeField]
    private float _angleLimit = 90.0f;

    [SerializeField]
    private bool _flip;

    // �𑜓x
    // �����I��PickResolutionMin�`PickResolutionMax�͈̔͂Ɋۂ߂���
    [SerializeField]
    [Range(SplineUtility.PickResolutionMin, SplineUtility.PickResolutionMax)]
    private int _resolution = 4;

    // �v�Z��
    // �����I��10��ȉ��Ɋۂ߂���
    [SerializeField]
    [Range(1, 10)]
    private int _iterations = 2;

    private void Start()
    {
        //_spline = GetComponent<SplineContainer>();
    }

    private void FixedUpdate()
    {
        if (_spline == null || _car == null)
            return;

        // �X�v���C���ɂ����钼�߈ʒu�����߂�
        float _distance = SplineUtility.GetNearestPoint(_spline.Spline, _car.transform.position, out var nearestPoint, out var t, _resolution, _iterations);
        
        // �ԂƃX�v���C���ɂ����钼�߈ʒu�̊p�x�̍������߂�
        float signedAngle;
        if(!_flip)
            signedAngle = Vector3.SignedAngle(_car.transform.forward, _spline.Spline.EvaluateTangent(t), Vector3.up);
        else
            signedAngle = Vector3.SignedAngle(-_car.transform.forward, _spline.Spline.EvaluateTangent(t), Vector3.up);


        // Debug.Log
        // ("Angle::" + signedAngle + ":: Near::" + nearestPoint);

        // Ray
        //float distance = Vector3.Distance(_car.transform.position, nearestPoint);
        //Vector3.forwa
        //Vector3 direction =�@_car.transform.position - nearestPoint;
        //Vector3 normalization_dir = direction / direction.magnitude;
        //Debug.DrawRay(_car.transform.position, normalization_dir * distance, Color.red, 5, false); // -> ���K�������x�N�g���Ōv�Z


        // �t�����Ă��邩�𔻒肵�AUI��\������
        if (signedAngle <= -_angleLimit || signedAngle >= _angleLimit)
        {
            _caveatUI.SetActive(true);
        }
        else
        {
            _caveatUI.SetActive(false);
        }

        // �f�o�b�O�L�[
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���ʂ𔽉f
            if (!_flip)
                _car.transform.rotation = Quaternion.LookRotation(_spline.Spline.EvaluateTangent(t));
            else
                _car.transform.rotation = Quaternion.LookRotation(-_spline.Spline.EvaluateTangent(t));
            _car.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            nearestPoint.y += 0.3f;
            _car.transform.position = nearestPoint;
        }
    }
}