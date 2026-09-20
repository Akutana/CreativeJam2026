using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private FPSController player;
    [SerializeField] private TextMeshProUGUI transitionText;
    [SerializeField] private TextMeshProUGUI narrationText;

    [SerializeField] private float blackScreenDuration = 2f;
    [SerializeField] private float transitionDuration = 3f;

    int currentDay = -1;

    private void Start()
    {
        transitionText.gameObject.SetActive(false);
        narrationText.gameObject.SetActive(false);

        screenFader.SetClear();

        Debug.Log(currentDay);
    }

    private IEnumerator StartDay(int day)
    {
        currentDay = day;

        player.SetInputEnabled(false);

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

        player.SetInputEnabled(true);
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

    private IEnumerator GameOver()
    {
        Debug.Log("Resetting day 1");

        player.SetInputEnabled(false);

        yield return screenFader.FadeIn();

        // do reset stuff

        yield return new WaitForSeconds(blackScreenDuration);

        transitionText.text = "Game Over";
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

        player.SetInputEnabled(true);
    }

    private IEnumerator Win()
    {
        Debug.Log("Win");

        player.SetInputEnabled(false);

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

        player.SetInputEnabled(true);
    }

    private void Update()
    {
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
            StartCoroutine(GameOver());
        }

        if (Input.GetKeyDown(KeyCode.Y) &&
            currentDay == 1)
        {
            StartCoroutine(Win());
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