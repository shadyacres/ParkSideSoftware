# Author: James Pagan-Lodge
# Company: Park Side Software
# Project: Swipebox
# Version: 0.1.1.1
# Date: 11/05/2015

import logging
from suds.client import Client


# Open the customerID.txt file and extract it into a list using readines()
# using the 'with' construct automatically closes the file when it is finished
# parse the list 'data' to find the last cusID that was added
with open("customerID.txt", "r") as cusIDFile:
	data = cusIDFile.readlines();
	print(data)


# Enable logging for the web service calls
logging.basicConfig(level=logging.INFO)
logging.getLogger('suds.client').setLevel(logging.DEBUG)

# Use SUDS library to consume SOAP web service calls
url = ''
client = Client(url)
# pass the cusID to the web service
# result = client.service.PASS_THE_CUS_ID(CUSID)
# print(result)
