using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private float defaultFadeDuration = 1f;

    public void SetBlack()
    {
        SetAlpha(1f);
    }

    public void SetClear()
    {
        SetAlpha(0f);
    }

    public void SetScreenAlpha(float alpha)
    {
        SetAlpha(alpha);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f, defaultFadeDuration);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f, 1f, defaultFadeDuration);
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(1f, 0f, duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return Fade(0f, 1f, duration);
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = blackScreen.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            blackScreen.color = new Color(
                color.r,
                color.g,
                color.b,
                alpha
            );

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = blackScreen.color;
        blackScreen.color = new Color(
            color.r,
            color.g,
            color.b,
            alpha
        );
    }
}