//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using UnityEngine.SceneManagement;

//public class CutsceneManager : MonoBehaviour
//{
//    [SerializeField] private Image cutsceneImage;
//    [SerializeField] private Text dialogueText;
//    [SerializeField] private Sprite[] preModeCutsceneImages; 
//    [SerializeField] private string[] preModeDialogueLines;
//    [SerializeField] private Sprite[] midGameCutsceneImages; 
//    [SerializeField] private string[] midGameDialogueLines; 
//    [SerializeField] private Image fadeOverlay; 
//    [SerializeField] private float fadeDuration = 1f;
//    string scene = "Space";

//    private int currentCutsceneIndex = 0;
//    private bool isMidGameCutscene = false;

//    private void Start()
//    {
//        isMidGameCutscene = MainMenu.MidGame;
//        fadeOverlay.gameObject.SetActive(true);
//        fadeOverlay.color = new Color(0, 0, 0, 1);
//        if (!isMidGameCutscene)
//        {
//            StartCoroutine(PlayCutscene(preModeCutsceneImages, preModeDialogueLines, "Space"));
//        }

//        else
//        {
//            scene = "GroundCampaign";
//            TriggerMidGameCutscene();
//        }
//    }

//    public void TriggerMidGameCutscene()
//    {
//        currentCutsceneIndex = 0;
//        StartCoroutine(PlayCutscene(midGameCutsceneImages, midGameDialogueLines, "GroundCampaign"));

//    }

//    void Update()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            SceneManager.LoadScene(scene);
//        }
//    }

//    private IEnumerator PlayCutscene(Sprite[] cutsceneImages, string[] dialogueLines, string Scene)
//    {
//        cutsceneImage.gameObject.SetActive(true);
//        dialogueText.gameObject.SetActive(true);

//        while (currentCutsceneIndex < cutsceneImages.Length)
//        {
//            yield return StartCoroutine(FadeToBlack());
//            cutsceneImage.sprite = cutsceneImages[currentCutsceneIndex];
//            dialogueText.text = dialogueLines[currentCutsceneIndex];

//            yield return StartCoroutine(FadeFromBlack());
//            yield return new WaitForSeconds(4f);// wait time


//            currentCutsceneIndex++;


//        }

//        yield return StartCoroutine(FadeToBlack());
//        cutsceneImage.gameObject.SetActive(false);
//        dialogueText.gameObject.SetActive(false);
//        yield return StartCoroutine(FadeFromBlack());
//        isMidGameCutscene = false;

//        SceneManager.LoadScene(Scene);



//    }

//    private IEnumerator FadeToBlack()
//    {
//        Color tempColor = fadeOverlay.color;
//        tempColor.a = 0f;
//        fadeOverlay.color = tempColor;
//        fadeOverlay.gameObject.SetActive(true);

//        while (fadeOverlay.color.a < 1f)
//        {
//            tempColor.a += Time.deltaTime / fadeDuration;
//            fadeOverlay.color = tempColor;
//            yield return null;
//        }
//    }

//    private IEnumerator FadeFromBlack()
//    {
//        Color tempColor = fadeOverlay.color;

//        while (fadeOverlay.color.a > 0f)
//        {
//            tempColor.a -= Time.deltaTime / fadeDuration;
//            fadeOverlay.color = tempColor;
//            yield return null;
//        }

//        fadeOverlay.gameObject.SetActive(false);
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Sprite[] preModeCutsceneImages;
    [SerializeField] private string[] preModeDialogueLines;
    [SerializeField] private Sprite[] midGameCutsceneImages;
    [SerializeField] private string[] midGameDialogueLines;
    [SerializeField] private Sprite[] endGameCutsceneImages;
    [SerializeField] private string[] endGameDialogueLines;
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;
    private string scene = "Space";

    private int currentCutsceneIndex = 0;
    private bool isMidGameCutscene = false;
    private bool isEndGameCutscene = false;

    private void Start()
    {
        isMidGameCutscene = MainMenu.MidGame;
        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.color = new Color(0, 0, 0, 1);
        if (!isMidGameCutscene)
        {
            StartCoroutine(PlayCutscene(preModeCutsceneImages, preModeDialogueLines, "Space"));
        }
        else if(MainMenu.EndGame)
        {
            TriggerEndGameCutscene();
        }            
        else
        {
            scene = "GroundCampaign";
            TriggerMidGameCutscene();
        }
    }

    public void TriggerMidGameCutscene()
    {
        currentCutsceneIndex = 0;
        StartCoroutine(PlayCutscene(midGameCutsceneImages, midGameDialogueLines, "GroundCampaign"));
    }

    public void TriggerEndGameCutscene()
    {
        currentCutsceneIndex = 0;
        isEndGameCutscene = true;
        StartCoroutine(PlayCutscene(endGameCutsceneImages, endGameDialogueLines, "Main Menu"));
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(scene);
        }
    }

    private IEnumerator PlayCutscene(Sprite[] cutsceneImages, string[] dialogueLines, string nextScene)
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

        if (isMidGameCutscene)
        {
            isMidGameCutscene = false;
        }
        else if (isEndGameCutscene)
        {
            isEndGameCutscene = false;
        }

        SceneManager.LoadScene(nextScene);
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
