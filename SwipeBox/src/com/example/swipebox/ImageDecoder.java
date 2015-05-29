package com.example.swipebox;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

/**
 * Image decoder class that contains a static method for converting images from a file path to a Bitmap.
 * These are used to reduce memory issues and to allow larger bitmaps to be converted.
 * 
 * @author James Meade
 */
public class ImageDecoder 
{
	
	/**
	 * Converts image from file path to a Bitmap and scales the Bitmap so that it isn't too large for the memory.
	 * 
	 * @param fileName the file path to be used.
	 * @return returns the scaled Bitmap of the image.
	 */
	public static Bitmap decodeSampledBitmapFromResource(String fileName) 
	{
		Bitmap bitmap = BitmapFactory.decodeFile(fileName);
		
		int scale = (int) (bitmap.getHeight() * (512.0 / bitmap.getWidth()));

		Bitmap scaledBitmap = Bitmap.createScaledBitmap(bitmap, 512, scale, true);
	    
	    return scaledBitmap;
	}

}