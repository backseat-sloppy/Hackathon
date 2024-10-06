using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightModulation : MonoBehaviour
{
    public Light pointLight;   // Reference to the point light component
    public float intensityScale = 1f;  // Scale for the light intensity multiplier
    public float noiseSpeed = 1f;      // Speed at which noise changes (affects how fast intensity changes)
    public float baseIntensity = 1f;   // Base intensity value around which the noise modulates

    private float noiseOffset;

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }

        // Random starting point for noise
        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Generate smooth noise-based value for the light intensity
        float noiseValue = Mathf.PerlinNoise(Time.time * noiseSpeed + noiseOffset, 0f);

        // Adjust the light intensity based on noise value
        pointLight.intensity = baseIntensity + (noiseValue - 0.5f) * intensityScale;
    }
}
