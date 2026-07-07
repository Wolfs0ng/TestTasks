using System.Collections.Generic;
using MagicTower.Battle.Projectiles;
using UnityEngine;

namespace MagicTower.Battle.Data
{
	[CreateAssetMenu(fileName = "SpellData", menuName = "Magic Tower/Battle/Spell Data")]
	public class SpellDataSO : ScriptableObject
	{
		[SerializeField] private float cooldown = 1f;
		[SerializeField] private int damage = 10;
		[SerializeField] private float projectileSpeed = 8f;
		[SerializeField] private BaseProjectile projectilePrefab;
		[SerializeField] private float explosionRadius = 2f;
		[SerializeField] private GameObject explosionVfxPrefab;
		[SerializeField] private float explosionVfxLifetime = 1.5f;
		[SerializeField] private float arcHeight = 3f;
		[SerializeField] private List<BaseDamageEffectSO> postDamageEffects = new();

		public float Cooldown => cooldown;
		public int Damage => damage;
		public float ProjectileSpeed => projectileSpeed;
		public BaseProjectile ProjectilePrefab => projectilePrefab;
		public float ExplosionRadius => explosionRadius;
		public GameObject ExplosionVfxPrefab => explosionVfxPrefab;
		public float ExplosionVfxLifetime => explosionVfxLifetime;
		public float ArcHeight => arcHeight;
		public IReadOnlyList<BaseDamageEffectSO> PostDamageEffects => postDamageEffects;
	}
}