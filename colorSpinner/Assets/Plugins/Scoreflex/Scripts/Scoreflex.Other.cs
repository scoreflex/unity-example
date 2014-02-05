using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

public partial class Scoreflex
{
	#if !UNITY_IPHONE && !UNITY_ANDROID
	void Awake()
	{
		if(Instance == null)
		{
			initialized = false;
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if(Instance != this)
		{
			GameObject.Destroy(gameObject);
		}
	}

	public string GetPlayerId()
	{
		Debug.Log(ErrorNotLive);
		return string.Empty;
	}

	public float GetPlayingTime()
	{
		Debug.Log(ErrorNotLive);
		return 0f;
	}

	public void ShowFullscreenView(string resource, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public View ShowPanelView(string resource, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		Debug.Log(ErrorNotLive);
		return null;
	}

	public void SetDeviceToken(string deviceToken)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowDeveloperGames(string developerId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowDeveloperProfile(string developerId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowGameDetails(string gameId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowGamePlayers(string gameId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowLeaderboard(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowLeaderboardOverview(string leaderboardId, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerChallenges(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerFriends(string playerId = null, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerNewsFeed(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerProfile(string playerId = null, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerProfileEdit(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerRating(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowPlayerSettings(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void HideRanksPanel()
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void ShowSearch(Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void StartPlayingSession()
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void StopPlayingSession()
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void Get(string resource, Dictionary<string,object> parameters, System.Action<bool,Dictionary<string,object>> callback)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false, new Dictionary<string,object>());
		return;
	}

	public void Put(string resource, Dictionary<string,object> parameters, System.Action<bool,Dictionary<string,object>> callback)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false, new Dictionary<string,object>());
		return;
	}
	
	public void Post(string resource, Dictionary<string,object> parameters, System.Action<bool,Dictionary<string,object>> callback)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false, new Dictionary<string,object>());
		return;
	}

	public void PostEventually(string resource, Dictionary<string,object> parameters, System.Action<bool,Dictionary<string,object>> callback)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false, new Dictionary<string,object>());
		return;
	}

	public void Delete(string resource, Dictionary<string,object> parameters, System.Action<bool,Dictionary<string,object>> callback)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false, new Dictionary<string,object>());
		return;
	}

	public void SubmitTurn(string challengeInstanceId, long score, Dictionary<string,object> parameters = null, System.Action<bool> callback = null)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false);
		return;
	}

	public void SubmitScore(string leaderboardId, long score, Dictionary<string,object> parameters = null, System.Action<bool> callback = null)
	{
		Debug.Log(ErrorNotLive);
		if(callback != null) callback(false);
		return;
	}

	public void SubmitScoreAndShowRanksPanel(string leaderboardId, long score, Dictionary<string,object> parameters = null, Gravity gravity = Gravity.Top)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

	public void SubmitTurnAndShowChallengeDetail(string challengeLeaderboardId, long score, Dictionary<string,object> parameters = null)
	{
		Debug.Log(ErrorNotLive);
		return;
	}

#endif
}


































