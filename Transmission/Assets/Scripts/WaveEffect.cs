using UnityEngine;
using System.Collections;


public class WaveEffect : MonoBehaviour
{
    // Creates a line renderer that follows a Sin() function
    // and animates it.

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public bool mouseTestEnabled = false;
    public bool horizontal = true;
    public int SPEED_FACTOR = 3;

    public float MIN_WAVE_LENGTH = 2.0f;
    public float MAX_WAVE_LENGTH = 30.0f;
    public float OFFSET_X = 0.0f;
    public float OFFSET_Y = 0.0f;
    public float OFFSET_Z = 1.0f;
    public float SCALE_X = 1.0f;
    public float SCALE_Y = 1.0f;
    public float MIN_AMPLITUDE = 1.0f;
    private int lengthOfLineRenderer = 512;
    private float[] buffer;
    private int MAX_BUFFER = 512;
    private float MAX_AMP_ACC = 0.05f;  // acceleration of amplitude
    private float MIN_AMP_ACC = -0.05f;  // deceleration of amplitude
    private float xinc = 0.05f;
    private float wavelength = 10.0f;
    private float amplitude = 0.0f;
    private float amp_acc = 0.0f;  // a factor for acceleration to make transitions smoother

    private float period = 0.0f;

    private AudioSource wave_audio;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  //"Particles/Additive"));
        //lineRenderer.material = Resources.Load("line.mat", typeof(Material)) as Material;
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;

        buffer = new float[MAX_BUFFER];
        for (int i = 0; i < MAX_BUFFER; i++)
        {
            float x = ((float)i) / wavelength;
            buffer[i] = Mathf.Sin(x) * amplitude;
            period = x;

        }
        // amplitude accelration starts at 0
        amp_acc = 0;
        // wave_audio is the audio component, that I will change on the fly
        wave_audio = GetComponent<AudioSource>();
        if (wave_audio != null)
        {
            wave_audio.Play();
            wave_audio.PlayDelayed(44100); // 44100
        }
    }


    

    // this code checks mouse input to test the function
    void DebugInput()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            amp_acc += 0.01f;
            amp_acc = Mathf.Max(amp_acc, MAX_AMP_ACC);
            amplitude += 0.05f;
            if (amplitude > 2.0f) amplitude = 2.0f;
        }
        else
        {
            amp_acc -= 0.01f;
            amp_acc = Mathf.Max(amp_acc, MIN_AMP_ACC);
            amplitude -= 0.05f;
            if (amplitude < MIN_AMPLITUDE) amplitude= MIN_AMPLITUDE;
        }
        wavelength = Mathf.Max(MIN_WAVE_LENGTH, ((float)(Screen.width - mousePos.x)) / ((float)Screen.width) * MAX_WAVE_LENGTH);


    }

    // interface functions
    public void SetWavelength(float wavelength_in)
    {
        wavelength = Mathf.Max(MIN_WAVE_LENGTH, wavelength_in);
    }

    // turns up the transmission
    public void TurnUp()
    {
        amp_acc += 0.01f;
        amp_acc = Mathf.Max(amp_acc, MAX_AMP_ACC);
        amplitude += 0.05f;
        if (amplitude > 2.0f) amplitude = 2.0f;

    }
    // turn down the transmission
    public void TurnDown()
    {
        amp_acc -= 0.01f;
        amp_acc = Mathf.Max(amp_acc, MIN_AMP_ACC);
        amplitude -= 0.05f;
        if (amplitude < 0.0f) amplitude = 0.0f;
    }

    void UpdateBuffer()
    {
        // in case I want it to rotate
        /*
        float[] oldy = new float[SPEED_FACTOR];
        for (int i =0; i < SPEED_FACTOR; i++)
        {
            oldy[i] = buffer[i];
        }
        */
        for (int i = 0; i < MAX_BUFFER -SPEED_FACTOR; i++)
        {
            buffer[i] = buffer[i + SPEED_FACTOR];
        }
        for (int i = 0; i < SPEED_FACTOR; i++)
        {
            buffer[MAX_BUFFER - SPEED_FACTOR+i] = Mathf.Sin(period) * amplitude;
            period += 1.0f / wavelength; // can accelerate , decelerate
        }
        if (mouseTestEnabled)
        {
            DebugInput();
        }
    }

    void Update()
    {
        UpdateBuffer();

        wave_audio.pitch = 10.0f/wavelength;
        wave_audio.volume = amplitude;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var t = Time.time;
        float x = -MAX_BUFFER / 2 * xinc;

        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            if (horizontal)
            {
                lineRenderer.SetPosition(i, new Vector3(x*SCALE_X + OFFSET_X, buffer[i]*SCALE_X + OFFSET_Y, OFFSET_Z));
            } else
            {
                lineRenderer.SetPosition(i, new Vector3(buffer[i]* SCALE_X + OFFSET_X, x*SCALE_Y + OFFSET_Y, OFFSET_Z));
            }
            x += xinc;
        }
    }
}
