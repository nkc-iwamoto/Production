using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class RuteCollider : MonoBehaviour
{
    private Subject<Unit> isHit = new Subject<Unit>();
    public System.IObservable<Unit> IsHit => isHit.AsObservable();

    [SerializeField]
    private int playerLayer;

    private void Awake()
    {
        isHit = new Subject<Unit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) { return; }
        if(other.gameObject.TryGetComponent(out Bullet component)) { return; }
        Debug.LogWarning(isHit);
        isHit.OnNext(default);
    }
}
