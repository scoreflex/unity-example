package com.scoreflex.unity3d;

import com.scoreflex.Scoreflex;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class PlaySoloBroadcastReceiver extends BroadcastReceiver {
	final String method = "HandlePlaySolo";
	
	@Override
	public void onReceive(Context arg0, Intent arg1) {
		String leaderboardId = arg1.getStringExtra(Scoreflex.INTENT_PLAY_LEVEL_EXTRA_LEADERBOARD_ID);		
		Helper.sendMessage(method, leaderboardId);
	}
}
