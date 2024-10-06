using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXArcController : MonoBehaviour
{
    public VisualEffect vfx; // Reference to the VFX Graph
    public GameObject[] portalPrefabs; // Array of portal prefabs
    private int currentPrefabIndex = 0; // Index to track the current prefab

    private string arcParameter = "ArcControl"; // The name of the exposed arc parameter
    private string spawnRateParameter = "SpawnRate"; // Exposed spawn rate parameter
    private float arcValue = 0f; // Initial arc value (in radians)
    private float maxArcRadians = Mathf.PI * 2; // Max arc value (2π radians for full circle)
    private float timeToFullArc = 3f; // Time to reach full arc (in seconds)
    private float initialSpawnRate = 0f; // Initial spawn rate (0 = no particles emitted)
    private float additionalActiveTime = 4f; // Additional time to keep the portal active

    private float elapsedTime; // Elapsed time since the start of the duration
    private Coroutine spawnCoroutine; // Reference to the running coroutine
    private bool shouldStopPortal = false; // Flag to indicate if the portal should be stopped
    private bool isFullyFormed = false; // Flag to indicate if the portal is fully formed
    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // List to keep track of spawned prefabs

    void Start()
    {
        // Set the arc and spawn rate to 0 initially (no particles emitted)
        vfx.SetFloat(arcParameter, 0f);
        vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 at start
    }

    void Update()
    {
        // Simulate concentration by holding down the spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerPortal(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            TriggerPortal(false);
        }
    }

    // New method to trigger the portal based on concentration level
    public void TriggerPortal(bool isConcentrating)
    {
        if (isConcentrating && spawnCoroutine == null)
        {
            // If concentrating, start the portal effect
            spawnCoroutine = StartCoroutine(PortalMaking());
        }
        else if (!isConcentrating && spawnCoroutine != null)
        {
            // If no longer concentrating, set the flag to stop the portal after additional active time
            shouldStopPortal = true;
        }
    }

    public IEnumerator PortalMaking()
    {
        while (true)
        {
            // Increase arc value
            ArcIncrease();

            // Increase spawn rate
            IncreaseSpawnRate();

            // Increase major radius and blur
            Radius();

            // Wait for the next frame
            yield return null;

            // Check if the portal is fully formed
            if (elapsedTime >= timeToFullArc)
            {
                isFullyFormed = true;
            }

            // Spawn prefabs only if the portal has been active for more than 4 seconds
            if (elapsedTime >= 4f && isFullyFormed && spawnedPrefabs.Count == 0)
            {
                // Instantiate the current prefab and add it to the list
                GameObject spawnedPrefab = Instantiate(portalPrefabs[currentPrefabIndex], transform.position, transform.rotation);
                spawnedPrefabs.Add(spawnedPrefab);

                // Update the prefab index for the next cycle
                currentPrefabIndex = (currentPrefabIndex + 1) % portalPrefabs.Length;
            }

            // Check if the portal should be stopped
            if (shouldStopPortal)
            {
                if (isFullyFormed)
                {
                    // Wait for the additional active time
                    yield return new WaitForSeconds(additionalActiveTime);
                }

                // Stop the portal effect
                StopPortal();
                yield break; // Exit the coroutine
            }
        }
    }

    void StopPortal()
    {
        // Reset arc value and spawn rate
        arcValue = 0;
        vfx.SetFloat(arcParameter, arcValue);
        vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 (stop emission)
        elapsedTime = 0f; // Reset elapsed time
        shouldStopPortal = false; // Reset the flag
        isFullyFormed = false; // Reset the fully formed flag
        spawnCoroutine = null; // Clear the coroutine reference

        // Destroy all spawned prefabs
        foreach (GameObject prefab in spawnedPrefabs)
        {
            Destroy(prefab);
        }
        spawnedPrefabs.Clear(); // Clear the list of spawned prefabs
    }

    void Radius()
    {
        // Lerp between 0 and 1 over the course of timeToFullArc
        float t = elapsedTime / timeToFullArc;
        float radius = Mathf.Lerp(0f, 1f, t);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    void ArcIncrease()
    {
        // Gradually increase the arc value over time (from 0 to 2π radians)
        arcValue += (maxArcRadians / timeToFullArc) * Time.deltaTime;
        arcValue = Mathf.Clamp(arcValue, 0, maxArcRadians);

        // Update the VFX Graph with the new arc value
        vfx.SetFloat(arcParameter, arcValue);
    }

    void IncreaseSpawnRate()
    {
        // Increase the spawn rate based on elapsed time intervals
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 3f)
        {
            vfx.SetInt(spawnRateParameter, 400000);
        }
        else if (elapsedTime >= 2f)
        {
            vfx.SetInt(spawnRateParameter, 4000);
        }
        else if (elapsedTime >= 1f)
        {
            vfx.SetInt(spawnRateParameter, 600);
        }
        else if (elapsedTime >= 0f)
        {
            vfx.SetInt(spawnRateParameter, 5);
        }
    }
}
