package com.example.swipebox;

import android.os.Bundle;
import android.support.v4.app.NavUtils;

import com.actionbarsherlock.app.SherlockActivity;
import com.actionbarsherlock.view.MenuItem;

/**
 * About Activity that displays all of the third-party libraries and licenses for them that were used in the application.
 * 
 * @author James Meade
 */
public class AboutActivity extends SherlockActivity 
{

	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_about);
		
		getSupportActionBar().setDisplayHomeAsUpEnabled(true); // Show up icon (back arrow) on ActionBar.
	}
	
	@Override
	public boolean onOptionsItemSelected(MenuItem item) 
	{
		int id = item.getItemId();
		if (id == android.R.id.home) 
		{
			NavUtils.navigateUpFromSameTask(this); // Navigate back to the home screen.
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
}