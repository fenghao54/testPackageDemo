using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class role : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
 {

    

    public RectTransform roler;
    public void OnBeginDrag( PointerEventData eventData ) {
      
    }

    public void OnDrag( PointerEventData eventData ) {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle( roler, eventData.position, eventData.enterEventCamera, out pos );
        roler.position = pos;
    }

    public void OnEndDrag( PointerEventData eventData ) {
      
    }

    private void OnTriggerEnter2D( Collider2D coll ) {
        if( coll.tag == "role" ) {
            Debug.Log( "碰撞发生"+coll.gameObject.name );

        }
    }

    //private void OnCollisionEnter2D( Collision2D collisionInfo ) {
    //    Debug.Log( "碰撞发生" );
    //}
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
