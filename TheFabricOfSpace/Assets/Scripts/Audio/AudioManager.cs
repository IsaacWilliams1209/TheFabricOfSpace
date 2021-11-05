using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioType
{
    MainAudio,
    WaterAmbience,
    SpaceAmbience,
    ButtonSelect,
    ButtonHover,
    SheepWalking
}

public class AudioManager : Singleton<AudioManager>
{

    List<AudioController> audioList;
    AudioController desiredAudio = null;
    public AudioController lastActiveAudio = null;
    public bool stopAudio = false;

    protected override void Awake()
    {
        base.Awake();
        audioList = GetComponentsInChildren<AudioController>().ToList();
        audioList.ForEach(x => x.gameObject.SetActive(false));
        PlayAudio(AudioType.MainAudio);
    }

    public void PlayAudio(AudioType audioToPlay)
    {
        desiredAudio = audioList.Find(x => x.audioType == audioToPlay);

        if (desiredAudio != null)
        {
            desiredAudio.gameObject.SetActive(true);
            desiredAudio.gameObject.GetComponent<AudioSource>().Play();
            lastActiveAudio = desiredAudio;
            desiredAudio = null;
        }
        else { Debug.LogWarning("Could not find the desired audio"); }
    }


    public void StopDesiredAudio(AudioType audioToStop)
    {
        lastActiveAudio = audioList.Find(x => x.audioType == audioToStop);

        if (lastActiveAudio != null) {
            lastActiveAudio.gameObject.GetComponent<AudioSource>().Stop();
            lastActiveAudio = null;
        }

    }


}
