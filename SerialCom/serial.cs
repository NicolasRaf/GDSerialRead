using System;
using System.IO.Ports;
using System.Threading;

class Program
{
    static void Main()
    {
        string portName = "COM8";
        int baudRate = 9600;

        SerialPort serialPort = new SerialPort(portName, baudRate)
        {
            DtrEnable = true,
            RtsEnable = true
        };

        try
        {
            serialPort.Open();
            Console.WriteLine("Conectado à porta " + portName);

            while (true)
            {
                if (serialPort.BytesToRead > 0)
                {
                    string data = serialPort.ReadExisting();
                    Console.WriteLine("Dados recebidos: " + data);
                }
                Thread.Sleep(100); // Evita uso excessivo de CPU
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao conectar: " + ex.Message);
        }
        finally
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Porta serial fechada.");
            }
        }
    }
}
