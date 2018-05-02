using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthOverlay : MonoBehaviour
{
    //[Header("First image should be the worst.")]
    //public Sprite spritesheet;
    //private Sprite[] _separateImages;
    public GameObject imagePrefab;
    public Sprite[] sprites;
    private PlayerHealth _ph;
    //private Image[] _images;
    private List<Image> _images;
    
    [Header("Crossfade stuff")]
    [SerializeField]
    private bool _useHealthPerImage = false;
    [SerializeField]
    private float _healthPerImage = 0;

    [SerializeField]
    private float _immediateTimeChange = 0.001f;

    [SerializeField]
    private float _normalTimeChange = 0.01f;

    // Use this for initialization
    private void Start()
    {
        _ph = GlobalObjectManager.Instance.player.GetComponent<PlayerHealth>();
        if (_ph == null) Debug.LogError("WARNING: PlayerHealth missing from UI_HealthOverlay");
        //_image = GetComponentInChildren<Image>();
        //List<Image> images = new List<Image>();
        if (sprites.Length != 0 && !_useHealthPerImage) _healthPerImage = (int)SettingsComponent.gl_MaxHealth / sprites.Length;
        _images = new List<Image>();
        foreach (Sprite sprite in sprites)
        {
            GameObject prefab = Instantiate(imagePrefab);
            prefab.transform.SetParent(gameObject.transform, false);
            prefab.name = "HealthOverlay";
            Image image = prefab.GetComponent<Image>();
            image.sprite = sprite;
            image.overrideSprite = sprite;
            image.CrossFadeAlpha(0, _immediateTimeChange, false);
            _images.Add(image);
            //prefab.SetActive(true);
        }
    }

    // Update is called once per frame
    public void OnHealthChange()
    {
        //Debug.Log("Hello");
        float currentHealth = _ph.currentHealth;
        if (currentHealth > 0 && _images.Count != 0)
        {
            //Debug.Log("Yas");
            var maxHealth = SettingsComponent.gl_MaxHealth;
            if (currentHealth > maxHealth - maxHealth % _healthPerImage && currentHealth <= maxHealth)
            {
                SetAllImagesTransparent(_normalTimeChange);
                return;
            }
            for (int i = 0; i < _images.Count; i++)
            {
                if (currentHealth > _healthPerImage * i && currentHealth <= _healthPerImage * (i + 1))
                {
                    //Debug.Log("No");
                    Image image1 = _images[i] ?? null;
                    Image image2 = null;
                    if (i + 1 < _images.Count) _images[i + 1] = _images[i + 1] ?? null;
                    SetAllImagesTransparent(new Image[] { image1, image2 }, _immediateTimeChange);
                    if (image2 != null) image2.CrossFadeAlpha(currentHealth / (_healthPerImage * (i + 1)), _normalTimeChange, false);
                    if (image1 != null) image1.CrossFadeAlpha((1 - currentHealth / (_healthPerImage * (i + 1))), _normalTimeChange, false);
                    break;
                }
            }
        }

        //_image.cross
    }

    private void SetAllImagesTransparent(float duration, bool ignoreTimeScale = false)
    {
        foreach (Image image in _images)
        {
            if (image != null) image.CrossFadeAlpha(0, duration, ignoreTimeScale);
        }
    }

    private void SetAllImagesTransparent(Image exception, float duration, bool ignoreTimeScale = false)
    {
        foreach (Image image in _images)
        {
            if (image != null && exception != null) if (image != exception) image.CrossFadeAlpha(0, duration, ignoreTimeScale);
        }
    }

    private void SetAllImagesTransparent(Image[] exceptions, float duration, bool ignoreTimeScale = false)
    {
        foreach (Image image in _images)
        {
            foreach (Image exception in exceptions)
                if (exception != null && exception == image) return;
            if (image != null) image.CrossFadeAlpha(0, duration, ignoreTimeScale);
        }
    }
}
