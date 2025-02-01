using Assets.Scripts.Character.Enemy;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public static class EventManager
    {
        public static Action<Collider2D, int> onBulletHit;
        public static Func<Collider2D, Item, int, (bool Success, Item RemainingItem, int RemainingCount)> onItemHit;
        public static Action<int> onClearInventorySlot;
        public static Action<InventorySlotUI> onSelectInventorySlot;
        public static Action<EnemyController> onEnemyDie;
    }
}