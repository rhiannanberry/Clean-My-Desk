using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
		List<string> listHold = new List<string>();
		float parentHeight = GetComponent<RectTransform>().rect.height;
		float childHeight = scoreItemPrefab.GetComponent<RectTransform>().rect.height;
		int length = (int) Mathf.Floor(parentHeight / childHeight);
		for (int i = 0; i < length; i ++) {
			GameObject scr = Instantiate(scoreItemPrefab);
			scr.transform.SetParent(transform);
			scr.transform.localScale = new Vector3(1,1,1);
			string str = scoreDesc[Random.Range(0,scoreDesc.Count)];
			int scoreNum = Random.Range(1,9999);
			scr.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
			scr.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreNum.ToString();
			listHold.Add(str);
			scoreDesc.Remove(str);
			tot += scoreNum;	
		}
			scoreDesc.AddRange(listHold);
			GameObject totscr = Instantiate(scoreItemPrefab);
			totscr.transform.SetParent(transform);
			totscr.transform.localScale = new Vector3(1,1,1);
			totscr.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total";
			totscr.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tot.ToString();
	}
}
