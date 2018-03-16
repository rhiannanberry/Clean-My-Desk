using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GenerateScores : MonoBehaviour {
	public enum ScoreType {spawn, despawn, diff, percentSpawnTotal};

	[System.Serializable]
	public struct ScoreTypeDesc {
		public ScoreType type;
		public List<string> descs;
	}
	public GameObject scoreItemPrefab;
	public List<ScoreTypeDesc> scoreMappings;
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

	public void SpawnScoreItemsSeeded() {
		int tot = 0;
		GameObject.Find("TimeModeUI").SetActive(false);
		SaveData.UpdateTotals();
		int spawnScore = SaveData.SpawnCount;
		int despawnScore = SaveData.DespawnCount;
		int diffScore = (despawnScore - spawnScore) * 100;
		int percentSpawnTotalScore = (spawnScore == 0 || SaveData.SpawnCountTotal == 0) ? 0 : (int) (100.0f * (spawnScore*1.0f / SaveData.SpawnCountTotal));

		int[] scorePatterns = {spawnScore, despawnScore, diffScore, percentSpawnTotalScore};

		foreach (ScoreTypeDesc pair in scoreMappings) {
			string desc;
			int scr;
			switch(pair.type) {
				case ScoreType.despawn: scr = spawnScore;
				break;
				case ScoreType.diff: scr = diffScore;
				break;
				case ScoreType.percentSpawnTotal: scr = percentSpawnTotalScore;
				break;
				case ScoreType.spawn: scr = spawnScore;
				break;
				default: scr = 0;
					pair.descs.Add("You broke it!!");
				break;
			}
				desc = pair.descs[Random.Range(0, pair.descs.Count)];

				GameObject scrObj = Instantiate(scoreItemPrefab);
				scrObj.transform.SetParent(transform);
				scrObj.transform.localScale = new Vector3(1,1,1);
				scrObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
				scrObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scr.ToString();
				tot += scr;
		}
			SaveData.ResetRunCounts();
			GameObject totscr = Instantiate(scoreItemPrefab);
			totscr.transform.SetParent(transform);
			totscr.transform.localScale = new Vector3(1,1,1);
			totscr.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total";
			totscr.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tot.ToString();

	}
}
