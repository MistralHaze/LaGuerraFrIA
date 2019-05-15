using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Para este GameManager si vamos a querer pasar informacion entre escenas tendriamos que usar un singleton en una escena de preload
    [Header("Canvas Stuff")]
    public GameObject MainHUDCanvas;
    public GameObject ChoiceCanvas;
    public Text material;
    public Text energy;
    public Text special;
    public Text turnText;
    public Image turnImageBackground;
    public Button passTurn;
    public Button Unit1;
    public Button Unit2;
    public Button Unit3;
    [Header("Faction stuff")]    
    public List<Faction> players;

    //Esto necesario para la AI. 
    //TODO recopilar ciudades en el gameManager (igual es más óptimo usar un array y meter desde editor), recopilar bases y luna.
    //Las unidades que se vayan actualizando cuando se muevan.
    public List<CityCell> cities;
    public List<HexCell> units;   //Las unidades se tendrán que actualizar
    public List<HexCell> bases;

    public HexCell moonCell;


    [Range(0, 2)]
    public int humanFaction;
    [Header("Other references")]
    public HexGrid hexGrid;

    private bool gameBegins;
    /*
    public enum Factions
    {
        USA, CCCP, ALIENS
    }
    */
    int turn;

	// Use this for initialization
	void Start () {
        gameBegins = false;
        turn = 0;
	}

    void Update()
    {
        if (gameBegins)
        {
            //updateMaterials
            material.text = "Materials: " + players[0].GetMaterial();
            energy.text = "Energy: " + players[0].GetEnergy();
        }
    }    

    //Comprobar que el flow de esto no deja funciones sin terminar
    public void NextTurn()
    {
        turn++;

        //Turno del jugador
        if (turn % 3 == 1)  
        {
            resetTurnBoolsUnit(players[0].GetListUnits());
            players[0].PlayHuman();
            showCanvasTurn();
        }
        else
        {
            hideCanvasTurn();
        }
        //Turno de NPC1
        if (turn % 3 == 2)
        {
            resetTurnBoolsUnit(players[1].GetListUnits());
            players[1].PlayIA();
        }
        //Turno de NPC2
        if (turn % 3 == 0)
        {
            resetTurnBoolsUnit(players[2].GetListUnits());
            players[2].PlayIA();
        }
    }

    void resetTurnBoolsUnit(List<Unit> listOfUnits)
    {
        for (int i = 0; i < listOfUnits.Count; i++)
        {
            listOfUnits[i].movedThisTurn = false;
            listOfUnits[i].attackedThisTurn = false;
        }
    }

    void showCanvasTurn()
    {
        turnText.gameObject.SetActive(true);
        turnImageBackground.gameObject.SetActive(true);
        passTurn.gameObject.SetActive(true);
        Unit1.gameObject.SetActive(true);
        Unit2.gameObject.SetActive(true);
        Unit3.gameObject.SetActive(true);
    }

    void hideCanvasTurn()
    {
        turnText.gameObject.SetActive(false);
        turnImageBackground.gameObject.SetActive(false);
        passTurn.gameObject.SetActive(false);
        Unit1.gameObject.SetActive(false);
        Unit2.gameObject.SetActive(false);
        Unit3.gameObject.SetActive(false);
    }

    void playerFactionSetUp()
    {
        //Añadimos componente de selección a la facción elegida
        players[0].gameObject.AddComponent<SelectionComponent>();
        players[0].GetComponent<SelectionComponent>().hexGrid = hexGrid;
        players[0].GetComponent<SelectionComponent>().pathFinding = GetComponent<PathFinding>();
        //Ponemos la facción en modo jugador
        players[0].selectionComponent = players[0].GetComponent<SelectionComponent>();
        players[0].playerFaction = true;
        //Actualizamos la función de los botones de spawn de unidades
        Unit1.onClick.AddListener(players[0].gameObject.GetComponent<MainBuilding>().CreateBasicUnit);
        Unit2.onClick.AddListener(players[0].gameObject.GetComponent<MainBuilding>().CreateIntermediateUnit);
        Unit3.onClick.AddListener(players[0].gameObject.GetComponent<MainBuilding>().CreateAdvancedUnit);
    }

    public void PauseGame()
    {
    }

    public void WinGame() 
    {
    }

    public void LoseGame()
    {
    }

    public void GoToMenu()
    {
    }

    public int GetTurn() 
    {
        return turn;
    }

    public void CheckIfCanEndTurn()
    {
        bool canEnd = true;
        //Comprobamos que todas las unidades han finalizado sus acciones
        List<Unit> auxUnits;
        for (int i = 0; i < players.Count; i++)
        {
            auxUnits = players[i].GetListUnits();
            for (int j = 0; j < auxUnits.Count; j++)
            {
                if (auxUnits[j].moving || auxUnits[j].attacking)
                {
                    canEnd = false;
                }
            }
        }
        if (canEnd)
        {
            NextTurn();
        }
    }

    //El jugador elige facción
    public void ChooseFaction(int faction)
    {
        //Modificamos el array de facciones de manera que en el índice 0 estará la facción del jugador
        Faction aux = players[0];
        players[0] = players[faction];
        players[faction] = aux;

        playerFactionSetUp();

        ChoiceCanvas.SetActive(false);
        MainHUDCanvas.SetActive(true);
        gameBegins = true;
        NextTurn();
    }
}
