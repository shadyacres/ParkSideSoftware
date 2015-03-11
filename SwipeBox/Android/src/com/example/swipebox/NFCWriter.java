package com.example.swipebox;

import java.io.IOException;
import java.nio.charset.Charset;
import java.util.Locale;

import android.content.Context;
import android.content.Intent;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.nfc.tech.Ndef;
import android.nfc.tech.NdefFormatable;
import android.os.Vibrator;
import android.widget.Toast;

public class NFCWriter 
{
	Context context;
	
	public NFCWriter(Context refContext) 
	{
		context = refContext;
	}
	
	/**
	 * Writing text to an NFC tag from a new intent and deals only with writing to an NFC tag.
	 * 
	 * @param refIntent this is the Intent that is being used to start the NFC communications.
	 * @param refMessage this is the String message that will be written into the NFC tag.
	 * @see
	 * @since 1.0
	 */
	public void writeText(Intent refIntent, String refMessage)
	{
		String message = refMessage;
		Intent intent = refIntent;
		
		Tag tag = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG); // NFC tag
		
		boolean encodeInUtf8 = false;
		
		/* Language encoding for NDEF data. */
		Locale locale = new Locale("en", "US");
		byte[] langBytes = locale.getLanguage().getBytes(Charset.forName("US-ASCII"));
		
		/* UTF encoding for NDEF data. */
		Charset utfEncoding = encodeInUtf8 ? Charset.forName("UTF-8") : Charset.forName("UTF-16");
		byte[] textBytes = message.getBytes(utfEncoding);
		int utfBit = encodeInUtf8 ? 0 : (1 << 7);
		char status = (char) (utfBit + langBytes.length);
		
		/* Payload for the NDEF message. */
		byte[] payload = new byte[1 + langBytes.length + textBytes.length];
		payload[0] = (byte) status;
		
		System.arraycopy(langBytes, 0, payload, 1, langBytes.length);
		System.arraycopy(textBytes, 0, payload, 1 + langBytes.length, textBytes.length);
		
		/* Create new NDEF record inside new NDEF message to store the text to be written to NFC. */
		NdefRecord ndefRecord = new NdefRecord(NdefRecord.TNF_WELL_KNOWN, NdefRecord.RTD_TEXT, new byte[0], payload);
		NdefMessage newMessage = new NdefMessage(new NdefRecord[] {ndefRecord});
		
		writeNdefMessageToTag(newMessage, tag);
	}
	
	/**
	 * Write NDEF message to NFC tag method.
	 * 
	 * @param refMessage
	 * @param refDetectedTag
	 * @return boolean
	 * @see
	 * @since 1.0
	 */
	private boolean writeNdefMessageToTag(NdefMessage refMessage, Tag refDetectedTag)
	{
		NdefMessage message = refMessage;
		Tag detectedTag = refDetectedTag;
		
		int size = message.toByteArray().length;
		try
		{
			Ndef ndef = Ndef.get(detectedTag);
			
			if(ndef != null)
			{
				ndef.connect();
				
				if(!ndef.isWritable())
				{
					System.out.println("Tag is read only.");
					
					return false;
				}
				else
				{
						
				}
				if(ndef.getMaxSize() < size)
				{
					System.out.println("The data is too large and cannot be written to tag.");
					
					return false;
				}
				else
				{
						
				}
				
				ndef.writeNdefMessage(message);
				ndef.close();
				
				System.out.println("NDEF message written.");
				
				Vibrator v = (Vibrator)context.getSystemService(Context.VIBRATOR_SERVICE);
	        	v.vibrate(800);
	        	
				Toast.makeText(context, "Swiped in successfully!", Toast.LENGTH_SHORT).show();
				
				return true;
			}
			else
			{
				NdefFormatable ndefFormat = NdefFormatable.get(detectedTag);
				
				if(ndefFormat != null)
				{
					try
					{
						ndefFormat.connect();
						ndefFormat.format(message);
						ndefFormat.close();
						
						System.out.println("The data is written to the tag.");
						
						return true;
					}
					catch (IOException e)
					{
						System.out.println("Failed to format tag.");
						
						return false;
					}
				}
				else
				{
					System.out.println("NDEF is not supported.");
					
					return false;
				}
			}
				
		}
		catch (Exception e)
		{
			System.out.println("Swipe in failed.");
		}
		return false;
	}

}