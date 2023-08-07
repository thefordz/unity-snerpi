using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClickSound : MonoBehaviour
{
    public void PlayClickMenuSound()
    {
        AudioManager.Instance.PlaySFX("MenuClick");
    }
}