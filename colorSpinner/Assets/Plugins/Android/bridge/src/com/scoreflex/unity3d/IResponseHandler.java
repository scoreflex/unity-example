package com.scoreflex.unity3d;

import com.scoreflex.Scoreflex.Response;

public interface IResponseHandler {

	public void onFailure(Response errorResponse);

	public void onSuccess(Response response);
}
