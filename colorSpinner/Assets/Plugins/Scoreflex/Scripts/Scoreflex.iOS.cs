using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

public partial class Scoreflex
{
#if UNITY_IPHONE
	void Awake()
	{
		if(Instance == null)
		{
			try
			{
				scoreflexListenForChallengesAndPlaySolo();
				scoreflexSetUnityObjectName(gameObject.name);
				scoreflexSetClientId(ClientId, ClientSecret, Sandbox);

				initialized = true;
			}
			catch(System.EntryPointNotFoundException)
			{
				Debug.LogWarning("Failed to boot Scoreflex; not linked (EntryPointNotFoundException).");
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

	public string _GetPlayerId()
	{
		var buffer = new byte[512];
		scoreflexGetPlayerId(buffer, buffer.Length);
		int stringLength = 0;
		while(stringLength < buffer.Length && buffer[stringLength] != '\0') stringLength++;
		string result = System.Text.Encoding.Unicode.GetString(buffer);
		return result;
	}

	public float _GetPlayingTime()
	{
		return scoreflexGetPlayingTime();
	}

	public void _ShowFullscreenView(string resource, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowFullscreenView(resource, json);
	}

	public View _ShowPanelView(string resource, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		int handle = scoreflexShowPanelView(resource, json, (int) gravity);

		return new View(handle);
	}

	public void _SetDeviceToken(string deviceToken)
	{
		scoreflexSetDeviceToken(deviceToken);
	}

	public void _ShowDeveloperGames(string developerId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowDeveloperGames(developerId, json);
	}

	public void _ShowDeveloperProfile(string developerId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowDeveloperProfile(developerId, json);
	}

	public void _ShowGameDetails(string gameId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowGameDetails(gameId, json);
	}

	public void _ShowGamePlayers(string gameId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowGamePlayers(gameId, json);
	}

	public void _ShowLeaderboard(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowLeaderboard(leaderboardId, json);
	}

	public void _ShowLeaderboardOverview(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowLeaderboardOverview(leaderboardId, json);
	}

	public void _ShowPlayerChallenges(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerChallenges(json);
	}

	public void _ShowPlayerFriends(string playerId = null, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerFriends(playerId, json);
	}

	public void _ShowPlayerNewsFeed(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerNewsFeed(json);
	}

	public void _ShowPlayerProfile(string playerId = null, Dictionary<string,object> parameters = null)
	{
		var json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerProfile(playerId, json);
	}

	public void _ShowPlayerProfileEdit(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerProfileEdit(json);
	}

	public void _ShowPlayerRating(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerRating(json);
	}

	public void _ShowPlayerSettings(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowPlayerSettings(json);
	}

	public void _ShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowRanksPanel(leaderboardId, score, json, (int) gravity);
	}

	public void _HideRanksPanel()
	{
		scoreflexHideRanksPanel();
	}

	public void _ShowSearch(Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexShowSearch(json);
	}

	public void _StartPlayingSession()
	{
		scoreflexStartPlayingSession();
	}

	public void _StopPlayingSession()
	{
		scoreflexStopPlayingSession();
	}

	public void _Get(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexGet(resource, json, handlerKey);
	}

	public void _Put(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexPut(resource, json, handlerKey);
	}
	
	public void _Post(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);
		
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);
		
		scoreflexPost(resource, json, handlerKey);
	}

	public void _PostEventually(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexPostEventually(resource, json, handlerKey);
	}

	public void _Delete(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexDelete(resource, json, handlerKey);
	}

	public void _SubmitTurn(string challengeInstanceId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexSubmitTurn(challengeInstanceId, score, json, handlerKey);
	}

	public void _SubmitScore(string leaderboardId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		string handlerKey = callback == null ? null : RegisterCallback(callback);

		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexSubmitScore(leaderboardId, score, json, handlerKey);
	}

	public void _SubmitScoreAndShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexSubmitScoreAndShowRanksPanel(leaderboardId, score, json, (int) gravity);
	}

	public void _SubmitTurnAndShowChallengeDetail(string challengeLeaderboardId, long score, Dictionary<string,object> parameters = null)
	{
		string json = parameters == null ? null : MiniJSON.Json.Serialize(parameters);

		scoreflexSubmitTurnAndShowChallengeDetail(challengeLeaderboardId, score, json);
	}

	#region Imports

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexGet(string resource, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexPut(string resource, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexPost(string resource, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexPostEventually(string resource, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexDelete(string resource, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowFullscreenView(string resource, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern int scoreflexShowPanelView(string resource, string json = null, int isOnTop = 1);

	[DllImport ("__Internal")]
	private static extern void scoreflexHidePanelView(int handle);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSetUnityObjectName(string unityObjectName);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSetClientId(string clientId, string secret, bool sandbox);

	[DllImport ("__Internal")]
	private static extern void scoreflexGetPlayerId(byte[] buffer, int bufferLength);

	[DllImport ("__Internal")]
	private static extern void scoreflexListenForChallengesAndPlaySolo();

	[DllImport ("__Internal")]
	private static extern float scoreflexGetPlayingTime();

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSetDeviceToken(string deviceToken);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowDeveloperGames(string developerId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowDeveloperProfile(string developerId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowGameDetails(string gameId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowGamePlayers(string gameId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowLeaderboard(string leaderboardId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowLeaderboardOverview(string leaderboardId, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerChallenges(string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerFriends(string playerId = null, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerNewsFeed(string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerProfile(string playerId = null, string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerProfileEdit(string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerRating(string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowPlayerSettings(string json = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowRanksPanel(string leaderboardId, long score, string json = null, int isOnTop = 1);

	[DllImport ("__Internal")]
	private static extern void scoreflexHideRanksPanel();

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexShowSearch(string json = null);

	[DllImport ("__Internal")]
	private static extern void scoreflexStartPlayingSession();

	[DllImport ("__Internal")]
	private static extern void scoreflexStopPlayingSession();

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSubmitTurn(string challengeId, long score, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSubmitScore(string leaderboardId, long score, string json = null, string handler = null);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSubmitScoreAndShowRanksPanel(string leaderboardId, long score, string json = null, int isOnTop = 1);

	[DllImport ("__Internal", CharSet = CharSet.Unicode)]
	private static extern void scoreflexSubmitTurnAndShowChallengeDetail(string challengeId, long score, string json = null);
	#endregion

#endif
}


































