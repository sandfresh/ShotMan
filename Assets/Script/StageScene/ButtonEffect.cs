using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
     
    public float Scale = 0.95f;

    private Vector3 from;
    private Vector3 to;

    void Start()
    {
       to =  0.95f*gameObject.GetComponent<RectTransform>().localScale;
       from = gameObject.GetComponent<RectTransform>().localScale;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);

        GetComponent<RectTransform>().DOScale(to, Time.fixedDeltaTime * 3);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);

        GetComponent<RectTransform>().DOScale(from, Time.fixedDeltaTime * 3);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onExit(gameObject);

        GetComponent<RectTransform>().DOScale(to, Time.fixedDeltaTime * 3);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);

        GetComponent<RectTransform>().DOScale(from, Time.fixedDeltaTime * 3);
    }

    //void Start()
    //{
    //    GetComponent<Button>().onClick.AddListener(() =>
    //    StartCoroutine(Scaling()));
    //    GetComponent<RectTransform>().localScale = from;

    //}
    //IEnumerator Scaling()
    //{
    //    GetComponent<RectTransform>().localScale = to;
    //    yield return new WaitForSeconds(Time.fixedDeltaTime * 3);
    //    GetComponent<RectTransform>().localScale = from;
    //} 

}
