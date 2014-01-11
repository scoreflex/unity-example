using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	public GUISkin skin;

	void OnGUI()
	{
		if(skin != null) GUI.skin = skin;

		GUILayout.BeginArea(new Rect(10, 10, 96, 96));
		if(!GameStateController.IsSessionInProgress)
		{
			if(GUILayout.Button("Start"))
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
			/*if(GUILayout.Button("Challenges"))
			{
				Scoreflex.Instance.ShowPlayerChallenges();
			}*/
		}
		GUILayout.EndArea();
	}
}
