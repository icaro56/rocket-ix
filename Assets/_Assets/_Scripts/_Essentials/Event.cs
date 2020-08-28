using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

public class Event
{
	protected Action<GameObject> observers;

	public Event(){	}

	public void AddObserver(Action<GameObject> action)
	{
		observers += action;
	}

	public void RemoveObserver(Action<GameObject> action)
	{
		observers -= action;
	}

	public void Invoke(GameObject invoker)
	{
		if(observers != null)
			observers(invoker);
	}
}
