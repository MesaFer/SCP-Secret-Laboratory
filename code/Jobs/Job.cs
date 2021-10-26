using Sandbox;
using System.Collections.Generic;

namespace SCP
{

	[Library( "job" ), AutoGenerate]
	public partial class Job : Asset
	{

		[Property]
		public string ID { get; set; }
		[Property]
		public string DisplayName { get; set; }

		[Property, ResourceType( "vmdl" )]
		public string Model { get; set; }
		[FGDType( "array:string" )]
		public string[] Clothes { get; set; }

		[Property]
		public string Description { get; set; }
		[Property]
		public int MaxPlayers { get; set; }
		[Property]
		public int Salary { get; set; }
		[Property]
		public string Category { get; set; }
		[Property]
		public float Stamina { get; set; }
		[Property]
		public float WalkSpeed { get; set; }
		[Property]
		public float SprintSpeed { get; set; }
		[Property]
		public float DefaultSpeed { get; set; }
		[Property]
		public int SortOrder { get; set; }



		public static IReadOnlyList<Job> All => _all;
		internal static List<Job> _all = new();

		protected override void PostLoad()
		{
			base.PostLoad();
			if ( !_all.Contains( this ) )
				_all.Add( this );
		}

		public static Job GetByClassName( string className )
		{

			foreach ( Job job in Job.All )
			{
				if ( job.ID == className )
				{
					return job;
				}
			}
			return null;
		}

	}
}



