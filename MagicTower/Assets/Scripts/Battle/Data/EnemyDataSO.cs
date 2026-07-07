using MagicTower.Battle.Controllers;
using UnityEngine;

namespace MagicTower.Battle.Data
{
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Magic Tower/Battle/Enemy Data")]
	public class EnemyDataSO : ScriptableObject
	{
		[field: SerializeField] private EnemyController enemyPrefab;
		[field: SerializeField] private int maxHealth = 10;
		[field: SerializeField] private float moveSpeed = 2f;
		[field: SerializeField] private int attackDamage = 1;
		[field: SerializeField] private float attackInterval = 1f;
		[field: SerializeField] private float attackRange = 1.5f;

		public EnemyController EnemyPrefab => enemyPrefab;
		public int MaxHealth => maxHealth;
		public float MoveSpeed => moveSpeed;
		public int AttackDamage => attackDamage;
		public float AttackInterval => attackInterval;
		public float AttackRange => attackRange;
	}
}