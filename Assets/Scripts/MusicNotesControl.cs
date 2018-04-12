using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNotesControl : MonoBehaviour {

	private ParticleSystem[] psyss;
	private AudioManager manager;
	// Use this for initialization
	void Start () {
		psyss = GetComponentsInChildren<ParticleSystem>();
		manager = GameController.Instance.GetComponent<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (manager.currentSong.source.isPlaying) {
			if( !psyss[0].gameObject.activeSelf) {
				foreach(ParticleSystem psys in psyss) {
					psys.gameObject.SetActive(true);
				}
			}
		} else {
			if (psyss[0].gameObject.activeSelf) {
				foreach(ParticleSystem psys in psyss) {
					psys.gameObject.SetActive(false);
				}
			}
		}
	}
}
