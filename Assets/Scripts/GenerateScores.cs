using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateScores : MonoBehaviour {
	public GameObject scoreItemPrefab;

	public List<string> scoreDesc;
	public int length = 3;

	private List<string> notListed;
	// Use this for initialization
	void Start () {
		notListed = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnScoreItems() {
		for (int i = 0; i < length; i ++) {
			GameObject scr = Instantiate(scoreItemPrefab);
			scr.transform.SetParent(transform);
			scr.transform.localScale = new Vector3(1,1,1);
			string str = scoreDesc[Random.Range(0,scoreDesc.Count)];
			scr.GetComponentInChildren<Text>().text = str + ": " + Random.Range(1,99999).ToString();
			scoreDesc.Remove(str);
		}
	}
}
