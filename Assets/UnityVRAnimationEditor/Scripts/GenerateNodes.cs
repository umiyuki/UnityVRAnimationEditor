using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//実行時に各ノードを生成
public class GenerateNodes : MonoBehaviour {

    [SerializeField] Transform rootObject;
    [SerializeField] GameObject nodePrefab;

    Mesh wireframeCubeMesh;
    Mesh cubeMesh;

    [SerializeField]Mesh pentaMesh;
    Mesh wireframePentaMesh;

    [SerializeField]Mesh octaMesh;
    Mesh wireframeOctaMesh;

    [SerializeField] Material rootNodeMaterial;
    [SerializeField] Material fkNodeMaterial;
    [SerializeField] Material ikNodeMaterial;
    [SerializeField] Material hilightedMaterial;

    public bool RecordAllChildObjects = true; //全ての子オブジェクトの変化を記録する
    Transform[] allChildObjects;
    Vector3[] savedChildPositions;
    Quaternion[] savedChildRotations;
    Vector3[] savedChildScales;
    Vector3[] tempChildPositions;
    Quaternion[] tempChildRotations;
    Vector3[] tempChildScales;
    int undoID = 0;

    public float nodeSize = 0.1f;

    public bool IsShowNodeName = true; //ノード名を表示するか否か

    public Dictionary<Transform, Node> transformToNodeDic = new Dictionary<Transform, Node>();

	// Use this for initialization
	void Start () {
        wireframeCubeMesh = CreateWireFrameCubeMesh();

        //cubeMeshを取得する
        var cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeMesh = cubeObj.GetComponent<MeshFilter>().sharedMesh;
        Destroy(cubeObj);

        //wireframePentaMeshを取得する
        wireframePentaMesh = new Mesh();
        wireframePentaMesh.vertices = pentaMesh.vertices;
        wireframePentaMesh.SetIndices(MakeWireFrameIndices(pentaMesh.triangles), MeshTopology.Lines, 0);

        //wireframeOctaMeshを取得する
        wireframeOctaMesh = new Mesh();
        wireframeOctaMesh.vertices = octaMesh.vertices;
        wireframeOctaMesh.SetIndices(MakeWireFrameIndices(octaMesh.triangles), MeshTopology.Lines, 0);

        if (RecordAllChildObjects)
        {
            if (rootObject != null)
            {
                allChildObjects = rootObject.GetComponentsInChildren<Transform>();
            }
            else
            {
                allChildObjects = GetComponentsInChildren<Transform>();
            }
            int length = allChildObjects.Length;
            savedChildPositions = new Vector3[length];
            savedChildRotations = new Quaternion[length];
            savedChildScales = new Vector3[length];
            tempChildPositions = new Vector3[length];
            tempChildRotations = new Quaternion[length];
            tempChildScales = new Vector3[length];
        }

        List<GameObject> objects;

        GameObject[] rootObjects;
        //Root以下の全てのゲームオブジェクトを走査する
        if (rootObject != null)
        {
            rootObjects = new GameObject[1] { rootObject.gameObject };
            objects = FindObjectwithTag("Node", rootObject);
        }
        else
        {
            rootObjects = FindRootObjects();
        }

        objects = new List<GameObject>();
        foreach (var rootObject in rootObjects)
        {
            var childObjects = FindObjectwithTag("Node", rootObject.transform);
            objects.AddRange(childObjects);
        }

        foreach (var obj in objects)
        {
            CreateNode(obj, Node.eNodeType.ROOT, wireframeCubeMesh, cubeMesh, rootNodeMaterial);
        }


        objects = new List<GameObject>();
        foreach (var rootObject in rootObjects)
        {
            var childObjects = FindObjectwithTag("FKNode", rootObject.transform);
            objects.AddRange(childObjects);
        }

        foreach (var obj in objects)
        {
            CreateNode(obj, Node.eNodeType.FK, wireframePentaMesh, pentaMesh, fkNodeMaterial);
        }

        objects = new List<GameObject>();
        foreach (var rootObject in rootObjects)
        {
            var childObjects = FindObjectwithTag("IKNode", rootObject.transform);
            objects.AddRange(childObjects);
        }

        foreach (var obj in objects)
        {
            CreateNode(obj, Node.eNodeType.IK, wireframeOctaMesh, octaMesh, ikNodeMaterial);
        }
    }

    void CreateNode(GameObject target, Node.eNodeType nodeType, Mesh wireframeMesh, Mesh normalMesh, Material normalMaterial)
    {
        GameObject nodeObject = Instantiate(nodePrefab, null);
        var node = nodeObject.GetComponent<Node>();
        node.type = nodeType;
        node.followTarget = target.transform;
        node.wireframeMesh = wireframeMesh;
        node.normalMesh = normalMesh;
        node.normalMaterial = normalMaterial;
        node.highlightedMaterial = hilightedMaterial;
        nodeObject.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
        node.generateNodes = this;
        if (IsShowNodeName)
        {
            nodeObject.GetComponentInChildren<TMPro.TextMeshPro>().SetText(target.name);
        }
        else
        {
            nodeObject.GetComponentInChildren<TMPro.TextMeshPro>().gameObject.SetActive(false);
        }

        transformToNodeDic.Add(target.transform, node);
    }

    public static GameObject[] FindRootObjects()
    {
        return System.Array.FindAll(GameObject.FindObjectsOfType<GameObject>(), (item) => item.transform.parent == null);
    }

    //メッシュをワイヤフレーム化
    public int[] MakeWireFrameIndices(int[] triangles)
    {
        int[] indices = new int[2 * triangles.Length];
        int i = 0;
        for (int t = 0; t < triangles.Length; t += 3)
        {
            indices[i++] = triangles[t];        //start
            indices[i++] = triangles[t + 1];   //end
            indices[i++] = triangles[t + 1];   //start
            indices[i++] = triangles[t + 2];   //end
            indices[i++] = triangles[t + 2];   //start
            indices[i++] = triangles[t];        //end
        }
        return indices;
    }

    //ワイヤフレーム用のキューブメッシュを返す
    Mesh CreateWireFrameCubeMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[8]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f,0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
            };

        mesh.SetIndices(new int[]{
            0,1,0,2,1,3,2,3,0,4,1,5,2,6,3,7,4,5,5,7,6,7,4,6
        }, MeshTopology.Lines, 0);

        return mesh;
    }

    public List<GameObject> FindObjectwithTag(string _tag, Transform parent)
    {
        List<GameObject> objList = new List<GameObject>();
        GetChildObject(parent, _tag, objList);

        if (parent.tag == _tag)
        {
            objList.Add(parent.gameObject);
        }

        return objList;
    }

    public void GetChildObject(Transform parent, string _tag, List<GameObject> list)
    {

        for (int i = 0; i < parent.childCount; i++)
        {
            
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                list.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag,list);
            }
        }
    }

    public void RecordAllGrab()
    {
        for(int i=0; i<allChildObjects.Length; i++)
        {
            savedChildPositions[i] = allChildObjects[i].localPosition;
            savedChildRotations[i] = allChildObjects[i].localRotation;
            savedChildScales[i] = allChildObjects[i].localScale;
        }
        
    }

    public void RecordAllUngrab()
    {
        for (int i = 0; i < allChildObjects.Length; i++)
        {
            tempChildPositions[i] = allChildObjects[i].localPosition;
            tempChildRotations[i] = allChildObjects[i].localRotation;
            tempChildScales[i] = allChildObjects[i].localScale;

            //Grabする前の時点と比較
            allChildObjects[i].localPosition = savedChildPositions[i];
            allChildObjects[i].localRotation = savedChildRotations[i];
            allChildObjects[i].localScale = savedChildScales[i];

            //アングラブ時にアンドゥに記録する事でアニメーションウインドウで録画できる
            UnityEditor.Undo.RecordObject(allChildObjects[i], "Record All" + undoID.ToString());
            undoID++; 

            allChildObjects[i].localPosition = tempChildPositions[i];
            allChildObjects[i].localRotation = tempChildRotations[i];
            allChildObjects[i].localScale = tempChildScales[i];

        }
    }
}
