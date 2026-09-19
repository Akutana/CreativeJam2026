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
        currentDay = 0;
        Debug.Log("Starting day 0");

        screenFader.SetBlack();

        player.SetInputEnabled(false);

        StartCoroutine(StartGame());
    }

    private void StartDay1()
    {
        currentDay = 1;
        Debug.Log("Starting day 1");
    }

    private IEnumerator StartGame()
    {
        // Black screen
        yield return new WaitForSeconds(blackScreenDuration);

        // Black fade out
        yield return screenFader.FadeOut();

        // Player can now move
        player.SetInputEnabled(true);
    }

    private void Update()
    {
        ////////////////// DEBUG ///////////////////
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartDay1();
        }
        ////////////////////////////////////////////
    }
}