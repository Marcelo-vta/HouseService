using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuActions : MonoBehaviour
{
    public void IniciaJogo()
    {
        SceneManager.LoadScene(1);
        GameController.Init();
    }

    public void VoltaMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        SceneController.Instance.LoadScene(5);
    }

    public void Credits()
    {
        SceneController.Instance.LoadScene(4);
    }

    public void Voltar()
    {
        SceneController.Instance.BackToPreviousScene();
    }
}

