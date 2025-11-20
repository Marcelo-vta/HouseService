using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public void IniciaJogo()
    {
        GameController.Init();
        SceneController.Instance.LoadScene(1);
    }

    public void Options()
    {
        SceneController.Instance.LoadScene(2);
    }

    public void Credits()
    {
        SceneController.Instance.LoadScene(3);
    }

    public void Menu()
    {
        SceneController.Instance.LoadScene(0);
    }

    public void Voltar()
    {
        SceneController.Instance.BackToPreviousScene();
    }
}