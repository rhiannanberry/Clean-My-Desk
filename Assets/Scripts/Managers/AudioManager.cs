using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

public class AudioManager : MonoBehaviour {

	public Song[] songs;

	[HideInInspector]
	public int currentSongIndex;

	[HideInInspector]
	public Song currentSong;

	void Awake() {
		for (int i = 0; i < songs.Length; i++) {
			if (i == 0) {
				songs[i].prev = songs[songs.Length - 1];
			} else {
				songs[i].prev = songs[i - 1];
			}
			if (i == songs.Length - 1) {
				songs[i].next = songs[0];
			} else {
				songs[i].next = songs[i + 1];
			}
		}
		if (songs.Length > 0) {
			currentSong = songs[0];
			songs[0].source = gameObject.AddComponent<AudioSource>();
			UpdateSongAudioSource();
		}
	}
	// Use this for initialization
	void Start () {
		Play("Turbo Giant");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play (string name) {
		Song s = Array.Find(songs, song => song.name == name);
		s.source.Play();
	}

	public Song NextSong() {
		currentSong.source.Stop();
		currentSong.source = null;
		currentSong = currentSong.next;
		UpdateSongAudioSource();
		PlaySong();
		return currentSong;
	}

	public Song PrevSong() {
		currentSong.source.Stop();
		currentSong.source = null;
		currentSong = currentSong.prev;
		UpdateSongAudioSource();
		PlaySong();
		return currentSong;
	}

	public void PlaySong() {
		currentSong.source.Play();
	}

	public void PauseSong() {
		currentSong.source.Pause();
	}
//multiply song audio but music audio slider when that gets set
	private void UpdateSongAudioSource() {
		currentSong.source = gameObject.GetComponent<AudioSource>();
		currentSong.source.clip = currentSong.clip;
		currentSong.source.volume = currentSong.volume;
		currentSong.source.pitch = currentSong.pitch;
		currentSong.source.name = currentSong.name;
	}
}
