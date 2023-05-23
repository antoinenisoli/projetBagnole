using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreStar : MonoBehaviour
{
    [SerializeField] Image starImage;
    [SerializeField] Sprite fullSprite, emptySprite;

    public void Set(bool value)
    {
        starImage.sprite = value ? fullSprite : emptySprite;
    }
}
