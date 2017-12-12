using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class InstructionsScript : MonoBehaviour {
    public Image currImage;
    public Button nextButton;
    public Button backButton;
    public int i = 0;
    public Sprite [] images;

    public void BtnNext () {
        if (i < (images.Length - 1))
        i++;
    }

    public void BtnBack () {
        if (i > 0)
        i--;
    }

    void Update () {
        currImage.sprite = images[i];
    }
}
