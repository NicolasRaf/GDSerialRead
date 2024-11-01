using Godot;
using System;
using System.IO.Ports;
using System.Threading;

public partial class SerialRead : Node2D
{

	private string portName;
	private int baudRate = 9600;
	private SerialPort serialPort;
	private Thread serialThread;
	private bool isRunning = true;
	private string[] portList = SerialPort.GetPortNames();
	private int portIndex = 0;

	// Variável para armazenar os dados lidos
	private string receivedData = "";

	public override void _Ready()
	{
		portName = portList[portIndex];
		ConnectToSerial();
	}

	private void serialPortOpen(SerialPort serialPort){
		serialPort.Open();
		GD.Print("Conectado à porta " + portName);
	}

	private void ConnectToSerial()
	{
		try
		{
			serialPort = new SerialPort(portName, baudRate)
			{
				DtrEnable = true,
				RtsEnable = true
			};
			serialPortOpen(serialPort);

			// Inicia uma thread para ler a porta serial continuamente
			serialThread = new Thread(ReadSerial);
			serialThread.Start();
		}
		catch (Exception ex)
		{
			GD.Print("Erro ao conectar: " + ex.Message);
			portName = portList[portIndex];
		}
	  }
	}

	private void ReadSerial()
	{
		while (isRunning && serialPort.IsOpen)
		{
			try
			{
				if (serialPort.BytesToRead > 0)
				{
					receivedData = serialPort.ReadExisting();
				} else {
					serialPort.Close();
					
				}

				Thread.Sleep(100); // Evita o uso excessivo da CPU
			}
			catch (Exception ex)
			{	
				GD.Print("Erro ao ler dados: " + ex.Message);

			}
		}
	}

	public override void _Process(double delta)
	{
		if(!serialPort.IsOpen){
			Thread.Sleep(1000);			
			ConnectToSerial();
		}

		// Verifica se há dados recebidos e exibe no console do Godot
		if (!string.IsNullOrEmpty(receivedData))
		{
			GD.Print("Dados recebidos: " + receivedData);
			receivedData = ""; // Limpa a variável após o uso
		}
	}

	public override void _ExitTree()
	{
		// Finaliza a leitura e fecha a conexão ao encerrar o nó
		isRunning = false;
		if (serialPort != null && serialPort.IsOpen)
		{
			serialPort.Close();
			GD.Print("Porta serial fechada.");
		}

		// Encerra a thread de leitura
		if (serialThread != null && serialThread.IsAlive)
		{
			serialThread.Join();
		}
	}
}
