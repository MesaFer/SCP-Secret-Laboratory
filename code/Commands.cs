using System;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP
{
	public partial class ScpGame
	{

		[ServerCmd( "killeveryone" )]
		public static void KillEveryone()
		{
			foreach ( Player player in All.OfType<Player>() )
			{
				player.TakeDamage( DamageInfo.Generic( 100f ) );
			}
		}

		[ServerCmd( "sethealth" )]
		public static void SetHealth( int health )
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;

			caller.Health = health;
		}

		[ServerCmd( "setarmor" )]
		public static void SetArmor( float armor )
		{

			var caller = ConsoleSystem.Caller.Pawn as ScpPlayer;
			if ( caller == null ) return;
			caller.Armor = armor;

		}

		[ServerCmd( "spawn_entity" )]
		public static void SpawnEntity( string entName )
		{
			var owner = ConsoleSystem.Caller.Pawn;

			if ( owner == null )
				return;

			var attribute = Library.GetAttribute( entName );

			if ( attribute == null || !attribute.Spawnable )
				return;

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 200 )
				.UseHitboxes()
				.Ignore( owner )
				.Size( 2 )
				.Run();

			var ent = Library.Create<Entity>( entName );
			if ( ent is BaseCarriable && owner.Inventory != null )
			{
				if ( owner.Inventory.Add( ent, true ) )
					return;
			}

			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) );

			//Log.Info( $"ent: {ent}" );
		}

		[ServerCmd( "setjob" )]
		public static void SetJob( string jobid )
		{

			var caller = ConsoleSystem.Caller.Pawn as ScpPlayer;
			if ( caller == null ) return;
			bool res = caller.SetJob( jobid );
			if(res)
			{
				var newjob = Job.GetByClassName( jobid );
				ConsoleSystem.Caller.SetScore( "job", newjob.DisplayName );
			}
			Log.Info( (res) ? "Ваша роль была изменена!" : "Произошла ошибка при смене роли!" );

		}

		[ServerCmd( "returnbone" )]
		public static void ReturnBone()
		{

			var caller = ConsoleSystem.Caller.Pawn as ScpPlayer;
			if ( caller == null ) return;
			caller.BrokenBone = false;
			Log.Info( caller.CurrentJob.Name );
			//Log.Info( "Current job: " + caller.CurrentJob.Name );

		}

		[ServerCmd( "getjob" )]
		public static void GetJob()
		{

			var caller = ConsoleSystem.Caller.Pawn as ScpPlayer;
			if ( caller == null ) return;
			Log.Info( caller.CurrentJob.Name );
			//Log.Info( "Current job: " + caller.CurrentJob.Name );

		}

		[ServerCmd( "setrpname" )]
		public static void SetRpName( string newName )
		{

			var caller = ConsoleSystem.Caller.Pawn as ScpPlayer;
			if ( caller == null ) return;
			caller.RpName = newName;
			ConsoleSystem.Caller.SetScore( "rpname", newName );

		}


		[ServerCmd( "damageoui" )]
		public static void DamageTarget( int damage )
		{
			var caller = ConsoleSystem.Caller.Pawn;
			var damageAmount = DamageInfo.Generic( damage );
			caller.TakeDamage( damageAmount );
		}

		[ServerCmd( "createtestent" )]
		public static void CreateTestEnt()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;

			new HealthUsable()
			{
				Position = caller.Position + caller.Rotation.Forward * 50
			};
		}
	}
}
