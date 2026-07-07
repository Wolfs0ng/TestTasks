using System;
using System.Collections.Generic;
using MagicTower.Battle.Controllers;
using UnityEngine;

namespace MagicTower.Battle.UI
{
	public class DamageNumberSpawner : MonoBehaviour
	{
		[SerializeField] private TowerController tower;
		[SerializeField] private Color towerColor;
		[SerializeField] private EnemyManager enemyManager;
		[SerializeField] private Color enemyColor;
		[SerializeField] private DamageNumberView damageNumberPrefab;
		[SerializeField] private RectTransform damageNumberContainer;
		[SerializeField] private Camera worldCamera;
		[SerializeField] private Vector3 worldOffset = new(0f, 1.5f, 0f);

		private readonly Dictionary<TowerController, Action<int>> towerHandlers = new();
		private readonly Dictionary<EnemyController, Action<int>> enemyHandlers = new();

		private void Awake()
		{
			if (worldCamera == null)
			{
				worldCamera = Camera.main;
			}
		}

		private void OnEnable()
		{
			SubscribeToTower();
			SubscribeToEnemyManager();
			SubscribeToExistingEnemies();
		}

		private void OnDisable()
		{
			UnsubscribeFromEnemyManager();
			UnsubscribeFromTower();
			UnsubscribeFromAllEnemies();
		}

		private void SubscribeToTower()
		{
			if (tower == null)
			{
				return;
			}

			if (towerHandlers.ContainsKey(tower))
			{
				return;
			}

			Action<int> handler = damageAmount => HandleDamageTaken(tower.Transform, damageAmount, towerColor);

			towerHandlers.Add(tower, handler);
			tower.OnDamageTaken += handler;
		}

		private void UnsubscribeFromTower()
		{
			foreach (KeyValuePair<TowerController, Action<int>> pair in towerHandlers)
			{
				if (pair.Key == null)
				{
					continue;
				}

				pair.Key.OnDamageTaken -= pair.Value;
			}

			towerHandlers.Clear();
		}

		private void SubscribeToEnemyManager()
		{
			if (enemyManager == null)
			{
				return;
			}

			enemyManager.OnEnemyRegistered += HandleEnemyRegistered;
			enemyManager.OnEnemyUnregistered += HandleEnemyUnregistered;
		}

		private void UnsubscribeFromEnemyManager()
		{
			if (enemyManager == null)
			{
				return;
			}

			enemyManager.OnEnemyRegistered -= HandleEnemyRegistered;
			enemyManager.OnEnemyUnregistered -= HandleEnemyUnregistered;
		}

		private void SubscribeToExistingEnemies()
		{
			if (enemyManager == null)
			{
				return;
			}

			IReadOnlyList<EnemyController> activeEnemies = enemyManager.ActiveEnemies;

			for (int i = 0; i < activeEnemies.Count; i++)
			{
				SubscribeToEnemy(activeEnemies[i]);
			}
		}

		private void HandleEnemyRegistered(EnemyController enemy)
		{
			SubscribeToEnemy(enemy);
		}

		private void HandleEnemyUnregistered(EnemyController enemy)
		{
			UnsubscribeFromEnemy(enemy);
		}

		private void SubscribeToEnemy(EnemyController enemy)
		{
			if (enemy == null)
			{
				return;
			}

			if (enemyHandlers.ContainsKey(enemy))
			{
				return;
			}

			Action<int> handler = damageAmount => HandleDamageTaken(enemy.Transform, damageAmount, enemyColor);

			enemyHandlers.Add(enemy, handler);
			enemy.OnDamageTaken += handler;
		}

		private void UnsubscribeFromEnemy(EnemyController enemy)
		{
			if (enemy == null)
			{
				return;
			}

			if (!enemyHandlers.TryGetValue(enemy, out Action<int> handler))
			{
				return;
			}

			enemy.OnDamageTaken -= handler;
			enemyHandlers.Remove(enemy);
		}

		private void UnsubscribeFromAllEnemies()
		{
			foreach (KeyValuePair<EnemyController, Action<int>> pair in enemyHandlers)
			{
				if (pair.Key == null)
				{
					continue;
				}

				pair.Key.OnDamageTaken -= pair.Value;
			}

			enemyHandlers.Clear();
		}

		private void HandleDamageTaken(Transform targetTransform, int damageAmount, Color color)
		{
			if (targetTransform == null)
			{
				return;
			}

			if (damageAmount <= 0)
			{
				return;
			}

			Vector3 worldPosition = targetTransform.position + worldOffset;
			SpawnDamageNumber(worldPosition, damageAmount, color);
		}

		private void SpawnDamageNumber(Vector3 worldPosition, int damageAmount, Color color)
		{
			if (damageNumberPrefab == null || damageNumberContainer == null || worldCamera == null)
			{
				return;
			}

			DamageNumberView damageNumber = Instantiate(damageNumberPrefab, damageNumberContainer);

			if (damageNumber == null)
			{
				return;
			}

			damageNumber.Initialize(damageAmount, worldPosition, damageNumberContainer, worldCamera, color);
		}
	}
}