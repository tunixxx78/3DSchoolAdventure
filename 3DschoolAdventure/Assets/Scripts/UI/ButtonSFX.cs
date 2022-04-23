using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void OnMouseOver()
    {
        Debug.Log("Hoveraa");
        FindObjectOfType<SoundFX>().buttonEnterExit.Play();
    }

    public void OnMouseExit()
    {
        Debug.Log("Ei en‰‰ hoveraa");
        FindObjectOfType<SoundFX>().buttonEnterExit.Play();
    }

    public void ButtonClicked()
    {
        Debug.Log("Painettu");
        FindObjectOfType<SoundFX>().buttonPress.Play();
    }
}
