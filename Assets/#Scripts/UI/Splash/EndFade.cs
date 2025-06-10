using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndFade : MonoBehaviour
{
    [SerializeField]
    private Image _image = null;

    [System.Serializable]
    public struct FadeTime
    {
        public float In;
        public float Full;
        public float Out;
    }
    [SerializeField] private FadeTime _fadeTime;

    [SerializeField]
    private bool _isStartAnimaton = false;
    private bool _isEndAnimation = false;

    public bool IsStartAnimation
    {
        set => _isStartAnimaton = value;
    }

    public bool IsEndAnimation => _isEndAnimation;

    private void Awake()
    {
        _image.enabled = false;
    }

    private void Start()
    {
        if (_isStartAnimaton == true)
        {
            StartCoroutine(FadeOut());
        }
    }

    private void Update()
    {
        if (_isStartAnimaton == true)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void StartAnima()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        IEnumerator enumerator = FadeIn();
        yield return enumerator;

        _isEndAnimation = true;
        yield return null;
    }

    private IEnumerator FadeIn()
    {
        _image.enabled = true;
        float elapsedTime = 0.0f;
        Color startColor = _image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < _fadeTime.In)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _fadeTime.In);
            _image.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        _image.color = endColor;

        _isEndAnimation = true;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        Color startColor = _image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < _fadeTime.Out)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _fadeTime.Out);
            _image.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        _image.color = endColor;

        _isEndAnimation = true;
    }
}
