using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed multiplier
    public float rotationSpeed = 30f;  // Rotation speed multiplier
    public float noiseScale = 1f;  // Scale for noise, controls smoothness of movement

    private float offsetX;
    private float offsetY;
    private float offsetZ;

    void Start()
    {
        // Random starting points for noise, ensuring each axis has unique noise
        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);
        offsetZ = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Generate smooth random movement using Perlin noise
        float noiseX = Mathf.PerlinNoise(Time.time * noiseScale + offsetX, 0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(Time.time * noiseScale + offsetY, 0f) - 0.5f;
        float noiseZ = Mathf.PerlinNoise(Time.time * noiseScale + offsetZ, 0f) - 0.5f;

        // Apply movement
        Vector3 movement = new Vector3(noiseX, noiseY, noiseZ) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Generate smooth random rotation using Perlin noise
        float noiseRotX = Mathf.PerlinNoise(Time.time * noiseScale + offsetX, 1f) - 0.5f;
        float noiseRotY = Mathf.PerlinNoise(Time.time * noiseScale + offsetY, 1f) - 0.5f;
        float noiseRotZ = Mathf.PerlinNoise(Time.time * noiseScale + offsetZ, 1f) - 0.5f;

        // Apply rotation
        Vector3 rotation = new Vector3(noiseRotX, noiseRotY, noiseRotZ) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation);
    }
}

