package com.example.swipebox;


/**
 * User object that contains all of the details related to a user.
 * 
 * @author James Meade
 */
public class User
{
	private String ID;
	private String name;
	private String email;
	private String phoneNumber;
	
	private String password;
	
	/**
	 * User constructor.
	 */
	public User()
	{
		
	}
	
	/**
	 * Get ID variable.
	 * 
	 * @return returns the ID variable.
	 */
	public String getID() 
	{
		return ID;
	}
	
	/**
	 * Set ID variable.
	 * 
	 * @param ID sets the ID variable.
	 */
	public void setID(String ID) 
	{
		this.ID = ID;
	}
	
	/**
	 * Get name variable.
	 * 
	 * @return returns the name variable.
	 */
	public String getName() 
	{
		return name;
	}
	
	/**
	 * Set name variable.
	 * 
	 * @param name sets the name variable.
	 */
	public void setName(String name) 
	{
		this.name = name;
	}
	
	/**
	 * Get email variable.
	 * 
	 * @return returns the email variable.
	 */
	public String getEmail() 
	{
		return email;
	}
	
	/**
	 * Set email variable.
	 * 
	 * @param email sets the email variable.
	 */
	public void setEmail(String email) 
	{
		this.email = email;
	}
	
	/**
	 * Get phone number variable.
	 * 
	 * @return returns the phone number variable.
	 */
	public String getPhoneNumber() 
	{
		return phoneNumber;
	}
	
	/**
	 * Set phone number variable.
	 * 
	 * @param phoneNumber sets the phone number variable.
	 */
	public void setPhoneNumber(String phoneNumber) 
	{
		this.phoneNumber = phoneNumber;
	}

	/**
	 * Get password variable.
	 * 
	 * @return returns password variable.
	 */
	public String getPassword() 
	{
		return password;
	}

	/**
	 * Set password variable.
	 * 
	 * @param password sets the password variable.
	 */
	public void setPassword(String password) 
	{
		this.password = password;
	}	

}