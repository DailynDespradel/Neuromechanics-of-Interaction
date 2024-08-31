using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Oculus.Interaction
{
    /// <summary>
    /// This componenet makes it possible to connect IPointables in the
    /// inspector to Unity Events that are broadcast on IPointable events.
    /// </summary>
    public class VisualUnityEventWrapper : MonoBehaviour
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
        private bool isSelecting = false;

        private IEnumerator SelectDelayRoutine()
        {
            isSelecting = true;
            _whenSelect.Invoke();
            yield return new WaitForSeconds(0.5f); //Delay for 500 millisenconds 
            isSelecting = false;
            _whenUnselect.Invoke();
        }

        protected virtual void Awake()
        {
            Pointable = _pointable as IPointable;
        }

        protected virtual void Start()
        {
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
                    if (!isSelecting)
                    {
                        StartCoroutine(SelectDelayRoutine());
                    }
                    break;
                case PointerEventType.Unselect:
                    if (_pointers.Contains(evt.Identifier))
                    {
                        _whenRelease.Invoke();
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


