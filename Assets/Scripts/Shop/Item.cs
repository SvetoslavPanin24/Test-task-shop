using UnityEngine;
using System;
using System.Collections;
using Game.Managers;
using Game.Utils;

namespace Game
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
    public class Item : ScriptableObject, IItem
    {
        [SerializeField] protected int id;
        [SerializeField] protected string itemName;
        [SerializeField] protected bool isUnlocked;
        [SerializeField] protected bool isTemporary;
        [SerializeField] protected float unlockTime;

        public int Id => id;
        public string Name => itemName;
        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }
        public bool IsTemporary
        {
            get => isTemporary;
            set => isTemporary = value;
        }
        public float UnlockTime
        {
            get => unlockTime;
            set => unlockTime = value;
        }

        [field: SerializeField] public Sprite Sprite { get; private set; }

        public void Unlock()
        {
            IsUnlocked = true;
            EventBus.Invoke(new ItemStatusChangedEvent(Id, IsUnlocked, IsTemporary));
        }

        public void Lock()
        {
            IsUnlocked = false;
            IsTemporary = false;
            EventBus.Invoke(new ItemStatusChangedEvent(Id, IsUnlocked, IsTemporary));
        }

        public void TemporarilyUnlock(float duration)
        {
            IsTemporary = true;
            Unlock();
            UnlockTime = Time.time + duration;
            ShopManager.Instance.StartCoroutine(LockAfterTime(duration));
        }

        public IEnumerator LockAfterTime(float duration)
        {
            yield return new WaitForSeconds(duration);
            UnlockTime = 0;
            Lock();
        }

        public string GetRemainingTimeFormatted()
        {
            if (!IsUnlocked || !IsTemporary) return string.Empty;
            float remainingTime = UnlockTime - Time.time;
            if (remainingTime <= 0) return "00:00:00";
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        public class ItemStatusChangedEvent
        {
            public int ItemId { get; private set; }
            public bool IsUnlocked { get; private set; }
            public bool IsTemporary { get; private set; }

            public ItemStatusChangedEvent(int ItemId, bool IsUnlocked, bool IsTemporary)
            {
                this.ItemId = ItemId;
                this.IsUnlocked = IsUnlocked;
                this.IsTemporary = IsTemporary;
            }
        }
    }
}