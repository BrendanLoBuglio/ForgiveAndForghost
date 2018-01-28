using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class aerobat : ImageEffectBase
{
    public LayerMask layersToRender;

    public Texture2D noiseTexture;
    public float dispersionValue = 0.001f;
    public float effectStartAlpha = 0.7f;
    public float fadeRate = .6f;
    public float whiteShiftValue = 0.01f;
    public Vector2 wind;

    public RenderTexture input;
    public RenderTexture fbo;

    public RenderingPath renderingPath = RenderingPath.Forward;

    private GameObject replacementCamera;
    private Camera replacementCameraCam;
    private Camera myCamera;
    public bool debug;
    bool effectEnabled = true;

    private bool debugShowInput = false;

    override protected void OnDisable()
    {
        DestroyImmediate(input);
        DestroyImmediate(fbo);
        DestroyImmediate(replacementCameraCam);
        base.OnDisable();
    }

    void Start()
    {
    }

    void Initialize()
    {
        replacementCamera = new GameObject("Aerobat Replacement Cam");
        replacementCameraCam = replacementCamera.AddComponent<Camera>();
        replacementCameraCam.enabled = false;
        replacementCamera.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnPreRender()
    {
        if (myCamera == null)
        {
            myCamera = GetComponent<Camera>();
        }

        if (input == null || input.width != myCamera.pixelWidth || input.height != myCamera.pixelHeight)
        {
            DestroyImmediate(input);
            input = new RenderTexture(myCamera.pixelWidth, myCamera.pixelHeight, 0, RenderTextureFormat.ARGB32);
            input.DiscardContents();
            input.hideFlags = HideFlags.HideAndDontSave;
        }
        if (fbo == null || fbo.width != myCamera.pixelWidth || fbo.height != myCamera.pixelHeight)
        {
            DestroyImmediate(fbo);
            fbo = new RenderTexture(myCamera.pixelWidth, myCamera.pixelHeight, 0, RenderTextureFormat.ARGB32);
            fbo.DiscardContents();
            fbo.hideFlags = HideFlags.HideAndDontSave;
        }
        if (replacementCameraCam == null)
        {
            Initialize();
        }
        replacementCameraCam.transform.position = transform.position;
        replacementCameraCam.transform.rotation = transform.rotation;
        replacementCameraCam.CopyFrom(myCamera);
        replacementCameraCam.targetTexture = input;
        replacementCameraCam.cullingMask = layersToRender;
        replacementCameraCam.Render();

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Graphics.Blit(source, input, material, 3);

        if (!debug)
        {
            Graphics.Blit(input, fbo, material, 0);
            material.SetTexture("_Noise", noiseTexture);
            material.SetFloat("_Dispersion", dispersionValue);
            material.SetFloat("_StartAlpha", effectStartAlpha);
            material.SetFloat("_FadeValue", fadeRate);
            material.SetFloat("_WhiteShiftValue", whiteShiftValue);
            material.SetFloat("_WindX", wind.x);
            material.SetFloat("_WindY", wind.y);

            // flip for antialiasing, removed because antialiasing doesn't work in deferred anymore
            //material.SetFloat("_Antialiasing", QualitySettings.antiAliasing);

            Graphics.Blit(fbo, input, material, 1);
            Graphics.Blit(input, fbo);

            if (debugShowInput)
                Graphics.Blit(input, source);
            else if (effectEnabled)
            {
                Graphics.Blit(input, fbo);          //in two draw original input over the effect
                Graphics.Blit(fbo, source, material, 2);
            }
        }
        else
        {
            Graphics.Blit(input, source);
        }
        Graphics.Blit(source, destination);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                debugShowInput = !debugShowInput;
            }
        }
    }

    IEnumerator delayedEnable()
    {
        yield return new WaitForSeconds(.5f);
        effectEnabled = true;
    }


}
