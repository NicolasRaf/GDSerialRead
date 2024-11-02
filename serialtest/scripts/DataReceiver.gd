extends Node

"""
HOW TO USE!

This node is responsible for receiving and processing the data sent from the SerialRead node.
To utilize this functionality, ensure that both this scene (DataReceiver.gd) and the SerialRead.cs script 
are set as Global (Autoload) in your project.

To interact with the serial data:
- Call `SerialRead.StartReading()` to begin reading data from the serial port.
- Call `SerialRead._ExitTree()` to stop reading data and properly close the connection.

* The script automatically starts reading data when the node is initialized.
* If you stop the reading process, it will not resume until you explicitly call `StartReading()` again.
* The code attempts to connect to a serial port only when data is being sent.
	- If access to the port is unavailable or if no data is received for 3 seconds, 
	  the script will attempt to connect to the next available port.

This node serves as a manager for the incoming data, allowing you to process and store it as needed.
You can create additional variables to hold the parsed or transformed data as necessary.

Note: This functionality only works if both this node and the SerialRead.cs script 
are configured as Autoloads in your project settings.
"""

# Variable responsible for storing the data received from the serial port
var data: String; 

func _process(delta):
	# Check if there is any new data received from the serial port
	if data:
		print("Received Data: ", data)  # Print the received data to the output console
