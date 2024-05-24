using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> audioClips;

    public void PlayAudio(int index)
    {
        index = Mathf.Clamp(index, 0, audioClips.Count - 1);
        source.PlayOneShot(audioClips[index]);
    }

    public void PlayRandomAudio()
    {
        int index = Random.Range(0, audioClips.Count);
        source.PlayOneShot(audioClips[index]);
    }
}

