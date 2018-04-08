using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GenerateScores : MonoBehaviour {
	public enum ScoreType {spawn, despawn, diff, percentSpawnTotal, maxVelocity, timeOfDay, rememberYourMeds, timesGotDistracted, pureRandom};

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

    //Edit this one only!!
	public void SpawnScoreItemsSeeded() {
		int tot = 0;
		GameObject.Find("EndMenu").transform.Find("MainModeUI").gameObject.SetActive(true);
		SaveData.UpdateTotals();
		int spawnScore = SaveData.SpawnCount;
		int despawnScore = SaveData.DespawnCount;
		int diffScore = (int) (Mathf.Log(despawnScore - spawnScore, 2) * 10);
		int percentSpawnTotalScore = (spawnScore == 0 || SaveData.SpawnCountTotal == 0) ? 0 : (int) (100.0f * (spawnScore*1.0f / SaveData.SpawnCountTotal));
        int maxVelocityScore = SaveData.MaxVelocity;
        int timeOfDay = System.DateTime.Now.Hour;
        int timeOfDayScore = (timeOfDay < 12) ? (23 - timeOfDay) * 2 : timeOfDay * 2;//The later you're up playing, the more you're rewarded :p
        int rememberYourMedsScore = SaveData.MedsTaken * 2;
        int timesGotDistracted = -SaveData.TimesGotDistracted;
        int pureRandomScore = (int) (Random.value * 100);

		int[] scorePatterns = {spawnScore, despawnScore, diffScore, percentSpawnTotalScore, maxVelocityScore, timeOfDayScore, pureRandomScore};

		foreach (ScoreTypeDesc pair in scoreMappings) {
			string desc;
			int scr;
			switch(pair.type) {
				case ScoreType.despawn: scr = despawnScore;
				break;
				case ScoreType.diff: scr = diffScore;
				break;
				case ScoreType.percentSpawnTotal: scr = percentSpawnTotalScore;
				break;
				case ScoreType.spawn: scr = spawnScore;
				break;
                case ScoreType.maxVelocity: scr = maxVelocityScore;
                break;
                case ScoreType.timeOfDay: scr = timeOfDayScore;
                break;
                case ScoreType.rememberYourMeds: scr = rememberYourMedsScore;
                break;
                case ScoreType.timesGotDistracted: scr = timesGotDistracted;
                break;
                case ScoreType.pureRandom: scr = pureRandomScore;
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
    private void OnApplicationFocus(bool focus) {
        if (!focus) {
            SaveData.TimesGotDistracted++;
        }
    }
}
