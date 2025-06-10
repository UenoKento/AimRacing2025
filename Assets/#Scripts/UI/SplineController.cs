/*
 * @file    SplineController.cs
 * @brief   スプラインからプレイヤーに逆走を知らせるUIを表示する処理
 * @date    2024/07/12  作成
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

    // 解像度
    // 内部的にPickResolutionMin～PickResolutionMaxの範囲に丸められる
    [SerializeField]
    [Range(SplineUtility.PickResolutionMin, SplineUtility.PickResolutionMax)]
    private int _resolution = 4;

    // 計算回数
    // 内部的に10回以下に丸められる
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

        // スプラインにおける直近位置を求める
        float _distance = SplineUtility.GetNearestPoint(_spline.Spline, _car.transform.position, out var nearestPoint, out var t, _resolution, _iterations);
        
        // 車とスプラインにおける直近位置の角度の差を求める
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
        //Vector3 direction =　_car.transform.position - nearestPoint;
        //Vector3 normalization_dir = direction / direction.magnitude;
        //Debug.DrawRay(_car.transform.position, normalization_dir * distance, Color.red, 5, false); // -> 正規化したベクトルで計算


        // 逆走しているかを判定し、UIを表示する
        if (signedAngle <= -_angleLimit || signedAngle >= _angleLimit)
        {
            _caveatUI.SetActive(true);
        }
        else
        {
            _caveatUI.SetActive(false);
        }

        // デバッグキー
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 結果を反映
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