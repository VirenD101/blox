using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BlockMovement : MonoBehaviour {
    public float rollSpeed = 500;
    private bool isRolling = false;

    void Update() {
        if (isRolling) return;

        if (Keyboard.current.wKey.isPressed) Assemble(Vector3.forward);
        else if (Keyboard.current.sKey.isPressed) Assemble(Vector3.back);
        else if (Keyboard.current.aKey.isPressed) Assemble(Vector3.left);
        else if (Keyboard.current.dKey.isPressed) Assemble(Vector3.right);
    }

    void Assemble(Vector3 dir) {
        // Determine current orientation
        bool isVertical = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up)) > 0.9f;

        // Calculate how far the edge is from the center in the move direction
        float distToEdge = 0.5f;
        if (!isVertical) {
            // If we are lying down, check if our move direction is the "long" way (the block's local up axis)
            float alignment = Mathf.Abs(Vector3.Dot(dir, transform.up));
            if (alignment > 0.9f) distToEdge = 1.0f; 
        }

        // Calculate the pivot point exactly on the bottom corner
        Vector3 anchor = transform.position + (dir * distToEdge) + (Vector3.down * (isVertical ? 1.0f : 0.5f));
        Vector3 axis = Vector3.Cross(Vector3.up, dir);

        StartCoroutine(Roll(anchor, axis));
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis) {
        isRolling = true;
        
        float totalAngle = 90;
        float elapsed = 0;
        float duration = 0.2f; // Adjust this to change speed (lower is faster)
        
        Quaternion startRotation = transform.rotation;
        Vector3 startPosition = transform.position;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Use SmoothStep for a better feel
            float curve = t * t * (3f - 2f * t);
            
            // Calculate the current rotation angle
            float currentAngle = curve * totalAngle;
            
            transform.position = startPosition;
            transform.rotation = startRotation;
            transform.RotateAround(anchor, axis, currentAngle);
            
            yield return null;
        }

        // Snap coordinates
        SnapToGrid();
        
        isRolling = false;
    }

    void SnapToGrid() {
        Vector3 pos = transform.position;
        
        // Check orientation
        bool isVertical = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up)) > 0.9f;

        // Snap Height
        pos.y = isVertical ? 1.0f : 0.5f;

        // 3. Snap X and Z
        if (isVertical) {
            // Standing block logic
            pos.x = Mathf.Round(pos.x);
            pos.z = Mathf.Round(pos.z);
        } else {
            // Lying Block Logic
            bool lyingAlongX = Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) > 0.9f;
            
            if (lyingAlongX) {
                pos.x = Mathf.Floor(pos.x) + 0.5f;
                pos.z = Mathf.Round(pos.z);
            } else {
                pos.z = Mathf.Floor(pos.z) + 0.5f;
                pos.x = Mathf.Round(pos.x);
            }
        }

        transform.position = pos;

        // Rotation
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = Mathf.Round(rot.x / 90) * 90;
        rot.y = Mathf.Round(rot.y / 90) * 90;
        rot.z = Mathf.Round(rot.z / 90) * 90;
        transform.rotation = Quaternion.Euler(rot);

        CheckFloor();
    }

    void CheckFloor() {
        // Find state
        bool isVertical = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up)) > 0.9f;

        // Check for win
        if (isVertical) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) {
                if (hit.collider.CompareTag("Goal")) {
                    // Small delay
                    Invoke("TriggerWin", 0.2f); 
                    return; 
                }
            }
        }

        // Safety and Tip check
        if (isVertical) {
            if (!IsTileAtPosition(transform.position + Vector3.down)) {
                StartCoroutine(FallAnimation(null));
            }
        } else {
            Vector3 longAxis = transform.up;
            bool frontSafe = IsTileAtPosition(transform.position + Vector3.down + (longAxis * 0.5f));
            bool backSafe = IsTileAtPosition(transform.position + Vector3.down - (longAxis * 0.5f));

            if (!frontSafe && !backSafe) {
                StartCoroutine(FallAnimation(null));
            } else if (!frontSafe) {
                StartCoroutine(FallAnimation(longAxis));
            } else if (!backSafe) {
                StartCoroutine(FallAnimation(-longAxis));
            }
        }
    }

    bool IsTileAtPosition(Vector3 pos) {
        RaycastHit hit;
        // Floor Check
        if (Physics.Raycast(pos + Vector3.up, Vector3.down, out hit, 2f)) {
            return hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Goal");
        }
        return false;
    }

    IEnumerator FallAnimation(Vector3? direction) {
        isRolling = true; // Lock controls so no spam

        if (direction.HasValue) {
            Vector3 pivot = transform.position + (direction.Value * 0.5f) + (Vector3.down * 0.5f);
            Vector3 axis = Vector3.Cross(Vector3.up, direction.Value);
            
            float angle = 0;
            while (angle < 90f) {
                float step = 450f * Time.deltaTime; // Tip Speed
                if (angle + step > 90f) step = 90f - angle;
                transform.RotateAround(pivot, axis, step);
                angle += step;
                yield return null;
            }
        }

        // GRAVITY LOGIC when falling
        float elapsed = 0;
        while (elapsed < 1.2f) {
            transform.position += Vector3.down * 18f * Time.deltaTime;
            transform.Rotate(new Vector3(1.2f, 0.5f, 0.3f) * 200f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetToStart();
    }

    void TriggerWin() {
        Debug.Log("LEVEL COMPLETE!");
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(nextSceneIndex);
        } else {
            Debug.Log("All game levels cleared! Returning to main menu console.");
            SceneManager.LoadScene(0);
        }
    }

    void ResetToStart() {
        // Starting Coords
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = Quaternion.identity;
        isRolling = false;
    }
}