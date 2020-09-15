﻿using System.Collections;
using System.Collections.Generic;
using AntDiary.Scripts.Ants;
using UnityEngine;

namespace AntDiary{
	public class BuilderAnt : StrategyAnt<BuilderAntData>{
		
		// Update is called once per frame
		protected override float MovementSpeed => SelfCommonData.BasicMovementSpeed;
		
		protected override Strategy<BuilderAntData> CreateInitialStrategy(){
			return new RoundStrategy();
		}
	}

}

