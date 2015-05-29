package com.example.swipebox;

import org.ksoap2.serialization.SoapObject;

/**
 * 
 * @author James Meade
 *
 */
public class CreateUser 
{
	
	/**
	 * 
	 * @param response
	 * @return
	 */
	public static User createUserFromResponse(SoapObject response)
	{
		User user = new User();
		
		int testCount = response.getPropertyCount();
		
		String ID = response.getProperty(0).toString();
		String email = response.getProperty(1).toString();
		String name = response.getProperty(2).toString();
		String phoneNumber = response.getProperty(3).toString();
		
		user.setID(ID);
		user.setEmail(email);
		user.setName(name);
		user.setPhoneNumber(phoneNumber);
		
		System.out.println("Property count: " + testCount);
		System.out.println("Property 0: " + ID);
		System.out.println("Property 1: " + email);
		System.out.println("Property 2: " + name);
		System.out.println("Property 3: " + phoneNumber);
		
		return user;
	}

}