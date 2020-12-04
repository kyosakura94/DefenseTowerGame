using UnityEngine;
using UnityEditor;
using Types;

public class CustomWindow : EditorWindow
{
    // Variables
    Color color;
    Texture2D headerSectionTex;
    Texture2D panel1SectionTex;
    Texture2D panel2SectionTex;
    Texture2D panel3SectionTex;

    Color headerColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    Rect headerRect;
    Rect panel1Rect;
    Rect panel2Rect;
    Rect panel3Rect;

    public string searchTag;
    public string replaceTag;
    public string newname;

    public Texture2D texture;
    public Mesh mesh;
    public Color colorTest = Color.white;
    public string nickname = "Default";
    public Material material;

    public float baseSize;

    static TowerData towerData;
    static EnemyData enemyData;
    static BuildManagerData buildManager;

    public static TowerData towerInfo { get { return towerData; } }
    public static EnemyData enemyInfo { get { return enemyData; } }
    public static BuildManagerData buildInfo { get { return buildManager; } }


    [MenuItem("Custom/LevelDesignTool")]
    public static void OpenWindow()
    {
        CustomWindow window = (CustomWindow)GetWindow(typeof(CustomWindow));
        window.minSize = new Vector2(900, 300);
        window.Show();
    }

    // Window display items

    
    private void OnEnable()
    {
        InitTextures();
        initData();
    }

    public static void initData()
    {

        towerData = (TowerData)ScriptableObject.CreateInstance(typeof(TowerData));
        enemyData = (EnemyData)ScriptableObject.CreateInstance(typeof(EnemyData));
        buildManager = (BuildManagerData)ScriptableObject.CreateInstance(typeof(BuildManagerData));

    }

    void InitTextures()
    {
        headerSectionTex = new Texture2D(1, 1);
        headerSectionTex.SetPixel(0, 0, headerColor);
        headerSectionTex.Apply();

        panel1SectionTex = Resources.Load<Texture2D>("Icons/panel1");
        panel2SectionTex = Resources.Load<Texture2D>("Icons/panel2");
        panel3SectionTex = Resources.Load<Texture2D>("Icons/panel3");
    }

    void DrawLayout()
    {
        headerRect.x = 0;
        headerRect.y = 0;
        headerRect.width = Screen.width;
        headerRect.height = 50f;

        panel1Rect.x = 0;
        panel1Rect.y = 50;
        panel1Rect.width = Screen.width / 3f;
        panel1Rect.height = Screen.width - 50;

        panel2Rect.x = Screen.width / 3f;
        panel2Rect.y = 50;
        panel2Rect.width = Screen.width / 3f;
        panel2Rect.height = Screen.width - 50;

        panel3Rect.x = (Screen.width / 3f) * 2f;
        panel3Rect.y = 50;
        panel3Rect.width = Screen.width / 3f;
        panel3Rect.height = Screen.width - 50;

        GUI.DrawTexture(headerRect, headerSectionTex);
        GUI.DrawTexture(panel1Rect, panel1SectionTex);
        GUI.DrawTexture(panel2Rect, panel2SectionTex);
        GUI.DrawTexture(panel3Rect, panel3SectionTex);


    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerRect);
        GUILayout.Space(15);
        GUILayout.Label("GAME OBJECT TOOLS", EditorStyles.whiteLargeLabel);

        GUILayout.EndArea();
    }

    void DrawPanel1Setting() {

        GUILayout.BeginArea(panel1Rect);
        GUILayout.Label("Custom Enemy", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Movement Type"); 
        enemyData.enemyMovement = (EnemyMovement)EditorGUILayout.EnumPopup(enemyData.enemyMovement);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (GUILayout.Button("Create", GUILayout.Height(30)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ENEMY);
        }
        GUILayout.EndArea();
    }

    void DrawPanel2Setting()
    {
        GUILayout.BeginArea(panel2Rect);
        GUILayout.Label("Custom Tower", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tower Type    ");
        towerData.towerType = (TowerType)EditorGUILayout.EnumPopup(towerData.towerType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Choose Enemy");
        towerData.enemySelect = (EnemySelect)EditorGUILayout.EnumPopup(towerData.enemySelect);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (GUILayout.Button("Create", GUILayout.Height(30)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.TOWER);
        }
        GUILayout.EndArea();
    }


    void DrawPanel3Setting()
    {
        GUILayout.BeginArea(panel3Rect);


        GUILayout.Label("Game Scenarios", EditorStyles.boldLabel);
        GUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Choose Ending");
        buildManager.ending = (Ending)EditorGUILayout.EnumPopup(buildManager.ending);

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5f);
        if (GUILayout.Button("Create", GUILayout.Height(30)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.SENERIOS);
        }

        GUILayout.EndArea();

    }
    private void OnGUI()
    {

        DrawLayout();
        DrawHeader();
        DrawPanel1Setting();
        DrawPanel2Setting();
        DrawPanel3Setting();

    }

    // Methods
    void Colorizer()
    {

        foreach (GameObject g in Selection.gameObjects)
        {
            Renderer ren = g.GetComponent<Renderer>();
            if (ren)
                ren.sharedMaterial.color = color;
        }

    }

    void changeName(string newname) {

        if (Selection.activeTransform != null)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                g.name = newname;
            }
        }
    }

    void FindTag()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
        Selection.objects = gameObjects;
    }

    void ReplaceTag()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
        TagHelper.AddTag(replaceTag);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].tag = replaceTag;
        }
        Selection.objects = gameObjects;
    }


    void changeSize(float size)
    {
        if (Selection.activeTransform != null)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                g.transform.localScale = Vector3.one * baseSize;
            }
        }
    }
    
  

    private static Texture2D TextureField(string name, Texture2D texture)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(145f), GUILayout.Height(40));
        GUILayout.EndHorizontal();
        return result;
    }

    private static Mesh SetMesh(string name,Mesh mesh)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Mesh)EditorGUILayout.ObjectField(mesh, typeof(Mesh), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

    private static Material setMaterial(string name, Material mat)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Material)EditorGUILayout.ObjectField(mat, typeof(Material), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

   
}

public class GeneralSettings : EditorWindow
{
    public enum SettingsType
    {
        ENEMY,
        TOWER,
        SENERIOS
    }

    static SettingsType dataSetting;
    static GeneralSettings window;
    static bool lazer = false;
    public static void OpenWindow(SettingsType setting) {

        dataSetting = setting;
        window = (GeneralSettings)GetWindow(typeof(GeneralSettings));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    void OnGUI()
    {
        switch (dataSetting)
        {
            case SettingsType.ENEMY:
                DrawEnemySetting((EnemyCharacter)CustomWindow.enemyInfo);
                break;
            case SettingsType.TOWER:
                switch (CustomWindow.towerInfo.towerType)
                {
                    case TowerType.Shoot:

                        DrawTowerShotSetting(((TowerCharacter)CustomWindow.towerInfo));
                        break;
                    case TowerType.Lazer:
                        lazer = true;
                        DrawTowerLazerSetting(((TowerCharacter)CustomWindow.towerInfo));
                        break;
                    case TowerType.Barrier:
                        DrawTowerBarrierSetting(((TowerCharacter)CustomWindow.towerInfo));
                        break;
                    default:
                        break;
                }
                
                break;
            case SettingsType.SENERIOS:

                DrawEndingSetting(((BuildManagerData)CustomWindow.buildInfo));

                break;
            default:
                break;
        }
    }


    void DrawEndingSetting(BuildManagerData data)
    {
        GUILayout.Label("Custom Ending Level", EditorStyles.boldLabel);
        GUILayout.Space(5f);
        data.endingName = EditorGUILayout.TextField("Ending Name", data.endingName);

        GUILayout.Space(5f);
        data.timeofLevel = SetFloat("Level Time", data.timeofLevel);

        GUILayout.Space(5f);
        data.towerNumber = SetFloat("Tower Number", data.towerNumber);
        GUILayout.Space(10f);

        GameObject[] building = GameObject.FindGameObjectsWithTag("Manager");

        if (building.Length >= 1)
        {
            EditorGUILayout.HelpBox("Remove the existed one before create new one!", MessageType.Warning);
        }
        else if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
            window.Close();
        }

    }


    
    void DrawTowerShotSetting(TowerCharacter tower)
    {
        GUILayout.Label("Custom Shoot Tower", EditorStyles.boldLabel);

        GUILayout.Space(5f);
        tower.prefab = SetObject("BaseModel", tower.prefab);

        GUILayout.Space(5f);
        tower.towername = EditorGUILayout.TextField("Tower Name", tower.towername);

        GUILayout.Space(5f);
        tower.rangeRadius = SetFloat("RangeRadius", tower.rangeRadius);

        GUILayout.Space(5f);
        tower.turnSpeed = SetFloat("TurnSpeed", tower.turnSpeed);


        GUILayout.Space(5f);
        tower.mesh = SetObject("Mesh", tower.mesh);


        GUILayout.Space(5f);
        tower.upgradeEffect = SetObject("UpgradeEffect", tower.upgradeEffect);

        GUILayout.Space(5f);
        tower.upGradeMat = setMaterial("UpGradeMat", tower.upGradeMat);

        GUILayout.Space(5f);
        tower.fireRate = SetFloat("FireRate", tower.fireRate);

        GUILayout.Space(5f);
        tower.bulletPrefab = SetObject("BulletPrefab", tower.bulletPrefab);

        GUILayout.Space(10f);


        

        if (tower.prefab == null)
        {
            EditorGUILayout.HelpBox("This tower needs a [BaseModel] before it can be created.", MessageType.Warning);
        }
        else if (tower.towername == null || tower.towername.Length < 1)
        {
            EditorGUILayout.HelpBox("This tower needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
            window.Close();
        }

    }

    void DrawTowerLazerSetting(TowerCharacter tower) {

        GUILayout.Label("Custom Barrier Tower", EditorStyles.boldLabel);
        GUILayout.Space(5f);
        tower.prefab = SetObject("BaseModel", tower.prefab);

        GUILayout.Space(5f);
        tower.towername = EditorGUILayout.TextField("Tower Name", tower.towername);

        GUILayout.Space(5f);
        tower.rangeRadius = SetFloat("RangeRadius", tower.rangeRadius);

        GUILayout.Space(5f);
        tower.turnSpeed = SetFloat("TurnSpeed", tower.turnSpeed);


        GUILayout.Space(5f);
        tower.mesh = SetObject("Mesh", tower.mesh);


        GUILayout.Space(5f);
        tower.upgradeEffect = SetObject("UpgradeEffect", tower.upgradeEffect);

        GUILayout.Space(5f);
        tower.upGradeMat = setMaterial("UpGradeMat", tower.upGradeMat);

        GUILayout.Space(5f);
        tower.lazerEffect = SetObject("LazerEffect", tower.lazerEffect);

        GUILayout.Space(5f);
        tower.damageOverTime = SetFloat("DamageOver", tower.damageOverTime);

        GUILayout.Space(5f);
        tower.slowAmount = SetFloat("SlowAmount", tower.slowAmount);


        GUILayout.Space(10f);
        if (tower.prefab == null)
        {
            EditorGUILayout.HelpBox("This tower needs a [BaseModel] before it can be created.", MessageType.Warning);
        }
        else if (tower.towername == null || tower.towername.Length < 1)
        {
            EditorGUILayout.HelpBox("This tower needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
            window.Close();
        }
    }

    void DrawTowerBarrierSetting(TowerCharacter tower)
    {
        GUILayout.Label("Custom Barrier Tower", EditorStyles.boldLabel);
        GUILayout.Space(5f);
        tower.prefab = SetObject("BaseModel", tower.prefab);

        GUILayout.Space(5f);
        tower.towername = EditorGUILayout.TextField("Tower Name", tower.towername);

        GUILayout.Space(5f);
        tower.rangeRadius = SetFloat("RangeRadius", tower.rangeRadius);

        GUILayout.Space(5f);
        tower.turnSpeed = SetFloat("TurnSpeed", tower.turnSpeed);


        GUILayout.Space(5f);
        tower.mesh = SetObject("Mesh", tower.mesh);


        GUILayout.Space(5f);
        tower.upgradeEffect = SetObject("UpgradeEffect", tower.upgradeEffect);

        GUILayout.Space(5f);
        tower.upGradeMat = setMaterial("UpGradeMat", tower.upGradeMat);

        GUILayout.Space(5f);
        tower.totaltime = SetFloat("LifeTime", tower.totaltime);

        GUILayout.Space(10f);
        if (tower.prefab == null)
        {
            EditorGUILayout.HelpBox("This tower needs a [BaseModel] before it can be created.", MessageType.Warning);
        }
        else if (tower.towername == null || tower.towername.Length < 1)
        {
            EditorGUILayout.HelpBox("This tower needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
            window.Close();
        }
    }

    void DrawEnemySetting(EnemyCharacter enemy)
    {
        
        GUILayout.Label("Custom Enemy", EditorStyles.boldLabel);

        GUILayout.Space(5f);
        enemy.prefab = SetObject("BaseModel", enemy.prefab);

        GUILayout.Space(5f);
        enemy.enemyName = EditorGUILayout.TextField("Object Name", enemy.enemyName);

        GUILayout.Space(5f);
        enemy.startSpeed = SetFloat("StartSpeed", enemy.startSpeed);

        GUILayout.Space(5f);
        enemy.totalHealth = SetFloat("TotalHealth", enemy.totalHealth);

        GUILayout.Space(5f);
        enemy.turnSpeed = SetFloat("TurnSpeed", enemy.turnSpeed);

        GUILayout.Space(5f);
        enemy.color = EditorGUILayout.ColorField("Color", enemy.color);

        GUILayout.Space(5f);
        enemy.mesh = SetMesh("Mesh", enemy.mesh);
       

        GUILayout.Space(5f);
        enemy.dieEffect = SetObject("Die Effect", enemy.dieEffect);

        GUILayout.Space(5f);
        enemy.material = setMaterial("Material", enemy.material);

        GUILayout.Space(10f);


        if (enemy.prefab == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [BaseModel] before it can be created.", MessageType.Warning);
        }
        else if (enemy.enemyName == null || enemy.enemyName.Length < 1)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
            window.Close();
            lazer = false;
        }

    }

    private static float SetFloat(string name, float variables)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (float)EditorGUILayout.FloatField(variables, GUILayout.Height(20));
        GUILayout.EndHorizontal();

        return result;
    }

    private static Mesh SetMesh(string name, Mesh mesh)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Mesh)EditorGUILayout.ObjectField(mesh, typeof(Mesh), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

    private static Transform SetTransfrom(string name, Transform mesh)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Transform)EditorGUILayout.ObjectField(mesh, typeof(Transform), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

    private static GameObject SetObject(string name, GameObject obj)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

    private static Material setMaterial(string name, Material mat)
    {
        GUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperLeft;
        style.fixedWidth = 145;
        GUILayout.Label(name, style);
        var result = (Material)EditorGUILayout.ObjectField(mat, typeof(Material), false, GUILayout.Width(145f), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        return result;
    }

    void SaveData()
    {
        string prefabPath;
        string newPrefabPath = "Assets/Prefabs/";
        string dataPath = "Assets/PrefabsData/";

        switch (dataSetting)
        {
            case SettingsType.ENEMY:
                dataPath += "Enemy/" + CustomWindow.enemyInfo.enemyName + ".asset";
                AssetDatabase.CreateAsset(CustomWindow.enemyInfo, dataPath);

                newPrefabPath += "Enemy/" + CustomWindow.enemyInfo.enemyName + ".prefab";

                prefabPath = AssetDatabase.GetAssetPath(CustomWindow.enemyInfo.prefab);

                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject enemyPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

                enemyPrefab.tag = "Enemy";
                if (!enemyPrefab.GetComponent<Enemy>())
                {
                    enemyPrefab.AddComponent(typeof(Enemy));

                }

                enemyPrefab.GetComponent<Enemy>().enemy = CustomWindow.enemyInfo;
                enemyPrefab.GetComponent<MeshFilter>().mesh = CustomWindow.enemyInfo.mesh;
                enemyPrefab.GetComponent<Renderer>().material = CustomWindow.enemyInfo.material;


                prefabPath = "";
                newPrefabPath = "Assets/Prefabs/";
                dataPath = "Assets/PrefabsData/";

                break;

            case SettingsType.TOWER:
                dataPath += "Tower/" + CustomWindow.towerInfo.towername + ".asset";
                AssetDatabase.CreateAsset(CustomWindow.towerInfo, dataPath);

                newPrefabPath += "Tower/" + CustomWindow.towerInfo.towername + ".prefab";

                prefabPath = AssetDatabase.GetAssetPath(CustomWindow.towerInfo.prefab);

                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject towerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

                towerPrefab.tag = "Tower";

                if (!towerPrefab.GetComponent<Tower>())
                {
                    towerPrefab.AddComponent(typeof(Tower));

                }
                if (lazer)
                {
                    if (!towerPrefab.GetComponent<LineRenderer>())
                    {
                        towerPrefab.AddComponent(typeof(LineRenderer));

                    }
                }

                towerPrefab.GetComponent<Tower>().tower = CustomWindow.towerInfo;
                //towerPrefab.GetComponent<MeshFilter>().mesh = CustomWindow.enemyInfo.mesh;
                //towerPrefab.GetComponent<Renderer>().material = CustomWindow.enemyInfo.material;


                prefabPath = "";
                newPrefabPath = "Assets/Prefabs/";
                dataPath = "Assets/PrefabsData/";



                break;
            case SettingsType.SENERIOS:


                    dataPath += "Ending/" + CustomWindow.buildInfo.endingName + ".asset";

                    AssetDatabase.CreateAsset(CustomWindow.buildInfo, dataPath);

                    CreatenNewObject();
                break;
            default:
                break;
        }

    }

    void CreatenNewObject()
    {
        GameObject gameObject = new GameObject();

        gameObject.name = "BuildManager";
        gameObject.tag = "Manager";

        BuildManager buildManager = gameObject.AddComponent<BuildManager>();

        buildManager.GetComponent<BuildManager>().data = CustomWindow.buildInfo;
    }


}

