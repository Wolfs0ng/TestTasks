using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	public class EnemyMovement : MonoBehaviour
	{
		[SerializeField] private float rotationSpeed = 12f;

		private Transform target;
		private GameController gameController;
		private float moveSpeed;
		private float stoppingDistance;
		private bool isInitialized;

		public bool IsInRange { get; private set; }

		public void Initialize(Transform target, float moveSpeed, float stoppingDistance, GameController gameController)
		{
			this.target = target;
			this.moveSpeed = moveSpeed;
			this.stoppingDistance = stoppingDistance;
			this.gameController = gameController;

			IsInRange = false;
			isInitialized = true;
		}

		private void Update()
		{
			if (!CanMove())
			{
				return;
			}

			MoveToTarget();
		}

		private bool CanMove()
		{
			if (!isInitialized)
			{
				return false;
			}

			if (target == null)
			{
				return false;
			}

			if (gameController != null && !gameController.IsPlaying)
			{
				return false;
			}

			return true;
		}

		private void MoveToTarget()
		{
			Vector3 currentPosition = transform.position;
			Vector3 targetPosition = target.position;

			targetPosition.y = currentPosition.y;

			Vector3 direction = targetPosition - currentPosition;
			float distance = direction.magnitude;

			IsInRange = distance <= stoppingDistance;

			if (IsInRange)
			{
				return;
			}

			Vector3 normalizedDirection = direction.normalized;
			transform.position += normalizedDirection * moveSpeed * Time.deltaTime;

			RotateTowards(normalizedDirection);
		}

		private void RotateTowards(Vector3 direction)
		{
			if (direction.sqrMagnitude <= 0.001f)
			{
				return;
			}

			Quaternion targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
	}
}