package com.example.swipebox;

import android.annotation.TargetApi;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
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

public class ContactActivity extends SherlockActivity 
{
	private static final int SELECT_PICTURE = 1;
	
	private ImageButton contactImage;
	private TextView name, id;
	
	private SharedPreferences userDetails;
	
	/* NFC */
	private ForegroundDispatch foregroundDispatch;
	private NfcAdapter nfcAdapter;

	@SuppressWarnings("deprecation")
	@TargetApi(Build.VERSION_CODES.JELLY_BEAN)
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_contact);
		
		userDetails = getSharedPreferences("userdetails", 0);
		
		contactImage = (ImageButton) findViewById(R.id.contactImage);
		
		String userDetailsChecker = userDetails.getString("image", null);
		
		if(userDetailsChecker != null)
		{
			if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.JELLY_BEAN)
			{
				Bitmap selectedImage = BitmapFactory.decodeFile(userDetailsChecker);
	            Drawable drawable = new BitmapDrawable(getResources(), selectedImage);
				
				contactImage.setBackgroundDrawable(drawable);
			
			}
			else
			{
				Bitmap selectedImage = BitmapFactory.decodeFile(userDetails.getString("image", null));
	            Drawable drawable = new BitmapDrawable(getResources(), selectedImage);
				
				contactImage.setBackground(drawable);
			}
		}
		else
		{
			
		}
		
		contactImage.setOnClickListener(new OnClickListener() 
		{
	        @Override
	        public void onClick(View view) 
	        {   
	        	/**
	            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(ContactActivity.this);
	            alertBuilder.setTitle("Change Image");
	            alertBuilder.setMessage("Select your image here.");
	            alertBuilder.setCancelable(true);
	            alertBuilder.setPositiveButton("Confirm", new DialogInterface.OnClickListener() 
	            {
	                public void onClick(DialogInterface dialog, int id) 
	                {
	                    dialog.cancel();
	                }
	            });

	            AlertDialog alertDialog = alertBuilder.create();
	            alertDialog.show();
	            */
	        	
	        	Intent intent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
                startActivityForResult(intent, SELECT_PICTURE);
	        	
	        }
	    });
		
		name = (TextView) findViewById(R.id.nameTextView);
		id = (TextView) findViewById(R.id.idNumberTextView);
		
		name.setText(userDetails.getString("emailaddress", null));
		id.setText(userDetails.getString("password", null));
		
		/* NFC instances */
		foregroundDispatch = new ForegroundDispatch(); // Declaring the foreground dispatch class	
		nfcAdapter = NfcAdapter.getDefaultAdapter(this);
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
                
                Bitmap selectedImage = BitmapFactory.decodeFile(filePath);
                Drawable drawable = new BitmapDrawable(getResources(), selectedImage);
                
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
        case R.id.action_settings:
        	/* Opens up the Settings activity */
        	Intent settingsIntent = new Intent(this, SettingsActivity.class);
        	startActivity(settingsIntent);
        	return true;
        case R.id.sign_out:
        	/* Signs out the user. */
        	SharedPreferences userDetails = getSharedPreferences("userdetails", 0);
        	Editor edit = userDetails.edit();
        	edit.clear();
        	edit.commit();
        	
        	Toast.makeText(this, "Signed out", Toast.LENGTH_SHORT).show();
        	
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
		/* Stops the foreground dispatch on pause */
		super.onPause();
		foregroundDispatch.stopForegroundDispatch(this, nfcAdapter);
	}
	
	@Override
	public void onResume() 
	{
		/* Sets up the foreground dispatch on resume */
		super.onResume();
		foregroundDispatch.setupForegroundDispatch(this, nfcAdapter);
		
	}
	
	@Override
    public void onNewIntent(Intent intent) 
	{
		/* Handles the new intent method */
		SharedPreferences sharedPreferences = getSharedPreferences("userdetails", 0);
		
		String emailAddress = sharedPreferences.getString("emailaddress", null);
		
		NFCWriter nfcWriter = new NFCWriter(this);
		nfcWriter.writeText(intent, emailAddress);
    }
	
}