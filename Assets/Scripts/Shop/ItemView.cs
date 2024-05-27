using Game.Managers;
using Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.Item;

namespace Game.Ui
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private int itemId;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image itemImage;

        private bool timerIsActive;

        private IItem item;

        private void OnEnable()
        {
            EventBus.Subscribe<ItemStatusChangedEvent>(UpdateView);
        }

        private void Start()
        {
            item = ShopManager.Instance.GetItem(itemId);
            InitializeView();
        }

        private void Update()
        {
            if (timerIsActive)
            {
                timerText.text = item.GetRemainingTimeFormatted();
            }
            else
            {
                timerText.text = string.Empty;
            }
        }

        public void InitializeView()
        {
            statusText.text = item.IsUnlocked ? "Unlocked" : "Locked";
            itemImage.sprite = item.Sprite;
        }
        public void UpdateView(ItemStatusChangedEvent e)
        {
            if (e.ItemId == itemId)
            {
                statusText.text = item.IsUnlocked ? "Unlocked" : "Locked";
                if (e.IsTemporary)
                {
                    timerIsActive = true;
                }
                else
                {
                    timerIsActive = false;
                }
            }
        }
    }
}