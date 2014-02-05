package com.scoreflex.unity3d;

import android.content.Context;
import android.content.Intent;

public class BroadcastReceiver extends android.content.BroadcastReceiver {

	private IBroadcastReceiver realReceiver;
	
	public BroadcastReceiver(IBroadcastReceiver realReceiver)
	{
		this.realReceiver = realReceiver;
	}
	
	@Override
	public void onReceive(Context arg0, Intent arg1) {
		realReceiver.onReceive(arg0, arg1);
	}

}
