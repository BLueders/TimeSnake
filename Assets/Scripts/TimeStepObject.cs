using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeStepObject : MonoBehaviour {

	public abstract void Advance();

	protected void Register(){
		TimeStepManager.RegisterTimeStepObject(this);
	}

	protected void UnRegister(){
		TimeStepManager.UnregisterTimeStepObject(this);
	}

	protected virtual void OnDestroy()
	{
		UnRegister();
	}
}
