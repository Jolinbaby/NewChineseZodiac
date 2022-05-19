using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public AudioSource audioSource;


    [Header("“Ù–ß")]
    [SerializeField]
    private AudioClip jumpAudio;
    [SerializeField]
    private AudioClip throwAudio;
    [SerializeField]
    private AudioClip boomAudio;
    [SerializeField]
    private AudioClip dizzyAudio;
    [SerializeField]
    private AudioClip PickUpAudio;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnPickUpAudio()
    {
        audioSource.clip = PickUpAudio;
        audioSource.Play();
    }

    public void OnJumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void OnThrowAudio()
    {
        audioSource.clip = throwAudio;
        audioSource.Play();
    }

    public void OnBoomAudio()
    {
        audioSource.clip = boomAudio;
        audioSource.Play();
    }

    public void OnDizzyAudio()
    {
        audioSource.clip = dizzyAudio;
        audioSource.Play();
    }
}
