using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardView : MonoBehaviour
{
    [SerializeField]
    LocalizeStringEvent itemLabel;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    Animator _animator;
    IItem _item;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Init(IItem item)
    {
        itemLabel.StringReference.SetReference("Items", string.Format("ITEM_{0}_LABEL", item.Id.ToUpper()));
        itemLabel.RefreshString();
        Addressables.LoadAssetAsync<Sprite>(item.Id).Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded) {
            spriteRenderer.sprite = obj.Result;
        } else {
            Debug.LogError($"AssetReference sprite {_item.Id} failed to load.");
        }
    }

    public void Disappear()
    {
        transform.transform.parent = null;
        _animator.SetTrigger("disappear");
    }

    public void OnDestroyAnimationFinished()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Resources.UnloadAsset(spriteRenderer.sprite);
    }
}
