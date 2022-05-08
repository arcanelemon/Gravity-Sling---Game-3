using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RadialIndicator : MonoBehaviour
{
    private Transform target;
    public Transform Target => target;

    private RectTransform rect;

    private bool visible;
    public bool Visible => visible;

    CanvasGroup group;

    [SerializeField]
    Sprite[] sprites;

    public enum Type
    {
        Help,
        Attract1,
        Attract2
    }

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }

    public void SetTarget(Transform target) 
    {
        this.target = target;
    }

    public void SetAngle(float angle) 
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        rect.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }

    public void SetSpriteFromType(Type type) 
    {

    }

    public void SetVisibility(bool visible) 
    {
        this.visible = visible;

        if (visible)
            group.alpha = 1;
        else
            group.alpha = 0;
    }
}
