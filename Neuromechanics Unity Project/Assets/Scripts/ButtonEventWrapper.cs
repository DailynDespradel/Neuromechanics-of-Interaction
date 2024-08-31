/*
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

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

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                        // Send 0 to ArduinoCommunication script when unhovered
                        SendToArduinoCommunication(0);
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
                        // Send 0 to ArduinoCommunication script when unselected
                        SendToArduinoCommunication(0);
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(DelayedEvent(_whenSelect)); // Delay event invocation
                        // Send 1 to ArduinoCommunication script when selected
                        SendToArduinoCommunication(1);
                    }
                    break;
            }
        }

        // Method to send data to ArduinoCommunication script
        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
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
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

        protected bool _started = false;
        private bool isContinuousSending = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                        // Send 0 to ArduinoCommunication script when unhovered
                        SendToArduinoCommunication(0);
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
                        // Send 0 to ArduinoCommunication script when unselected
                        SendToArduinoCommunication(0);
                        // Stop continuous sending when unselected
                        StopContinuousSending();
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        _whenSelect.Invoke();
                        // Start sending continuous values to ArduinoCommunication script when selected
                        StartContinuousSending(1);
                    }
                    break;
            }
        }

        // Method to send data to ArduinoCommunication script
        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
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

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        private void StartContinuousSending(int value)
        {
            if (!isContinuousSending)
            {
                isContinuousSending = true;
                StartCoroutine(ContinuousSendingCoroutine(value));
            }
        }

        private IEnumerator ContinuousSendingCoroutine(int value)
        {
            while (isContinuousSending)
            {
                SendToArduinoCommunication(value);
                yield return null;
            }
        }

        private void StopContinuousSending()
        {
            isContinuousSending = false;
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
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

        protected bool _started = false;
        private bool isContinuousSending = false;
        private MeshRenderer meshRenderer;
        private Color originalColor;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                originalColor = meshRenderer.material.color;
            }
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                        // Send 0 to ArduinoCommunication script when unhovered
                        SendToArduinoCommunication(0);
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
                        // Send 0 to ArduinoCommunication script when unselected
                        SendToArduinoCommunication(0);
                        // Stop continuous sending when unselected
                        StopContinuousSending();
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(SelectDelay());
                    }
                    break;
            }
        }

        private IEnumerator SelectDelay()
        {
            yield return new WaitForSeconds(2.5f); // Delay for 2.5 seconds
            _whenSelect.Invoke(); // Invoke the select event after the delay
                                  // Start sending continuous values to ArduinoCommunication script when selected
            StartContinuousSending(1);
        }


        // Method to send data to ArduinoCommunication script
        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
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

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        private IEnumerator ColorChangeDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            if (meshRenderer != null)
            {
                // Restore the original material color
                meshRenderer.material.color = originalColor;
            }
        }

        private void StartContinuousSending(int value)
        {
            if (!isContinuousSending)
            {
                isContinuousSending = true;
                StartCoroutine(ContinuousSendingCoroutine(value));
            }
        }

        private IEnumerator ContinuousSendingCoroutine(int value)
        {
            while (isContinuousSending)
            {
                SendToArduinoCommunication(value);
                yield return null;
            }
        }

        private void StopContinuousSending()
        {
            isContinuousSending = false;
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
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

        protected bool _started = false;
        private bool isSelected = false; // Added variable to keep track of the selected state
        private bool isContinuousSending = false;
        private MeshRenderer meshRenderer;
        private Color originalColor;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer is null)
            {
                originalColor = meshRenderer.material.color;
            }
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                        isSelected = false; // Update selected state when unselected
                        StopContinuousSending(); // Stop continuous sending when unselected
                        SendToArduinoCommunication(0);
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
                        isSelected = false; // Update selected state when unselected
                        StopContinuousSending(); // Stop continuous sending when unselected
                        SendToArduinoCommunication(0);
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(SelectDelay());
                    }
                    isSelected = true; // Update selected state when selected
                    SendToArduinoCommunication(1);
                    StartContinuousSending(1);
                    break;
            }
        }

        private IEnumerator SelectDelay()
        {
            yield return new WaitForSeconds(2.5f);
            _whenSelect.Invoke();
        }

        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
            }
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewAdded));
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenInteractorViewRemoved));
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewAdded));
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            StartCoroutine(DelayedEvent(_whenSelectingInteractorViewRemoved));
        }

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f);
            unityEvent.Invoke();
        }

        private void StartContinuousSending(int value)
        {
            if (!isContinuousSending)
            {
                isContinuousSending = true;
                StartCoroutine(ContinuousSendingCoroutine(value));
            }
        }

        private IEnumerator ContinuousSendingCoroutine(int value)
        {
            while (isContinuousSending)
            {
                SendToArduinoCommunication(value);
                yield return new WaitForSeconds(1f); // Adjust the interval as needed
            }
        }

        private void StopContinuousSending()
        {
            isContinuousSending = false;
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
    public class ButtonEventWrapper : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;
        private IInteractableView InteractableView;

        [SerializeField]
        private UnityEvent _whenSelectContinuous;
        [SerializeField]
        private UnityEvent _whenSelectEnded;
        public ArduinoCommunication arduinoCommunication;

        protected bool _started = false;
        private bool isSelectContinuous = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
            }
        }

        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Select:
                    if (!isSelectContinuous)
                    {
                        isSelectContinuous = true;
                        _whenSelectContinuous.Invoke(); // Invoke the continuous select event
                        SendToArduinoCommunication(1); // Send 1 to Arduino when the combined state begins
                    }
                    break;
                default:
                    if (isSelectContinuous)
                    {
                        isSelectContinuous = false;
                        _whenSelectEnded.Invoke(); // Invoke the event when the combined state ends
                        SendToArduinoCommunication(0); // Send 0 to Arduino when the combined state ends
                    }
                    break;
            }
        }

        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
            }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

        protected bool _started = false;
        private bool isSelected = false; // Added variable to keep track of the selected state
        private bool isContinuousSending = false;
        private MeshRenderer meshRenderer;
        private Color originalColor;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                originalColor = meshRenderer.material.color;
            }
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                        // Send 0 to ArduinoCommunication script when unhovered
                        SendToArduinoCommunication(0);
                    }
                    isSelected = false; // Update selected state when unselected
                    StopContinuousSending(); // Stop continuous sending when unselected
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                        // Send 0 to ArduinoCommunication script when unselected
                        SendToArduinoCommunication(0);
                        isSelected = false; // Update selected state when unselected
                        StopContinuousSending(); // Stop continuous sending when unselected
                    }
                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                        StartCoroutine(SelectDelay());
                    }
                    isSelected = true; // Update selected state when selected
                    StartContinuousSending(1); // Start sending continuous 1 when selected
                    break;
            }
        }

        private IEnumerator SelectDelay()
        {
            yield return new WaitForSeconds(2.5f); // Delay for 2.5 seconds
            _whenSelect.Invoke(); // Invoke the select event after the delay
        }

        // Method to send data to ArduinoCommunication script
        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
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

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        private void StartContinuousSending(int value)
        {
            if (!isContinuousSending)
            {
                isContinuousSending = true;
                StartCoroutine(ContinuousSendingCoroutine(value));
            }
        }

        private IEnumerator ContinuousSendingCoroutine(int value)
        {
            while (isContinuousSending)
            {
                SendToArduinoCommunication(value);
                yield return null;
            }
        }

        private void StopContinuousSending()
        {
            isContinuousSending = false;
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
using System.IO.Ports;

namespace Oculus.Interaction
{
    public class ButtonEventWrapper : MonoBehaviour
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

        public ArduinoCommunication arduinoCommunication; // Reference to ArduinoCommunication script

        protected bool _started = false;
        private bool isSelected = false; // Added variable to keep track of the selected state

        private Coroutine continuousSendingCoroutine; // Coroutine to send continuous data

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(InteractableView);
            this.EndStart(ref _started);

            // Find the ArduinoCommunication script in the scene
            arduinoCommunication = FindObjectOfType<ArduinoCommunication>();
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
                    isSelected = false;
                    StopContinuousSending(); // Stop continuous sending when unselected
                    SendToArduinoCommunication(0); // Send 0 to Arduino when unselected
                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        _whenHover.Invoke();
                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                        isSelected = false;
                        StopContinuousSending(); // Stop continuous sending when unselected
                        SendToArduinoCommunication(0); // Send 0 to Arduino when unselected
                    }
                    break;
                case InteractableState.Select:
                    isSelected = true;
                    StartContinuousSending(); // Start sending continuous 1 when selected
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

        private IEnumerator DelayedEvent(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(2.0f); // Delay for 2 seconds
            unityEvent.Invoke();
        }

        // Method to send data to ArduinoCommunication script
        private void SendToArduinoCommunication(int value)
        {
            if (arduinoCommunication != null)
            {
                arduinoCommunication.SendDataToArduino(value);
            }
        }

        // Start continuous sending coroutine
        private void StartContinuousSending()
        {
            if (continuousSendingCoroutine == null)
            {
                continuousSendingCoroutine = StartCoroutine(ContinuousSendingCoroutine());
            }
        }

        // Coroutine to send continuous data
        private IEnumerator ContinuousSendingCoroutine()
        {
            while (isSelected)
            {
                SendToArduinoCommunication(1); // Send 1 to Arduino when selected
                yield return null;
            }
            continuousSendingCoroutine = null; // Coroutine finished
        }

        // Stop continuous sending coroutine
        private void StopContinuousSending()
        {
            if (continuousSendingCoroutine != null)
            {
                StopCoroutine(continuousSendingCoroutine);
                continuousSendingCoroutine = null;
            }
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
