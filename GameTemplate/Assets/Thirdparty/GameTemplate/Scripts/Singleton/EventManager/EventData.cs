using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventData {
	private string EventIdKey = "EventId";
	public Dictionary<string, object> Data;

	public EventData(string eventId) {
		Data = new Dictionary<string, object>();
		Data[EventIdKey] = eventId;
	}

	public object this[string key] {
		get {
			return Data[key];
		}
		set {
			Data[key] = value;
		}
	}

	public void Log() {
		string logStr = "Event: " + Data[EventIdKey] + '(';
		int keyIndex = 0;
		foreach (var kvp in Data) {
			keyIndex++;
			if (kvp.Key != EventIdKey) {
				logStr += kvp.Key + ":" + kvp.Value;
				if (keyIndex < Data.Count) 
					logStr += " ";
			}
		}
		logStr += ")";
		Debug.Log(logStr);
	}
}
