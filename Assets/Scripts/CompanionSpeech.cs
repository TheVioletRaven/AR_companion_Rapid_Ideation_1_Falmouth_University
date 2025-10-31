using System.Collections;
using UnityEngine;

public class CompanionSpeech : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip greetingClip;

    bool isTalking;

    void Awake()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Speak();
    }

    public void Speak()
    {
        if (audioSource && greetingClip)
        {
            audioSource.clip = greetingClip;
            audioSource.Play();
            if (!isTalking) StartCoroutine(TalkMotion());
        }
        else
        {
            Debug.LogWarning("AudioSource of GreetingClip ontbreekt op " + name);
        }
    }

    IEnumerator TalkMotion()
    {
        isTalking = true;

        while (audioSource && audioSource.isPlaying)
        {
            // alleen naar de camera kijken (geen bob meer)
            var cam = Camera.main;
            if (cam)
            {
                Vector3 look = cam.transform.position - transform.position;
                look.y = 0f;
                if (look.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(look.normalized, Vector3.up),
                        Time.deltaTime * 6f
                    );
            }

            yield return null;
        }

        isTalking = false;
    }
}
