using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private FPSController player;
    [SerializeField] private TextMeshProUGUI transitionText;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private TextMeshProUGUI policeCallText;
    [SerializeField] private TextMeshProUGUI policeCallPromptText;

    [SerializeField] private float blackScreenDuration = 2f;
    [SerializeField] private float transitionDuration = 3f;

    [SerializeField] private int choiceTimerDuration = 60;

    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;

    [SerializeField] private KeyCode callPoliceKey;

    int currentDay = -1;

    private Coroutine choiceTimerCoroutine;

    private bool choiceTimerStarted = false;

    private void Start()
    {
        transitionText.gameObject.SetActive(false);
        narrationText.gameObject.SetActive(false);
        policeCallText.gameObject.SetActive(false);
        policeCallPromptText.gameObject.SetActive(false);

        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);

        screenFader.SetClear();
    }

    private IEnumerator StartDay(int day)
    {
        currentDay = day;

        player.SetInputEnabled(InputMode.DISABLED);

        yield return screenFader.FadeIn();

        switch (day)
        {
            case 0:
                yield return new WaitForSeconds(blackScreenDuration);

                transitionText.text = "Day 0";
                transitionText.gameObject.SetActive(true);

                yield return new WaitForSeconds(transitionDuration);

                transitionText.gameObject.SetActive(false);

                yield return StartDay0();

                break;

            case 1:
                yield return new WaitForSeconds(blackScreenDuration);

                transitionText.text = "Day 1";
                transitionText.gameObject.SetActive(true);

                yield return new WaitForSeconds(transitionDuration);

                transitionText.gameObject.SetActive(false);

                yield return StartDay1();

                break;
        }

        yield return new WaitForSeconds(blackScreenDuration);

        yield return screenFader.FadeOut();

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

        // time loop day
        // the player tries to find the murderer

        yield return null;
    }

    private IEnumerator GameOver(string message)
    {
        Debug.Log("Resetting day 1");

        player.SetInputEnabled(InputMode.DISABLED);

        yield return screenFader.FadeIn();

        // do reset stuff

        yield return new WaitForSeconds(blackScreenDuration);

        transitionText.text = message;
        transitionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.gameObject.SetActive(false);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.text = "Day 1";
        transitionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.gameObject.SetActive(false);

        yield return StartDay1();

        yield return new WaitForSeconds(blackScreenDuration);

        yield return screenFader.FadeOut();

        player.SetInputEnabled(InputMode.ENABLED);
    }

    private IEnumerator Win()
    {
        Debug.Log("Win");

        player.SetInputEnabled(InputMode.DISABLED);

        yield return screenFader.FadeIn();

        yield return new WaitForSeconds(transitionDuration);

        transitionText.text = "You found the murderer";
        transitionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.gameObject.SetActive(false);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.text = "Day 2";
        transitionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionDuration);

        transitionText.gameObject.SetActive(false);

        yield return new WaitForSeconds(blackScreenDuration);

        yield return screenFader.FadeOut();

        transitionText.text = "Imagine this is the main menu";
        transitionText.gameObject.SetActive(true);

        player.SetInputEnabled(InputMode.ENABLED);
    }

    private IEnumerator StartChoiceTimer()
    {
        choiceTimerStarted = true;

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

        StartCoroutine(GameOver("The killer saw you accusing other suspects. He killed you."));
    }

    private IEnumerator CallPolice()
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

        yield return new WaitForSeconds(1f);

        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(callPoliceKey) && choiceTimerStarted)
        {
            StartCoroutine(CallPolice());
        }

        ////////////////// DEBUG ///////////////////
        if (Input.GetKeyDown(KeyCode.B) &&
            currentDay == -1)
        {
            StartCoroutine(StartDay(0));
        }

        if (Input.GetKeyDown(KeyCode.N) &&
            currentDay == 0)
        {
            StartCoroutine(StartDay(1));
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