using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private FadeAnimation[] _fadeImages;
    [SerializeField]
    private bool _isAnimation = false;
    [SerializeField]
    private int _animationCount = 0;

    private bool _isSplashEnd = false;

    public bool IsSplashEnd => _isSplashEnd;

	private void Start()
    {
        _animationCount = 0;
    }

    private void Update()
    {
        if (_isAnimation == true)
        {
            if (_animationCount < _fadeImages.Length)
            {
                _fadeImages[_animationCount].StartAnimation = true;

                if (_fadeImages[_animationCount].GetEndAnimationFlag() == true)
                {
                    _animationCount++;
                }
            }
            else
            {
                _isSplashEnd = true;
                _isAnimation = false;
                _animationCount = 0;
            }
        }
    }
}