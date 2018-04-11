using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {
	private Slider masterSlider, musicSlider, sfxSlider;
	// Use this for initialization
	void Start () {
		masterSlider = transform.Find("Panel/AudioPanel/MasterAudio/Slider").GetComponent<Slider>();
		musicSlider = transform.Find("Panel/AudioPanel/MusicAudio/Slider").GetComponent<Slider>();
		sfxSlider = transform.Find("Panel/AudioPanel/SFXAudio/Slider").GetComponent<Slider>();
		masterSlider.value = GameController.Instance.masterVolume;
		musicSlider.value = GameController.Instance.musicVolume;
		sfxSlider.value = GameController.Instance.sfxVolume;
	}

	public void UpdateVolume() {
		GameController.Instance.masterVolume = masterSlider.value;
		GameController.Instance.musicVolume = musicSlider.value;
		GameController.Instance.sfxVolume = sfxSlider.value;
		GameController.Instance.audioManager.UpdateVolume();
	}	
}
