using UnityEngine;

public class LegoYodaScream : MonoBehaviour
{
    [SerializeField] private AudioSource myAudio;
    public void playAudio()
    {
        myAudio.PlayOneShot(myAudio.clip);
    }
}
