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

        yield return FadeTransition();

        Debug.Log("Starting day" + day);
    }

    private IEnumerator FadeTransition()
    {
        player.SetInputEnabled(false);

        yield return screenFader.FadeIn();

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
        ////////////////////////////////////////////
    }
}