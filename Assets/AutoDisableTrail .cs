using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class AutoDisableTrail : MonoBehaviour
{
    [Header("Target & detection")]
    public Transform target;
    public float velocityThreshold = 0.01f;
    public float stationaryTimeToDisable = 0.25f;

    [Header("Options")]
    public bool clearWhenDisabled = true;
    public bool useRigidbodyVelocity = false;

    [Header("Fade Settings")]
    [Tooltip("Tempo (em segundos) para o trail desaparecer suavemente.")]
    public float fadeOutDuration = 0.5f;

    TrailRenderer trail;
    Rigidbody rbTarget;
    Vector3 lastPosition;
    float stationaryTimer = 0f;
    bool isDisabled = false;
    bool isFading = false;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        if (target == null) target = transform;
        if (useRigidbodyVelocity)
            rbTarget = target.GetComponent<Rigidbody>();
        lastPosition = target.position;
    }

    void Update()
    {
        float speed = 0f;
        if (useRigidbodyVelocity && rbTarget != null)
            speed = rbTarget.linearVelocity.magnitude;
        else
        {
            Vector3 delta = target.position - lastPosition;
            speed = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-6f);
            lastPosition = target.position;
        }

        if (speed <= velocityThreshold)
        {
            stationaryTimer += Time.deltaTime;
            if (!isDisabled && stationaryTimer >= stationaryTimeToDisable)
                DisableTrailSmoothly();
        }
        else
        {
            stationaryTimer = 0f;
            if (isDisabled && !isFading)
                EnableTrail();
        }
    }

    void DisableTrailSmoothly()
    {
        if (isFading) return;
        StartCoroutine(FadeOutTrail());
    }

    IEnumerator FadeOutTrail()
    {
        isFading = true;

        // Captura o gradiente original do trail
        Gradient originalGradient = trail.colorGradient;
        Gradient newGradient = new Gradient();

        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            float t = elapsed / fadeOutDuration;
            float alphaMultiplier = Mathf.Lerp(1f, 0f, t);

            // recria gradiente com o alpha reduzido
            GradientColorKey[] colorKeys = originalGradient.colorKeys;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[originalGradient.alphaKeys.Length];

            for (int i = 0; i < alphaKeys.Length; i++)
            {
                float baseAlpha = originalGradient.alphaKeys[i].alpha;
                alphaKeys[i] = new GradientAlphaKey(baseAlpha * alphaMultiplier, originalGradient.alphaKeys[i].time);
            }

            newGradient.SetKeys(colorKeys, alphaKeys);
            trail.colorGradient = newGradient;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // garante opacidade 0 total no fim
        GradientAlphaKey[] zeroAlphaKeys = new GradientAlphaKey[originalGradient.alphaKeys.Length];
        for (int i = 0; i < zeroAlphaKeys.Length; i++)
            zeroAlphaKeys[i] = new GradientAlphaKey(0f, originalGradient.alphaKeys[i].time);

        newGradient.SetKeys(originalGradient.colorKeys, zeroAlphaKeys);
        trail.colorGradient = newGradient;

        if (clearWhenDisabled)
            trail.Clear();

        #if UNITY_2019_1_OR_NEWER
        trail.emitting = false;
        #else
        trail.enabled = false;
        #endif

        isDisabled = true;
        isFading = false;
    }

    void EnableTrail()
    {
        #if UNITY_2019_1_OR_NEWER
        trail.emitting = true;
        #else
        trail.enabled = true;
        #endif

        // restaura o gradiente original (alpha total)
        Gradient g = trail.colorGradient;
        GradientAlphaKey[] restoredAlpha = new GradientAlphaKey[g.alphaKeys.Length];
        for (int i = 0; i < restoredAlpha.Length; i++)
            restoredAlpha[i] = new GradientAlphaKey(1f, g.alphaKeys[i].time);

        Gradient restored = new Gradient();
        restored.SetKeys(g.colorKeys, restoredAlpha);
        trail.colorGradient = restored;

        isDisabled = false;
    }
}
