using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Sprite[] preModeCutsceneImages; // Array to hold pre-mode cutscene images
    [SerializeField] private string[] preModeDialogueLines; // Array to hold pre-mode dialogue lines
    [SerializeField] private Sprite[] midGameCutsceneImages; // Array to hold mid-game cutscene images
    [SerializeField] private string[] midGameDialogueLines; // Array to hold mid-game dialogue lines
    [SerializeField] private Image fadeOverlay; // Image component for the black fade overlay
    [SerializeField] private float fadeDuration = 1f; // Duration for the fade effect

    private int currentCutsceneIndex = 0;
    private bool isMidGameCutscene = false;

    private void Start()
    {
        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.color = new Color(0, 0, 0, 1); // Ensure overlay starts fully opaque
        if (!isMidGameCutscene)
        {
            // Start the pre-mode cutscene at the beginning of the game
            StartCoroutine(PlayCutscene(preModeCutsceneImages, preModeDialogueLines));
        }
    }

    public void TriggerMidGameCutscene()
    {
        isMidGameCutscene = true;
        currentCutsceneIndex = 0; // Reset index for mid-game cutscene
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

            yield return new WaitForSeconds(4f); // Wait for 5 seconds before moving to the next cutscene
            currentCutsceneIndex++;
        }

        // Cutscene is over, hide the cutscene canvas
        yield return StartCoroutine(FadeToBlack());
        cutsceneImage.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        yield return StartCoroutine(FadeFromBlack());

        isMidGameCutscene = false; // Reset the flag after cutscene ends
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
