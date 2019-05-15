using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CameraInitialMovement : MonoBehaviour {

    public TimelineAsset ToAliens;
    public TimelineAsset ToURSS;
    public TimelineAsset ToUSA;

    public AudioClip USATheme;
    public AudioClip URSSTheme;
    public AudioClip AliensTheme;

    PlayableDirector pDirector;
    CamControl cc;
    AudioSource aSource;

    void Start()
    {
        pDirector = GetComponent<PlayableDirector>();
        cc = GetComponent<CamControl>();
        aSource = GetComponent<AudioSource>();
    }

    public void MoveToAliens()
    {
        pDirector.playableAsset = ToAliens;
        pDirector.Play();
        cc.enabled = true;
        aSource.clip = AliensTheme;
        aSource.Play();
    }

    public void MoveToURSS()
    {
        pDirector.playableAsset = ToURSS;
        pDirector.Play();
        cc.enabled = true;
        aSource.clip = URSSTheme;
        aSource.Play();
    }

    public void MoveToUSA()
    {
        pDirector.playableAsset = ToUSA;
        pDirector.Play();
        cc.enabled = true;
        aSource.clip = USATheme;
        aSource.Play();
    }
}
