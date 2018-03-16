using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData {
	private static int timeModeLevel = 0, despawnCount = 0, spawnCount = 0, despawnCountTotal = 0, spawnCountTotal = 0;

	public static int TimeModeLevel {
		get {
			return timeModeLevel;
		}
		set {
			timeModeLevel = value;
		}
	}

	public static int DespawnCount {
		get {
			return despawnCount;
		}

		set {
			despawnCount = value;
		}
	}
	public static int SpawnCount {
		get {
			return spawnCount;
		}

		set {
			spawnCount = value;
		}
	}

	public static int DespawnCountTotal {
		get {
			return despawnCountTotal;
		}
	}

	public static int SpawnCountTotal {
		get {
			return spawnCountTotal;
		}
	}

	public static void UpdateTotals() {
		despawnCountTotal += despawnCount;
		spawnCountTotal += spawnCount;
	}

	public static void ResetRunCounts() {
		spawnCount = 0;
		despawnCount = 0;
	}
	
}
