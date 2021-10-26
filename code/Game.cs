using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SCP
{
	public partial class ScpGame : Game
	{
		public ScpHUD ScpHUD;


		public ScpGame()
		{
			if ( IsClient ) ScpHUD = new ScpHUD();

		}

		[Event.Hotload]
		public void HotloadUpdate()
		{
			if ( !IsClient ) return;
			ScpHUD?.Delete();
			ScpHUD = new ScpHUD();
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			ScpPlayer player = new( client );
			client.Pawn = player;

			player.Respawn();

		}
		
	}
}
