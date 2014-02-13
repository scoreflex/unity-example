package com.scoreflex.unity3d;

import android.annotation.TargetApi;
import android.os.Build;
import android.os.Bundle;

import com.scoreflex.Scoreflex;
import com.unity3d.player.UnityPlayerActivity;

public class ScoreflexUnityActivity extends UnityPlayerActivity {

	@TargetApi(Build.VERSION_CODES.GINGERBREAD)
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Scoreflex.onCreateMainActivity(this, this.getIntent(), true);
	}
}
