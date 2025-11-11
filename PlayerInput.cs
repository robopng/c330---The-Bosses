using UnityEngine;

/// <summary>
/// Handles all player input detection and provides a clean "interface"
/// for other scripts to read from.
/// 
/// New Bindings:
/// - WASD: Movement
/// - Left Shift (Tap): Dash
/// - Left Click (Tap): Melee Attack
/// - Left Click (Hold): Charge Ranged Attack
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The time in seconds to hold Left Click before it counts as a charge (not a melee).")]
    public float meleeOrChargeThreshold = 0.25f;
    [Tooltip("The max time in seconds to hold for a full ranged charge.")]
    public float maxChargeTime = 2.0f;

    [Header("Debug")]
    [Tooltip("Enable to see all input and position logs in the console.")]
    public bool enableDebugLogging = true;

    // --- PUBLIC INPUT PROPERTIES ---
    public Vector2 MovementInput { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool IsCharging { get; private set; }
    public bool RangedAttackReleased { get; private set; }
    public float ChargePercent { get; private set; }

    // --- Internal State ---
    private float leftClick_holdTimer = 0f;
    private bool wasChargingLastFrame = false;

    void Update()
    {
        // --- Reset all "one-frame" press/release flags ---
        AttackPressed = false;
        DashPressed = false;
        RangedAttackReleased = false;

        // --- 1. Movement (WASD) ---
        MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // --- 2. Dash (Left Shift) ---
        // This is the changed line
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashPressed = true;
            if (enableDebugLogging)
                Debug.Log($"[PlayerInput] Event: DashPressed (Frame: {Time.frameCount})");
        }

        // --- 3. Melee Attack / Ranged Charge (Left Click) ---

        // A. Button Just Pressed Down (Mouse 0)
        if (Input.GetMouseButtonDown(0))
        {
            leftClick_holdTimer = 0f;
            IsCharging = false;
            ChargePercent = 0f;
            wasChargingLastFrame = false; // For debug logging
        }

        // B. Button is Being Held
        if (Input.GetMouseButton(0))
        {
            leftClick_holdTimer += Time.deltaTime;

            if (leftClick_holdTimer >= meleeOrChargeThreshold)
            {
                IsCharging = true;
                
                float actualChargeTime = leftClick_holdTimer - meleeOrChargeThreshold;
                ChargePercent = Mathf.Clamp01(actualChargeTime / maxChargeTime);
            }
        }

        // C. Button Was Just Released
        if (Input.GetMouseButtonUp(0))
        {
            if (IsCharging)
            {
                RangedAttackReleased = true;
                if (enableDebugLogging)
                    Debug.Log($"[PlayerInput] Event: RangedAttackReleased. Final Charge: {ChargePercent:P1} (Frame: {Time.frameCount})");
            }
            else
            {
                AttackPressed = true;
                if (enableDebugLogging)
                    Debug.Log($"[PlayerInput] Event: AttackPressed (Hold time: {leftClick_holdTimer:F2}s) (Frame: {Time.frameCount})");
            }

            leftClick_holdTimer = 0f;
            IsCharging = false;
        }

        // --- CONTINUOUS DEBUG LOGGING ---
        if (enableDebugLogging)
        {
            if (IsCharging && !wasChargingLastFrame)
            {
                Debug.Log($"[PlayerInput] Event: ChargeStarted (at {Time.time})");
            }
            if(IsCharging)
            {
                Debug.Log($"[PlayerInput] Status: Charging... {ChargePercent:P0}");
            }
            wasChargingLastFrame = IsCharging;
        }
    }
}