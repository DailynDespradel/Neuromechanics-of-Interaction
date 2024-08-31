using UnityEngine;

public class ColorChangeController : MonoBehaviour
{
    private Renderer _renderer;
    private Color _originalColor;
    public Color _selectedColor = Color.blue; // Change this to your desired selected color

    private bool _isSelected = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    private void Update()
    {
        if (_isSelected)
        {
            _renderer.material.color = Color.Lerp(_renderer.material.color, _selectedColor, Time.deltaTime * 10f);
        }
        else
        {
            _renderer.material.color = Color.Lerp(_renderer.material.color, _originalColor, Time.deltaTime * 10f);
        }
    }

    public void OnSelect()
    {
        _isSelected = true;
        Invoke("Deselect", 0.3f); // Deselect after 300 milliseconds
    }

    private void Deselect()
    {
        _isSelected = false;
    }
}
