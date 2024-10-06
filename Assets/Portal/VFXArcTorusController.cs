using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXArcController : MonoBehaviour
{
    public VisualEffect vfx; // Reference to the VFX Graph
    private string arcParameter = "ArcControl"; // The name of the exposed arc parameter
    private string spawnRateParameter = "SpawnRate"; // Exposed spawn rate parameter
    private float arcValue = 0f; // Initial arc value (in radians)
    private float maxArcRadians = Mathf.PI * 2; // Max arc value (2π radians for full circle)
    private float timeToFullArc = 5f; // Time to reach full arc (in seconds)
    private float initialSpawnRate = 0f; // Initial spawn rate (0 = no particles emitted)
    public string majorRadiusParameter = "majorRadius";
    public string minorRadiusParameter = "minorRadius";
    private float elapsedTime; // Elapsed time since the start of the duration
    private Coroutine spawnCoroutine; // Reference to the running coroutine
    private float majorRadius;
    private float minorRadius;
    public float maxRadius = 1.5f;
    S
    void Start()
    {
        // Set the arc and spawn rate to 0 initially (no particles emitted)
        vfx.SetFloat(arcParameter, 0f);
        vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 at start
        vfx.SetFloat(majorRadiusParameter, 0f);
    }


    void Update()
    {
        // Check if spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Start the coroutine to handle the arc and spawn rate increase
            spawnCoroutine = StartCoroutine(PortalMaking());
            
        }
        // WILIAM ER GAY
        // Check if spacebar is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Stop the coroutine
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }

            // Reset arc value and spawn rate
            arcValue = 0;
            vfx.SetFloat(arcParameter, arcValue);
            vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 (stop emission)
            elapsedTime = 0f; // Reset elapsed time
        }
    }

    void RadiusAndBlur()
    {
        // gradually change the major radius value over time (from 0 to maxRadius)
        majorRadius += (maxRadius / timeToFullArc) * Time.deltaTime;
        majorRadius = Mathf.Clamp(majorRadius, 0, maxRadius);

       vfx.SetFloat(majorRadiusParameter, 0f);

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
            RadiusAndBlur();

            // Wait for the next frame
            yield return null;
        }
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
        // fucking, lort bøsse kode!!!
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

        print(vfx.GetInt(spawnRateParameter));
    }
}