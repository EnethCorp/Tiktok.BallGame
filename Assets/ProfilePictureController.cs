using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProfilePictureController : MonoBehaviour
{
    private enum Place
    {
        First,
        Second,
        Third,
        Winner
    }

    [SerializeField] private Place place;
    private Image image;

    [SerializeField] private Sprite DefaultSprite;
    private static int resetCount = 3;

    void Awake()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        if (GameManager.Instance.ResetProfilePictures && resetCount > 0)
        {
            this.image.sprite = DefaultSprite;
            resetCount--;
        }
        if (resetCount == 0)
        {
            GameManager.Instance.ResetProfilePictures = false;
            resetCount = 3;
        }

        if (Time.frameCount % 30 != 0)
            return;

        switch (place)
        {
            case Place.First:
                StartCoroutine(this.CorrectProfilePicture(0));
                break;
            case Place.Second:
                StartCoroutine(this.CorrectProfilePicture(1));
                break;
            case Place.Third:
                StartCoroutine(this.CorrectProfilePicture(2));
                break;
            case Place.Winner:
                StartCoroutine(this.CorrectWinnerPicture());
                break;
        }   
    }

    private IEnumerator CorrectProfilePicture(int place)
    {
        if (GameManager.Instance.UserList.Count < place + 1 || !GameManager.Instance.UserList[place])
            yield break;

        Texture2D texture = MaterialCreator.Instance.CreateTexture(GameManager.Instance.UserList[place].Username);
        if (texture == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CorrectProfilePicture(place));
            yield break;
        }

        Sprite sprite = Sprite.Create(
            texture, 
            new Rect(Vector2.zero, new Vector2(128f, 128f)),
            new Vector2(0.5f, 0.5f)
        );

        this.image.sprite = sprite;
    }
    private IEnumerator CorrectWinnerPicture()
    {
        if (GameManager.Instance.WinnerList.Count < 1)
            yield break;

        Texture2D texture = MaterialCreator.Instance.CreateTexture(GameManager.Instance.WinnerList[0].Username);
        if (texture == null)
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(CorrectWinnerPicture());
            yield break;
        }

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(Vector2.zero, 
            new Vector2(128f, 128f)),
            new Vector2(0.5f, 0.5f)
        );

        this.image.sprite = sprite;
    }
}
