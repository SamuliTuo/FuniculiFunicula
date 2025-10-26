using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public List<AudioSource> freeSources = new List<AudioSource>();

    [SerializeField] private List<AudioSource> audioSourcesInUse = new List<AudioSource>();


    public void PlayClip(string clipName)
    {
        var source = GetFreeSource();
        AudioClip clip;
        clips.TryGetValue(clipName, out clip);
        if (clip == null)
        {
            print("Couldn't find clip: " + clipName);
            return;
        }
        source.clip = clip;
        source.Play();
        ReturnSourceToPoolAfterDelay(10, source);
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
