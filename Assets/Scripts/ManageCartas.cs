using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageCartas : MonoBehaviour {

  public GameObject carta;                     // A carta a ser dscartada
  private string[] numerosCarta = new string[]{"ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king"}; // numero das cartas disponiveis
  private string[] naipesCartas = new string[]{"clubs", "diamonds", "spades", "hearts"};

  private bool primeiraCartaSelecionada, segundaCartaSelecionada; // indicadores para cada carta escolhida em cada linha
  private GameObject carta1, carta2;                              // GameObject da 1ª e 2ª carta selecionada
  private string linhaCarta1, linhaCarta2;                        // linha da cata selecionada
 
  bool timerAcionado;                                             // indicador de pausa no Timer ou Start Timer
  float timer;                                                    // variável do tempo

  int numTentativas = 0;                                          // numero de tentativas da rodada
  int numAcertos = 0;                                             // numero de match de pares acertados
  AudioSource somOK;                                              // som de acerto

  int ultimoJogo = 0;
  int recorde = 0;
  private const int recordeInicial = 1000000000;

  // Start is called before the first frame update
  void Start() {
    MostraCartas();
    UpdateTentativas();
    somOK = GetComponent<AudioSource>();
    ultimoJogo = PlayerPrefs.GetInt("Jogadas", 0);
    recorde = PlayerPrefs.GetInt("Recorde");
    if (recorde == 0) {
      recorde = recordeInicial;
      PlayerPrefs.SetInt("Recorde", recordeInicial);
    }
    GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo Anterior = " + ultimoJogo;
    GameObject.Find("recorde").GetComponent<Text>().text = "Recorde = " + recorde;
  }

  // Update is called once per frame
  void Update() {
    if (timerAcionado) {
      timer += Time.deltaTime;
      if (timer > 1) {
        timerAcionado = false;
        if (carta1.tag == carta2.tag) {
          Destroy(carta1);
          Destroy(carta2);
          numAcertos++;
          somOK.Play();
          if (numAcertos == 26) {
            PlayerPrefs.SetInt("Jogadas", numTentativas);
            SceneManager.LoadScene("Lab3_final");
          }
        }
        else {
          carta1.GetComponent<Tile>().EscondeCarta();
          carta2.GetComponent<Tile>().EscondeCarta();
        }
        primeiraCartaSelecionada = false;
        segundaCartaSelecionada = false;
        carta1 = null;
        carta2 = null;
        linhaCarta1 = "";
        linhaCarta2 = "";
        timer = 0;
      }
    }
  }

  void MostraCartas() {
    for (int i = 0;i < 4;i++) {
      int[] arrayEmbaralhado = CriaArrayEmbaralhado();
      for (int j = 0;j < 13;j++) {
        AddUmaCarta(i, j, arrayEmbaralhado[j]);
      }
    }
  }

  void AddUmaCarta(int linha, int rank, int valor) {
    GameObject centro = GameObject.Find("centroDaTela");
    float escalaCartaOriginal = carta.transform.localScale.x;
    float fatorEscalaX = (650 * escalaCartaOriginal) / 100.0f;
    float fatorEscalaY = (945 * escalaCartaOriginal) / 100.0f;
    Vector3 novaPosicao = centro.transform.position + new Vector3((rank - 6.0f) * fatorEscalaX, (linha - 1) * fatorEscalaY, 0);
    GameObject c = Instantiate<GameObject>(carta, novaPosicao, Quaternion.identity);
    c.tag = "" + valor;
    c.name = "" + linha + "_" + valor;
    string numeroCarta = numerosCarta[valor];
    string nomeDaCarta = numeroCarta + "_of_" + naipesCartas[linha];
    Sprite s1 = Resources.Load<Sprite>(nomeDaCarta);
    GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);
  }

  public int[] CriaArrayEmbaralhado() {
    int[] novoArray = new int[13];
    for (int i = 0;i < 13;i++) novoArray[i] = i;
    int temp;
    for (int i = 0;i < 13;i++) {
      temp = novoArray[i];
      int r = Random.Range(i, 13);
      novoArray[i] = novoArray[r];
      novoArray[r] = temp;
    }
    return novoArray;
  }

  public void CartaSelecionada(GameObject carta) {
    if (!primeiraCartaSelecionada) {
      linhaCarta1 = carta.name.Substring(0, 1);
      primeiraCartaSelecionada = true;
      carta1 = carta;
      carta1.GetComponent<Tile>().RevelaCarta();
    }
    else if (!segundaCartaSelecionada) {
      if (carta.name == carta1.name) return;
      linhaCarta2 = carta.name.Substring(0, 1);
      segundaCartaSelecionada = true;
      carta2 = carta;
      carta2.GetComponent<Tile>().RevelaCarta();
      VerificaCartas();
    }
  }

  public void VerificaCartas() {
    DisparaTimer();
    numTentativas++;
    UpdateTentativas();
  }

  public void DisparaTimer() {
    timerAcionado = true;
  }

  void UpdateTentativas() {
    GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
  }
}
