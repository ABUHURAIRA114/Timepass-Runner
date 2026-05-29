using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager i;

    [SerializeField] private List<AudioClip> backgroundSounds;
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource jumpSource;

    [SerializeField] private float volume;

    void Awake() {
        
        if (i == null)
            i = this;
    }

    private void Start() {

        backgroundSource.Play();
        SceneManager.activeSceneChanged += SetBackgroundAudio;
    }

    private void Update() {

        SetVolume();
        AudioToggle();
    }

    public void AudioToggle() {

        if(GameManager.instance.GetState() == GameState.Paused) 
            backgroundSource.Pause();
        else
            if (GameManager.instance.unPaused)
                backgroundSource.UnPause();
        
        if (GameManager.instance.GetState() == GameState.Restarting)
            backgroundSource.Stop();
    }

    private void SetVolume() {

        backgroundSource.volume = volume;
        jumpSource.volume = volume;
        hitSource.volume = volume;
    }

    private void SetBackgroundAudio(Scene arg0, Scene arg1) {

        backgroundSource.resource = backgroundSounds[arg1.buildIndex];
        backgroundSource.Play();
    }

    public void PlayJump() {

        jumpSource.Play();
    }

    public void PlayHit() {

        hitSource.Play();
    }

    public void SetVolume(float v) {

        volume = v;
    }

    public float Volume {

        get { return volume; }
    }
}
