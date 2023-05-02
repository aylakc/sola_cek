using System.Collections;
using UnityEngine;

public class StampController : MonoBehaviour
{
    // the particle effect to spawn
    public GameObject particleEffect;

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

    public GameObject restartButton;


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
                    
                    PlayEffect();
                }
            }
        }
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
        

        // spawn the particle effect
        Instantiate(particleEffect, transform.position, Quaternion.identity);
    }
    
    private void PlaySecondSoundEffect()
    {
        var soundEffect = soundEffects[Random.Range(0, soundEffects.Length)];
        audioSource.PlayOneShot(soundEffect);

        var delayDuration = 10f;
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
        StartCoroutine(yesImage.GetComponent<ShakeController>().ShakeGameObject());
        
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(stampBase.GetComponent<ShakeController>().ShakeGameObject());
    }
}