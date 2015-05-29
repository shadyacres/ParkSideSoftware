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
		
		/*
		SharedPreferences userDetails = getSharedPreferences("userdetails", Context.MODE_PRIVATE);
		Editor edit = userDetails.edit();
		edit.clear();
		edit.commit();
		*/
		
		SharedPreferences spChecker = getSharedPreferences("userdetails", 0);
		
		if(spChecker.contains("user"))
		{
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
			
			signIn.getBackground().setColorFilter(new LightingColorFilter(0xFF27668C, 0xFF27668C));
			
			signIn.setOnClickListener(new OnClickListener() 
			{
				
				@Override
				public void onClick(View v) 
				{
					LoginTask loginTask = new LoginTask(MainActivity.this);
					
					String emailAddressText = emailAddress.getText().toString();
					String passwordText = password.getText().toString();
					
					String[] stringArray = new String[2];
					
					stringArray[0] = emailAddressText;
					stringArray[1] = passwordText;
					
					loginTask.execute(stringArray);
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
        	Intent aboutIntent = new Intent(this, AboutActivity.class);
        	startActivity(aboutIntent);
        	return true;
			
		default:
            return super.onOptionsItemSelected(item);
		}
	}
	
}