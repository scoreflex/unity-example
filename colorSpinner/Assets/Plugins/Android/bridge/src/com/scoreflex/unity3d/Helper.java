package com.scoreflex.unity3d;

import java.util.Map;

import com.scoreflex.Scoreflex;
import com.scoreflex.ScoreflexView;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.*;
import android.support.v4.content.LocalBroadcastManager;

public class Helper
{
	private static String gameObjectName = "Scoreflex";
	
	//Note that this is only affected from within the UI thread; it does not need to be thread safe.
	private static ScoreflexView ranksPanelView = null;
	
	public static void setupBroadcastReceivers(final Activity activity) {
		activity.runOnUiThread(new Runnable() {
			public void run()
			{
				LocalBroadcastManager localManager = LocalBroadcastManager.getInstance(activity);

				IntentFilter challengeIntentFilter = new IntentFilter(Scoreflex.INTENT_START_CHALLENGE);
				BroadcastReceiver challengeBroadcastReceiver = new ChallengeBroadcastReceiver();
				localManager.registerReceiver(challengeBroadcastReceiver, challengeIntentFilter);
				
				IntentFilter playSoloIntentFilter = new IntentFilter(Scoreflex.INTENT_PLAY_LEVEL);
				BroadcastReceiver playSoloReceiver = new PlaySoloBroadcastReceiver();
				localManager.registerReceiver(playSoloReceiver, playSoloIntentFilter);
			}
		});		
	}
	
	public static void setGameObjectName(final String gameObjectName) {
		Helper.gameObjectName = gameObjectName;
	}

	public static void put(Map<Object,Object> map, Object key, Object value)
	{
		map.put(key,  value);
	}
	
	public static void sendMessage(final String method, final String message)
	{
		UnityPlayer.UnitySendMessage(gameObjectName, method, message);
	}
	
	public static void preloadResource(final Activity activity, final String resource)
	{
		activity.runOnUiThread(new Runnable() {
			public void run() {
				Scoreflex.preloadResource(activity, resource);
			}
		});
	}
	
	public static void freePreloadedResources(final Activity activity, final String resource)
	{
		activity.runOnUiThread(new Runnable() {
			public void run() {
				Scoreflex.freePreloadedResources(resource);
			}
		});
	}
	
	public static void startActivityWithIntent(final Activity activity, final Intent intent)
	{		
		activity.runOnUiThread(new Runnable() {
			public void run() {
				activity.startActivity(intent);
			}
		});
	}
	
	public static void hideRanksPanel(final Activity activity)
	{
		activity.runOnUiThread(new Runnable() {
			public void run() {
				if(Helper.ranksPanelView != null)
				{
					Helper.ranksPanelView.close();
					Helper.ranksPanelView = null;
				}
			}
		});
	}

	public static void showRanksPanel(final Activity activity, final String leaderboardId, final int gravity, final Scoreflex.RequestParams params, final boolean b)
	{		
		activity.runOnUiThread(new Runnable() {
			public void run() {
				if(Helper.ranksPanelView != null)
				{
					Helper.ranksPanelView.close();
					Helper.ranksPanelView = null;
				}
				
				Helper.ranksPanelView = Scoreflex.showRanksPanel(activity, leaderboardId, gravity, params, b);
			}
		});
	}
	
	public static void get(String resource, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.get(resource, params, new ResponseHandler(callbackKey));
	}
	
	public static void put(String resource, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.put(resource, params, new ResponseHandler(callbackKey));
	}
	
	public static void post(String resource, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.get(resource, params, new ResponseHandler(callbackKey));
	}
	
	public static void postEventually(String resource, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.get(resource, params, new ResponseHandler(callbackKey));
	}
	
	public static void delete(String resource, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.delete(resource, new ResponseHandler(callbackKey));
	}

	public static void submitTurn(String challengeInstanceId, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.submitTurn(challengeInstanceId, params, new ResponseHandler(callbackKey));
	}
	
	public static void submitScore(String leaderboardId, long score, Scoreflex.RequestParams params, String callbackKey)
	{
		Scoreflex.submitScore(leaderboardId, score, params, new ResponseHandler(callbackKey));
	}
}
