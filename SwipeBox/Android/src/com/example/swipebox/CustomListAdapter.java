package com.example.swipebox;

import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class CustomListAdapter extends BaseAdapter 
{
	private Context context;
	private ArrayList<ListData> listData = new ArrayList<ListData>();
	
	public CustomListAdapter(Context refContext, ArrayList<ListData> refListData)
	{
		context = refContext;
		listData = refListData;
	}
	
	@Override
	public int getCount() 
	{
		return listData.size();
	}
	@Override
	public Object getItem(int position) 
	{
		return listData.get(position);
	}
	@Override
	public long getItemId(int position) 
	{
		return 0;
	}
	@Override
	public View getView(int position, View convertView, ViewGroup parent) 
	{
		ViewHolder holder;
		
		if(convertView == null)
		{
			LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
			convertView = inflater.inflate(R.layout.listview_item, parent, false);
			
			holder = new ViewHolder();
			holder.name = (TextView) convertView.findViewById(R.id.nameTextView);
			holder.idNumber = (TextView) convertView.findViewById(R.id.idNumberTextView);
			
			holder.name.setText(listData.get(position).getName());
			holder.idNumber.setText(listData.get(position).getIdNumber());
			
			convertView.setTag(holder);
		}
		else
		{
			holder = (ViewHolder) convertView.getTag();
		}
		
		return convertView;
	}
	
	/**
	 * View holder pattern to recycle views to increase performance within the ListView.
	 * 
	 * @author James Meade
	 *
	 */
	private static class ViewHolder
	{
		TextView name, idNumber;
	}

}