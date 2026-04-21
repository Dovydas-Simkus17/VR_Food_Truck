using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
public class EndGameManager : MonoBehaviour
{
    public static EndGameManager sharedInstance;
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
    public AudioClip winMusic;
    public AudioClip loseMusic;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (System.Enum.TryParse(scene.name, out Loader.Scene loadedScene))
        {
            if (loadedScene == Loader.Scene.EndGameScene)
            {
                BindUI();
                StartCoroutine(PlaySequence());
            }
        }

    }

    void BindUI()
    {
        servedText = GameObject.Find("PositiveNumber").GetComponent<TextMeshProUGUI>();
        leftText = GameObject.Find("NegativeNumber").GetComponent<TextMeshProUGUI>();
        ratingText = GameObject.Find("Ratings").GetComponent<TextMeshProUGUI>();

        servedText.text = "";
        leftText.text = "";
        ratingText.text = "";

        //tomatoEffect = GameObject.Find("FX_Fireworks_Green_Small");
        //flowerEffect = GameObject.Find("FX_BloodSplatter");
}
    IEnumerator PlaySequence()
    {
        int negativeNumber = Game_Manager.sharedInstance.negativeScore;
        int positiveNumber = Game_Manager.sharedInstance.positiveScore;
        int total = negativeNumber + positiveNumber;

        // reveal served
        yield return AnimateNumber(servedText, 0, positiveNumber, "Served: ");

        yield return new WaitForSeconds(0.5f);

        // reveal left
        yield return AnimateNumber(leftText, 0, negativeNumber, "Left: ");

        yield return new WaitForSeconds(0.5f);
        float rating = (float)positiveNumber / total;
        yield return AnimateRating(rating);

        // play result effects
        if (rating >= 0.7f)
            PlayWin();
        else
            PlayLose();
        StopCoroutine(PlaySequence());
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
        PlayVFX(flowerEffect);
        audioSource.PlayOneShot(winSound);
        MusicManager.sharedInstance.changeSong(winMusic);

    }

    void PlayLose()
    {
        PlayVFX(tomatoEffect);
        audioSource.PlayOneShot(loseSound);
        MusicManager.sharedInstance.changeSong(loseMusic);
    }
    void PlayVFX(GameObject vfxObject)
    {
        var ps = vfxObject.GetComponent<ParticleSystem>();

        if (ps == null) return;

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Clear();
        ps.Play();
    }
}
