package com.example.swipebox;

import android.annotation.TargetApi;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.drawable.BitmapDrawable;
import android.net.Uri;
import android.nfc.NfcAdapter;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.text.Html;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.actionbarsherlock.app.SherlockActivity;
import com.actionbarsherlock.view.Menu;
import com.actionbarsherlock.view.MenuItem;
import com.example.swipebox.nfc.ForegroundDispatch;
import com.example.swipebox.nfc.NFCWriter;
import com.google.gson.Gson;

/**
 * Contact Activity that shows the user's details as a contact page.
 * 
 * @author James Meade
 */
public class ContactActivity extends SherlockActivity
{
	private static final int SELECT_PICTURE = 1; // Select picture from phone media constant.
	
	private ImageButton contactImage;
	private TextView name, email, phoneNumber;
	
	private SharedPreferences userDetails;
	
	private User user;
	
	/* NFC */
	private ForegroundDispatch foregroundDispatch;
	private NfcAdapter nfcAdapter;
	private String nfcMessage;

	@SuppressWarnings("deprecation")
	@TargetApi(Build.VERSION_CODES.JELLY_BEAN)
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_contact);
		
		userDetails = getSharedPreferences("userdetails", 0); // Gets user's details SharedPreferences.
		
		/* Gson is used to get the User object from SharedPreferences. */
		Gson gson = new Gson();
	    String json = userDetails.getString("user", "");
	    user = gson.fromJson(json, User.class);
		
		contactImage = (ImageButton) findViewById(R.id.contactImage);
		
		String imageResource = userDetails.getString("image", null); // Image resource used for the image.
		
		/* Update image to saved image resource if available. */
		if(imageResource != null)
		{
			/* Check if phone version is less that Jelly Bean version to check which function to use to set image. */
			if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.JELLY_BEAN)
			{
				Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(imageResource); // Decode and compress image.
				
	            BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage); // Convert bitmap into a BitmapDrawable.
				
				contactImage.setBackgroundDrawable(drawable); // Set image background of contact profile.
			
			}
			else
			{
				Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(imageResource); // Decode and compress image.
				
	            BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage); // Convert bitmap into a BitmapDrawable.
				
				contactImage.setBackground(drawable); // Set image background of contact profile.
			}
		}
		
		contactImage.setOnClickListener(new OnClickListener() 
		{
			@Override
			public void onClick(View v) 
			{
				Intent intent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI); // Access phone media.
				startActivityForResult(intent, SELECT_PICTURE);
			}
		});
		
		name = (TextView) findViewById(R.id.nameTextView);
		email = (TextView) findViewById(R.id.emailTextView);
		phoneNumber = (TextView) findViewById(R.id.phoneNumberTextView);
		
		String nameText = "<b>Name: </b>" + user.getName(); // Text for user's name.
		String emailText = "<b>Email: </b>" + user.getEmail(); // Text for user's email.
		String phoneNumberText = "<b>Phone Number: </b>" + user.getPhoneNumber(); // Text for user's phone number.
		
		name.setText(Html.fromHtml(nameText)); // Set text for name by converting the text into a String.
		email.setText(Html.fromHtml(emailText)); // Set text for email by converting the text into a String.
		phoneNumber.setText(Html.fromHtml(phoneNumberText)); // Set text for phone number by converting the text into a String.
		
		/* NFC instances */
		foregroundDispatch = new ForegroundDispatch(); // Declare the foreground dispatch.
		nfcAdapter = NfcAdapter.getDefaultAdapter(this); // Get NfcAdapter.
		nfcMessage = user.getID(); // Set NFC message as user ID.
	}
	
	@SuppressWarnings("deprecation")
	@TargetApi(Build.VERSION_CODES.JELLY_BEAN)
	public void onActivityResult(int requestCode, int resultCode, Intent data) 
	{
        if (resultCode == RESULT_OK) 
        {
            if (requestCode == SELECT_PICTURE) 
            {
                Uri uri = data.getData(); // Gets data name of the file from intent.
                String[] projection = {MediaStore.Images.Media.DATA}; // Data stream for the image file.
                Cursor cursor = getContentResolver().query(uri, projection, null, null, null); // Reads image file.
                cursor.moveToFirst();
                
                int columnIndex = cursor.getColumnIndex(projection[0]); // Index of file.
                String filePath = cursor.getString(columnIndex); // File path of image file.
                cursor.close();
                
                Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(filePath); // Decode and compress image.
                
                BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage); // Convert bitmap into a BitmapDrawable.
                
                /* Check if phone version is less that Jelly Bean version to check which function to use to set image. */
                if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.JELLY_BEAN) 
                {
                    contactImage.setBackgroundDrawable(drawable); // Set image background of contact profile.
                    
                    
                    Editor edit = userDetails.edit();
                    
                    edit.putString("image", filePath); // Save image resource to SharedPreferences.
                    edit.commit();
                } 
                else 
                {
                    contactImage.setBackground(drawable); // Set image background of contact profile.
                    
                    Editor edit = userDetails.edit();
                    
                    edit.putString("image", filePath); // Save image resource to SharedPreferences.
                    edit.commit();
                } 
            }
        }
    }
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) 
	{
		getSupportMenuInflater().inflate(R.menu.contact, menu);

		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) 
	{
		switch (item.getItemId())
		{
        case R.id.sign_out:
        	/* Signs out the user. */
        	SharedPreferences userDetails = getSharedPreferences("userdetails", 0);
        	Editor edit = userDetails.edit();
        	edit.clear(); // Clears user's details.
        	edit.commit();
        	
        	Toast.makeText(this, "Signed out.", Toast.LENGTH_SHORT).show();
        	
        	Intent mainActivity = new Intent(this, MainActivity.class);
        	mainActivity.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
        	startActivity(mainActivity);
        	finish(); // This closes the activity so that it is no longer active.
        	return true;
			
		default:
            return super.onOptionsItemSelected(item);
		}
	}
	
	@Override
	public void onPause() 
	{
		/* Stops the foreground dispatch on pause. */
		super.onPause();
		foregroundDispatch.stopForegroundDispatch(this, nfcAdapter);
	}
	
	@Override
	public void onResume() 
	{
		/* Sets up the foreground dispatch on resume. */
		super.onResume();
		foregroundDispatch.setupForegroundDispatch(this, nfcAdapter);
		
	}
	
	@Override
    public void onNewIntent(Intent intent) 
	{
		/* Handles the new intent method. */
		NFCWriter nfcWriter = new NFCWriter(this);
		nfcWriter.writeText(intent, nfcMessage);
    }
	
}