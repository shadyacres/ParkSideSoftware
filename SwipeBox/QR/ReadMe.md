Python script to scan QR codes for user ids and then send them to the web service.

Requirements:
ZBar barcode reader (needs to be compiled on the Raspberry Pi) - http://zbar.sourceforge.net/
python-picamera library
'''
sudo apt-get install python-picamera
'''
python-pip
'''
sudo apt-get install python-pip
'''
suds-jurko
'''
sudo pip install suds-jurko
'''

Usage:
'''
python swipebox_qr.py
'''
To show what the camera sees:
'''
python swipebox_qr.py --show-gui 1
'''