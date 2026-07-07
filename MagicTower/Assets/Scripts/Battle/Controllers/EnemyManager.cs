using System;
using System.Collections.Generic;
using MagicTower.Battle.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicTower.Battle.Controllers
{
	public class EnemyManager : MonoBehaviour
	{
		public event Action<EnemyController> OnEnemyRegistered;
		public event Action<EnemyController> OnEnemyUnregistered;

		[Serializable]
		private class SpawnPeriod
		{
			[SerializeField] private float startTime;
			[SerializeField] private float spawnInterval = 2.5f;
			[SerializeField] private List<EnemyDataSO> availableEnemies = new();

			public float StartTime => startTime;
			public float SpawnInterval => spawnInterval;
			public IReadOnlyList<EnemyDataSO> AvailableEnemies => availableEnemies;
		}

		[SerializeField] private GameController gameController;
		[SerializeField] private TowerController towerTarget;
		[SerializeField] private List<Transform> spawnPoints = new();
		[SerializeField] private List<SpawnPeriod> spawnPeriods = new();

		private readonly List<EnemyController> activeEnemies = new();

		private float gameTime;
		private float spawnTimer;

		public IReadOnlyList<EnemyController> ActiveEnemies => activeEnemies;

		private void Update()
		{
			if (gameController == null)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: missing GameController.", this);
				return;
			}

			if (!gameController.IsPlaying)
			{
				return;
			}

			gameTime += Time.deltaTime;

			SpawnPeriod activePeriod = GetActiveSpawnPeriod();

			if (activePeriod == null)
			{
				return;
			}

			spawnTimer -= Time.deltaTime;

			if (spawnTimer > 0f)
			{
				return;
			}

			SpawnEnemy(activePeriod);
			spawnTimer = Mathf.Max(0.1f, activePeriod.SpawnInterval);
		}

		public void RegisterEnemy(EnemyController enemy)
		{
			if (enemy == null)
			{
				return;
			}

			if (activeEnemies.Contains(enemy))
			{
				return;
			}

			activeEnemies.Add(enemy);
			OnEnemyRegistered?.Invoke(enemy);
		}

		public void UnregisterEnemy(EnemyController enemy)
		{
			if (enemy == null)
			{
				return;
			}

			if (!activeEnemies.Remove(enemy))
			{
				return;
			}

			OnEnemyUnregistered?.Invoke(enemy);
		}

		public List<EnemyController> GetAliveEnemies()
		{
			List<EnemyController> enemies = new();

			for (int i = activeEnemies.Count - 1; i >= 0; i--)
			{
				EnemyController enemy = activeEnemies[i];

				if (enemy == null)
				{
					activeEnemies.RemoveAt(i);
					continue;
				}

				if (enemy.IsAlive)
				{
					enemies.Add(enemy);
				}
			}

			return enemies;
		}

		public List<EnemyController> GetVisibleEnemies()
		{
			List<EnemyController> enemies = new();

			for (int i = activeEnemies.Count - 1; i >= 0; i--)
			{
				EnemyController enemy = activeEnemies[i];

				if (enemy == null)
				{
					activeEnemies.RemoveAt(i);
					continue;
				}

				if (enemy.IsAlive && enemy.IsVisible)
				{
					enemies.Add(enemy);
				}
			}

			return enemies;
		}

		public bool TryGetClosestVisibleEnemy(Vector3 fromPosition, out EnemyController enemy)
		{
			enemy = null;
			float closestSqrDistance = float.MaxValue;

			for (int i = activeEnemies.Count - 1; i >= 0; i--)
			{
				EnemyController currentEnemy = activeEnemies[i];

				if (currentEnemy == null)
				{
					activeEnemies.RemoveAt(i);
					continue;
				}

				if (!currentEnemy.IsAlive)
				{
					continue;
				}

				if (!currentEnemy.IsVisible)
				{
					continue;
				}

				Vector3 offset = currentEnemy.Transform.position - fromPosition;
				float sqrDistance = offset.sqrMagnitude;

				if (sqrDistance >= closestSqrDistance)
				{
					continue;
				}

				closestSqrDistance = sqrDistance;
				enemy = currentEnemy;
			}

			return enemy != null;
		}

		public bool TryGetRandomVisibleEnemy(out EnemyController enemy)
		{
			List<EnemyController> visibleEnemies = GetVisibleEnemies();

			if (visibleEnemies.Count == 0)
			{
				enemy = null;
				return false;
			}

			enemy = visibleEnemies[Random.Range(0, visibleEnemies.Count)];
			return true;
		}

		public bool TryGetRandomAliveEnemy(out EnemyController enemy)
		{
			List<EnemyController> aliveEnemies = GetAliveEnemies();

			if (aliveEnemies.Count == 0)
			{
				enemy = null;
				return false;
			}

			enemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
			return true;
		}

		private SpawnPeriod GetActiveSpawnPeriod()
		{
			if (spawnPeriods == null || spawnPeriods.Count == 0)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: spawn periods list is empty.", this);
				return null;
			}

			SpawnPeriod activePeriod = null;

			for (int i = 0; i < spawnPeriods.Count; i++)
			{
				SpawnPeriod period = spawnPeriods[i];

				if (period == null)
				{
					continue;
				}

				if (gameTime >= period.StartTime)
				{
					activePeriod = period;
				}
			}

			return activePeriod;
		}

		private void SpawnEnemy(SpawnPeriod activePeriod)
		{
			if (towerTarget == null)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: missing Tower target Transform.", this);
				return;
			}

			if (spawnPoints == null || spawnPoints.Count == 0)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: spawn points list is empty.", this);
				return;
			}

			if (activePeriod.AvailableEnemies == null || activePeriod.AvailableEnemies.Count == 0)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: active spawn period has no available enemies.", this);
				return;
			}

			Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

			if (spawnPoint == null)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: selected spawn point is null.", this);
				return;
			}

			EnemyDataSO enemyData = activePeriod.AvailableEnemies[Random.Range(0, activePeriod.AvailableEnemies.Count)];

			if (enemyData == null)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: selected EnemyDataSO is null.", this);
				return;
			}

			if (enemyData.EnemyPrefab == null)
			{
				Debug.LogWarning($"{nameof(EnemyManager)} on {name}: EnemyDataSO {enemyData.name} has no enemy prefab assigned.", this);
				return;
			}

			EnemyController enemy = Instantiate(enemyData.EnemyPrefab, spawnPoint.position, spawnPoint.rotation);

			if (enemy == null)
			{
				Debug.LogError($"{nameof(EnemyManager)} on {name}: spawned prefab is missing {nameof(EnemyController)}.", this);
				return;
			}

			enemy.Initialize(enemyData, towerTarget, gameController, this);
		}
	}
}