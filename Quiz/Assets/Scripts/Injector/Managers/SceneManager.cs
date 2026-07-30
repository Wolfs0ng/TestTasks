namespace Scripts.Injector.Managers
{
    enum SceneName
    {
        Menu,
        Level
    }
    
    public class SceneManager
    {
        public void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.Level.ToString());
        }
        
        public void LoadMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.Menu.ToString());
        }
    }
}