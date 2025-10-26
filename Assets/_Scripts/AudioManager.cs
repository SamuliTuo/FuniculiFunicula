using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<SoundClip> SoundClips;
    [Space(20)]
    public List<AudioSource> freeSources = new List<AudioSource>();

    [SerializeField] private List<AudioSource> audioSourcesInUse = new List<AudioSource>();


    public void PlayClip(string clipName)
    {
        var source = GetFreeSource();
        SoundClip data = null;
        for (int i = 0; i < SoundClips.Count; i++)
        {
            if (SoundClips[i].clipName == clipName)
            {
                data = SoundClips[i];
                break;
            }
        }
        if (data == null)
        {
            print("Couldn't find audiodata for : " + clipName);
            return;
        }
        source.clip = data.clip[Random.Range(0, data.clip.Count)];
        source.volume = 1 + data.volumeTweak;
        source.Play();
        StartCoroutine(ReturnSourceToPoolAfterDelay(4, source));
    }

    AudioSource GetFreeSource()
    {
        if (freeSources.Count == 0)
        {
            print("No free audio sources.");
            return null;
        }
        var source = freeSources[0];
        freeSources.RemoveAt(0);
        audioSourcesInUse.Add(source);
        return source;
    }
    IEnumerator ReturnSourceToPoolAfterDelay(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        ReturnSourceToFreeList(source);
    }
    void ReturnSourceToFreeList(AudioSource source)
    {
        audioSourcesInUse.Remove(source);
        freeSources.Add(source);
    }
}

[System.Serializable]
public class SoundClip
{
    public string clipName;
    public List<AudioClip> clip;
    public bool randomize;
    public float volumeTweak;
    public SoundClip(string clipName, List<AudioClip> clip, bool randomize, float volumeTweak)
    {
        this.clipName = clipName;
        this.clip = clip;
        this.randomize = randomize;
        this.volumeTweak = volumeTweak;
    }
}
