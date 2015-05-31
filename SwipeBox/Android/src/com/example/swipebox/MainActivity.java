package com.example.swipebox;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.LightingColorFilter;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

import com.actionbarsherlock.app.ActionBar;
import com.actionbarsherlock.app.SherlockActivity;
import com.actionbarsherlock.view.Menu;
import com.actionbarsherlock.view.MenuItem;

/**
 * Main Activity which displays the login screen of the application.
 * 
 * @author James Meade
 */
public class MainActivity extends SherlockActivity
{
	private ActionBar actionBar;
	private EditText emailAddress, password;
	private Button signIn;

	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		SharedPreferences spChecker = getSharedPreferences("userdetails", 0);
	
		/* Checks if user details SharedPreferences exists. */
		if(spChecker.contains("user"))
		{
			/* Start Contact Activity if true. */
			startActivity(new Intent(this, ContactActivity.class));
		    finish();
		}
		else
		{
			actionBar = getSupportActionBar();
			actionBar.setDisplayShowTitleEnabled(true);
			
			password = (EditText) findViewById(R.id.passwordEditText);
			emailAddress = (EditText) findViewById(R.id.emailAddressEditText);
			
			signIn = (Button) findViewById(R.id.signInButton);
			
			signIn.getBackground().setColorFilter(new LightingColorFilter(0xFF27668C, 0xFF27668C)); // Set background colour of button.
			
			signIn.setOnClickListener(new OnClickListener() 
			{		
				@Override
				public void onClick(View v) 
				{
					LoginTask loginTask = new LoginTask(MainActivity.this);
					
					String emailAddressText = emailAddress.getText().toString(); // Gets email address that the user has entered.
					String passwordText = password.getText().toString(); // Gets the password that the user has entered.
					
					String[] stringArray = new String[2];
					
					stringArray[0] = emailAddressText;
					stringArray[1] = passwordText;
					
					loginTask.execute(stringArray); // Starts login to web service Thread.
				}
			});
		
		}
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) 
	{
		getSupportMenuInflater().inflate(R.menu.main, menu);

		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) 
	{
		switch (item.getItemId())
		{
        case R.id.about:
        	/* Open About Activity. */
        	Intent aboutIntent = new Intent(this, AboutActivity.class);
        	startActivity(aboutIntent);
        	return true;
			
		default:
            return super.onOptionsItemSelected(item);
		}
	}
	
}