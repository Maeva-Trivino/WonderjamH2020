using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    Vector3 intialPosition;

    private void Start()
    {
        intialPosition = transform.position;
    }

    public void ScreenShake(float amplitude, float period, float shakeDuration)
    {
        transform.position -= amplitude * Vector3.right;
        LTDescr shakeTween = LeanTween.moveX(gameObject, 2 * amplitude, period)
                            .setLoopPingPong();
        StartCoroutine(SlowDownShake(amplitude,shakeDuration,shakeTween));
    }

    private IEnumerator SlowDownShake(float amplitude, float shakeDuration, LTDescr tween)
    {
        float timer = 0f;
        float initialAmplitude = amplitude;
        while(timer < shakeDuration)
        {
            timer += Time.deltaTime;
            amplitude = initialAmplitude * (1-(timer/shakeDuration));
            yield return true;
        }
        LeanTween.cancel(tween.id);
        transform.position = intialPosition;
    }
}
