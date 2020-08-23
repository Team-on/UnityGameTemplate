using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {

	//Scene Loader
	public static event EventController.MethodContainer OnSceneNeedLoad;
	public void CallOnSceneNeedLoad(EventData ob = null) => OnSceneNeedLoad?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadStart;
	public void CallOnSceneLoadStart(EventData ob = null) => OnSceneLoadStart?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadEnd;
	public void CallOnSceneLoadEnd(EventData ob = null) => OnSceneLoadEnd?.Invoke(ob);

	//Settings
	public static event EventController.MethodContainer OnScreenResolutionChange;
	public void CallOnScreenResolutionChange(EventData ob = null) => OnScreenResolutionChange?.Invoke(ob);
}
