using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private float shakePeriod = 0.5f;
    [SerializeField] private float shakeAmplitude = 200f;
    [SerializeField] private int shakeTimes = 2;
    private float duration => shakePeriod * shakeTimes;

    [ContextMenu("Shake")]
    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        Vector2 originalPos = transform.position;
        float currTime = 0;
        while(currTime < duration)
        {
            yield return null;
            currTime += Time.deltaTime;
            float offset = shakeAmplitude * Mathf.Sin(currTime / shakePeriod * 2*Mathf.PI);
            transform.position = originalPos + new Vector2(offset, 0);
        }
        transform.position = originalPos;
    }
}
