using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace SCP
{
	public partial class ScoreboardEntry : Panel
	{
		public Client Client;

		public Image Icon;
		public Label PlayerName;
		public Label Job;
		public Label Rank;
		public Label Ping;
		public Label CategoryBorder;

		public ScoreboardEntry()
		{
			AddClass( "entry" );

			Icon = Add.Image( "", "icon" );
			PlayerName = Add.Label( "", "name" );
			Job = Add.Label( "", "job" );
			Rank = Add.Label( "", "rank" );
			Ping = Add.Label( "", "ping" );
			CategoryBorder = Add.Label( "", "catborder" );
			//Header: Nombre joueurs + Entry stats
			//Entry: Icône joueur, RpName, Job, Rank, 
		}

		RealTimeSince TimeSinceUpdate = 0;
		public override void Tick()
		{
			base.Tick();

			if ( !IsVisible )
				return;

			if ( !Client.IsValid() )
				return;

			if ( TimeSinceUpdate < 0.1f )
				return;

			TimeSinceUpdate = 0;
			UpdateData();
		}

		public virtual void UpdateData()
		{
			PlayerName.Text = Client.GetValue<string>( "rpname" );
			Job.Text = Client.GetValue<string>( "job" );
			Rank.Text = Client.GetValue<string>( "rank" );
			ulong SteamId = Client.GetValue<ulong>( "steamid", 0 );
			Icon.SetTexture( $"avatar:{SteamId}" );
			Ping.Text = Client.Ping.ToString();
			SetClass( "me", Client == Local.Client );

		}

		public virtual void UpdateFrom( Client client )
		{
			Client = client;
			UpdateData();
		}

	}
}
