using Godot;
using System;
using System.IO.Ports;
using System.Threading;

// DON'T CHANGE THIS SCRIPT IF YOU DON'T NEED TO 

/// <summary>
/// SerialRead is a class responsible for establishing and managing a serial connection 
/// with an external device and continuously reading data from the specified serial port.
/// The data read is then shared with other scripts in the Godot project.
/// </summary>
public partial class SerialRead : Node
{
	// Baud rate for the serial connection, commonly set to 9600 (adjust as needed for your device).
	private int baudRate = 9600;

	// Instance of SerialPort to manage communication with the external device via serial port.
	private SerialPort serialPort;

	// Thread for continuously reading incoming data from the serial port in the background.
	private Thread serialThread;

	// Reference to a node in the Godot scene where the received data will be sent.
	private Node dataReceiverNode;

	// Boolean flag to control the execution state of the serial reading thread.
	private bool isRunning = false;

	// Stores the name of the current serial port to which the program is attempting to connect.
	private string portName;

	// Array that lists all available serial ports on the system.
	private string[] portList = SerialPort.GetPortNames();

	// Index of the currently used serial port within the portList array.
	private int portIndex = 0;

	// Flag indicating whether to attempt reconnecting if a connection fails.
	private bool tryConnect;

	// String variable that holds data read from the serial port.
	public string receivedData = "";

	// Maximum time in milliseconds to wait for data after opening a connection.
	private const int connectionTimeout = 3000;

	/// <summary>
	/// Called when the node enters the scene tree for the first time. Initializes the
	/// serial port name from the list and attempts to establish a serial connection.
	/// </summary>
	public override void _Ready()
	{   
		// Assigns the DataReceiver node.
		dataReceiverNode = GetNode("/root/DataReceiver") as Node;
		StartReading();
	}

	/// <summary>
	/// Opens the provided SerialPort object to begin data transmission with the device.
	/// Should only be called if the SerialPort instance is configured successfully.
	/// </summary>
	/// <param name="serialPort">Configured SerialPort instance.</param>
	private void SerialPortOpen(SerialPort serialPort)
	{
		// Opens the serial connection and prints a message indicating successful connection.
		serialPort.Open();
		GD.Print("Connected to port " + portName);
	}

	/// <summary>
	/// Attempts to connect to the current serial port in portList.
	/// If successful, a new thread is started to read data; if not, it tries the next port.
	/// </summary>
	private void ConnectToSerial()
	{
		if (isRunning == false) return; // If not running, exit the function.

		try
		{
			// Get the name of the current port.
			portName = portList[portIndex];
			
			// Initializes and configures the SerialPort instance with DTR and RTS signals enabled.
			serialPort = new SerialPort(portName, baudRate)
			{
				DtrEnable = true,
				RtsEnable = true
			};
			
			// Opens the serial connection.
			SerialPortOpen(serialPort);

			// Starts a new thread to continuously read data from the serial port with timeout.
			serialThread = new Thread(() => ReadSerialWithTimeout(connectionTimeout));
			serialThread.Start();
		}
		catch (Exception ex)
		{
			// Prints an error message if the connection fails.
			GD.Print("Connection error: " + ex.Message);

			// Sets the flag to attempt reconnection and tries the next available port.
			tryConnect = true;
			serialPort.Close();
			portIndex = (portIndex + 1) % portList.Length; // Move to the next port.
			portName = portList[portIndex]; // Update the port name.
		}
	}

	/// <summary>
	/// Continuously reads data from the serial port while the connection is active.
	/// Checks for incoming data within a specified timeout interval.
	/// </summary>
	/// <param name="timeout">The maximum time to wait for data before trying the next port.</param>
	private void ReadSerialWithTimeout(int timeout)
	{
		DateTime startTime = DateTime.Now; // Record the start time.

		while (isRunning && serialPort.IsOpen)
		{
			try
			{
				// Checks if data is available and reads it if true.
				if (serialPort.BytesToRead > 0)
				{
					receivedData = serialPort.ReadExisting(); // Read the incoming data.
					ReadSerial(); // Proceed to continuous reading.
					return;
				}

				// Check if the timeout has been reached.
				if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
				{
					GD.Print("No data received on " + portName + ". Trying next port.");
					tryConnect = true; // Set the flag to try the next port.
					serialPort.Close(); 
					portIndex++; 
					return;
				}

				Thread.Sleep(100); // Reduces CPU usage.
			}
			catch (Exception ex)
			{
				// Prints an error message if reading fails.
				GD.Print("Error reading data: " + ex.Message);
			}
		}
	}

	/// <summary>
	/// Continuously reads data from the serial port while the connection is active.
	/// Data is stored in receivedData to be accessed by other scripts.
	/// </summary>
	private void ReadSerial()
	{
		while (isRunning && serialPort.IsOpen)
		{
			try
			{
				// Check if there are bytes to read and read them.
				if (serialPort.BytesToRead > 0)
				{
					receivedData = serialPort.ReadExisting(); // Read the incoming data.
				}

				Thread.Sleep(100); // Reduces CPU usage.
			}
			catch (Exception ex)
			{
				// Prints an error message if reading fails.
				GD.Print("Error reading data: " + ex.Message);
			}
		}
	}

	/// <summary>
	/// Runs every frame, checking the connection state and processing received data.
	/// Attempts reconnection if disconnected and updates the DataReceiver node.
	/// </summary>
	/// <param name="delta">Elapsed time since the last frame (in seconds).</param>
	async public override void _Process(double delta)
	{
		if (!serialPort.IsOpen && tryConnect)
		{	
			tryConnect = false; // Reset the reconnection flag.
			GD.Print("Trying to reconnect in 2 seconds");
			await ToSignal(GetTree().CreateTimer(2.0f), "timeout"); // Wait for 2 seconds before reconnecting.
			ConnectToSerial(); // Attempt to reconnect.
		}

		// If receivedData is not empty, update the dataReceiver node.
		if (!string.IsNullOrEmpty(receivedData))
		{ 
			if (dataReceiverNode != null)
			{
				dataReceiverNode.Set("data", receivedData); // Send the received data to the receiver node.
			}
			receivedData = ""; // Clear the received data after processing.
		} 
	}

	/// <summary>
	/// Stops the reading thread and closes the serial connection when the node exits
	/// the scene tree, ensuring resources are properly released.
	/// </summary>
	public override void _ExitTree()
	{
		isRunning = false; 
		SetProcess(false); // Disable the processing of this node.
		
		if (serialPort != null && serialPort.IsOpen)
		{
			serialPort.Close(); // Close the serial port.
			GD.Print("Serial port closed.");
		}

		if (serialThread != null && serialThread.IsAlive)
		{
			serialThread.Join(); // Wait for the reading thread to finish.
		}

		dataReceiverNode.Set("data", ""); // Clear any data in the receiver node.
		receivedData = ""; // Reset the received data.

		GD.Print("Read Stopped"); // Print a message indicating that reading has stopped.
	}

	/// <summary>
	/// Starts the serial data reading process if it is not already active.
	/// Initializes necessary flags and attempts a serial connection.
	/// </summary>
	private void StartReading()
	{
		if (isRunning) 
		{
			GD.Print("Reading already in progress."); 
			return; // Exit the function.
		}

		SetProcess(true); // Enable processing for this node.
		isRunning = true; 
		ConnectToSerial(); // Attempt to connect to the serial port.
		
		GD.Print("Starting reading"); 
	}
}
