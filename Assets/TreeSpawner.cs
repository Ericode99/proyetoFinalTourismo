using UnityEngine;
using UnityEngine.InputSystem;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public Transform spawnPoint;
    public float dropHeight = 1.5f;
    public float fallDuration = 0.5f;
    public LayerMask groundLayer = ~0;
    public InputActionProperty spawnAction;

    void OnEnable()
    {
        if (spawnAction != null)
            spawnAction.action.performed += OnSpawnPerformed;
    }

    void OnDisable()
    {
        if (spawnAction != null)
            spawnAction.action.performed -= OnSpawnPerformed;
    }

    void OnSpawnPerformed(InputAction.CallbackContext ctx)
    {
        SpawnTree();
    }

    void SpawnTree()
    {
        if (treePrefab == null || spawnPoint == null)
            return;

        Vector3 startPos = spawnPoint.position + Vector3.up * dropHeight;
        Vector3 targetPos = startPos;

        Ray ray = new Ray(startPos, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f, groundLayer))
        {
            targetPos = hit.point;
        }

        GameObject tree = Instantiate(treePrefab, startPos, Quaternion.identity);
        StartCoroutine(FallToGround(tree.transform, startPos, targetPos));
    }

    System.Collections.IEnumerator FallToGround(Transform tree, Vector3 startPos, Vector3 targetPos)
    {
        float t = 0f;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            tree.position = Vector3.Lerp(startPos, targetPos, t / fallDuration);
            yield return null;
        }

        tree.position = targetPos;
    }
}
