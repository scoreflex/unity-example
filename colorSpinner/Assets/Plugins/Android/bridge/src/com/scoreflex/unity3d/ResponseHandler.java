package com.scoreflex.unity3d;

import com.scoreflex.Scoreflex.Response;

public class ResponseHandler extends com.scoreflex.Scoreflex.ResponseHandler {

	private IResponseHandler handler;
	
	public ResponseHandler(IResponseHandler handler)
	{
		this.handler = handler;
	}
	
	@Override
	public void onFailure(Throwable e, Response errorResponse) {
		handler.onFailure(errorResponse);
	}

	@Override
	public void onSuccess(Response response) {
		handler.onSuccess(response);
	}

}
