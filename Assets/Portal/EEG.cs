using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EEG : MonoBehaviour
{
    // Placeholder: Import functions from the Unicorn Hybrid Black SDK
    [DllImport("UnicornSDK.dll")]
    private static extern int GetConcentrationLevel(); // Example SDK call

    private float concentrationThreshold = 0.75f; // Adjust based on desired difficulty
    
    void Update()
    {
        // Get concentration data from the brain sensor
        float concentration = GetConcentrationLevel(); // Simulated brain signal input

        // Check if the concentration is above the threshold to open the portal
        if (concentration >= concentrationThreshold)
        {
            //OpenPortal();
            print("portal open");
        }
        else if (concentration < concentrationThreshold )
        {
            print("portal close");
            //ClosePortal();
        }
    }
}