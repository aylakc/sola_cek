using System;
using System.Collections;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using UnityEngine;

public class MainPanelController : MonoBehaviour
{
    public TextMeshProUGUI voteCountText;
    public AudioClip sezenSong;
    // a reference to the audio source component
    private AudioSource audioSource;
    
#if !UNITY_EDITOR
        public void GetJSON() =>
        FirebaseDatabase.GetJSON("voteCount", gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForValueChanged() =>
        FirebaseDatabase.ListenForValueChanged("voteCount", gameObject.name, "DisplayInfo", "DisplayErrorObject");
#endif

    // Start is called before the first frame update
    void Start()
    {
        // int voteCount = PlayerPrefs.GetInt("voteCount", 0);

        // Update the text component with the visitor count
        // voteCountText.text = voteCount.ToString();
        // InvokeRepeating("UpdateVoteCount", 3f, 3f);

       
        // get the audio source component
        audioSource = GetComponent<AudioSource>();
        GameObject myObject = GameObject.Find("stamp");
        StampController myScript = myObject.GetComponent<StampController>();
        myScript.Voted += (sender, args) => { ChangeSong(); };
#if !UNITY_EDITOR
        GetJSON();
        ListenForValueChanged();
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void DisplayInfo(string info)
    {
        voteCountText.color = Color.white;
        voteCountText.text = info;
        Debug.Log(info);
    }

    public void DisplayError(string error)
    {
        voteCountText.color = Color.red;
        voteCountText.text = error;
        Debug.LogError(error);
    }

    void ChangeSong()
    {
        audioSource.Stop();

        audioSource.volume = 0.7f;
        audioSource.clip = sezenSong;

        audioSource.Play();
    }

    // void UpdateVoteCount()
    // {
    //     int voteCount = PlayerPrefs.GetInt("voteCount", 0);
    //
    //     // Update the text component with the visitor count
    //     voteCountText.text = voteCount.ToString();
    // }
}