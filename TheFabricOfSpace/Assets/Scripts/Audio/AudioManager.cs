using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioType
{
    MainAudio,
    WaterAmbience,
    SpaceAmbience
}

public class AudioManager : Singleton<AudioManager>
{

    List<AudioController> audioSelection;
    AudioController desiredAudio;

    protected override void Awake()
    {
        base.Awake();
        audioSelection = GetComponentsInChildren<AudioController>().ToList();
        audioSelection.ForEach(x => x.gameObject.SetActive(false));
        PlayAudio(AudioType.MainAudio);
    }

    private void Start()
    {

    }

    public void PlayAudio(AudioType audioToPlay)
    {
        desiredAudio = audioSelection.Find(x => x.audioType == audioToPlay);

        if (desiredAudio != null)
        {
            desiredAudio.gameObject.SetActive(true);
            desiredAudio.gameObject.GetComponent<AudioSource>().Play();
        }
        else { Debug.LogWarning("Could not find the desired audio"); }
    }
}
