public static class LoadingData
{
    private static string sceneName;
    
    public static void SetNextScene(string scene) 
    {
        sceneName = scene;
    }

    public static string GetNextScene() 
    {
        return sceneName;
    }
}
