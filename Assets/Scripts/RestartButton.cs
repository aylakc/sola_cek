using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Sprite[] buttonImages;
    public Image buttonImage;
    
    void Start()
    {
        int randomIndex = Random.Range(0, buttonImages.Length);
        buttonImage.sprite = buttonImages[randomIndex];
    }

    
    // called from Editor
    public void OnButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
