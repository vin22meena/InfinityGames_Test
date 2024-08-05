using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// CameraShake.cs Class, Handles Camera Shake Effect
/// </summary>

namespace LoopEnergyClone
{
    public class CameraShake : GenericSingleton<CameraShake>
    {
        [SerializeField] float _shakeDuration = 0.5f;  //How long shake should work
        [SerializeField] AnimationCurve _shakeAnimationCurve;  //Specific camera shake animation

        Coroutine m_cameraShakeCoroutine = null;  //Camera Shake Coroutine

        /// <summary>
        /// Camera Shake Function
        /// </summary>
        /// <param name="SuccessAction"></param>
        public void ShakeCamera(Action SuccessAction)
        {
            if (m_cameraShakeCoroutine != null)
                StopCoroutine(m_cameraShakeCoroutine);

            m_cameraShakeCoroutine = StartCoroutine(CameraShakeEffectCoroutine(SuccessAction));
        }


        /// <summary>
        /// Camera Shake Coroutine to handle the duration and animation
        /// </summary>
        /// <param name="SuccessAction"></param>
        /// <returns>Completion of Camera Shake Event</returns>
        IEnumerator CameraShakeEffectCoroutine(Action SuccessAction)
        {
            float elapsedTime = 0f;
            Vector3 shakeStartPosition = transform.position;

            while (elapsedTime < _shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                float shakeStrength = _shakeAnimationCurve.Evaluate(elapsedTime / _shakeDuration);

                transform.position = shakeStartPosition + UnityEngine.Random.insideUnitSphere * shakeStrength;
                yield return null;
            }

            transform.position = shakeStartPosition;

            SuccessAction?.Invoke();
        }

    }
}
