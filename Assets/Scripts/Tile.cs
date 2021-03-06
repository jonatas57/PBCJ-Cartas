using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

  public Sprite originalCarta;           // Sprite da carta desejada
  public Sprite backCarta;               // Sprite do avesso da carta

  // Start is called before the first frame update
  void Start() {
    EscondeCarta();
  }

  // Update is called once per frame
  void Update() {

  }

  public void OnMouseDown() {
    GameObject.Find("GameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject);
  }

  public void EscondeCarta() {
    GetComponent<SpriteRenderer>().sprite = backCarta;
  }

  public void RevelaCarta() {
    GetComponent<SpriteRenderer>().sprite = originalCarta;
  }

  public void SetCartaOriginal(Sprite novaCarta) {
    originalCarta = novaCarta;
  }
}
