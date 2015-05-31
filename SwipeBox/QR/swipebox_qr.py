#!/usr/bin/python

#Swipebox-QR.py
#Company: Park Side Software
#By Charlie Newsome

#Import all libraries needed.
import Tkinter as tk
import zbar
from PIL import Image, ImageTk
from ttk import Frame, Button, Style
import io
import picamera
import time
from suds.client import Client
from argparse import ArgumentParser
import threading

#Parse arguments for the show-gui argument.
parser = ArgumentParser()
parser.add_argument("--show-gui", help="Display a GUI window showing what the camera sees.")
args = parser.parse_args()

#Define the web service.
url = 'http://192.168.137.1:49718/SwipeBoxService.svc?wsdl'
client = Client(url)

#Define global variables canScan and panel1_image.
#canScan says whether the script should continue scanning the camera for QR codes.
#panel1_image is the image shown on GUI interface.
global canScan, panel1_image
canScan = True
panel1_image = None

#Function to set canScan to true and then call scanCameraGUI()
#Is called by Threading.timer so it can be delayed by 10 seconds.
def setCanScanTrueGUI():
	global canScan
	canScan = True
	scanCameraGUI()

#Function to set canScan to true and then call scanCamera()
#Is called by Threading.timer so it can be delayed by 10 seconds.
def setCanScanTrue():
	global canScan
	canScan = True
	scanCamera()

#Main loop code with extra code to update the GUI interface.
def scanCameraGUI():
	#Fetch the globals.
	global canScan, panel1_image
	#If scanning is allowed.
	if canScan:
		#Catch any errors.
		try:
			#Define an IO stream to put the image from the camera in.
			#This is so we don't have to write the image to a disk first.
			stream = io.BytesIO()
			
			#Get the Raspberry Pi camera.
			camera = picamera.PiCamera()
			#Set the resolution to 640x460.
			#Higher resoltions will take longer to scan.
			camera.resolution = (640, 460)
			#Take a snapshot then convert it to a JPEG, put the resulting image into the IO stream.
			camera.capture(stream, 'jpeg')
			#Close the camera as we don't need it anymore.
			camera.close()
			#Remove the camera instance from memory.
			del(camera)
			
			#Tell the user whats going on.
			print 'Scanning...'
	
			#Define a zbar scanner which will scan the image for QR codes.
			scanner = zbar.ImageScanner()
	
			#Enable the scanner.
			scanner.parse_config('enable')
	
			#Seek the IO stream to the start.
			stream.seek(0)
	
			#Open the IO stream with the Python Imaging Library (PIL).
			#Convert the image to grayscale as this will make it easier to scan.
			pil = Image.open(stream).convert('L')
			
			#Change the global to the new image.
	                panel1_image = ImageTk.PhotoImage(pil)
			#Update the image on the GUI interface.
	                panel1.configure(image=panel1_image)
	                root.update_idletasks()
	
			#Get the width and height of the image for the zbar scanner.
			width, height = pil.size
			#Store the grayscale image in memory as a string.
			raw = pil.tostring()
			#Remove the PIL instance.
			del(pil)
	
			#Define the image as a zbar image.
			image = zbar.Image(width, height, 'Y800', raw)
	
			#Scan the image for QR codes.
			scanner.scan(image)
	
			#Loop through any QR codes found.
			for symbol in image:
				#Tell the user that a QR code was found and any data it had.
				print 'decoded', symbol.type, 'symbol', '"%s"' % symbol.data
				#Catch any errors
	                        try:
					#Send the data from the QR code to the web service.
					result = client.service.AddMeeting(int(symbol.data))
					#If the web service accepted the data.
	                        	if result == True:
						#Tell the user that a meeting was added successfully.
						print 'successfully added meeting for user id ', symbol.data
						#Tell the user that the script will now stop for 10 seconds.
						#This is so the script doesn't scan the QR code multiple times resulting in multiple meetings.
						print 'stop for 10 seconds'
						#Stop scanning.
						canScan = False
						#Run the setCanScanTrueGUI function in 10 seconds.
						threading.Timer(10, setCanScanTrueGUI).start()
					#Remove the result variable from memory.
					del(result)
				#If there were any errors.
				except:
					#Ignore them and carry on.
					pass
			#Delete all variables that were declared in this function.
			del(image)
			del(stream)
			del(raw)
			del(scanner)
			del(width)
			del(height)
			#Run this function again.
			scanCameraGUI()
		#If the user is trying to end the script.
		except KeyboardInterrupt:
			#Destroy the GUI interface.
			root.destroy()
			#Raise the KeyboardInterrupt exception.
			raise
		#Any other errors.
		except:
			#Ignore them.
			pass

#Main loop code without GUI interface.
def scanCamera():
	#Fetch the globals.
	global canScan
	#If scanning is allowed.
	if canScan:
		#Catch any errors.
		try:
			#Define an IO stream to put the image from the camera in.
			#This is so we don't have to write the image to a disk first.
			stream = io.BytesIO()
			
			#Get the Raspberry Pi camera.
			camera = picamera.PiCamera()
			#Set the resolution to 640x460.
			#Higher resoltions will take longer to scan.
			camera.resolution = (640, 460)
			#Take a snapshot then convert it to a JPEG, put the resulting image into the IO stream.
			camera.capture(stream, 'jpeg')
			#Close the camera as we don't need it anymore.
			camera.close()
			#Remove the camera instance from memory.
			del(camera)
			
			#Tell the user whats going on.
			print 'Scanning...'
	
			#Define a zbar scanner which will scan the image for QR codes.
			scanner = zbar.ImageScanner()
	
			#Enable the scanner.
			scanner.parse_config('enable')
	
			#Seek the IO stream to the start.
			stream.seek(0)
	
			#Open the IO stream with the Python Imaging Library (PIL).
			#Convert the image to grayscale as this will make it easier to scan.
			pil = Image.open(stream).convert('L')
	
			#Get the width and height of the image for the zbar scanner.
			width, height = pil.size
			#Store the grayscale image in memory as a string.
			raw = pil.tostring()
			#Remove the PIL instance.
			del(pil)
	
			#Define the image as a zbar image.
			image = zbar.Image(width, height, 'Y800', raw)
	
			#Scan the image for QR codes.
			scanner.scan(image)
	
			#Loop through any QR codes found.
			for symbol in image:
				#Tell the user that a QR code was found and any data it had.
				print 'decoded', symbol.type, 'symbol', '"%s"' % symbol.data
				#Catch any errors
	                        try:
					#Send the data from the QR code to the web service.
					result = client.service.AddMeeting(int(symbol.data))
					#If the web service accepted the data.
	                        	if result == True:
						#Tell the user that a meeting was added successfully.
						print 'successfully added meeting for user id ', symbol.data
						#Tell the user that the script will now stop for 10 seconds.
						#This is so the script doesn't scan the QR code multiple times resulting in multiple meetings.
						print 'stop for 10 seconds'
						#Stop scanning.
						canScan = False
						#Run the setCanScanTrue function in 10 seconds.
						threading.Timer(10, setCanScanTrue).start()
					#Remove the result variable from memory.
					del(result)
				#If there were any errors.
				except:
					#Ignore them and carry on.
					pass
			#Delete all variables that were declared in this function.
			del(image)
			del(stream)
			del(raw)
			del(scanner)
			del(width)
			del(height)
			#Run this function again.
			scanCamera()
		#If there were any errors.
		except:
			#Ignore them.
			pass

#If the user wants to show a GUI window.
if args.show_gui:
	#Make a new window using Tkinter.
        root = tk.Tk()
	#Set the title of the window.
        root.title('Swipe Box - QR')
	#Set the size of the window.
        root.geometry("640x480+0+0")
	#Create a label to contain the image.
        panel1 = tk.Label(root)
	#Expand the label out so it fills the window.
        panel1.pack(side = tk.TOP, fill = tk.BOTH, expand = tk.YES)
	#Wait a second then start scanning.
        root.after(1000, scanCameraGUI)
	#Main loop of the window.
        root.mainloop()
#If the user doen't want a GUI window.
else:
	#Run the recursive scanning function.
        scanCamera()