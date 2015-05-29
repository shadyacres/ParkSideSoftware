package com.example.swipebox;

import java.io.IOException;
import java.net.ConnectException;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.AsyncTask;
import android.os.Vibrator;
import android.widget.Toast;

import com.google.gson.Gson;

public class LoginTask extends AsyncTask<String[], Void, String>
{
	private static final String URL = "http://192.168.0.100/SwipeBoxServices/SwipeBoxService.svc?wsdl"; // URL for web service.
	//private static final String URL = "http://192.168.0.165/SwipeBoxServices/SwipeBoxService.svc?wsdl"; // URL for web service.
	private static final String NAMESPACE = "http://tempuri.org/"; // Namespace of the web service.
	private static final String SOAP_ACTION = "http://tempuri.org/ISwipeBoxService/GetClientByEmail"; // SOAP action needed to access method from web service.
	private static final String METHOD = "GetClientByEmail"; // Method being used from the web service.
	private static final String SOAP_ACTION_2 = "http://tempuri.org/ISwipeBoxService/AuthorizeUser"; // SOAP action needed to access method from web service.
	private static final String METHOD_2 = "AuthorizeUser"; // Method being used from the web service.
	
	private User user;
	
	private ProgressDialog progress;
	
	private Activity activity;
	
	public LoginTask(Activity refActivity)
	{
		activity = refActivity;
	}
	
	protected void onPreExecute() 
	{
		progress = new ProgressDialog(activity);
		progress.setMessage("Signing in...");
		progress.setIndeterminate(false);
		progress.setCancelable(true);
		progress.setProgressStyle(ProgressDialog.STYLE_SPINNER);
		progress.show();
    }
	

	@Override
	protected String doInBackground(String[]... params) 
	{
		//android.os.Debug.waitForDebugger(); // For testing purposes.
		
		String response = "";
		
		String[] userDetails = params[0];
		
		String emailAddress = userDetails[0];
		String password = userDetails[1];
		
		System.out.println("User details: " + emailAddress + password);
		
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
	    SoapObject request = new SoapObject(NAMESPACE, METHOD);
	    
	    PropertyInfo emailAddressInfo = new PropertyInfo(); // Sets the property to add to the SoapObject.
	    emailAddressInfo.setType(PropertyInfo.STRING_CLASS);
	    emailAddressInfo.setName("email"); // Defined variable name in the web service
	    emailAddressInfo.setValue(emailAddress); // Value of email address
	    
	    request.addProperty(emailAddressInfo);
	    
	    envelope.setAddAdornments(false);
	    envelope.dotNet = true;
	    envelope.implicitTypes = true;
	    
	    envelope.setOutputSoapObject(request); // Sets the SoapSerializationEnvelope with the the SoapObject data.
	    
	    System.out.println("First envelope body out: " + envelope.bodyOut.toString());
	    
	    HttpTransportSE transport = new HttpTransportSE(URL, 100000);
	    try 
	    {
	    	transport.call(SOAP_ACTION, envelope);
		    
	    	/* bodyIn is the body object received with this envelope. */
		    if(envelope.bodyIn != null) 
		    {
		    	System.out.println("Envelope: " + envelope.toString());
		    	
		    	System.out.println("envelope body in: " + envelope.bodyIn.toString());
			    	
		    	SoapObject userInfo = (SoapObject) envelope.getResponse(); // Returns a specific property at a certain index.
			    
		    	response = "correct_credentials";
		    	
			    user = CreateUser.createUserFromResponse(userInfo);
			    
			    System.out.println("UserInfo Response: " + response);
		    }
		    else
		    {
		    	response = "wrong_credentials";
		    	
		    	System.out.println("Envelope body in is null!");
		    }
	    }
	    catch (ConnectException e)
	    {
	    	e.printStackTrace();
	    	response = "cannot_connect";
	    }
	    catch (IOException e) 
	    {
	    	e.printStackTrace();
	    } 
	    catch (XmlPullParserException e) 
	    {
	    	e.printStackTrace();
	    }
	    catch (Exception e) 
	    {
	    	e.printStackTrace();
	    }
	    
	    SoapObject request2 = new SoapObject(NAMESPACE, METHOD_2);
	    
	    PropertyInfo passwordInfo = new PropertyInfo(); // Sets the property to add to the SoapObject.
	    passwordInfo.setType(PropertyInfo.STRING_CLASS);
	    passwordInfo.setName("pass"); // Defined variable name in the web service
	    passwordInfo.setValue(password); // Value of password
	    
	    request2.addProperty(emailAddressInfo);
	    request2.addProperty(passwordInfo);
	    
	    envelope.setOutputSoapObject(request2);
	    
	    System.out.println("Second envelope body out: " + envelope.bodyOut.toString());
	    
	    try 
	    {
	    	transport.call(SOAP_ACTION_2, envelope);
		    
	    	/* bodyIn is the body object received with this envelope. */
		    if(envelope.bodyIn != null) 
		    {
		    	System.out.println("Envelope: " + envelope.toString());
		    	
		    	System.out.println("envelope body in: " + envelope.bodyIn.toString());
			    	
		    	SoapPrimitive passwordVerification = (SoapPrimitive) envelope.getResponse(); // Returns a specific property at a certain index.
			    
		    	Boolean boolChecker = Boolean.valueOf(passwordVerification.toString());
		    	
		    	if(boolChecker == true)
		    	{
		    		response = "correct_credentials";
		    		
		    		user.setPassword(password);
		    	}
		    	else
		    	{
		    		response = "wrong_credentials";
		    	}
			    
			    System.out.println("Password verification: " + passwordVerification.toString());
			    
			    System.out.println("Password Response: " + response);
		    }
		    else
		    {
		    	response = "wrong_credentials";
		    	
		    	System.out.println("Envelope body in is null!");
		    }
	    }
	    catch (ConnectException e)
	    {
	    	e.printStackTrace();
	    	response = "cannot_connect";
	    }
	    catch (IOException e) 
	    {
	    	e.printStackTrace();
	    } 
	    catch (XmlPullParserException e) 
	    {
	    	e.printStackTrace();
	    }
	    catch (Exception e) 
	    {
	    	e.printStackTrace();
	    }
		
		return response;
	}
    
    @Override
    protected void onPostExecute(String result) 
    {
    	progress.dismiss();
    	
    	if(result.equals("cannot_connect"))
    	{
    		Toast.makeText(activity.getApplicationContext(), "Cannot connect to the server!", Toast.LENGTH_SHORT).show();
    	}
    	else if(result.equals("correct_credentials"))
    	{
    		SharedPreferences userDetails = activity.getSharedPreferences("userdetails", Context.MODE_PRIVATE);
			Editor edit = userDetails.edit();
			
			Gson gson = new Gson();
		    String json = gson.toJson(user);
		    edit.putString("user", json);
		    edit.commit();	
    		
    		Intent signInIntent = new Intent(activity, ContactActivity.class);
    		signInIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
    		activity.startActivity(signInIntent);
    		activity.finish(); // This closes the activity so that it is no longer active.
    		
    		Vibrator v = (Vibrator) activity.getSystemService(Context.VIBRATOR_SERVICE);
    		v.vibrate(800);
    		
    		Toast.makeText(activity.getApplicationContext(), "Signed in successfully!", Toast.LENGTH_SHORT).show();
    	}
    	else if(result.equals("wrong_credentials"))
    	{
    		Vibrator v = (Vibrator) activity.getSystemService(Context.VIBRATOR_SERVICE);
    		v.vibrate(800);
    		
    		Toast.makeText(activity.getApplicationContext(), "Incorrect email address or password.", Toast.LENGTH_SHORT).show();
    	}
    }

}