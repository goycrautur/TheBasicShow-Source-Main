using UnityEngine;

public class PlayButtonScri : MonoBehaviour
{
	public void StartleGame()
	{
		PlayerPrefs.SetString("CurrentMode", Mode);
		PlayerPrefs.SetString("CurDifficulity", Difficulity);
        PlayerPrefs.Save();
        //SceneManager.LoadSceneAsync(LoadScene);
		LoadingManagerThing.Instance.LoadSceneAsyncUHHH(LoadScene,0,false);
	}

    public void SetMode(string i) => Mode = i;

	[Header("Game Mode Settings")]
	[SerializeField] private string Mode;
	public string Difficulity;

	[Header("Scene Settings")]
	[SerializeField] private string LoadScene;
}

