/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.


using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Oculus.Interaction
{
    /// <summary>
    /// This component makes it possible to connect Interactables in the
    /// inspector to Unity Events that are broadcast on state changes.
    /// </summary>
    public class InteractableUnityEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        #region Properties

        public UnityEvent WhenHover => _whenHover;
        public UnityEvent WhenUnhover => _whenUnhover;
        public UnityEvent WhenSelect => _whenSelect;
        public UnityEvent WhenUnselect => _whenUnselect;
        public UnityEvent WhenInteractorViewAdded => _whenInteractorViewAdded;
        public UnityEvent WhenInteractorViewRemoved => _whenInteractorViewRemoved;
        public UnityEvent WhenSelectingInteractorViewAdded => _whenSelectingInteractorViewAdded;
        public UnityEvent WhenSelectingInteractorViewRemoved => _whenSelectingInteractorViewRemoved;

        #endregion

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenUnhover.Invoke();
                    }

                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                    }

                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenSelect.Invoke();
                    }

                    break;
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            WhenInteractorViewAdded.Invoke();
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            WhenInteractorViewRemoved.Invoke();
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            WhenSelectingInteractorViewAdded.Invoke();
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            WhenSelectingInteractorViewRemoved.Invoke();
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        #endregion
    }
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Oculus.Interaction
{
    public class InteractableUnityEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 1000 milliseconds
            unityEvent.Invoke();
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenUnhover.Invoke();
                    }
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(DelayedEvent(_whenSelect)); // Delay event invocation
                    }
                    break;
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewAdded)); // Delay event invocation
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewRemoved)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewAdded)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewRemoved)); // Delay event invocation
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        #endregion
    }
}
/*
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class InteractableUnityEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        [SerializeField]
        public string arduinoPortName = "COM8"; // Change to your Arduino's port
        [SerializeField]
        public int arduinoBaudRate = 9600; // Change to your Arduino's baud rate

        // Create a SerialPort object for Arduino communication
        private SerialPort arduinoPort;

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Initialize the SerialPort for Arduino communication
            arduinoPort = new SerialPort(arduinoPortName, arduinoBaudRate);
            arduinoPort.Open();
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 1000 milliseconds
            unityEvent.Invoke();
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenUnhover.Invoke();
                        // Send 0 to Arduino when unhovered
                        SendToArduino(0);
                    }
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                        // Send 0 to Arduino when unselected
                        SendToArduino(0);
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(DelayedEvent(_whenSelect)); // Delay event invocation
                        // Send 1 to Arduino when selected
                        SendToArduino(1);
                    }
                    break;
            }
        }

       // Method to send data to Arduino through the SerialPort
        private void SendToArduino(int value)
        {
            if (arduinoPort.IsOpen)
            {
                arduinoPort.Write(value.ToString());
            }
        }

        protected virtual void OnDestroy()
        {
            // Close the SerialPort when the script is destroyed
            if (arduinoPort != null && arduinoPort.IsOpen)
            {
                arduinoPort.Close();
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewAdded)); // Delay event invocation
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewRemoved)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewAdded)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewRemoved)); // Delay event invocation
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        #endregion
    }
}


using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class InteractableUnityEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        [SerializeField]
        public string arduinoPortName = "COM8"; // Change to your Arduino's port
        [SerializeField]
        public int arduinoBaudRate = 9600; // Change to your Arduino's baud rate

        // Create a SerialPort object for Arduino communication
        private SerialPort arduinoPort;

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Initialize the SerialPort for Arduino communication
            arduinoPort = new SerialPort(arduinoPortName, arduinoBaudRate);
            arduinoPort.Open();
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenUnhover.Invoke();
                        // Send 0 to Arduino when unhovered
                        SendToArduino("B0");
                    }
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                        // Send 0 to Arduino when unselected
                        SendToArduino("B0");
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(DelayedEvent(_whenSelect)); // Delay event invocation
                        // Send 1 to Arduino when selected
                        SendToArduino("B1");
                    }
                    break;
            }
        }

        // Method to send data to Arduino through the SerialPort
        private void SendToArduino(string data)
        {
            if (arduinoPort.IsOpen)
            {
                arduinoPort.Write(data);
            }
        }

        protected virtual void OnDestroy()
        {
            // Close the SerialPort when the script is destroyed
            if (arduinoPort != null && arduinoPort.IsOpen)
            {
                arduinoPort.Close();
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewAdded)); // Delay event invocation
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewRemoved)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewAdded)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewRemoved)); // Delay event invocation
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        #endregion
    }
}





using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class InteractableUnityEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenHover;
        [SerializeField]
        private UnityEvent _whenUnhover;
        [SerializeField]
        private UnityEvent _whenSelect;
        [SerializeField]
        private UnityEvent _whenUnselect;
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        [SerializeField]
        public string arduinoPortName = "COM8"; // Change to your Arduino's port
        [SerializeField]
        public int arduinoBaudRate = 9600; // Change to your Arduino's baud rate

        // Create a SerialPort object for Arduino communication
        private SerialPort arduinoPort;

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Initialize the SerialPort for Arduino communication
            arduinoPort = new SerialPort(arduinoPortName, arduinoBaudRate);
            arduinoPort.Open();
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenUnhover.Invoke();
                        // Send 0 to Arduino when unhovered
                        SendToArduino(0);
                    }
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                        // Send 0 to Arduino when unselected
                        SendToArduino(0);
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(DelayedEvent(_whenSelect)); // Delay event invocation
                        // Send 1 to Arduino when selected
                        SendToArduino(1);
                    }
                    break;
            }
        }

       // Method to send data to Arduino through the SerialPort
        private void SendToArduino(int value)
        {
            if (arduinoPort.IsOpen)
            {
                arduinoPort.Write(value.ToString());
            }
        }

        protected virtual void OnDestroy()
        {
            // Close the SerialPort when the script is destroyed
            if (arduinoPort != null && arduinoPort.IsOpen)
            {
                arduinoPort.Close();
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewAdded)); // Delay event invocation
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewRemoved)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewAdded)); // Delay event invocation
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewRemoved)); // Delay event invocation
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        #endregion
    }
}
*/