# Author: James Pagan-Lodge
# Company: Park Side Software
# Project: Swipebox
# Version: 0.1.1.0
# Date: 11/05/2015


# Open the customerID.txt file and extract it into a list using readines()
# using the 'with' construct automatically closes the file when it is finished
# parse the list 'data' to find the last cusID that was added
with open("customerID.txt", "r") as cusIDFile:
	data = cusIDFile.readlines();
	
