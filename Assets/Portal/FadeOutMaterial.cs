using UnityEngine;
using System.Collections;

public class FadeOutMaterial : MonoBehaviour
{
    public float fadeDuration = 4.3f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color materialColor = renderer.material.color;

        float elapsedTime = 0f;
        float targetAlpha = 0.5f; // Set the target alpha to 0.5
        materialColor.a = 0f; // Start with alpha at 0 (completely transparent)
        renderer.material.color = materialColor; // Apply the initial transparent color

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = (elapsedTime / fadeDuration) * targetAlpha; // Calculate alpha based on elapsed time
            materialColor.a = alpha;
            renderer.material.color = materialColor;

            // Print the current alpha value to the console
            //Debug.Log("Current Alpha: " + alpha);

            yield return null;
        }

        // Ensure the final alpha is set to the target alpha
        materialColor.a = targetAlpha;
        renderer.material.color = materialColor;

        // Print the final alpha value to the console
        Debug.Log("Final Alpha: " + targetAlpha);
    }
}
