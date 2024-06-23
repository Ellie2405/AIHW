using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speedMod;

    float noise;
    float elapsedTime = 0;
    float noiseEmissionRate = .5f;

    void Start()
    {
        Application.targetFrameRate = 60;
        Debug.Log("Setting Application to fps cap to 60");
        elapsedTime = noiseEmissionRate;
    }

    void Update()
    {
        UpdateMovement();
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (noise != 0)
        {
            if (elapsedTime >= noiseEmissionRate)   //an internal cooldown for consistent noise like walking
            {
                EmitNoise();

                elapsedTime = 0.0f;
            }
        }
    }

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal));

        if (speed > 0)  //make "noise" while walking
        {
            noise = 5;
        }
        else
        {
            noise = 0;
        }

        Vector3 direction = new(horizontal, 0, vertical);
        direction.Normalize();

        transform.Translate(Time.deltaTime * speed * speedMod * direction);
    }

    void EmitNoise()
    {
        NoiseManager.Instance.EmitNoise(Aspect.Affiliation.Player,transform.position,noise);
    }
}
