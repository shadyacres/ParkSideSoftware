package com.example.swipebox;

import java.io.IOException;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.SoapFault;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Vibrator;
import android.widget.Toast;

public class LoginTask extends AsyncTask<String[], Void, String>
{
	private static final String URL = "http://localhost/SwipeBoxServices/SwipeBoxService.svc?wsdl"; // URL for web service.
	public static final String NAMESPACE = "http://tempuri.org/Import";
	public static final String SOAP_ACTION = "http://tempuri.org/ISwipeBoxService/GetClientByEmail";
	private static final String METHOD = "GetClientByEmail";
	
	private String response;
	
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
		String[] arrayList = params[0];
		
		String emailAddress = arrayList[0];
		String password = arrayList[1];
		
		//if(emailAddress.equals("james.meade22@btinternet.com") && password.equals("hello"))
		//{
			System.out.println("Accepted: " + emailAddress + password);
			
			SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
		    SoapObject request = new SoapObject(NAMESPACE, METHOD);
		    envelope.bodyOut = request;
		    HttpTransportSE transport = new HttpTransportSE(URL);
		    try 
		    {
		    	transport.call(SOAP_ACTION, envelope);
			    
			    // bodyIn is the body object received with this envelope.
			    if (envelope.bodyIn != null) 
			    {
			    	if (envelope.bodyIn instanceof SoapObject) 
			    	{
			    
					    // getProperty() Returns a specific property at a certain index.
				    	SoapPrimitive resultSOAP = (SoapPrimitive) ((SoapObject) envelope.bodyIn).getProperty(0);
					    response = resultSOAP.toString();
			    		
			    	} 
			    	else if (envelope.bodyIn instanceof SoapFault) 
			    	{
			    	    SoapFault soapFault = (SoapFault) envelope.bodyIn;
			    	    System.out.println("Soap Fault: " + soapFault.getMessage());
			    	    throw new Exception(soapFault.getMessage());
			    	}
			    }
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
		    	response = e.getMessage();
		    }
			
			return response;
		    //return "accepted";
		//}
		//else
		//{
			//System.out.println("Rejected: " + emailAddress + password);
			
			//return "rejected";
		//}
	}
    
    @Override
    protected void onPostExecute(String result) 
    {
       //if(result == "accepted") 
        //{	
        	//progress.dismiss();
        	
        	//Toast.makeText(activity.getApplicationContext(), "Successfully signed in.", Toast.LENGTH_SHORT).show();
        	
        	Toast.makeText(activity.getApplicationContext(), result, Toast.LENGTH_SHORT).show();
        	
			//Intent signInIntent = new Intent(activity, ContactActivity.class);
			//signInIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
			//activity.startActivity(signInIntent);
			//activity.finish(); // This closes the activity so that it is no longer active.
        //} 	
    	//else
    	//{	
    		//progress.dismiss();
    		
    		//Vibrator v = (Vibrator)activity.getSystemService(Context.VIBRATOR_SERVICE);
    		//v.vibrate(800);
    		
    		//Toast.makeText(activity.getApplicationContext(), "Unsuccessful.", Toast.LENGTH_SHORT).show();
    	//}
    }

}