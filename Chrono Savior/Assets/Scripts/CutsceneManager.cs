using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Sprite[] preModeCutsceneImages; 
    [SerializeField] private string[] preModeDialogueLines;
    [SerializeField] private Sprite[] midGameCutsceneImages; 
    [SerializeField] private string[] midGameDialogueLines; 
    [SerializeField] private Image fadeOverlay; 
    [SerializeField] private float fadeDuration = 1f;

    private int currentCutsceneIndex = 0;
    private bool isMidGameCutscene = false;

    private void Start()
    {
        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.color = new Color(0, 0, 0, 1);
        if (!isMidGameCutscene)
        {
            StartCoroutine(PlayCutscene(preModeCutsceneImages, preModeDialogueLines, "Space"));
        }
    }

    public void TriggerMidGameCutscene()
    {
        isMidGameCutscene = true;
        currentCutsceneIndex = 0;
        StartCoroutine(PlayCutscene(midGameCutsceneImages, midGameDialogueLines));
    }

    private IEnumerator PlayCutscene(Sprite[] cutsceneImages, string[] dialogueLines)
    {
        cutsceneImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        while (currentCutsceneIndex < cutsceneImages.Length)
        {
            yield return StartCoroutine(FadeToBlack());
            cutsceneImage.sprite = cutsceneImages[currentCutsceneIndex];
            dialogueText.text = dialogueLines[currentCutsceneIndex];
            yield return StartCoroutine(FadeFromBlack());

            yield return new WaitForSeconds(4f); // wait time
            currentCutsceneIndex++;
        }

        yield return StartCoroutine(FadeToBlack());
        cutsceneImage.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        yield return StartCoroutine(FadeFromBlack());

        isMidGameCutscene = false;
    }

    private IEnumerator FadeToBlack()
    {
        Color tempColor = fadeOverlay.color;
        tempColor.a = 0f;
        fadeOverlay.color = tempColor;
        fadeOverlay.gameObject.SetActive(true);

        while (fadeOverlay.color.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeDuration;
            fadeOverlay.color = tempColor;
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        Color tempColor = fadeOverlay.color;

        while (fadeOverlay.color.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeDuration;
            fadeOverlay.color = tempColor;
            yield return null;
        }

        fadeOverlay.gameObject.SetActive(false);
    }
}
