using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private FPSController player;
    [SerializeField] private TextMeshProUGUI transitionText;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private TextMeshProUGUI policeCallText;
    [SerializeField] private TextMeshProUGUI policeCallPromptText;
    [SerializeField] private TextMeshProUGUI monologText;
    [SerializeField] private TextMeshProUGUI interactionMessage;
    [SerializeField] private Image deathImage;
    [SerializeField] private Image winImage;

    [SerializeField] private Image morningImage;
    [SerializeField] private Animator animator;

    [SerializeField] private float blackScreenDuration = 2f;
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private float narrationDuration = 4f;

    [SerializeField] private int choiceTimerDuration = 60;

    [SerializeField] private float morningFadeDuration = 1f;

    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button backToMenuButton;

    [SerializeField] private KeyCode callPoliceKey;

    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject deadAdrian;

    [SerializeField] private GameObject[] collidersAdrian;

    [SerializeField] private float morningImageDuration;

    [SerializeField] private GameObject yesterdayDiner;

    int currentDay = -1;

    private Coroutine choiceTimerCoroutine;

    private bool choiceTimerStarted = false;

    private void Start()
    {
        transitionText.gameObject.SetActive(false);
        narrationText.gameObject.SetActive(false);
        policeCallText.gameObject.SetActive(false);
        policeCallPromptText.gameObject.SetActive(false);
        monologText.gameObject.SetActive(false);

        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);

        morningImage.gameObject.SetActive(false);
        deathImage.gameObject.SetActive(false);
        winImage.gameObject.SetActive(false);

        yesterdayDiner.SetActive(false);

        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        button.SetActive(false);

        deadAdrian.gameObject.SetActive(true);

        StartCoroutine(StartDay(0, false, true));
    }

    private IEnumerator StartDay(int day, bool fadeIn, bool fadeOut)
    {
        currentDay = day;

        player.SetInputEnabled(InputMode.DISABLED);

        if (fadeIn)
            yield return screenFader.FadeIn();

        switch (day)
        {
            case 0:
                yield return new WaitForSeconds(blackScreenDuration);

                animator.gameObject.SetActive(true);
                animator.Play("ZZZs sept 20");
                yield return ShowMorning(morningImageDuration);

                yield return StartDay0();

                break;

            case 1:
                narrationText.text = "Today, the owner of the diner that I work at was murdered. Everyone was dismissed early to allow the investigation to take its course. I can’t help but feel that something is wrong with the police’s suspicions.";
                narrationText.gameObject.SetActive(true);

                yield return new WaitForSeconds(narrationDuration);

                narrationText.gameObject.SetActive(false);

                yield return new WaitForSeconds(blackScreenDuration);

                animator.gameObject.SetActive(true);
                animator.Play("ZZZs sept19"); 
                yield return ShowMorning(morningImageDuration);

                yield return new WaitForSeconds(1f);

                narrationText.text = "I don’t understand. Did my alarm clock malfunction? I could have sworn that today was the 21st. I can’t believe the sous-chef told us to come in even though our head chef was just murdered.";
                narrationText.gameObject.SetActive(true);

                yield return new WaitForSeconds(narrationDuration);

                narrationText.gameObject.SetActive(false);

                deadAdrian.gameObject.SetActive(false);

                foreach (GameObject character in characters)
                {
                    character.gameObject.SetActive(true);
                }

                yesterdayDiner.SetActive(true);
                button.SetActive(true);

                player.ResetPlayerPosition();

                yield return screenFader.FadeOut();

                player.SetInputEnabled(InputMode.NEXT_MESSAGE_ONLY);

                monologText.text = "I can’t believe my eyes, my boss is here. Was what happened yesterday a dream? Or have I been given a chance to make it right? Maybe I should figure out if I can prevent it.";
                monologText.gameObject.SetActive(true);

                yield return new WaitForSeconds(narrationDuration);

                monologText.gameObject.SetActive(false);

                yield return StartDay1();

                break;
        }

        if (fadeOut)
        {        
            yield return new WaitForSeconds(blackScreenDuration);

            yield return screenFader.FadeOut();
        }

        player.SetInputEnabled(InputMode.ENABLED);
    }

    private IEnumerator StartDay0()
    {
        Debug.Log("Starting day 0");

        // show the context of the crime

        yield return null;
    }

    private IEnumerator StartDay1()
    {
        Debug.Log("Starting day 1");

        choiceTimerCoroutine = StartCoroutine(StartChoiceTimer());

        foreach (GameObject collider in collidersAdrian)
        {
            collider.gameObject.SetActive(false);
        }

        yield return null;
    }

    private IEnumerator ShowMorning(float duration)
    {
        morningImage.gameObject.SetActive(true);

        // Fade in
        Color color = morningImage.color;
        color.a = 0f;
        morningImage.color = color;

        float timer = 0f;

        while (timer < morningFadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / morningFadeDuration);
            morningImage.color = color;

            yield return null;
        }

        color.a = 1f;
        morningImage.color = color;

        // Image displayed
        yield return new WaitForSeconds(duration);

        // Fade out
        timer = 0f;

        while (timer < morningFadeDuration)
        {
            timer += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(timer / morningFadeDuration);
            morningImage.color = color;

            yield return null;
        }

        color.a = 0f;
        morningImage.color = color;

        morningImage.gameObject.SetActive(false);
    }

    private IEnumerator GameOver(string message)
    {
        Debug.Log("Resetting day 1");

        player.SetInteractionPromptActive(false);

        interactionMessage.gameObject.SetActive(false);

        player.SetInputEnabled(InputMode.DISABLED);

        yield return screenFader.FadeIn();

        // do reset stuff

        deathImage.gameObject.SetActive(true);
        narrationText.text = message;
        narrationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(blackScreenDuration);

        yield return new WaitForSeconds(transitionDuration);

        deathImage.gameObject.SetActive(false);
        narrationText.gameObject.SetActive(false);

        yield return new WaitForSeconds(transitionDuration);

        animator.gameObject.SetActive(true);
        animator.Play("ZZZs sept19");
        yield return ShowMorning(morningImageDuration);


        yield return new WaitForSeconds(blackScreenDuration);

        player.ResetPlayerPosition();

        yield return screenFader.FadeOut();

        yield return StartDay1();

        player.SetInputEnabled(InputMode.ENABLED);
    }

    private IEnumerator Win()
    {
        Debug.Log("Win");

        player.SetInteractionPromptActive(false);

        player.SetInputEnabled(InputMode.DISABLED);

        yield return screenFader.FadeIn();

        yield return new WaitForSeconds(transitionDuration);

        winImage.gameObject.SetActive(true);
        transitionText.text = "You found the murderer";
        transitionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.gameObject.SetActive(false);
        // winImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(transitionDuration);

        narrationText.text = "Explain that everything goes back to normal";
        narrationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(narrationDuration);

        backToMenuButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator StartChoiceTimer()
    {
        choiceTimerStarted = true;

        yield return new WaitForSeconds(1f);

        policeCallPromptText.text = "Press [" + callPoliceKey + "] to call the police";
        policeCallPromptText.gameObject.SetActive(true);

        yield return new WaitForSeconds(choiceTimerDuration);

        yield return GameOver("The killer found you while walking around. He killed you.");
    }

    public void RightAnswer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        policeCallText.gameObject.SetActive(false);
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);

        screenFader.SetClear();

        StartCoroutine(Win());
    }

    public void WrongAnswer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        policeCallText.gameObject.SetActive(false);
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);

        screenFader.SetClear();

        StartCoroutine(GameOver("The killer saw you accusing other suspects. He killed you."));
    }

    private void CallPolice()
    {
        policeCallPromptText.gameObject.SetActive(false);

        if (choiceTimerCoroutine != null)
        {
            StopCoroutine(choiceTimerCoroutine);
            choiceTimerCoroutine = null;

            Debug.Log("Timer stopped");
        }

        choiceTimerStarted = false;

        player.SetInputEnabled(InputMode.DISABLED);

        policeCallText.text = "Please tell us the identity of the murderer.";
        policeCallText.gameObject.SetActive(true);

        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);

        screenFader.SetScreenAlpha(0.65f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayerSeesDeadAdrian()
    {
        StartCoroutine(StartDay(1, true, false));
    }

    public void SetPoliceCallPromptActive(bool active)
    {
        policeCallPromptText.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (Input.GetKeyDown(callPoliceKey) && choiceTimerStarted)
        {
            CallPolice() ;
        }

        ////////////////// DEBUG ///////////////////
        if (Input.GetKeyDown(KeyCode.N) &&
            currentDay == 0)
        {
            StartCoroutine(StartDay(1, true, true));
        }

        if (Input.GetKeyDown(KeyCode.R) &&
            currentDay == 1)
        {
            StartCoroutine(GameOver("Game Over"));
        }

        if (Input.GetKeyDown(KeyCode.Y) &&
            currentDay == 1)
        {
            StartCoroutine(Win());
        }


        if (Input.GetKeyDown(KeyCode.O) &&
            currentDay == 1)
        {
            StartCoroutine(StartChoiceTimer());
        }

        if (Input.GetKeyDown(KeyCode.F) &&
            currentDay == 1)
        {
            choiceTimerCoroutine = StartCoroutine(StartChoiceTimer());
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) &&
            currentDay != 0)
        {
            currentDay = 0;
            Debug.Log("DEBUG - switching to day 0");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) &&
            currentDay != 1)
        {
            currentDay = 1;
            Debug.Log("DEBUG - switching to day 1");
        }
        ////////////////////////////////////////////
    }
}