using UnityEngine;

// =====================================================================
// MinerFlashlight.cs
// -----------------------------------------------------------------
// Controls the miner's headlamp/flashlight.
// The flashlight is a Unity "Spot Light" component, positioned under
// the Camera (or head), so it lights up whatever direction is faced.
//
// FEATURES:
//  1) Toggle the flashlight on/off with the F key
//  2) Battery drains over time (optional)
//  3) Steady light, no flicker effect
//  4) Exposes a public property so other scripts (generator, keycard
//     scanner) can check whether the flashlight is on or off
// =====================================================================

[RequireComponent(typeof(Light))] // This script requires a Light component
public class MinerFlashlight : MonoBehaviour
{
    [Header("Main Settings")]
    [Tooltip("Key used to toggle the flashlight")]
    public KeyCode toggleKey = KeyCode.F;

    [Tooltip("Whether the flashlight is on when the game starts")]
    public bool startsOn = true;

    [Header("Light Settings (Spot Light)")]
    [Tooltip("Range of the flashlight beam")]
    public float lightRange = 12f;

    [Tooltip("Width of the flashlight cone (degrees)")]
    [Range(1f, 179f)]
    public float spotAngle = 45f;

    [Tooltip("Light intensity (brightness)")]
    public float lightIntensity = 3f;

    [Header("Battery (optional) - set useBattery = false if not needed")]
    public bool useBattery = false;
    [Tooltip("Full battery capacity (in seconds)")]
    public float maxBattery = 120f;
    private float currentBattery;

    [Header("Sound Effects (optional)")]
    public AudioSource audioSource;      // Plays a sound when the toggle key is pressed
    public AudioClip toggleSound;        // Click sound

    // ----- Internal variables -----
    private Light flashlightLight;   // Reference to the Light component
    public bool IsOn { get; private set; } // Other scripts read the flashlight state from here

    void Awake()
    {
        flashlightLight = GetComponent<Light>();

        // Ensure the light type is set to "Spot"
        flashlightLight.type = LightType.Spot;
        flashlightLight.range = lightRange;
        flashlightLight.spotAngle = spotAngle;
        flashlightLight.intensity = lightIntensity;

        // Soft shadows make the mine feel more realistic
        flashlightLight.shadows = LightShadows.Soft;

        currentBattery = maxBattery;
    }

    void Start()
    {
        IsOn = startsOn;
        flashlightLight.enabled = IsOn;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }

        if (useBattery && IsOn)
        {
            currentBattery -= Time.deltaTime;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                SetFlashlight(false); // Turn off automatically when the battery runs out
            }
        }
    }

    private void ToggleFlashlight()
    {
        // No point trying to turn it on if the battery is empty
        if (useBattery && currentBattery <= 0f && !IsOn)
            return;

        SetFlashlight(!IsOn);
    }

    // Explicitly sets the flashlight state (e.g. the generator can force it off)
    public void SetFlashlight(bool state)
    {
        IsOn = state;
        flashlightLight.enabled = IsOn;

        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound);
        }
    }

    // Recharges the battery (e.g. called when a battery pickup is found near the generator)
    public void RechargeBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
    }

    // Exposes the battery percentage for other scripts (e.g. UI)
    public float GetBatteryPercent()
    {
        if (!useBattery) return 1f; // Always report 100% if the battery system is disabled
        return currentBattery / maxBattery;
    }
}
