using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private FPSController player;

    [SerializeField] private float blackScreenDuration = 2f;
    int currentDay;

    private void Start()
    {
        StartCoroutine(StartDay(0));
    }

    private IEnumerator StartDay(int day)
    {
        currentDay = day;

        player.SetInputEnabled(false);

        yield return screenFader.FadeIn();

        switch (day)
        {
            case 0:
                yield return StartDay0();
                break;

            case 1:
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

        yield return null;
    }

    private IEnumerator StartDay1()
    {
        Debug.Log("Starting day 1");

        yield return null;
    }

    private IEnumerator GameOver()
    {
        Debug.Log("Resetting day 1");

        player.SetInputEnabled(false);

        yield return screenFader.FadeIn();

        // do reset stuff

        yield return StartDay1();

        yield return new WaitForSeconds(blackScreenDuration);

        yield return screenFader.FadeOut();

        player.SetInputEnabled(true);
    }

    private void Update()
    {
        ////////////////// DEBUG ///////////////////
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(StartDay(1));
        }

        if (Input.GetKeyDown(KeyCode.R) &&
            currentDay == 1)
        {
            StartCoroutine(GameOver());
        }
        ////////////////////////////////////////////
    }
}