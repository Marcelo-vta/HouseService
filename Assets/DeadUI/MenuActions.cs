using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuActions : MonoBehaviour
{
    public void IniciaJogo()
    {
        SceneController.Instance.LoadScene(1);
        GameController.Init();
    }

    public void VoltaMenu()
    {
        SceneController.Instance.LoadScene(0);
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

