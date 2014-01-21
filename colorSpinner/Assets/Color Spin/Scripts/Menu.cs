using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	public GUISkin skin;

	void OnGUI()
	{
		if(skin != null) GUI.skin = skin;

		Rect guiArea = new Rect { width = 128, height = 256 };

		if(GameStateController.IsSessionInProgress && !GameStateController.IsPaused)
		{
			guiArea.x = 10;
			guiArea.y = 10;
		}
		else
		{
			guiArea.x = (Screen.width - guiArea.width) / 2;
			guiArea.y = (Screen.height - guiArea.height) / 2;
		}

		GUILayout.BeginArea(guiArea);
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
