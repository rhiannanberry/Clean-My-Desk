using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveData {
    public int timeModeLevel = 0, despawnCount = 0, spawnCount = 0, despawnCountTotal = 0, spawnCountTotal = 0, maxVelocity = 0, medsTaken = 0, timesGotDistracted = 0;
	public float master = 0.5f, music = 0.5f, sfx = 0.5f;
	public static SaveData sd;

	public SaveData() {
		timeModeLevel = despawnCount = spawnCount = despawnCountTotal = spawnCountTotal = maxVelocity = medsTaken = timesGotDistracted = 0;
		master = music = sfx = 0.5f;
	}

    public static void UpdateTotals() {
		sd.despawnCountTotal += sd.despawnCount;
		sd.spawnCountTotal += sd.spawnCount;
	}

	public static void ResetRunCounts() {
		sd.despawnCount = sd.spawnCount = sd.despawnCountTotal = sd.spawnCountTotal = sd.maxVelocity = sd.medsTaken = sd.timesGotDistracted = 0;
	}
}

public static class SaveLoad {
	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/saveDat.gd");
		bf.Serialize(file, SaveData.sd);
		file.Close();
	}

	public static void Load() {
		Debug.Log(SaveData.sd);
		if(File.Exists(Application.persistentDataPath + "/saveDat.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/saveDat.gd", FileMode.Open);
			SaveData.sd = (SaveData)bf.Deserialize(file);
			file.Close();
		} else {
			SaveData.sd = new SaveData();
		}
	}

}