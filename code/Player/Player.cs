using Sandbox;
using System;
using SCP;

namespace SCP
{

	partial class ScpPlayer : Player
	{
		[Net] public float Armor { get; set; }
		[Net] public float Stamina { get; set; }
		[Net] public float Food { get; set; }
		[Net] public string RpName { get; set; }
		[Net] public float TimeBrokenBone { get; set; }
		private float fallSpeed;
		private TimeSince timeSinceJumpReleased;
		[Net] public string CurrentJobID { get; set; }
		public Job CurrentJob;
		private bool FirstSpawn = true;

		public bool BrokenBone = false;


		public ScpPlayer()
		{

		}

		public ScpPlayer( Client cl )
		{
			Inventory = new Inventory( this );
			string defaultJobID = "dclass";
			this.Armor = 0;
			this.Stamina = 100.0f;
			this.TimeBrokenBone = 0f;
			this.CurrentJobID = defaultJobID;
			this.CurrentJob = Job.GetByClassName( defaultJobID );
			this.RpName = cl.Name;
			cl.SetValue( "rpname", RpName );
			cl.SetValue( "job", CurrentJob.DisplayName );
			cl.SetValue( "categorycolor", this.GetCategory().Color );
			cl.SetValue( "rank", "Player" );

		}

		public override void Respawn()
		{
			Job respawnJob = GetJob();
			string model = respawnJob.Model;
			SetModel( model );
			if ( !FirstSpawn )
				//Dress();
			FirstSpawn = false;

			Controller = new SCP.WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			//Dress( respawnJob.Clothes );
			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Controller = null;
			EnableAllCollisions = false;
			EnableDrawing = false;
			var Staminanon = 100 - Stamina;
			BrokenBone = false;
			Stamina += Staminanon;
			Job respawnJob = GetJob();
			string jobID = respawnJob.ID;
			if ( jobID == "scp173" )
			{
				Log.Info( "SCP - 173 Contained Successfully Containment Unit Unknown" );
			}


		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			TickPlayerUse();



			if ( LifeState != LifeState.Alive )
				return;



			var controller = GetActiveController();

			if ( controller != null )
				EnableSolidCollisions = !controller.HasTag( "noclip" );

			if ( !controller.HasTag( "noclip" ) && Input.Down( InputButton.Run ) )
			{
				if ( Stamina > 0 )
				{
					if ( Math.Abs( base.Velocity.y ) > 200 || Math.Abs( base.Velocity.x ) > 200 )
					{
						var player = Local.Pawn as ScpPlayer;
						Job respawnJob = GetJob();
						float Staminajob = respawnJob.Stamina;
						Stamina -= Staminajob;
					}
					else
					{
						RegenStamina();
					}

				}

			}
			else if ( !controller.HasTag( "noclip" ) && Input.Pressed( InputButton.Jump )  )
			{
				if ( Stamina > (GetJob().Stamina * 100) )
				{
					var player = Local.Pawn as ScpPlayer;
					Job respawnJob = GetJob();
					float Staminajob = respawnJob.Stamina * 100;
					Stamina -= Staminajob;
				}
			}
			else
			{
				RegenStamina();
			}


			if ( Input.Pressed( InputButton.View ) )
			{
				if ( Camera is ThirdPersonCamera )
				{
					Camera = new FirstPersonCamera();
				}
				else
				{
					Camera = new ThirdPersonCamera();

				}
			}


			if ( Input.Released( InputButton.Jump ) )
			{
				if ( timeSinceJumpReleased < 0.3f )
				{
					Game.Current?.DoPlayerNoclip( cl );
				}

				timeSinceJumpReleased = 0;
			}

			if ( Input.Left != 0 || Input.Forward != 0 )
			{
				timeSinceJumpReleased = 1;
			}

			FallDamages();


		}

		private void FallDamages()
		{
			if ( TimeBrokenBone <= 0)
			{
				TimeBrokenBone += 1;
				if ( TimeBrokenBone == 0)
				{
					BrokenBone = false;
				}
			}
			if ( GroundEntity != null )
			{
				if ( fallSpeed < -500 )
				{
					float fallDamageAmount = (fallSpeed + 500) / 3;
					var fallDamageInfo = DamageInfo.Generic( (fallDamageAmount * fallDamageAmount / 50) / 4 );
					float fallDamageAmountNum = (fallDamageAmount * fallDamageAmount / 50) / 4;
					var player = Local.Pawn as ScpPlayer;
					Job respawnJob = GetJob();
					string jobID = respawnJob.ID;
					if ( jobID != "scp173" )
					{
						base.TakeDamage( fallDamageInfo );
						if ( fallDamageAmountNum >= 25 )
						{
							BrokenBone = true;
							TimeBrokenBone -= 100;
							Stamina = 10;
						}

					}
					fallSpeed = 1;
				}

			}

			fallSpeed = base.Velocity.z;
		}

		private void RegenStamina()
		{
			if ( Stamina < 99.8f && BrokenBone == false)
			{
				Stamina += 0.2f;
			}
		}

		public bool SetJob( string jobId )
		{
			Job job = Job.GetByClassName( jobId );
			if ( job != null && job != CurrentJob )
			{
				CurrentJob = job;
				CurrentJobID = job.ID;
				Respawn();
				
				return true;
			}
			return false;
		}

		public Job GetJob()
		{
			if ( IsServer )
			{
				return CurrentJob;
			}
			else
			{
				return Job.GetByClassName( CurrentJobID );
			}

		}

		public Category GetCategory()
		{
			if ( IsServer )
			{
				return Category.GetByClassName( CurrentJob.Category );
			}
			else
			{
				return Category.GetByClassName( "default" );
			}
				

		}

		public override void TakeDamage(DamageInfo damages)
		{
			damages.Damage = damages.Damage - this.Armor;
			base.TakeDamage( damages );
		}


	}

}
