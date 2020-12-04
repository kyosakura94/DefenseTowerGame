//using UnityEngine;
//using UnityEditor;

//public enum OPTIONS
//{
//    PLAYER1 = 0,
//    PLAYER2 = 1
//}

//[CustomEditor(typeof(Tower))]
//public class CharacterCustomizerEditor : Editor
//{
//    private float labelWidth = 150f;
//    float thumbnailWidth = 70;
//    float thumbnailHeight = 70;


//    public OPTIONS op;

//    public override void OnInspectorGUI()
//    {
//        Tower cube = (CubeController)target;

//        op = (OPTIONS)EditorGUILayout.EnumPopup("Option", op);
//        //NAME OF PLAYER
//        GUILayout.Space(10f);
//        GUILayout.BeginHorizontal();

//            GUILayout.Label("Player Name", GUILayout.Width(labelWidth));

//            cube.playerName = GUILayout.TextField(cube.playerName);

//        GUILayout.EndHorizontal();

//        //COLOR
//        GUILayout.Space(10f);

//        GUILayout.BeginHorizontal();

//            GUILayout.Label("Random Color");
//            if (GUILayout.Button("Generate Color"))
//            {
//                cube.GenerateColor();
//            }

//        GUILayout.EndHorizontal();

//        //RESIZE

//        GUILayout.Space(10f);

//        GUILayout.BeginHorizontal();

//            cube.baseSize = EditorGUILayout.Slider("Size", cube.baseSize, .1f, 3f);
//            cube.transform.localScale = Vector3.one * cube.baseSize;

//        GUILayout.EndHorizontal();

//        //CHANGE MESH

//        GUILayout.Space(10f);

//        GUILayout.BeginHorizontal();

//            GUILayout.Label("Mesh Shapes");

//            if (GUILayout.Button(Resources.Load<Texture>("Thumbnails/Cube_Thumbnail"),
//                 GUILayout.Width(thumbnailWidth), GUILayout.Height(thumbnailHeight)))
//            {
//                cube.ChangeMesh("CUBE");
//                cube.mesh = "CUBE";
//            }

//            if (GUILayout.Button(Resources.Load<Texture>("Thumbnails/Sphere_Thumbnail"),
//                GUILayout.Width(thumbnailWidth), GUILayout.Height(thumbnailHeight)))
//            {
//                cube.ChangeMesh("SPHERE");
//                cube.mesh = "SPHERE";
//            }

//            if (GUILayout.Button(Resources.Load<Texture>("Thumbnails/Capsule_Thumbnail"),
//                GUILayout.Width(thumbnailWidth), GUILayout.Height(thumbnailHeight)))
//            {
//                cube.ChangeMesh("CAPSULE");
//                cube.mesh = "SPHERE";

//            }

//        GUILayout.EndHorizontal();

//        //SAVE BUTTON
//        GUILayout.Space(10f);
//        GUILayout.BeginHorizontal();

//        if (GUILayout.Button("SAVE"))
//        {
//            Save(op, cube);
//        }

//        if (GUILayout.Button("Reset"))
//        {
//            Debug.Log("Pressed reset button.");
//            cube.Reset();
//        }
//        if (GUILayout.Button("Prefab"))
//        {
//            cube.createPrefab();
//        }

//        GUILayout.EndHorizontal();
//    }

//    void Save(OPTIONS op, CubeController cube)
//    {
//        switch (op)
//        {
//            case OPTIONS.PLAYER1:
//                Debug.Log("Player1");
//                SaveSystem.SavePlayer(cube, "Player1");
//                break;
//            case OPTIONS.PLAYER2:
//                Debug.Log("Player2");
//                SaveSystem.SavePlayer(cube, "Player2");
//                break;
//            default:
//                Debug.LogError("Unrecognized Option");
//                break;
//        }
//    }
//}
