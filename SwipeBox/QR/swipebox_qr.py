#!/usr/bin/python
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

parser = ArgumentParser()
parser.add_argument("--show-gui", help="Display a GUI window showing what the camera sees.")
args = parser.parse_args()

url = 'http://192.168.0.100/SwipeBoxServices/SwipeBoxService.svc?wsdl'
client = Client(url)

global canScan
canScan = True

def setCanScanTrueGUI():
	global canScan
	canScan = True
	print '10 seconds have passed, back to work'
	scanCameraGUI()

def setCanScanTrue():
	global canScan
	canScan = True
	print '10 seconds have passed, back to work'
	scanCamera()

def scanCameraGUI():
	global canScan
	if canScan:
		try:
			stream = io.BytesIO()
	
			camera = picamera.PiCamera()
			camera.resolution = (640, 460)
			camera.capture(stream, 'jpeg')
			camera.close()
	
			print 'Scanning...'
	
			scanner = zbar.ImageScanner()
	
			scanner.parse_config('enable')
	
			stream.seek(0)
	
			pil = Image.open(stream).convert('L')
			
	                panel1_image = ImageTk.PhotoImage(pil)
	                panel1.configure(image=panel1_image)
	                root.update_idletasks()
	
			width, height = pil.size
			raw = pil.tostring()
	
			image = zbar.Image(width, height, 'Y800', raw)
	
			scanner.scan(image)
	
			for symbol in image:
				print 'decoded', symbol.type, 'symbol', '"%s"' % symbol.data
	                        result = client.service.AddMeeting(symbol.data)
	                        if result == True:
					print 'successfully added meeting for user id ', symbol.data
					print 'stop for 10 seconds'
					canScan = False
					threading.Timer(10, setCanScanTrueGUI).start()
			del(image)
			del(stream)
			scanCameraGUI()
		except:
			raise

def scanCamera():
	global canScan
	if canScan:
		try:
			stream = io.BytesIO()
	
			camera = picamera.PiCamera()
			camera.resolution = (640, 460)
			camera.capture(stream, 'jpeg')
			camera.close()
	
			print 'Scanning...'
	
			scanner = zbar.ImageScanner()
	
			scanner.parse_config('enable')
	
			stream.seek(0)
	
			pil = Image.open(stream).convert('L')
	
			width, height = pil.size
			raw = pil.tostring()
	
			image = zbar.Image(width, height, 'Y800', raw)
	
			scanner.scan(image)
	
			for symbol in image:
				print 'decoded', symbol.type, 'symbol', '"%s"' % symbol.data
	                        result = client.service.AddMeeting(symbol.data)
	                        if result == True:
					print 'successfully added meeting for user id ', symbol.data
					print 'stop for 10 seconds'
					canScan = False
					threading.Timer(10, setCanScanTrue).start()
			del(image)
			del(stream)
			scanCamera()
		except:
			raise

if args.show_gui:
        root = tk.Tk()
        root.title('Swipe Box - QR')
        root.geometry("640x480+0+0")
        panel1 = tk.Label(root)
        panel1.pack(side = tk.TOP, fill = tk.BOTH, expand = tk.YES)
        root.after(1000, scanCameraGUI)
        root.mainloop()
else:
        scanCamera()
