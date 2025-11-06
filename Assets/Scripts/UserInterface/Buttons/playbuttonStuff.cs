using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScri : MonoBehaviour
{
	public void StartleGame()
	{
		PlayerPrefs.SetString("CurrentMode", Mode);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(LoadScene);
	}

    public void SetMode(string i) => Mode = i;

	[Header("Game Mode Settings")]
	[SerializeField] private string Mode;

	[Header("Scene Settings")]
	[SerializeField] private string LoadScene;
}

