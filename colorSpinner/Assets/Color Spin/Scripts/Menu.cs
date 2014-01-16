using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	public GUISkin skin;

	void OnGUI()
	{
		if(skin != null) GUI.skin = skin;

		GUILayout.BeginArea(new Rect(10, 10, 128, 192));
		if(!GameStateController.IsSessionInProgress)
		{
			if(GUILayout.Button("Play Solo"))
			{
				GameStateController.NewGame();
			}
		}
		if(GameStateController.IsSessionInProgress)
		{
			if(GUILayout.Button(GameStateController.IsPaused ? "Continue" : "Pause"))
			{
				GameStateController.IsPaused = !GameStateController.IsPaused;
			}
		}
		if(Scoreflex.Instance.Live && (!GameStateController.IsSessionInProgress || GameStateController.IsPaused))
		{
			if(GUILayout.Button("Scoreflex"))
			{
				Scoreflex.Instance.ShowPlayerProfile();
			}
			if(GUILayout.Button("Leaderboards"))
			{
				Scoreflex.Instance.ShowLeaderboard(GameStateController.LeaderboardID);
			}
			if(GUILayout.Button("Play with friends"))
			{
				Scoreflex.Instance.ShowPlayerChallenges();
			}
			if(GUILayout.Button("More Games..."))
			{
				Scoreflex.Instance.ShowDeveloperGames(GameStateController.DeveloperID);
			}
		}
		GUILayout.EndArea();
	}
}
