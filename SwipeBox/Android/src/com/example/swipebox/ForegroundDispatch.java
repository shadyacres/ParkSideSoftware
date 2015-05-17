package com.example.swipebox;

import android.app.Activity;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.IntentFilter.MalformedMimeTypeException;
import android.nfc.NfcAdapter;

/**
 * This class sets up the foreground dispatch (which intercepts a specific intent and claims priority when needed)
 * for the NFC communications.
 * 
 * @author James Meade
 * @version %I%, %G%
 * @since 1.0
 */
public class ForegroundDispatch 
{
	private String MIME_TEXT_PLAIN = "text/plain";

	/**
	 * Foreground dispatch default constructor.
	 * 
	 * @see
	 * @since 1.0
	 */
	public ForegroundDispatch() 
	{
		
	}
	
	/**
	 * Sets up the foreground dispatch which sets up the pending intent for an NFC connection.
	 * 
	 * @param refActivity this is a reference to a class' activity.
	 * @param refAdapter this is a reference to the adapter associated with the NFC communications.
	 * @see
	 * @since 1.0
	 */
	public void setupForegroundDispatch(Activity refActivity, NfcAdapter refAdapter) 
	{
		Activity activity = refActivity;
		NfcAdapter adapter = refAdapter;
		
		/* Create intent. */
        Intent intent = new Intent(activity.getApplicationContext(), activity.getClass());
        intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
        PendingIntent pendingIntent = PendingIntent.getActivity(activity.getApplicationContext(), 0, intent, 0);
        
        /* String array of tech list. */
        String[][] techList = new String[][]{};
        
        /* Create intent filters. */
        IntentFilter[] filters = new IntentFilter[1];
        filters[0] = new IntentFilter();
        filters[0].addAction(NfcAdapter.ACTION_NDEF_DISCOVERED);
        filters[0].addCategory(Intent.CATEGORY_DEFAULT);
        
        try 
        {
            filters[0].addDataType(MIME_TEXT_PLAIN);
        } 
        catch (MalformedMimeTypeException e) 
        {
            throw new RuntimeException("Check your mime type.");
        }
        
        adapter.enableForegroundDispatch(activity, pendingIntent, filters, techList);
    }
	
	/**
	 * Stop foreground dispatch.
	 * 
	 * @param refActivity this is a reference to a class' activity.
	 * @param adapter NFC adapter associated with this foreground dispatch.
	 * @see
	 * @since 1.0
	 */
	public void stopForegroundDispatch(Activity refActivity, NfcAdapter adapter) 
	{
		Activity activity = refActivity;
		
        adapter.disableForegroundDispatch(activity);
    }

}