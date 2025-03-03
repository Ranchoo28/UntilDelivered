using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generic audio management class
 */
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private List<AudioClip> audioClips;

    void Start()
    {
        Debug.Log("Starting audio manager for " + gameObject.name);
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Play audio clip at index
     * @param index Index of audio clip
     */
    public void Play(int index){
        if (audioClips.Count > index)
        {
            Debug.Log("Playing audio clip " + audioClips[index].name + " at "+ index + " on " + gameObject.name);
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
            Debug.LogError("Audio clip index out of bounds on " + gameObject.name);
    }

    /**
     * Stop audio playback
     */
    public void Stop(){
        audioSource.Stop();
    }

    /**
     * Pause audio playback
     */
    public void Pause(){
        audioSource.Pause();
    }

    /**
     * Unpause audio playback
     */
    public void Unpause(){
        audioSource.UnPause();
    }

    /**
     * Set audio volume
     * @param volume Volume level
     */
    public void SetVolume(float volume){
        audioSource.volume = volume;
    }

    /**
     * Check if audio is playing
     * @return True if audio is playing
     */
    public bool isPlaying(){
        return audioSource.isPlaying;
    }


    /**
     * Set audio loop
     * @param loop True to loop audio
     */
    public void setLoop(bool loop){
        audioSource.loop = loop;
    }

    /**
     * Load audio asset from resources folder
     * @param path Path to audio asset
     */
    public void LoadAudioAsset(string path){
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null)
            audioClips.Add(clip);
        else
            Debug.LogError("Audio clip not found at " + path);
    }
}
