using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("ServiceLocator")]

public class AudioManager 
{
    public enum Clips { 
        BUTTONCLICK = 0,
        LASERSHOOT = 1,
        COLLISION = 2,
        PICKUP = 3,
        JETENGINES = 4
    }

    class AudioObject
    {
        public AudioClip clip;
        public float volume;

        internal AudioObject(AudioClip c, float vol)
        {
            clip = c;
            volume = vol;
        }
    }

    Dictionary<Clips, AudioObject> clips;
    Queue<AudioObject> queue;

    GameObject audioGO;

    // Pooling
    AudioSource[] sources;
    const int MAX_AUDIO = 4;
    int nextIdxAvailable;

    internal AudioManager()
    {
        clips = new Dictionary<Clips, AudioObject>();
        // queue of events
        queue = new Queue<AudioObject>();
        // 4 is the maximum sounds played at one frame
        sources = new AudioSource[MAX_AUDIO];
        // Current free space
        nextIdxAvailable = 0;
        // game object that holds AudioSource components
        audioGO = new GameObject();
        audioGO.name = "AudioManageGameObject";
        GameObject.DontDestroyOnLoad(audioGO);
    }

    public void Init()
    {
        for (int i = 0; i < MAX_AUDIO; i++)
        {
            sources[i] = audioGO.AddComponent<AudioSource>();
            if (sources[i] == null)
                Debug.Log(i + " not inialized");    
        }
        // Loading audio clips
        clips.Add(Clips.BUTTONCLICK, new AudioObject(Resources.Load<AudioClip>("AudioClips/button_click"), 1.0f));
        clips.Add(Clips.LASERSHOOT, new AudioObject(Resources.Load<AudioClip>("AudioClips/laser_shoot"), 0.05f));
        clips.Add(Clips.COLLISION, new AudioObject(Resources.Load<AudioClip>("AudioClips/explosion"), 0.5f));
        clips.Add(Clips.PICKUP, new AudioObject(Resources.Load<AudioClip>("AudioClips/fragment_pickup"), 1.0f));
        clips.Add(Clips.JETENGINES, new AudioObject(Resources.Load<AudioClip>("AudioClips/jet_engine"), 0.25f));
    }

    public void Register(Clips clip)
    {
        AudioObject obj;
        if (clips.TryGetValue(clip, out obj))
        {
            queue.Enqueue(obj);
        }
        // if there is space to play a clip, call it.
        if (queue.Count < MAX_AUDIO)
            PlayNext();
    }

    public AudioClip GetClip(Clips clip)
    {
        AudioObject obj;
        if (clips.TryGetValue(clip, out obj))
            return obj.clip;

        return null;
    }

    public float GetClipLength(Clips clip)
    {
        AudioObject obj;
        if (clips.TryGetValue(clip, out obj))
            return obj.clip.length;

        return 0;
    }

    private void PlayNext()
    {
        // if quere is empty, do nothing
        if (queue.Count == 0) return;
        // if next idx is -1, every source is taken
        if (nextIdxAvailable == -1) return;

        AudioObject obj = queue.Dequeue();
        if (obj == null)
            Debug.Log("obj null");
        if (sources[nextIdxAvailable] == null)
            Debug.Log("Source " + nextIdxAvailable + " is null");

        sources[nextIdxAvailable].clip = obj.clip;
        sources[nextIdxAvailable].volume = obj.volume;
        sources[nextIdxAvailable].Play();
        // registering callback for ending play.
        ServiceLocator.Instance.StartCoroutine(DelayedCallback(obj.clip.length, AudioFinished, nextIdxAvailable));

        // find next position availabel
        for (int i = 0; i < MAX_AUDIO; i++)
            if (!sources[i].isPlaying)
            {
                nextIdxAvailable = i;
                return;
            }
        // flag for no position avaialable 
        nextIdxAvailable = -1;
    }

    private void AudioFinished(int idx)
    {
        // free this audiosource to be played.
        nextIdxAvailable = idx;
        // when an audio has finished, play another.
        PlayNext();
    }

    private IEnumerator DelayedCallback(float time, Action<int> callback, int idx) 
    {
        yield return new WaitForSeconds(time);
        callback(idx);
    }
}
