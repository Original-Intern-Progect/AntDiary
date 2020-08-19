using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace AntDiary{
	public class AStarSearcher{
		private NodeGraph _graph;

		public bool Completed{
			get;
			private set;
		}

		public List<IPathNode> Route{
			get;
			private set;
		}
		
		
		public AStarSearcher(NodeGraph graph){
			_graph = graph;
			Route = null;
			Completed = false;
			
			
		}

		/// <summary>
		/// 経路探索を行う
		/// 結果はRouteに入る
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void SearchRoute(IPathNode from, IPathNode to){
			
			Route = new List<IPathNode>();
			if(from == to){
				Completed = true;
				return;
			}
			
			AStarNode root = new AStarNode(from, null, 0, to);
			

			AStarNode goal = SearchProcess(root);
			if(goal == null){
				Completed = true;
				return;
			}

			
			AStarNode parent = goal;
			while(parent != null){
				Route.Add(parent.Node);
				// Debug.Log(parent);
				parent = parent.Parent;
			}
			Route.Reverse();
			
			Completed = true;
			
		}

		private AStarNode SearchProcess(AStarNode root){ 
			List<AStarNode> openNodes = new List<AStarNode>();
			List<IPathNode> closedNodes = new List<IPathNode>();
			openNodes.Add(root);
			
			
			while(openNodes.Count > 0){
				openNodes.Sort((nodeA, nodeB) => {
					if(nodeA.TotalCost > nodeB.TotalCost) return 1;
					if(nodeA.TotalCost < nodeB.TotalCost) return -1;
					return 0;
				});

				//最小コストを選択
				AStarNode aStarNode = openNodes[0];

				var connectedNodes = aStarNode.Node.GetConnectedNodes();
				foreach(var edge in aStarNode.Node.Edges){
					var cost = aStarNode.Cost + edge.Cost;
					
					var other = edge.GetOtherNode(aStarNode.Node);
					
					//CloseされたノードはOpenしない
					if (closedNodes.Contains(other))
					{
						continue;
					}
					
					AStarNode childNode = new AStarNode(other, aStarNode, cost, aStarNode.DestNode);
					
					if(other == aStarNode.DestNode){
						return childNode;
					}
					
					openNodes.Add(childNode);
				}

				openNodes.Remove(aStarNode);
				
				//Closeする
				closedNodes.Add(aStarNode.Node);
				
			}

			return null;
		}

	}
}