using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBox : MonoBehaviour
{
    [SerializeField] private Vector3 _size;
    [SerializeField, Min(0.25f)] private float _waitTime;
    [SerializeField] private Dictionary<Collider, Vector3> _ObjectsStartPos;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        _ObjectsStartPos = new Dictionary<Collider, Vector3>();
        Collider[] TempColliders = Physics.OverlapBox(transform.position, _size / 2, Quaternion.identity, _layerMask);
        for (int i = 0; i < TempColliders.Length; i++)
        {
            _ObjectsStartPos.Add(TempColliders[i], TempColliders[i].gameObject.transform.position + (Vector3.up * 10));
        }
        StartCoroutine(Schekker());
    }


    private IEnumerator Schekker()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitTime * 60);
            List<Collider> TempColliders = Physics.OverlapBox(transform.position, _size / 2, Quaternion.identity, _layerMask).ToList();
            foreach (KeyValuePair<Collider, Vector3> item in _ObjectsStartPos)
            {
                if (!TempColliders.Contains(item.Key))
                {
                    item.Key.gameObject?.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
                    item.Key.gameObject.transform.position = item.Value;
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, _size);
    }
}
