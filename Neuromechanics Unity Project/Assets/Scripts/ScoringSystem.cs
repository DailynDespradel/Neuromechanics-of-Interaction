/*
using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public TMP_Text scoreText;  // Reference to the TextMeshPro Text component where the score will be displayed
    public int scoreValue = 10;  // Amount of score to add when a target is selected

    private int score = 0;  // Current score

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore()
    {
        score += scoreValue;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Task: " + score;
        }
    }
}
*/
using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TextMeshPro Text component where the score will be displayed
    public int scoreValue = 1; // Amount of score to add when a target is selected
    public int scoreThresholdToShowRestCanvas = 10; // Score value at which to show the "Rest" message

    public TMP_Text restText; // Reference to the TextMeshPro Text component for the "Rest" message

    private int score = 0; // Current score
    private bool restCanvasShown = false; // Flag to track if the "Rest" canvas has been shown

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore()
    {
        score += scoreValue;
        UpdateScoreDisplay();

        // Check if the score has reached the threshold to show the "Rest" canvas
        if (score >= scoreThresholdToShowRestCanvas && !restCanvasShown)
        {
            ShowRestCanvas();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Number of Button Press: " + score;
        }
    }

    private void ShowRestCanvas()
    {
        if (restText != null)
        {
            restText.text = "Rest Time!"; // Set the text to display for the "Rest" canvas
            restCanvasShown = true;
        }
    }
}
