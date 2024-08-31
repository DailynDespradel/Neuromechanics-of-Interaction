using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Oculus.Interaction
{
    public class TargetEventWrapper : MonoBehaviour
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

        protected virtual void Awake()
        {
            Pointable = _pointable as IPointable;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);

            // Initialize the Pointable reference here
            if (_pointable != null)
            {
                Pointable = _pointable as IPointable;
            }
            else
            {
                Debug.LogWarning("No IPointable component assigned.");
            }

            _pointers = new HashSet<int>();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started && Pointable != null)
            {
                Pointable.WhenPointerEventRaised += HandlePointerEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started && Pointable != null)
            {
                Pointable.WhenPointerEventRaised -= HandlePointerEventRaised;
            }
        }

        private bool _selectDelayed = false;
        private float _selectDelayTimer = 0.0f;
        private const float SELECT_DELAY = 5.0f;

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            // ... (rest of the method remains unchanged)
        }

        private void Update()
        {
            // ... (rest of the method remains unchanged)
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
