using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CollisionToArduino : MonoBehaviour
{
    public string portName = "COM4"; // Adjust the COM port name
    public int baudRate = 9600;

    private SerialPort arduinoPort;

    private void Start()
    {
        arduinoPort = new SerialPort(portName, baudRate);
        arduinoPort.Open();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            print("Collided");
            // Send command to Arduino to turn on vibration motor
            arduinoPort.Write("1");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            print("EXIT");
            // Send command to Arduino to turn off vibration motor
            arduinoPort.Write("0");
        }
    }

    private void OnApplicationQuit()
    {
        arduinoPort.Close();
    }
}
