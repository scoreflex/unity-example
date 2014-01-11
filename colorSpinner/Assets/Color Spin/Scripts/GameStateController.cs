using UnityEngine;
using System.Collections;

public static class GameStateController
{
	public const string LeaderboardID = "Scores";
	
	const int maxMisses = 3;

	private static GameState State;

	public static bool IsSessionInProgress {
		get {
			return State != null;
		}
	}

	public static bool IsPaused { get; set; }

	public static int Hits { get {
			return State == null ? 0 : State.hits;
		} }

	public static int Misses { get {
			return State == null ? 0 : State.misses;
		} }

	public static void Reward()
	{
		State.hits++;
	}

	public static void Penalize()
	{
		State.misses++;

		if(State.misses == maxMisses)
		{
			EndGame();
		}
	}

	public static void EndGame()
	{
		if(Scoreflex.Instance.Live)
		{
			Scoreflex.Instance.SubmitScoreAndShowRanksPanel(LeaderboardID, State.hits, gravity:Scoreflex.Gravity.Bottom);
		}
		State = null;
	}

	static void Callback(bool b)
	{
		Debug.Log("Got Callback: " + b);
	}

	public static void NewGame()
	{
		if(Scoreflex.Instance.Live)
		{
			Scoreflex.Instance.HideRanksPanel();
		}
		State = new GameState();
	}
}
