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
        float targetAlpha = materialColor.a; // Store the target alpha value
        materialColor.a = 0f; // Start with alpha at 0 (completely transparent)
        renderer.material.color = materialColor; // Apply the initial transparent color

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeDuration);
            materialColor.a = alpha;
            renderer.material.color = materialColor;
            yield return null;
        }

        // Ensure the final alpha is set to the target alpha
        materialColor.a = targetAlpha;
        renderer.material.color = materialColor;
    }
}
