package com.example.swipebox;

import org.ksoap2.serialization.SoapObject;

/**
 * Create user function for creating a User object from a SoapObject received from the web service.
 * 
 * @author James Meade
 */
public class CreateUser 
{
	
	/**
	 * Create a User object from the SoapObject received from the web service.
	 * 
	 * @param response SoapObject received from the web service.
	 * @return returns a User object containing all of the user's details.
	 */
	public static User createUserFromResponse(SoapObject response)
	{
		User user = new User();
		
		int testCount = response.getPropertyCount(); // Get property count of the SoapObject.
		
		String ID = response.getProperty(0).toString(); // Get ID property value.
		String email = response.getProperty(1).toString(); // Get email property value.
		String name = response.getProperty(2).toString(); // Get name property value.
		String phoneNumber = response.getProperty(3).toString(); // Get phone number property value.
		
		user.setID(ID); // Set User ID.
		user.setEmail(email); // Set User email.
		user.setName(name); // Set User name.
		user.setPhoneNumber(phoneNumber); // Set User phone number.
		
		System.out.println("Property count: " + testCount);
		System.out.println("Property 0: " + ID);
		System.out.println("Property 1: " + email);
		System.out.println("Property 2: " + name);
		System.out.println("Property 3: " + phoneNumber);
		
		return user;
	}

}