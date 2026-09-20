using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicJukebox : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioClip> musicTracks = new();

    [Header("Record Change")]
    [SerializeField] private AudioSource recordSource;
    [SerializeField] private AudioClip recordChangeSound;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float musicVolume = 0.65f;

    [SerializeField, Range(0f, 1f)]
    private float recordVolume = 0.35f;

    [Header("Fade")]
    [SerializeField]
    private float fadeOutDuration = 0.8f;

    [SerializeField]
    private float fadeInDuration = 1.0f;

    [Header("Record Change Timing")]
    [SerializeField]
    private float recordChangeDelay = 0.12f;

    private int lastTrack = -1;
    private Coroutine jukeboxRoutine;

    private void Start()
    {
        SetupSources();

        if (musicTracks.Count > 0)
            jukeboxRoutine = StartCoroutine(JukeboxLoop());
    }

    private void SetupSources()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (musicSource != null)
        {
            musicSource.loop = false;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
        }

        if (recordSource != null)
        {
            recordSource.loop = false;
            recordSource.playOnAwake = false;
            recordSource.volume = recordVolume;
        }
    }

    private IEnumerator JukeboxLoop()
    {
        
        while (true)
        {
            AudioClip nextTrack = GetRandomTrack();

            if (nextTrack == null)
                yield break;

            
            musicSource.clip = nextTrack;
            musicSource.volume = 0f;
            musicSource.Play();

        
            yield return FadeVolume(
                musicSource,
                0f,
                musicVolume,
                fadeInDuration
            );

           
            float waitTime =
                nextTrack.length - fadeOutDuration;

            if (waitTime > 0f)
                yield return new WaitForSeconds(waitTime);

          
            yield return FadeVolume(
                musicSource,
                musicSource.volume,
                0f,
                fadeOutDuration
            );

            musicSource.Stop();

            
            PlayRecordChange();

            if (recordChangeDelay > 0f)
                yield return new WaitForSeconds(recordChangeDelay);
        }
    }

    private AudioClip GetRandomTrack()
    {
        if (musicTracks.Count == 0)
            return null;

        if (musicTracks.Count == 1)
        {
            lastTrack = 0;
            return musicTracks[0];
        }

        int index;

        do
        {
            index = Random.Range(0, musicTracks.Count);
        }
        while (index == lastTrack);

        lastTrack = index;

        return musicTracks[index];
    }

    private void PlayRecordChange()
    {
        if (recordSource == null || recordChangeSound == null)
            return;

        recordSource.PlayOneShot(
            recordChangeSound,
            recordVolume
        );
    }

    private IEnumerator FadeVolume(
        AudioSource source,
        float from,
        float to,
        float duration
    )
    {
        if (duration <= 0f)
        {
            source.volume = to;
            yield break;
        }

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);


            t = t * t * (3f - 2f * t);

            source.volume = Mathf.Lerp(from, to, t);

            yield return null;
        }

        source.volume = to;
    }

    private void OnDestroy()
    {
        if (jukeboxRoutine != null)
            StopCoroutine(jukeboxRoutine);
    }
}