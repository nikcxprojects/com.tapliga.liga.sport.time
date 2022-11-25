using System;
using UnityEngine;
using System.Collections;

using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public static Action OnPressed { get; set; } = delegate { };

    private void OnMouseDown()
    {
        OnPressed?.Invoke();
    }

    public void Init()
    {

    }

    Vector2 GetPosition()
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = Random.Range(-4.43f, 3.29f);

        return new Vector2(x, y);
    }
}
