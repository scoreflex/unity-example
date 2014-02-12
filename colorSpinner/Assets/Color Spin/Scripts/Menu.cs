using UnityEngine;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
	public GUISkin skin;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

	void OnGUI()
	{
		if(skin != null) GUI.skin = skin;

		Rect guiArea = new Rect { width = 256, height = 300 };

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
		if(Scoreflex.Live && (!GameStateController.IsSessionInProgress || GameStateController.IsPaused))
		{
			if(GUILayout.Button("Scoreflex"))
			{
				Scoreflex.ShowPlayerProfile();
			}
			if(GUILayout.Button("Leaderboards"))
			{
				Scoreflex.ShowLeaderboard(GameStateController.LeaderboardID);
			}
			if(GUILayout.Button("Play with friends"))
			{
				Scoreflex.ShowPlayerChallenges();
			}
			if(GUILayout.Button("More Games..."))
			{
				Scoreflex.ShowDeveloperGames(GameStateController.DeveloperID);
			}
		}

		GUILayout.EndArea();
	}
}
