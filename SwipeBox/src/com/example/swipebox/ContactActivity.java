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

public class ContactActivity extends SherlockActivity
{
	private static final int SELECT_PICTURE = 1;
	
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
		
		userDetails = getSharedPreferences("userdetails", 0);
		
		Gson gson = new Gson();
	    String json = userDetails.getString("user", "");
	    user = gson.fromJson(json, User.class);
		
		contactImage = (ImageButton) findViewById(R.id.contactImage);
		
		String userDetailsChecker = userDetails.getString("image", null);
		
		if(userDetailsChecker != null)
		{
			if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.JELLY_BEAN)
			{
				Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(userDetailsChecker);
				
	            BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage);
				
				contactImage.setBackgroundDrawable(drawable);
			
			}
			else
			{
				//Bitmap selectedImage = BitmapFactory.decodeFile(userDetails.getString("image", null));
				
				Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(userDetailsChecker);
				
	            BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage);
				
				contactImage.setBackground(drawable);
			}
		}
		else
		{
			
		}
		
		contactImage.setOnClickListener(new OnClickListener() 
		{
			@Override
			public void onClick(View v) 
			{
				Intent intent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
				startActivityForResult(intent, SELECT_PICTURE);
			}
		});
		
		name = (TextView) findViewById(R.id.nameTextView2);
		email = (TextView) findViewById(R.id.emailTextView2);
		phoneNumber = (TextView) findViewById(R.id.phoneNumberTextView2);
		
		name.setText(user.getName());
		email.setText(user.getEmail());
		phoneNumber.setText(user.getPhoneNumber());
		
		/* NFC instances */
		foregroundDispatch = new ForegroundDispatch(); // Declaring the foreground dispatch class	
		nfcAdapter = NfcAdapter.getDefaultAdapter(this);
		nfcMessage = user.getID();
	}
	
	@SuppressWarnings("deprecation")
	@TargetApi(Build.VERSION_CODES.JELLY_BEAN)
	public void onActivityResult(int requestCode, int resultCode, Intent data) 
	{
        if (resultCode == RESULT_OK) 
        {
            if (requestCode == SELECT_PICTURE) 
            {
                Uri uri = data.getData();
                String[] projection = {MediaStore.Images.Media.DATA};
                Cursor cursor = getContentResolver().query(uri, projection, null, null, null);
                cursor.moveToFirst();
                
                int columnIndex = cursor.getColumnIndex(projection[0]);
                String filePath = cursor.getString(columnIndex);
                cursor.close();
                
                Bitmap selectedImage = ImageDecoder.decodeSampledBitmapFromResource(filePath);
                
                BitmapDrawable drawable = new BitmapDrawable(getResources(), selectedImage);
                
                if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.JELLY_BEAN) 
                {
                    contactImage.setBackgroundDrawable(drawable);
                    
                    
                    Editor edit = userDetails.edit();
                    
                    edit.putString("image", filePath);
                    edit.commit();
                } 
                else 
                {
                    contactImage.setBackground(drawable);
                    
                    Editor edit = userDetails.edit();
                    
                    edit.putString("image", filePath);
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
        	edit.clear(); // Clears users details.
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