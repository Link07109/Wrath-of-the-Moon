using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private Button startButton;
    private Button messageButton; // change this to show more options
    private Button exitButton;
    private Label messageLabel;
    
    // Start is called before the first frame update
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        messageButton = root.Q<Button>("message-button");
        exitButton = root.Q<Button>("exit-button");
        messageLabel = root.Q<Label>("message-label");

        startButton.clicked += StartButtonPressed;
        messageButton.clicked += MessageButtonPressed;
        exitButton.clicked += ExitButtonPressed;
    }

    private void StartButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }

    private void MessageButtonPressed()
    {
        messageLabel.text = "gamer text"; // get rid of this
        messageLabel.style.display = DisplayStyle.Flex; // replace label with a visual element
    }

    private void ExitButtonPressed()
    {
        Application.Quit();
    }
}
