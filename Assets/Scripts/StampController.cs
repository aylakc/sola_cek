using System;
using System.Collections;
using FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEngine;
using Random = UnityEngine.Random;

public class StampController : MonoBehaviour
{
    // the particle effect to spawn
    public GameObject particleEffect;
    public GameObject particleEffectRed;

    // the audio clips to play
    public AudioClip[] soundEffects;
    public AudioClip hammerEffect;

    // a reference to the audio source component
    private AudioSource audioSource;

    // flag to prevent multiple collisions
    private bool _hasCollided = false;
    public float radius = 0.5f;
    public LayerMask layerMask;

    private Animator animator;

    public GameObject stampBase;
    public GameObject yesImage;
    public GameObject megafon;

    public GameObject restartButton;

    public event EventHandler Voted;

    private void Awake()
    {
        restartButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // get the audio source component
        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        animator.enabled = false;
    }


    private void Update()
    {
        if (!_hasCollided)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("base"))
                {
                    Debug.Log("Collision detected with " + collider.gameObject.name);
                    // Do something when a collision is detected, such as play a sound or spawn a particle effect
                    _hasCollided = true;

                    // Get a reference to the script component you want to disable
                    DraggableController scriptToDisable = gameObject.GetComponent<DraggableController>();

                    // Disable the script by setting the 'enabled' property to false
                    scriptToDisable.enabled = false;
                    DestroyImmediate(scriptToDisable);

                    // var voteCount = PlayerPrefs.GetInt("voteCount", 0);
                    // voteCount++;

                    // PlayerPrefs.SetInt("voteCount", voteCount);
                    ModifyNumberWithTransaction();
                    Voted?.Invoke(this, EventArgs.Empty);

                    PlayEffect();
                }
            }
        }
    }

    public void ModifyNumberWithTransaction()
    {
#if !UNITY_EDITOR
        FirebaseDatabase.ModifyNumberWithTransaction("voteCount", 1, gameObject.name, "DisplayInfo",
            "DisplayError");
#endif
    }

    public void DisplayInfo(string info)
    {
        Debug.Log(info);
    }

    public void DisplayError(string error)
    {
        Debug.LogError(error);
    }

    // play a random sound effect and spawn the particle effect
    public void PlayEffect()
    {
        animator.enabled = true;
        animator.Play("ScaleUp");
        // play a random sound effect
        audioSource.PlayOneShot(hammerEffect);
        // Wait for the duration of the first sound effect before playing the second one
        Invoke("PlaySecondSoundEffect", hammerEffect.length);
        yesImage.SetActive(true);
        megafon.SetActive(true);


        // spawn the particle effect
        var particlePos = transform.position;
        Instantiate(particleEffect, particlePos, Quaternion.identity);
        Instantiate(particleEffectRed, particlePos, Quaternion.identity);
    }

    private void PlaySecondSoundEffect()
    {
        var soundEffect = soundEffects[Random.Range(0, soundEffects.Length)];
        audioSource.PlayOneShot(soundEffect);

        var delayDuration = 5f;
        if (soundEffect.length < delayDuration)
        {
            delayDuration = soundEffect.length;
        }

        StartCoroutine(StartShakeCoroutines());
        Invoke("ActivateRestartButton", delayDuration);
    }

    private void ActivateRestartButton()
    {
        restartButton.SetActive(true);
    }

    IEnumerator StartShakeCoroutines()
    {
        StartCoroutine(megafon.GetComponent<ShakeController>().ShakeGameObject());
        yield return new WaitForSeconds(1f);
        StartCoroutine(yesImage.GetComponent<ShakeController>().ShakeGameObject());
    }
}