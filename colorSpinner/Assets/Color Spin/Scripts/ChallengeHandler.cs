using UnityEngine;
using System.Collections.Generic;

public class ChallengeHandler : MonoBehaviour
{
	void Start()
	{
		Scoreflex.Instance.ChallengeHandlers += HandleChallenge;
	}

	void HandleChallenge(Dictionary<string,object> challengeSpecifications)
	{
		string challengeId = challengeSpecifications["id"].ToString();

		GameStateController.AcceptChallenge(challengeId);

		Debug.Log("Accepting challenge: " + challengeId);
	}

	void OnDestroy()
	{
		Scoreflex.Instance.ChallengeHandlers -= HandleChallenge;
	}
}
