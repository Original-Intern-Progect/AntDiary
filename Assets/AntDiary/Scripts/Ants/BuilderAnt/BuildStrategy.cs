using UnityEngine;

namespace AntDiary{
	public class BuildStrategy: BuilderStrategy{
		private NestBuildableElement _distElement;

		private float CommitResourcePerSec{ get; } = 3.0f;
		
		public BuildStrategy(BuilderAnt ant, NestBuildableElement distElement) : base(ant){
			_distElement = distElement;

			UpdateInterval = 1.0f;
		}

		public override void StartStrategy(){
			
		}

		public override void PeriodicUpdate(){
			Debug.Log("<BuildStrategy> commit");
			_distElement.Commit(CommitResourcePerSec);

			if(!_distElement.IsUnderConstruction){
				HostAnt.ChangeStrategy(new RoundStrategy(HostAnt));
			}
		}

		public override void FinishStrategy(){
			
		}
	}
}