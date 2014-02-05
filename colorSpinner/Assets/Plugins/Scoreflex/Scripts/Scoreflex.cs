using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

public partial class Scoreflex : MonoBehaviour
{
	public enum Gravity { Bottom = 0, Top = 1 };

	public class View
	{
		public readonly int handle;

		public View(int _handle)
		{
			handle = _handle;
		}

		public void Close()
		{
			#if UNITY_IPHONE
			Scoreflex.scoreflexHidePanelView(handle);
			#elif UNITY_ANDROID
			if(Scoreflex.Instance != null) Scoreflex.Instance.HidePanelView(handle);
			#endif
		}
	}

	public static Scoreflex Instance { get; private set; }

	public string ClientId;
	public string ClientSecret;
	public bool Sandbox;

	private bool initialized = false;

	public bool Live {
		get {
			return initialized;
		}
	}

	private const string ErrorNotLive = "Scoreflex: Method called while not live.";
	
	public System.Action<string> PlaySoloHandlers = null;
	public System.Action<Dictionary<string,object>> ChallengeHandlers = null;

}


































