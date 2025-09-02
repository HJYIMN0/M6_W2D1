using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Material cubeMaterial;

    [SerializeField] private int poolSize;
    [SerializeField] private int poolCapacity;

    [SerializeField] private float _sphereRadius = 5f;

    [SerializeField] private float _delayBetweenCubes = 0.5f;

    private IObjectPool<GameObject> cubePool;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject cube = cubePool.Get();
            StartCoroutine(ReleaseCubeAfterDelay(cube, _delayBetweenCubes));
        }
    }

    private void Start()
    {
        cubePool = new ObjectPool<GameObject>(
            CreateCube,
            OnGetCube,
            OnReleaseCube,
            OnDestroyCube,
            false,
            poolSize,
            poolCapacity);
    }

    IEnumerator ReleaseCubeAfterDelay(GameObject cube, float delay)
    {
        yield return new WaitForSeconds(delay);
        cubePool.Release(cube);
    }

    public GameObject CreateCube()
    {
        GameObject cube = Instantiate(cubePrefab);
        cube.GetComponent<Renderer>().material = cubeMaterial;
        cube.SetActive(false);
        return cube;
    }

    private void OnGetCube(GameObject cube)
    {
        cube.SetActive(true);
        cube.transform.position = Random.insideUnitSphere * _sphereRadius;
        cube.transform.rotation = Random.rotation;
    }

    private void OnReleaseCube(GameObject cube)
    {
        cube.SetActive(false);
    }

    public void OnDestroyCube(GameObject cube)
    {
        Destroy(cube);
    }
}
