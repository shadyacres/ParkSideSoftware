package com.example.swipebox;


/**
 * 
 * @author James Meade
 *
 */
public class User
{
	private String ID;
	private String name;
	private String email;
	private String phoneNumber;
	
	private String password;
	
	public User()
	{
		
	}
	
	public String getID() 
	{
		return ID;
	}
	public void setID(String ID) 
	{
		this.ID = ID;
	}
	public String getName() 
	{
		return name;
	}
	public void setName(String name) 
	{
		this.name = name;
	}
	public String getEmail() 
	{
		return email;
	}
	public void setEmail(String email) 
	{
		this.email = email;
	}
	public String getPhoneNumber() 
	{
		return phoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) 
	{
		this.phoneNumber = phoneNumber;
	}

	public String getPassword() 
	{
		return password;
	}

	public void setPassword(String password) 
	{
		this.password = password;
	}	

}