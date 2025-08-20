using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    public static ScreenFade Instance;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;

        StartScreenFaidIn(1);
    }

    public void StartScreenFaidIn(float duration, Action onFadeFinished = null)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(ScreenFadeIn(duration, onFadeFinished));
    }

    public void StartScreenFaidOut(float duration, Action onFadeFinished = null)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(ScreenFadeOut(duration, onFadeFinished));
    }

    private IEnumerator ScreenFadeIn(float duration, Action onFadeFinished = null)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        float time = 0;

        while (_canvasGroup.alpha > 0.01f)
        {
            time += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }
        _canvasGroup.alpha = 0;

        onFadeFinished?.Invoke();
    }

    private IEnumerator ScreenFadeOut(float duration, Action onFadeFinished = null)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        float time = 0;

        while (_canvasGroup.alpha < 0.99f)
        {
            time += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }
        _canvasGroup.alpha = 1;

        onFadeFinished?.Invoke();
    }
}
