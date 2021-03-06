using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TelaFinal : MonoBehaviour {

  // Start is called before the first frame update
  void Start() {
    int recorde = PlayerPrefs.GetInt("Recorde");
    int jogadas = PlayerPrefs.GetInt("Jogadas");
    if (recorde > jogadas) {
      PlayerPrefs.SetInt("Recorde", jogadas);
      GetComponent<AudioSource>().Play();
      GameObject.Find("textoNovoRecorde").GetComponent<Text>().text = "NOVO RECORDE";
      recorde = jogadas;
    }
    GameObject.Find("Jogadas").GetComponent<Text>().text += jogadas;
    GameObject.Find("Recorde").GetComponent<Text>().text += recorde;
  }

  public void NovoJogo() {
    SceneManager.LoadScene("Lab3");
  }

  public void Sair() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}
