using UnityEngine;

namespace AntDiary{
	internal class AStarNode{

		internal IPathNode Node{
			get;
		}

		internal AStarNode Parent{
			get;
		}

		internal float Cost{
			get;
		}

		internal float HeuristicCost{
			get;
		}

		internal float TotalCost => Cost + HeuristicCost;

		internal IPathNode DestNode{
			get;
		}
		
		// public AStarNodeStatus status;

		public AStarNode(IPathNode node, AStarNode parent, float cost, IPathNode to){
			Node = node;
			Parent = parent;
			Cost = cost;
			DestNode = to;
			HeuristicCost = Vector2.Distance(node.WorldPosition, to.WorldPosition);
			
		}

		public override string ToString(){
			
			//return $"node:{Node.Uid} c:{Cost} h:{HeuristicCost}: tc:{TotalCost} from:{(Parent == null ? "null" : Parent.Node.Uid.ToString())}";
			return $"node:{Node.WorldPosition} c:{Cost} h:{HeuristicCost}: tc:{TotalCost} from:{(Parent == null ? "null" : Parent.Node.WorldPosition.ToString())}";
		}
	}
}