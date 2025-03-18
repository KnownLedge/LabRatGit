using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//hey if you are reading this dont tell anyone one :) plz
public class _ : MonoBehaviour
{
    private Texture2D text2;
    private Sprite _sprite;
    private AudioClip _clip;
    private int count;
    private float cTime = 10;
    private bool countdown = false;
    private bool isPlaying = false;
    void Start()
    {
        text2 = Resources.Load<Texture2D>("Models/_ 2");
        _clip = Resources.Load<AudioClip>("Models/Collect Rat");
        _sprite = Sprite.Create(text2, new Rect(0,0,text2.width,text2.height), new Vector2(0.5f, 0.5f));
        _sprite.name = "SRAT";
    }

    void Update()
    {
        if (countdown)
        {
            cTime -= Time.deltaTime;
        }
        if(cTime <= 0)
        {
            count = 0;
            cTime = 10f;
            countdown = !countdown;
        }
        if (Input.GetKeyDown(KeyCode.O) && !isPlaying)
        {
            if (!countdown)
            {
                countdown = !countdown;
            }
            count++;
            if (count == 10 )
            {
                isPlaying = true;
                StartCoroutine(Scare());
            }
        }
       
    }
    private IEnumerator Scare()
    {
        var Scare = new GameObject("SCARE");
        var ScareI = Scare.AddComponent<Image>();
        var sound = Scare.AddComponent<AudioSource>();
        ScareI.sprite = _sprite;
        ScareI.transform.position = new Vector2(960, 540);
        ScareI.transform.localScale = new Vector2(20, 12);
        Scare.transform.SetParent(GameObject.Find("Canvas").transform);
        sound.clip = _clip;
        sound.Play();

        yield return new WaitForSeconds(1f);
        Destroy(ScareI);
        yield return new WaitForSeconds(1f);
        Destroy(Scare);
    }
}
