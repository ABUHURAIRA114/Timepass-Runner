using UnityEngine;

public class EdgeCollider : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Vector3 direction = Vector3.right;

    private void Update() {

        MoveBasedOnVisibility();
    }

    private void MoveBasedOnVisibility() {

        if ( Culling.IsVisible(Camera.main, transform.position + direction) )
            transform.position -= direction * speed * Time.deltaTime;

        if ( !Culling.IsVisible(Camera.main, transform.position - direction) )
            transform.position += direction * speed * Time.deltaTime;
    }
}
