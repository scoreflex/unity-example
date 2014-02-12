using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

public partial class Scoreflex
{
	#if UNITY_ANDROID

	//These figures are derived from the Android SDK manual for android.view.Gravity.
	//http://developer.android.com/reference/android/view/Gravity.html
	private readonly Dictionary<Gravity,int> androidGravity = new Dictionary<Gravity,int>() {
		{ Gravity.Bottom, 80 },
		{ Gravity.Top, 48 }
	};

	private static AndroidJavaObject UnityActivity { get {
			var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
			return activity;
		} }

	private static AndroidJavaClass ScoreflexClass { get {
			return new AndroidJavaClass("com.scoreflex.Scoreflex");
		} }

	private static AndroidJavaClass Helper { get {
			return new AndroidJavaClass("com.scoreflex.unity3d.Helper");
		} }

	void Awake()
	{
		if(Instance == null)
		{
			try
			{
				var unityActivity = UnityActivity;

				ScoreflexClass.CallStatic("initialize", unityActivity, ClientId, ClientSecret, Sandbox);
				Helper.CallStatic("setupBroadcastReceivers", unityActivity);

				initialized = true;
				registerForPushNotification();
			}
			catch(System.Exception ex)
			{
				Debug.LogWarning("Failed to boot Scoreflex.");
				Debug.LogException(ex);
				initialized = false;
			}
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if(Instance != this)
		{
			GameObject.Destroy(gameObject);
		}
	}

	#region Internal queue to handler callbacks from Java runtime on the Unity main thread.
	private string lastObservedInstanceName = null;
	void Update()
	{
		string currentName = gameObject.name;
		if(currentName != lastObservedInstanceName)
		{
			Helper.CallStatic("setGameObjectName", currentName);

			lastObservedInstanceName = currentName;
		}
	}
	#endregion

	private static Dictionary<string,object> PullFiguresFromResponse(AndroidJavaObject response)
	{
		AndroidJavaObject mJson = response.Call<AndroidJavaObject>("getJSONObject");

		string jsonString = mJson.Call<string>("toString");
		var result = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string,object>;

		return result;
	}

	private static string GetScoreflexActivityConstant(string constantName)
	{
		AndroidJavaClass scoreflexActivityClass = new AndroidJavaClass("com.scoreflex.ScoreflexActivity");
		var constantID = AndroidJNI.GetStaticFieldID(scoreflexActivityClass.GetRawClass(), constantName, "Ljava/lang/String;");
		string constantValue = AndroidJNI.GetStaticStringField(scoreflexActivityClass.GetRawClass(), constantID);
		return constantValue;
	}

	private static AndroidJavaObject CreateScoreflexActivityIntent(string showWhat)
	{
		AndroidJavaClass scoreflexActivityClass = new AndroidJavaClass("com.scoreflex.ScoreflexActivity");

		var showWhatKeyID = AndroidJNI.GetStaticFieldID(scoreflexActivityClass.GetRawClass(), "INTENT_SHOW_EXTRA_KEY", "Ljava/lang/String;");
		string showWhatKey = AndroidJNI.GetStaticStringField(scoreflexActivityClass.GetRawClass(), showWhatKeyID);

		var showWhatID = AndroidJNI.GetStaticFieldID(scoreflexActivityClass.GetRawClass(), showWhat, "Ljava/lang/String;");
		string showWhatValue = AndroidJNI.GetStaticStringField(scoreflexActivityClass.GetRawClass(), showWhatID);
		AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", UnityActivity, scoreflexActivityClass);
		
		intent.Call<AndroidJavaObject>("putExtra", showWhatKey, showWhatValue);

		return intent;
	}

	private void AddParametersToIntentIfNotNull(AndroidJavaObject intent, Dictionary<string,object> _parameters)
	{
		if(_parameters != null)
		{
			AndroidJavaObject parameters = CreateRequestParamsFromDictionary(_parameters);
			string key = GetScoreflexActivityConstant("INTENT_EXTRA_REQUEST_PARAMS_KEY");
			intent.Call<AndroidJavaObject>("putExtra", key, parameters);
		}
	}

	private void AddFigureToIntentIfNotNull(AndroidJavaObject intent, string figure, string constantName)
	{
		if(figure != null)
		{
			string key = GetScoreflexActivityConstant(constantName);
			intent.Call<AndroidJavaObject>("putExtra", key, figure);
		}
	}

	private AndroidJavaObject CreateRequestParamsFromDictionary(Dictionary<string,object> source, long? score = null)
	{
		AndroidJavaClass mapAssist = new AndroidJavaClass("com.scoreflex.unity3d.MapAssist");

		AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
		if(source != null)
		{
			foreach(KeyValuePair<string,object> kvp in source)
			{
				var value = kvp.Value == null ? null : kvp.Value.ToString();
				mapAssist.CallStatic("put", map, kvp.Key, value);
			}
		}
		if(score.HasValue)
		{
			mapAssist.CallStatic("put", map, "score", score.Value.ToString());
		}
		AndroidJavaObject requestParams = new AndroidJavaObject("com.scoreflex.Scoreflex$RequestParams", map);
		return requestParams;
	}

	private void StartActivityWithIntent(AndroidJavaObject intent)
	{
		Helper.CallStatic("startActivityWithIntent", UnityActivity, intent);
	}

	public string _GetPlayerId()
	{
		return ScoreflexClass.CallStatic<string>("getPlayerId");
	}
	
	public float _GetPlayingTime()
	{
		long l = ScoreflexClass.CallStatic<long>("getPlayingSessionTime");
		return (float) l;
	}
	
	public void _ShowFullscreenView(string resource, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_FULLSCREEN_VIEW");
		AddFigureToIntentIfNotNull(intent, resource, "INTENT_EXTRA_FULLSCREEN_RESOURCE");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}

	//private readonly Dictionary<int,AndroidJavaObject> scoreflexViewByHandle = new Dictionary<int,AndroidJavaObject>();



	public void registerForPushNotification()
	{
		ScoreflexClass.CallStatic("registerForPushNotification", UnityActivity);
	}
	
	public View _ShowPanelView(string resource, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		throw new System.NotImplementedException();

		/*
		var droidParams = CreateRequestParamsFromDictionary(parameters);
#warning We probably cannot be passing this ref back; we need to refer to views with global refs
		AndroidJavaObject view = null;

		UnityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			var scoreflexClass = new AndroidJavaClass("com.scoreflex.Scoreflex");
			view = scoreflexClass.CallStatic<AndroidJavaObject>("showPanelView", UnityActivity, resource, droidParams, androidGravity[gravity]);
		}));

		const int timeout = 5000;
		int countdown = timeout;

		while(view == null && countdown > 0)
		{
			const int rest = 5;
			System.Threading.Thread.Sleep(rest);
			countdown -= rest;
		}

		if(view == null)
		{
			Debug.LogError("Scoreflex.ShowPanelView attempted to call up a panel view but request timed out.");
			return null;
		}
		else
		{
			int newHandle;
			do newHandle = Random.Range(1, int.MaxValue); while(scoreflexViewByHandle.ContainsKey(newHandle) == false);
			scoreflexViewByHandle[newHandle] = view;
			return new View(newHandle);
		}*/
	}

	private void _HidePanelView(int handle)
	{
		throw new System.NotImplementedException();
		/*
		if(scoreflexViewByHandle.ContainsKey(handle))
		{
			AndroidJavaObject view = scoreflexViewByHandle[handle];
			view.Call("close");
			scoreflexViewByHandle.Remove(handle);
		}*/
	}
	
	public void _SetDeviceToken(string deviceToken)
	{
		Debug.LogWarning("Scoreflex.setDeviceToken called on an Android device; will do nothing.");
	}
	
	public void _ShowDeveloperGames(string developerId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_DEVELOPER_GAMES");
		AddFigureToIntentIfNotNull(intent, developerId, "INTENT_EXTRA_DEVELOPER_PROFILE_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowDeveloperProfile(string developerId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_DEVELOPER_PROFILE");
		AddFigureToIntentIfNotNull(intent, developerId, "INTENT_EXTRA_DEVELOPER_PROFILE_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowGameDetails(string gameId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_GAME_DETAIL");
		AddFigureToIntentIfNotNull(intent, gameId, "INTENT_EXTRA_GAME_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowGamePlayers(string gameId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_GAME_PLAYERS");
		AddFigureToIntentIfNotNull(intent, gameId, "INTENT_EXTRA_GAME_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowLeaderboard(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_LEADERBOARD");
		Debug.Log("LeaderboardID: " + leaderboardId);
		AddFigureToIntentIfNotNull(intent, leaderboardId, "INTENT_EXTRA_LEADERBOARD_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowLeaderboardOverview(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_LEADERBOARD_OVERVIEW");
		AddFigureToIntentIfNotNull(intent, leaderboardId, "INTENT_EXTRA_LEADERBOARD_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerChallenges(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_CHALLENGES");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerFriends(string playerId = null, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_FRIENDS");
		AddFigureToIntentIfNotNull(intent, playerId, "INTENT_EXTRA_PLAYER_PROFILE_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerNewsFeed(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_NEWS_FEED");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}

		
	public void _ShowPlayerProfile(string playerId = null, Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_PROFILE");
		AddFigureToIntentIfNotNull(intent, playerId, "INTENT_EXTRA_PLAYER_PROFILE_ID");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerProfileEdit(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_PROFILE_EDIT");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerRating(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_RATING");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowPlayerSettings(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_PLAYER_SETTINGS");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}
	
	public void _ShowSearch(Dictionary<string,object> parameters = null)
	{
		AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_SEARCH");
		AddParametersToIntentIfNotNull(intent, parameters);
		StartActivityWithIntent(intent);
	}

	public void _ShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		var requestParams = CreateRequestParamsFromDictionary(parameters, score);
		Helper.CallStatic("showRanksPanel", UnityActivity, leaderboardId, androidGravity[gravity], requestParams, true);
	}
	
	public void _HideRanksPanel()
	{
		Helper.CallStatic("hideRanksPanel", UnityActivity);
	}
	
	public void _StartPlayingSession()
	{
		ScoreflexClass.CallStatic("startPlayingSession");
	}
	
	public void _StopPlayingSession()
	{
		ScoreflexClass.CallStatic("stopPlayingSession");
	}
	
	public void _Get(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters);
		string key = RegisterCallback(callback);
		Helper.CallStatic("get", resource, droidParams, key);
	}
	
	public void _Put(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters);
		string key = RegisterCallback(callback);
		Helper.CallStatic("put", resource, droidParams, key);
	}
	
	public void _Post(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters);
		string key = RegisterCallback(callback);
		Helper.CallStatic("post", resource, droidParams, key);
	}
	
	public void _PostEventually(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters);
		string key = RegisterCallback(callback);
		Helper.CallStatic("postEventually", resource, droidParams, key);
	}

	public void _Delete(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string key = RegisterCallback(callback);
		Helper.CallStatic("delete", resource, key);
	}
	
	public void _SubmitTurn(string challengeInstanceId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters, score);
		string key = RegisterCallback(callback);
		Helper.CallStatic("submitTurn", challengeInstanceId, droidParams, key);
	}
	
	public void _SubmitScore(string leaderboardId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		var droidParams = CreateRequestParamsFromDictionary(parameters);
		string key = RegisterCallback(callback);
		Helper.CallStatic("submitScore", leaderboardId, score, droidParams, key);
	}
	
	public void _SubmitScoreAndShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		_ShowRanksPanel(leaderboardId, score, gravity:gravity);
		_SubmitScore(leaderboardId, score, parameters, (success, dict) => { Debug.Log("Score submission " + (success ? "successful" : "failed")); });
	}
	
	public void _SubmitTurnAndShowChallengeDetail(string challengeInstanceId, long score, Dictionary<string,object> parameters = null)
	{
		_SubmitTurn(challengeInstanceId, score, parameters, (success, dict) => {
			AndroidJavaObject intent = CreateScoreflexActivityIntent("INTENT_EXTRA_SHOW_CHALLENGE_DETAIL");
			AddFigureToIntentIfNotNull(intent, challengeInstanceId, "INTENT_EXTRA_CHALLENGE_INSTANCE_ID");
			StartActivityWithIntent(intent);
		} );
	}

#endif
}


































