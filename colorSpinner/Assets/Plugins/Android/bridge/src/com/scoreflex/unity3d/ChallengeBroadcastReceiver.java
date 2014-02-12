package com.scoreflex.unity3d;

import org.json.JSONObject;

import com.scoreflex.Scoreflex;
import com.scoreflex.model.JSONParcelable;

import android.content.Context;
import android.content.Intent;

public class ChallengeBroadcastReceiver extends android.content.BroadcastReceiver {
	
	@Override
	public void onReceive(Context arg0, Intent arg1)
	{
		JSONParcelable jsonParcelable = arg1.getParcelableExtra(Scoreflex.INTENT_START_CHALLENGE_EXTRA_INSTANCE);
		JSONObject jsonObject = jsonParcelable.getJSONObject();
		String json = jsonObject.toString();
		Helper.sendMessage("HandleChallenge", json);
	}

}
