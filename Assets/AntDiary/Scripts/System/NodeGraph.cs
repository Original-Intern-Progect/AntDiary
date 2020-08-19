using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary{
	public class NodeGraph{
		private int _uidCounter;
		
		private readonly Dictionary<int, Node> _nodeMap;

		public NodeGraph(){
			_nodeMap = new Dictionary<int, Node>();
			_uidCounter = 0;
		}

		/// <summary>
		/// 位置からノードを取得する
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public IEnumerable<Node> SearchNodeByPos(Vector2 pos){
			foreach(var pair in _nodeMap){
				if(pair.Value.Pos.Equals(pos)){
					yield return pair.Value;
				}
			}
		}

		/// <summary>
		/// 新しくノードを作り登録する
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public Node CreateNode(Vector2 pos){
			int uid = GenerateUid();
			var node = new Node(uid, pos);
			_nodeMap.Add(uid, node);
			return node;
		}

		/// <summary>
		/// ノードを削除し、そのノードとの接続関係も削除する
		/// </summary>
		/// <param name="node"></param>
		public void RemoveNode(Node node)
		{
			var connectedNodes = node.GetConnectedNodes();
			foreach(var other in connectedNodes){
				//var connectedNode = pair.Key;
				if (other is Node connectedNode)
				{
					connectedNode.Disconnect(node);
					node.Disconnect(connectedNode);
				}
			}
			_nodeMap.Remove(node.Uid);
		}

		/// <summary>
		/// 	与えられたNode同士を接続する
		/// 	すでに接続されていた場合は道を置き換える
		/// 	nodesのサイズが2未満だとエラー
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="pathCostCoefficient">道の移動コスト係数 指定しなければ1</param>
		public void ConnectNodes(Node[] nodes, float pathCostCoefficient = 1.0f){
			if(nodes.Length < 2){
				throw new Exception("Node数が2未満になっています");
			}

			//2つを取り出す組み合わせ
			for(int i = 0; i < nodes.Length; i++){
				for(int j = i + 1; j < nodes.Length; j++){
					var nodeA = nodes[i];
					var nodeB = nodes[j];

					var distance = Vector2.Distance(nodeA.Pos, nodeB.Pos);
					
					var pathParam = new PathParam(distance, pathCostCoefficient);

					nodeA.Connect(nodeB, pathParam);
					nodeB.Connect(nodeA, pathParam);
				}
			}
		}
		
		// public void DisconnectNodes()

		/// <summary>
		///		新しいUIDを生成する
		/// </summary>
		private int GenerateUid(){
			return _uidCounter++;
		}

	}
}