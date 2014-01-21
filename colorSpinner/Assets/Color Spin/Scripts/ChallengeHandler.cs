using UnityEngine;
using System.Collections.Generic;

public class ChallengeHandler : MonoBehaviour
{
	void Start()
	{
		Scoreflex.Instance.PlaySoloHandlers += HandlePlaySolo;
		Scoreflex.Instance.ChallengeHandlers += HandleChallenge;
	}

	public static string turnSequence;

	void HandlePlaySolo(string leaderboardId)
	{
		GameStateController.NewGame();
	}

	void HandleChallenge(Dictionary<string,object> challengeSpecifications)
	{
		string challengeId = challengeSpecifications["id"].ToString();

		Dictionary<string,object> turnFigures = challengeSpecifications["turn"] as Dictionary<string,object>;

		GameStateController.AcceptChallenge(challengeId);

		var sb = new System.Text.StringBuilder();

		foreach(var kvp in turnFigures)
		{
			sb.Append(kvp.Key);
			sb.Append("=");
			sb.Append(kvp.Value);
			sb.Append("\n");
		}

		Debug.Log("Accepting challenge: " + sb.ToString());
		
		turnSequence = turnFigures["sequence"] as string;
	}

	void OnDestroy()
	{
		Scoreflex.Instance.PlaySoloHandlers -= HandlePlaySolo;
		Scoreflex.Instance.ChallengeHandlers -= HandleChallenge;
	}
}
