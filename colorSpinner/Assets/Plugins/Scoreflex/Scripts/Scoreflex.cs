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

	public delegate void Callback(bool success, Dictionary<string,object> response);

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

	private string PendingSoloPlayRequest = null;
	private Dictionary<string,object> PendingChallengeRequest = null;

	void CallPlaySoloHandlers(string s)
	{
		PendingSoloPlayRequest = s;
	}

	void CallChallengeHandlers(Dictionary<string,object> dict)
	{
		PendingChallengeRequest = dict;
	}

	void Update()
	{
		if(PendingSoloPlayRequest != null)
		{
			PlaySoloHandlers(PendingSoloPlayRequest);
			PendingSoloPlayRequest = null;
		}

		if(PendingChallengeRequest != null)
		{
			ChallengeHandlers(PendingChallengeRequest);
			PendingChallengeRequest = null;
		}

		PerformPendingCallbacks();
	}
	
	class PendingCallback
	{
		public Callback method;
		public bool success;
		public Dictionary<string,object> dictionary;
	}

	private readonly Queue<PendingCallback> PendingCallbacks = new Queue<PendingCallback>();
	
	void PerformPendingCallbacks()
	{
		lock(PendingCallbacks)
		{
			while(PendingCallbacks.Count > 0)
			{
				var pendingCallback = PendingCallbacks.Dequeue();
				pendingCallback.method(pendingCallback.success, pendingCallback.dictionary);
			}
		}
	}

	void EnqueueCallback(Callback callback, bool success, Dictionary<string,object> dictionary)
	{
		var pendingCallback = new PendingCallback {
			method = callback,
			success = success,
			dictionary = dictionary
		};
		
		lock(PendingCallbacks)
		{
			PendingCallbacks.Enqueue(pendingCallback);
		}
	}
}


































