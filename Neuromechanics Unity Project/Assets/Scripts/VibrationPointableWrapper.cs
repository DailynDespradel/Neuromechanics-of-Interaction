/*
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class VibrationPointableWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IPointable))]
        private MonoBehaviour _pointable;
        private IPointable Pointable;

        private HashSet<int> _pointers;

        [SerializeField]
        private UnityEvent _whenRelease;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenMove;
        [SerializeField]
        private UnityEvent _whenCancel;

        public UnityEvent WhenRelease => _whenRelease;

        public UnityEvent WhenHover => _whenHover;
        public UnityEvent WhenUnhover => _whenUnhover;
        public UnityEvent WhenSelect => _whenSelect;
        public UnityEvent WhenUnselect => _whenUnselect;
        public UnityEvent WhenMove => _whenMove;
        public UnityEvent WhenCancel => _whenCancel;

        protected bool _started = false;

        private SerialPort arduinoPort;
        public string portName = "COM4"; // Adjust the COM port name
        public int baudRate = 9600;

        private bool isVibrating = false;
        public float vibrationDuration = 1.0f; // Set the vibration duration in seconds
        private float vibrationTimer = 0.0f;

        protected virtual void Awake()
        {
            Pointable = _pointable as IPointable;
        }

        protected virtual void Start()
        {
            arduinoPort = new SerialPort(portName, baudRate);
            arduinoPort.Open();

            this.BeginStart(ref _started);
            Assert.IsNotNull(Pointable);
            _pointers = new HashSet<int>();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                Pointable.WhenPointerEventRaised += HandlePointerEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                Pointable.WhenPointerEventRaised -= HandlePointerEventRaised;
            }
        }

        private void Update()
        {
            if (isVibrating)
            {
                vibrationTimer -= Time.deltaTime;
                if (vibrationTimer <= 0)
                {
                    arduinoPort.Write("0"); // Turn off vibration motor
                    isVibrating = false;
                }
            }
        }

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Hover:
                    _whenHover.Invoke();
                    _pointers.Add(evt.Identifier);
                    break;
                case PointerEventType.Unhover:
                    _whenUnhover.Invoke();
                    _pointers.Remove(evt.Identifier);
                    break;
                case PointerEventType.Select:
                    _whenSelect.Invoke();
                    arduinoPort.Write("1"); // Turn on vibration motor
                    isVibrating = true;
                    vibrationTimer = vibrationDuration;
                    break;
                case PointerEventType.Unselect:
                    if (_pointers.Contains(evt.Identifier))
                    {
                        _whenRelease.Invoke();
                        arduinoPort.Write("0"); // Turn off vibration motor
                        isVibrating = false;
                    }
                    _whenUnselect.Invoke();
                    break;
                case PointerEventType.Move:
                    _whenMove.Invoke();
                    break;
                case PointerEventType.Cancel:
                    _whenCancel.Invoke();
                    _pointers.Remove(evt.Identifier);
                    break;
            }
        }

        private void OnApplicationQuit()
        {
            arduinoPort.Close();
        }

        #region Inject

        public void InjectAllPointableUnityEventWrapper(IPointable pointable)
        {
            InjectPointable(pointable);
        }

        public void InjectPointable(IPointable pointable)
        {
            _pointable = pointable as MonoBehaviour;
            Pointable = pointable;
        }

        #endregion
    }
}
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class VibrationPointableWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IPointable))]
        private MonoBehaviour _pointable;
        private IPointable Pointable;

        private HashSet<int> _pointers;

        [SerializeField]
        private UnityEvent _whenRelease;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenMove;
        [SerializeField]
        private UnityEvent _whenCancel;

        public UnityEvent WhenRelease => _whenRelease;

        public UnityEvent WhenHover => _whenHover;
        public UnityEvent WhenUnhover => _whenUnhover;
        public UnityEvent WhenSelect => _whenSelect;
        public UnityEvent WhenUnselect => _whenUnselect;
        public UnityEvent WhenMove => _whenMove;
        public UnityEvent WhenCancel => _whenCancel;

        protected bool _started = false;

        private SerialPort arduinoPort;
        public string portName = "COM4"; // Adjust the COM port name
        public int baudRate = 9600;

        private bool isVibrating = false;
        public float vibrationDuration = 1.0f; // Set the vibration duration in seconds
        private float vibrationTimer = 0.0f;

        // References to the target objects
        public GameObject targetObject1;
        public GameObject targetObject2;

        public float vibrationStrength = 0.5f; // Vibration strength for all objects

        protected virtual void Awake()
        {
            Pointable = _pointable as IPointable;
        }

        protected virtual void Start()
        {
            arduinoPort = new SerialPort(portName, baudRate);
            arduinoPort.Open();

            this.BeginStart(ref _started);
            Assert.IsNotNull(Pointable);
            _pointers = new HashSet<int>();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                Pointable.WhenPointerEventRaised += HandlePointerEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                Pointable.WhenPointerEventRaised -= HandlePointerEventRaised;
            }
        }

        private void Update()
        {
            if (isVibrating)
            {
                vibrationTimer -= Time.deltaTime;
                if (vibrationTimer <= 0)
                {
                    arduinoPort.Write("0"); // Turn off vibration motor
                    isVibrating = false;
                }
            }
        }

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Hover:
                    _whenHover.Invoke();
                    _pointers.Add(evt.Identifier);
                    break;
                case PointerEventType.Unhover:
                    _whenUnhover.Invoke();
                    _pointers.Remove(evt.Identifier);
                    break;
                case PointerEventType.Select:
                    _whenSelect.Invoke();
                    arduinoPort.Write("1"); // Turn on vibration motor
                    isVibrating = true;
                    vibrationTimer = vibrationDuration;
                    break;
                case PointerEventType.Unselect:
                    if (_pointers.Contains(evt.Identifier))
                    {
                        _whenRelease.Invoke();
                        arduinoPort.Write("0"); // Turn off vibration motor
                        isVibrating = false;
                    }
                    _whenUnselect.Invoke();
                    break;
                case PointerEventType.Move:
                    _whenMove.Invoke();
                    break;
                case PointerEventType.Cancel:
                    _whenCancel.Invoke();
                    _pointers.Remove(evt.Identifier);
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == gameObject || collision.gameObject == targetObject1 || collision.gameObject == targetObject2)
            {
                StartVibration();
            }
        }

        private void StartVibration()
        {
            isVibrating = true;
            vibrationTimer = vibrationDuration;

            arduinoPort.Write("1"); // Turn on vibration motor for main object

            if (targetObject1 != null)
            {
                Rigidbody targetRigidbody1 = targetObject1.GetComponent<Rigidbody>();
                if (targetRigidbody1 != null)
                {
                    targetRigidbody1.AddForce(Random.insideUnitSphere * vibrationStrength, ForceMode.Impulse);
                }
            }

            if (targetObject2 != null)
            {
                Rigidbody targetRigidbody2 = targetObject2.GetComponent<Rigidbody>();
                if (targetRigidbody2 != null)
                {
                    targetRigidbody2.AddForce(Random.insideUnitSphere * vibrationStrength, ForceMode.Impulse);
                }
            }
        }

        private void OnApplicationQuit()
        {
            arduinoPort.Close();
        }

        #region Inject

        public void InjectAllPointableUnityEventWrapper(IPointable pointable)
        {
            InjectPointable(pointable);
        }

        public void InjectPointable(IPointable pointable)
        {
            _pointable = pointable as MonoBehaviour;
            Pointable = pointable;
        }

        #endregion
    }
}

