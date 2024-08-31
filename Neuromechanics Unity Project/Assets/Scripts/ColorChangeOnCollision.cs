/*
using UnityEngine;
using System.IO.Ports;

public class ColorChangeOnCollision : MonoBehaviour
{
    public GameObject mainObject;
    public GameObject targetObject;
    private bool collisionOccurred = false;
    private float delayTimer = 0f;
    public float delayDuration = 5f;

    private Color originalColor;
    private Renderer mainRenderer;

    private void Start()
    {
        mainRenderer = mainObject.GetComponent<Renderer>();
        originalColor = mainRenderer.material.color;
    }

    private void Update()
    {
        if (collisionOccurred)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayDuration)
            {
                mainRenderer.material.color = Color.green;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = false;
            delayTimer = 0f;
            mainRenderer.material.color = originalColor;
        }
    }
}

using UnityEngine;
using System.IO.Ports;

public class ColorChangeOnCollision : MonoBehaviour
{
    public GameObject mainObject;
    public GameObject targetObject;

    private SerialPort serialPort;
    private bool collisionOccurred = false;

    private Color originalColor;
    private Renderer mainRenderer;

    private void Start()
    {
        mainRenderer = mainObject.GetComponent<Renderer>();
        originalColor = mainRenderer.material.color;

        // Initialize the serial port
        serialPort = new SerialPort("COM8", 9600); // Change "COM3" to your Arduino's serial port
        serialPort.Open();
    }

    private void Update()
    {
        if (collisionOccurred)
        {
            mainRenderer.material.color = Color.green;

            // Send 1 to Arduino when a collision occurs
            serialPort.Write("1");
        }
        else
        {
            mainRenderer.material.color = originalColor;

            // Send 0 to Arduino when no collision is detected
            serialPort.Write("0");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = false;
        }
    }

    private void OnDestroy()
    {
        // Close the serial port when the script is destroyed
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}


using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ColorChangeOnCollision : MonoBehaviour
{
    public GameObject mainObject;
    public GameObject targetObject;

    public SerialPort serialPort;
    private bool collisionOccurred = false;

    private Color originalColor;
    private Renderer mainRenderer;

    public float delayDuration = 2.0f; // Adjust this as needed

    private void Start()
    {
        mainRenderer = mainObject.GetComponent<Renderer>();
        originalColor = mainRenderer.material.color;

        // Initialize the serial port
        serialPort = new SerialPort("COM8", 9600); // Change "COM3" to your Arduino's serial port
        serialPort.Open();
    }

    private void Update()
    {
        if (collisionOccurred)
        {
            // Send 1 to Arduino immediately upon collision
            serialPort.Write("1");
        }
        else
        {
            // Send 0 to Arduino immediately upon exiting the collision
            serialPort.Write("0");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = true;
            StartCoroutine(ChangeColorWithDelay());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = false;
            // Reset color immediately upon exiting the collision
            mainRenderer.material.color = originalColor;
        }
    }

    private IEnumerator ChangeColorWithDelay()
    {
        yield return new WaitForSeconds(delayDuration);

        // Change the color to green after the delay
        mainRenderer.material.color = Color.green;
    }

    private void OnDestroy()
    {
        // Close the serial port when the script is destroyed
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}

/*
using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ColorChangeOnCollision : MonoBehaviour
{
    public GameObject targetObject;

    public string arduinoPortName = "COM8"; // Change to your Arduino's port
    public int arduinoBaudRate = 9600; // Change to your Arduino's baud rate

    private SerialPort serialPort;
    private bool collisionOccurred = false;

    private Color originalColor;
    private Renderer mainRenderer;

    public float delayDuration = 2.0f; // Adjust this as needed

    private void Start()
    {
        mainRenderer = GetComponent<Renderer>();
        originalColor = mainRenderer.material.color;

        // Initialize the serial port
        serialPort = new SerialPort(arduinoPortName, arduinoBaudRate);
        serialPort.Open();
    }

    private void Update()
    {
        if (collisionOccurred)
        {
            // Send "T1" to Arduino immediately upon collision
            SendToArduino("T1");
        }
        else
        {
            // Send "T0" to Arduino immediately upon exiting the collision
            SendToArduino("T0");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = true;
            StartCoroutine(ChangeColorWithDelay());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = false;
            // Reset color immediately upon exiting the collision
            mainRenderer.material.color = originalColor;
        }
    }

    private IEnumerator ChangeColorWithDelay()
    {
        yield return new WaitForSeconds(delayDuration);

        // Change the color to green after the delay
        mainRenderer.material.color = Color.green;
    }

    // Method to send data to Arduino through the SerialPort
    private void SendToArduino(string data)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Write(data);
        }
    }

    private void OnDestroy()
    {
        // Close the serial port when the script is destroyed
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
*/

using UnityEngine;
using System.Collections;

public class ColorChangeOnCollision : MonoBehaviour
{
    public GameObject mainObject;
    public GameObject targetObject;

    private bool collisionOccurred = false;

    private Color originalColor;
    private Renderer mainRenderer;

    public float delayDuration = 2.0f; // Adjust this as needed

    private ArduinoCommunication arduinoCommunication;

    private void Start()
    {
        mainRenderer = mainObject.GetComponent<Renderer>();
        originalColor = mainRenderer.material.color;

        // Find the ArduinoCommunication script in the scene
        arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
    }

    private void Update()
    {
        if (collisionOccurred)
        {
            // Send 1 to the central ArduinoCommunication script upon collision
            arduinoCommunication.SendDataToArduino(1);
        }
        else
        {
            // Send 0 to the central ArduinoCommunication script upon exiting the collision
            arduinoCommunication.SendDataToArduino(0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = true;
            StartCoroutine(ChangeColorWithDelay());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            collisionOccurred = false;
            // Reset color immediately upon exiting the collision
            mainRenderer.material.color = originalColor;
        }
    }

    private IEnumerator ChangeColorWithDelay()
    {
        yield return new WaitForSeconds(delayDuration);

        // Change the color to green after the delay
        mainRenderer.material.color = Color.green;
    }
}

