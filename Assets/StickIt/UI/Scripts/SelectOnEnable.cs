using UnityEngine;
using UnityEngine.UI;
public class SelectOnEnable : MonoBehaviour
{
    private void OnEnable() => GetComponent<Selectable>().Select();
}