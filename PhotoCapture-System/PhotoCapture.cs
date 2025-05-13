using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoCapture : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 30f;
    [SerializeField] private float maxZoom = 60f;
    
    [Header("UI")]
    [SerializeField] private Image photoPreview;
    [SerializeField] private GameObject photoUI;
    [SerializeField] private GameObject flashEffect;
    [SerializeField] private AudioSource cameraSound;
    
    private Texture2D screenCapture;
    private bool isPhotoMode;
    private bool isViewingPhoto;
    private string photoSavePath;
    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photoSavePath = Path.Combine(Application.dataPath, "Photos");
        Directory.CreateDirectory(photoSavePath);
        
        mainCamera.fieldOfView = maxZoom;
        photoUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePhotoMode();
        }

        if (!isPhotoMode) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float newFOV = mainCamera.fieldOfView - scroll * zoomSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
        }
        if (Input.GetMouseButtonDown(0) && !isViewingPhoto)
        {
            StartCoroutine(CapturePhoto());
        }
        
        if (Input.GetMouseButtonDown(1) && isViewingPhoto)
        {
            HidePhoto();
        }
    }
    private void TogglePhotoMode()
    {
        isPhotoMode = !isPhotoMode;
        photoUI.SetActive(isPhotoMode);
        
        if (!isPhotoMode)
        {
            HidePhoto();
            mainCamera.fieldOfView = maxZoom;
        }
    }
    private IEnumerator CapturePhoto()
    {
        if (flashEffect) StartCoroutine(FlashEffect());
        if (cameraSound) cameraSound.Play();

        yield return new WaitForEndOfFrame();
        screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenCapture.Apply();
        SavePhoto();
        ShowPhoto();
    }

    private void ShowPhoto()
    {
        isViewingPhoto = true;
        Sprite photoSprite = Sprite.Create(screenCapture, 
        new Rect(0, 0, screenCapture.width, screenCapture.height), 
        new Vector2(0.5f, 0.5f));
        photoPreview.sprite = photoSprite;
        photoPreview.gameObject.SetActive(true);
    }
    private void HidePhoto()
    {
        isViewingPhoto = false;
        photoPreview.gameObject.SetActive(false);
    }
    private void SavePhoto()
    {
        string fileName = $"Photo_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string filePath = Path.Combine(photoSavePath, fileName);
        File.WriteAllBytes(filePath, screenCapture.EncodeToPNG());
        Debug.Log($"Photo saved: {filePath}");
    }
    private IEnumerator FlashEffect()
    {
        flashEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashEffect.SetActive(false);
    }
}
