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
		int tot = 0;
		for (int i = 0; i < length; i ++) {
			GameObject scr = Instantiate(scoreItemPrefab);
			scr.transform.SetParent(transform);
			scr.transform.localScale = new Vector3(1,1,1);
			string str = scoreDesc[Random.Range(0,scoreDesc.Count)];
			int scoreNum = Random.Range(1,9999);
			scr.GetComponentInChildren<Text>().text = str + ": " + scoreNum.ToString();
			scoreDesc.Remove(str);
			tot += scoreNum;	
		}
			GameObject totscr = Instantiate(scoreItemPrefab);
			totscr.transform.SetParent(transform);
			totscr.transform.localScale = new Vector3(1,1,1);
			totscr.GetComponentInChildren<Text>().text = "Total:   " + tot.ToString();
	}
}
