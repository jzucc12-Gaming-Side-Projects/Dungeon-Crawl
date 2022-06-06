using System;
using System.Collections;
using UnityEngine;

namespace JZ.DISPLAY
{
    /// <summary>
    /// <para>Generates camera shaking</para>
    /// <para>Shaking can be called from any script</para>
    /// </summary>
    public class ScreenShake : MonoBehaviour
    {
        private static event Action<float, float> onShake = null;


        #region //Monobheaviour
        private void OnEnable() 
        {
            onShake += Shake;
        }

        private void OnDisable()
        {
            onShake -= Shake;
        }
        #endregion

        #region //Shaking
        public static void CallShake(float duration, float magnitude)
        {
            onShake?.Invoke(duration, magnitude);
        }

        [ContextMenu("Shake")]
        public void Shake(float duration, float magnitude)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeRoutine(duration, magnitude));
        }

        private IEnumerator ShakeRoutine(float duration, float magnitude)
        {
            float currentTimer = 0;
            Vector3 originalPosition = transform.localPosition;

            while(currentTimer < duration)
            {
                yield return null;
                currentTimer += Time.deltaTime;
                float xShake = UnityEngine.Random.Range(-magnitude, magnitude);
                float yShake = UnityEngine.Random.Range(-magnitude, magnitude);
                transform.localPosition = originalPosition + new Vector3(xShake, yShake, 0);
            }

            transform.localPosition = originalPosition;
        }
        #endregion
    }
}
