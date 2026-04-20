using UnityEngine;
using TMPro;
using System.Collections;

public class EndGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI servedText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI ratingText;

    [Header("Effects")]
    public GameObject tomatoEffect;
    public GameObject flowerEffect;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        int negativeNumber = Game_Manager.sharedInstance.negativeScore;
        int positiveNumber = Game_Manager.sharedInstance.positiveScore;
        int total = negativeNumber + positiveNumber;

        // Step 1: reveal served
        yield return AnimateNumber(servedText, 0, positiveNumber, "Served: ");

        yield return new WaitForSeconds(0.5f);

        // Step 2: reveal left
        yield return AnimateNumber(leftText, 0, negativeNumber, "Left: ");

        yield return new WaitForSeconds(0.5f);
        int rating = positiveNumber / total;
        yield return AnimateRating(rating);

        // Step 4: play result effects
        if (rating >= 0.7f)
            PlayWin();
        else
            PlayLose();
    }
    IEnumerator AnimateNumber(TextMeshProUGUI text, int from, int to, string prefix)
    {
        float duration = 1f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, time / duration));

            text.text = prefix + value;

            yield return null;
        }

        text.text = prefix + to;
    }
    IEnumerator AnimateRating(float target)
    {
        float current = 0f;

        while (current < target)
        {
            current += Time.deltaTime;

            float display = Mathf.Clamp01(current);

            ratingText.text = "Rating: " + Mathf.RoundToInt(display * 100) + "%";

            yield return null;
        }

        ratingText.text = "Rating: " + Mathf.RoundToInt(target * 100) + "%";
    }
    void PlayWin()
    {
        flowerEffect.SetActive(true);
        audioSource.PlayOneShot(winSound);
    }

    void PlayLose()
    {
        tomatoEffect.SetActive(true);
        audioSource.PlayOneShot(loseSound);
    }
}
