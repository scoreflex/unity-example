package com.scoreflex.unity3d;

import com.scoreflex.Scoreflex.Response;

public class ResponseHandler extends com.scoreflex.Scoreflex.ResponseHandler {

	final String callbackKey;
	
	public ResponseHandler(final String callbackKey)
	{
		this.callbackKey = callbackKey;
	}
	
	private void onWhatever(boolean success, Response response) {
		String jsonString = response.getJSONObject().toString();
		String bool = success ? "success" : "failure";
		String message = callbackKey + ":" + bool + ":" + jsonString;
		Helper.sendMessage("HandleCallback", message);
	}
	
	@Override
	public void onFailure(Throwable e, Response errorResponse) {
		onWhatever(false, errorResponse);
	}

	@Override
	public void onSuccess(Response response) {
		onWhatever(true, response);
	}

}
