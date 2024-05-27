using System.Collections;
using UnityEngine;

public interface IItem
{
    int Id { get; }
    string Name { get; }
    Sprite Sprite { get; }
    bool IsUnlocked { get; set; }
    bool IsTemporary { get; set; }
    float UnlockTime { get; set; }
    void Unlock();
    void Lock();
    void TemporarilyUnlock(float duration);
    IEnumerator LockAfterTime(float duration);
    string GetRemainingTimeFormatted();
}
