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
    private int maxSpawnRate = 400000; // Adjust this to control how many particles spawn
    private int startSpawnRate = 4; // Start spawn rate (0 = no particles emitted)
    private float duration = 50f; // Duration to increase spawn rate (in seconds)
    private float elapsedTime = 0f; // Elapsed time since the start of the duration

    void Start()
    {
        // Set the arc and spawn rate to 0 initially (no particles emitted)
        vfx.SetFloat(arcParameter, 0f);
        vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 at start
    }

    // Husk at implementere tids intervaller for at gøre spawn mængden bedre
    // Eventuelt implementer en startup time for hele baduljen


    void Update()
    {
       
        // Check if spacebar is held down
        if (Input.GetKey(KeyCode.Space))
        {
            // Gradually increase the arc value over time (from 0 to 2π radians)
            arcValue += (maxArcRadians / timeToFullArc) * Time.deltaTime;
            arcValue = Mathf.Clamp(arcValue, 0, maxArcRadians);

            // Update the VFX Graph with the new arc value
            vfx.SetFloat(arcParameter, arcValue);

            // Increase spawn rate when spacebar is pressed
            IncreaseSpawnRate();
        }

        // When spacebar is released, stop the particle emission
        if (Input.GetKeyUp(KeyCode.Space))
        {
            arcValue = 0; // Reset arc value
            vfx.SetFloat(arcParameter, arcValue);
            vfx.SetInt(spawnRateParameter, (int)initialSpawnRate); // Set spawn rate to 0 (stop emission)
            elapsedTime = 0f; // Reset elapsed time
        }
    }

    void IncreaseSpawnRate()
    {
        // Increase the spawn rate over the specified duration
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        int currentSpawnRate = (int)Mathf.Lerp(startSpawnRate, maxSpawnRate, Mathf.Cos(t)/2);
        vfx.SetInt(spawnRateParameter, currentSpawnRate);
        print(currentSpawnRate);
    }
}
