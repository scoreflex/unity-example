using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

public partial class Scoreflex : MonoBehaviour
{
	public string ClientId;
	public string ClientSecret;
	public bool Sandbox;

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
			if(Scoreflex.Instance != null) Scoreflex.Instance._HidePanelView(handle);
			#endif
		}
	}

	private static Scoreflex Instance;

	private bool initialized = false;

	public static bool Live {
		get {
			return Instance != null && Instance.initialized;
		}
	}

	private const string ErrorNotLive = "Scoreflex: Method called while not live.";
	
	public static System.Action<string> PlaySoloHandlers = null;
	public static System.Action<Dictionary<string,object>> ChallengeHandlers = null;

	// CALLBACK FACILITY //
	
	public delegate void Callback(bool success, Dictionary<string,object> response);
	
	private readonly Dictionary<string,Callback> CallbackTable = new Dictionary<string,Callback>();
	
	string RegisterCallback(Callback callback)
	{
		string key;
		var random = new System.Random();
		do {
			key = random.Next().ToString();
		} while(CallbackTable.ContainsKey(key));
		
		CallbackTable[key] = callback;
		
		return key;
	}
	
	void HandleCallback(string figure)
	{
		if(figure.Contains(":"))
		{
			bool success = figure.Contains("success");
			string handlerKey = figure.Split(':')[0];
			string jsonString = figure.Substring(handlerKey.Length + ":success:".Length); // :failure is the same length
			
			var dictionary = new Dictionary<string,object>();
			
			try
			{
				if(jsonString.Length > 0)
				{
					var parsed = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string,object>;

					foreach(var kvp in parsed)
					{
						dictionary.Add(kvp.Key, kvp.Value);
					}
				}
			}
			catch(System.Exception ex)
			{
				Debug.LogException(ex);
				Debug.LogError("Scoreflex: Received unparsable JSON code: " + jsonString);
			}
			
			if(CallbackTable.ContainsKey(handlerKey))
			{
				CallbackTable[handlerKey](success, dictionary);
				CallbackTable.Remove(handlerKey);
			}
			else
			{
				Debug.Log("Scoreflex: Received invalid callback code from native library: " + handlerKey);
			}
		}
		else
		{
			Debug.Log("Scoreflex: Received invalid callback code from native library: " + figure);
		}
	}
	
	void HandlePlaySolo(string figure)
	{
		if(PlaySoloHandlers == null)
		{
			Debug.LogError("Scoreflex: Instructed to play solo, but no handlers configured! Please assign to Scoreflex.Instance.PlaySoloHandlers");
		}
		else
		{
			PlaySoloHandlers(figure);
		}
	}
	
	void HandleChallenge(string figure)
	{
		if(ChallengeHandlers == null)
		{
			Debug.LogError("Scoreflex: Received challenge, but found no challenge handler! Please assign to Scoreflex.Instance.ChallengeHandlers");
		}
		else
		{
			var dict = MiniJSON.Json.Deserialize(figure) as Dictionary<string,object>;
			ChallengeHandlers(dict);
		}
	}

	// WRAPPERS //

	public string GetLanguageCode()
	{
		return _GetLanguageCode();
	}

	public void SetLanguageCode(string languageCode)
	{
		_SetLanguageCode(languageCode);
	}

	public void PreloadResource(string resource)
	{
		_PreloadResource(resource);
	}

	public void FreePreloadedResource(string resource)
	{
		_FreePreloadedResource(resource);
	}

	public bool IsReachable {
		get {
			return _IsReachable;
		}
	}

	public static string GetPlayerId()
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			return string.Empty;
		}
		else
			return Instance._GetPlayerId();
	}
	
	public static float GetPlayingTime()
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			return 0f;
		}
		else
			return Instance._GetPlayingTime();
	}
	
	public static void ShowFullscreenView(string resource, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowFullscreenView(resource, parameters);
	}

	public static View ShowPanelView(string resource, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			return null;
		}
		else
			return Instance._ShowPanelView(resource, parameters, gravity);
	}
		
	public static void SetDeviceToken(string deviceToken)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._SetDeviceToken(deviceToken);
	}
	
	public static void ShowDeveloperGames(string developerId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowDeveloperGames(developerId, parameters);
	}
	
	public static void ShowDeveloperProfile(string developerId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowDeveloperProfile(developerId, parameters);
	}
	
	public static void ShowGameDetails(string gameId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowGameDetails(gameId, parameters);
	}
	
	public static void ShowGamePlayers(string gameId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowGamePlayers(gameId, parameters);
	}
	
	public static void ShowLeaderboard(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowLeaderboard(leaderboardId, parameters);
	}
	
	public static void ShowLeaderboardOverview(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowLeaderboardOverview(leaderboardId, parameters);
	}
	
	public static void ShowPlayerChallenges(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerChallenges(parameters);
	}
	
	public static void ShowPlayerFriends(string playerId = null, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerFriends(playerId, parameters);
	}
	
	public static void ShowPlayerNewsFeed(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerNewsFeed(parameters);
	}
	
	
	public static void ShowPlayerProfile(string playerId = null, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerProfile(playerId, parameters);
	}
	
	public static void ShowPlayerProfileEdit(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerProfileEdit(parameters);
	}
	
	public static void ShowPlayerRating(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerRating(parameters);
	}
	
	public static void ShowPlayerSettings(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowPlayerSettings(parameters);
	}
	
	public static void ShowSearch(Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowSearch(parameters);
	}

	public static void ShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._ShowRanksPanel(leaderboardId, score, parameters, gravity);
	}
	
	public static void HideRanksPanel()
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._HideRanksPanel();
	}
	
	public static void StartPlayingSession()
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._StartPlayingSession();
	}
	
	public static void StopPlayingSession()
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._StopPlayingSession();
	}
	
	public static void Get(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			callback(false, new Dictionary<string,object>());
		}
		else
			Instance._Get(resource, parameters, callback);
	}
	
	public static void Put(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			callback(false, new Dictionary<string,object>());
		}
		else
			Instance._Put(resource, parameters, callback);
	}
	
	public static void Post(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			callback(false, new Dictionary<string,object>());
		}
		else
			Instance._Post(resource, parameters, callback);
	}
	
	public static void PostEventually(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			callback(false, new Dictionary<string,object>());
		}
		else
			Instance._PostEventually(resource, parameters, callback);
	}
	
	public static void Delete(string resource, Dictionary<string,object> parameters, Callback callback)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			callback(false, new Dictionary<string,object>());
		}
		else
			Instance._Delete(resource, parameters, callback);
	}
	
	public static void SubmitTurn(string challengeInstanceId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			if(callback != null) callback(false, new Dictionary<string,object>());
		}
		else
			Instance._SubmitTurn(challengeInstanceId, score, parameters, callback);
	}
	
	public static void SubmitScore(string leaderboardId, long score, Dictionary<string,object> parameters = null, Callback callback = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
			if(callback != null) callback(false, new Dictionary<string,object>());
		}
		else
			Instance._SubmitScore(leaderboardId, score, parameters, callback);
	}
	
	public static void SubmitScoreAndShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._SubmitScoreAndShowRanksPanel(leaderboardId, score, parameters, gravity);
	}
	
	public static void SubmitTurnAndShowChallengeDetail(string challengeInstanceId, long score, Dictionary<string,object> parameters = null)
	{
		if(!Live) {
			Debug.Log(ErrorNotLive);
		}
		else
			Instance._SubmitTurnAndShowChallengeDetail(challengeInstanceId, score, parameters);
	}

}


































