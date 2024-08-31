using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    [SerializeField]
    private string arduinoPortName = "COM8"; // Change to your Arduino's port
    [SerializeField]
    private int arduinoBaudRate = 9600; // Change to your Arduino's baud rate

    // Create a SerialPort object for Arduino communication
    private SerialPort arduinoPort;

    private bool objectSelected = false;

    private void Start()
    {
        // Initialize the SerialPort for Arduino communication
        arduinoPort = new SerialPort(arduinoPortName, arduinoBaudRate);
        arduinoPort.Open();
    }

    private void Update()
    {
        // Handle data from multiple game objects here
    }



    private void SendToArduino(int value)
    {
        if (arduinoPort.IsOpen)
        {
            string dataToSend = value.ToString();
            arduinoPort.Write(dataToSend);
            Debug.Log("Sent to Arduino: " + dataToSend); // Add this line for debugging
        }
    }



    // Create a method for game objects to send data to the ArduinoCommunication script
    public void SendDataToArduino(int value)
    {
        SendToArduino(value);
    }


    private void OnDestroy()
    {
        // Close the SerialPort when the script is destroyed
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}
