using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeManager : MonoBehaviour
{
  

    public Terrain _terrain;
    private List<TreeInstance> _trees;
    private List<TreeProduction> _treesCutting;
    [Range(1, 500)]
    public int _woodAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _trees = new List<TreeInstance>(_terrain.terrainData.treeInstances);
        _treesCutting = new List<TreeProduction>();
        
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TreeInstance? GetClosestTree(Vector3 position)
    {
        Debug.Log("closer tree called");
        float minDistance;
        TreeInstance selectedTree;

        if (_trees.Count > 0)
        {
            minDistance = Vector3.Distance(TreeCoordinateToWorld( _trees[0]), position);
            Debug.Log($"minDistance {minDistance}");
            selectedTree = _trees[0];
            foreach (TreeInstance tree in _trees)
            {
                float distance = Vector3.Distance(TreeCoordinateToWorld(tree), position);
                Debug.Log($"Distance {minDistance}");
                if (distance < minDistance)
                {
                    minDistance = distance;
                    Debug.Log($"minDistance {minDistance}");
                    selectedTree = tree;
                }
            }
            Debug.Log($"selected tree in position{TreeCoordinateToWorld(selectedTree)}");
            return selectedTree;
        }
        return null;
        
    }

    public bool Produce(TreeInstance tree, int strength)
    {
        bool present  = false;
        for(int i = 0; i < _treesCutting.Count; i++ )
        {
            if(tree.position == _treesCutting[i].tree.position)
            {
                present = true;
                return CutTree(i, strength);
                
            }
        }
        if(present == false)
        {
            _treesCutting.Add(new TreeProduction(tree, _woodAmount));
            return CutTree(_treesCutting.Count - 1, strength);
        }
        return false;
        
    }
    private bool CutTree(int i, int strength)
    {
        _treesCutting[i].woodAmount -= strength;
        Debug.Log(_treesCutting[i].woodAmount);
        Globals.GAME_RESOURCE["wood"].AddAmount(strength);

        if (_treesCutting[i].woodAmount <= 0)
        {
            Debug.Log("tree removed");
            _trees.Remove(_treesCutting[i].tree);
           _treesCutting.RemoveAt(i);
            ResetTrees();
            return false;
        }

        return true;
    }
    private void ResetTrees()
    {
        _terrain.terrainData.SetTreeInstances(_trees.ToArray(), true);

    }

    private Vector3 TreeCoordinateToWorld(TreeInstance treeInstance)
    {
        TerrainData terrainData = _terrain.terrainData;

            var treeInstancePos = treeInstance.position;
            Vector3 localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
            Vector3 worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
        return worldPos;

    }
}
