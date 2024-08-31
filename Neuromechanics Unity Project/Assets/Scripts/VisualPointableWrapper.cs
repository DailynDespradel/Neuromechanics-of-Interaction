using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Oculus.Interaction
{
    public class VisualPointableWrapper : MonoBehaviour
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

        [SerializeField]
        private Renderer objectRenderer; // Reference to the object's renderer component

        [SerializeField]
        private Color colorChange = Color.red; // Color to change to
        private Color originalColor;

        [SerializeField]
        private bool useMaterialChange = true; // Toggle between material and color change

        protected bool _started = false;
        private bool isSelecting = false;

        public UnityEvent WhenRelease => _whenRelease;

        //public UnityEvent WhenHover => _whenHover;
        //public UnityEvent WhenUnhover => _whenUnhover;
        //public UnityEvent WhenSelect => _whenSelect;
        //public UnityEvent WhenUnselect => _whenUnselect;
        //public UnityEvent WhenMove => _whenMove;
        //public UnityEvent WhenCancel => _whenCancel;

        protected virtual void Awake()
        {
            Pointable = _pointable as IPointable;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(Pointable);
            _pointers = new HashSet<int>();

            if (objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
            }

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

        private IEnumerator ChangeColorForDuration(Color newColor, float duration)
        {
            if (useMaterialChange && objectRenderer != null)
            {
                objectRenderer.material.color = newColor;
                yield return new WaitForSeconds(duration);
                objectRenderer.material.color = originalColor;
            }
            else if (!useMaterialChange && objectRenderer != null)
            {
                Color original = objectRenderer.material.color;
                objectRenderer.material.color = newColor;
                yield return new WaitForSeconds(duration);
                objectRenderer.material.color = original;
            }
        }

        private IEnumerator SelectForDuration(float duration)
        {
            isSelecting = true;
            yield return new WaitForSeconds(duration);
            isSelecting = false;
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
                        _whenSelect.Invoke();
                        StartCoroutine(ChangeColorForDuration(colorChange, 0.3f)); // Change color for 500 milliseconds
                        StartCoroutine(SelectForDuration(0.3f)); // Delay for 500 milliseconds
                    }
                    break;
                case PointerEventType.Unselect:
                    if (_pointers.Contains(evt.Identifier))
                    {
                        _whenRelease.Invoke();
                    }
                    _whenUnselect.Invoke();
                    StartCoroutine(ChangeColorForDuration(colorChange, 0.3f)); // Change color for 500 milliseconds
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

        public void InjectAllPointableUnityEventWrapper(IPointable pointable)
        {
            InjectPointable(pointable);
        }

        public void InjectPointable(IPointable pointable)
        {
            _pointable = pointable as MonoBehaviour;
            Pointable = pointable;
        }
    }
}
