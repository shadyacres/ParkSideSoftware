package com.example.swipebox;

import java.util.ArrayList;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.widget.FrameLayout;
import android.widget.ImageButton;
import android.widget.ListView;
import android.widget.RadioGroup.LayoutParams;

import com.actionbarsherlock.app.ActionBar;
import com.actionbarsherlock.app.SherlockActivity;
import com.actionbarsherlock.view.Menu;
import com.actionbarsherlock.view.MenuItem;
import com.actionbarsherlock.widget.SearchView;

public class SearchActivity extends SherlockActivity 
{

ActionBar actionBar;
	
	ListView listViewLayout;
	ImageButton contactImage;
	
	CustomListAdapter customListAdapter;

	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_search);
		
		actionBar = getSupportActionBar();
		actionBar.setDisplayShowTitleEnabled(true);
		
		listViewLayout = (ListView) findViewById(R.id.listViewLayout);
		
		ArrayList<ListData> arrayListOfListData = new ArrayList<ListData>();
		
		addDataToList(arrayListOfListData);
		
		customListAdapter = new CustomListAdapter(SearchActivity.this, arrayListOfListData);
		
		listViewLayout.setAdapter(customListAdapter);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) 
	{
		getSupportMenuInflater().inflate(R.menu.search, menu);
		
		 SearchManager searchManager = (SearchManager) getSystemService(Context.SEARCH_SERVICE);
		 SearchView searchView = (SearchView) menu.findItem(R.id.search).getActionView();
		 
		 FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		 
		 searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));
		 searchView.setIconifiedByDefault(false); // Do not iconify the widget; expand it by default
		 searchView.setLayoutParams(layoutParams);
		 searchView.setQueryHint("Search name");

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
        case R.id.contact:
        	Intent contactIntent = new Intent(this, ContactActivity.class);
        	startActivity(contactIntent);
        	return true;
			
		default:
            return super.onOptionsItemSelected(item);
		}
	}
	
	private void addDataToList(ArrayList<ListData> refArrayListOfListData)
	{
		String[] testNames = {"James Meade", "Daniel Blackmore", "Alastair Warren", "James Pagan-Lodge", "Charlire Newsome"};
		String[] testIdNumbers = {"s1104798", "s8276748", "s0192784", "s1928930", "s2340839"};
		
		for(String name : testNames)
		{
			ListData listData = new ListData();
			
			listData.setName(name);
			
			refArrayListOfListData.add(listData);
		}
		
		for (int i=0;i<testIdNumbers.length;i++) 
		{
			refArrayListOfListData.get(i).setIdNumber(testIdNumbers[i]);
		}
	}
}
