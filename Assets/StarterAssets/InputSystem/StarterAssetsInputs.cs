using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool shoot;
		public bool shoot2;
		public bool changeWeaponForward;
		public bool changeWeaponBack;
		public Vector2 scrollDirection;
		public bool reload;

		public bool shooting1;
		public bool shooting2;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
        }

		public void OnShoot2(InputValue value)
		{
			ShootInput2(value.isPressed);
		}

		public void OnChangeWeaponBack(InputValue value)
		{
			ChangeWeaponBack(value.isPressed);
		}

        public void OnChangeWeaponForward(InputValue value)
        {
			ChangeWeaponForward(value.isPressed);
        }

        public void OnScrollWheel(InputValue value)
        {
            ScrollWheel(value.Get<Vector2>());
        }

		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void ShootInput2(bool newShoot2State)
		{
			shoot2 = newShoot2State;
		}

		public void ChangeWeaponBack(bool newChangeWeaponBackState)
		{
			changeWeaponBack = newChangeWeaponBackState;
		}

        public void ChangeWeaponForward(bool newChangeWeaponForwardState)
        {
            changeWeaponForward = newChangeWeaponForwardState;
        }

        public void ScrollWheel(Vector2 newScrollDirection)
        {
            scrollDirection = newScrollDirection;
        }

		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
		}


        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}