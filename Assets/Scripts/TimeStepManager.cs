using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStepManager {

	public static List<TimeStepObject> stepObjects = new List<TimeStepObject>();

	public static void RegisterTimeStepObject(TimeStepObject obj){
		stepObjects.Add(obj);
		Debug.Log("Registered " + obj);
	}

	public static void UnregisterTimeStepObject(TimeStepObject obj){
		stepObjects.Remove(obj);
	}

	public static void AdvanceTimeStep(){
		List<int> removedIndices = new List<int>();
		for(int i = 0; i < stepObjects.Count; i++){
			
			if(stepObjects[i] == null){
				removedIndices.Add(i);
				continue;
			}
			stepObjects[i].Advance();
		}
		for(int i = removedIndices.Count - 1; i >= 0; i--){
			stepObjects.RemoveAt(removedIndices[i]);
		}
		TiledGameMap.Instance.ExecutePhysicsStep();
		TiledGameMap.Instance.ClearUpGameBoard();
	}
}
