using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public float moveInputx;
    public float moveInputy;

    public CharacterSlot currentSlot;
    public SceneGateSelect currentGate;
    public CharacterSlot selectSlot;

    public GameObject SceneGate;

    private PlayerInput playerInput;
    private InputAction move;
    private InputAction submit;
    private InputAction cancel;
    public GameObject HoverCharacterImages;

    Vector2 charaTransform;
    bool moveOn = false;
    public float speed;
    public GameObject[] charaSelectImage;


    public bool moveStop;
    public string selectScene;

    public static bool _nextRoundOn;//カーソル共有のシーン移動可能かどうか判別


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        submit = playerInput.actions["Submit"];
        cancel = playerInput.actions["Cancel"];
        _nextRoundOn = true;
    }
   
    
    private void Update()
    {

        if (0 != move.ReadValue<Vector2>().x || 0 != move.ReadValue<Vector2>().y)
        {
            moveStop = false;
        }
        else
        {
            moveStop = true;
        }
            Vector2 moveInput = move.ReadValue<Vector2>();
        moveInputx = moveInput.x;
        moveInputy = moveInput.y;

        if (submit.WasPressedThisFrame())
        {
            if (currentSlot != null && selectSlot == null)
            {
                selectSlot = currentSlot;
                HoverCharacterImages.GetComponent<ChangeHoverImage>().playerImage[playerInput.playerIndex].GetComponent<SelectCharaAnimation>().set = true;
                currentSlot.Select(playerInput.playerIndex);
            }
            else if (currentGate != null && _nextRoundOn &&
                     GameManager.Instance != null && GameManager.Instance.CanStartGame)
            {
                _nextRoundOn = false;
                RoundManager.roundCount = 1;
                currentGate.GoNextScene(selectScene);
            }
        }

        if (cancel.WasPressedThisFrame())
        {
            if (selectSlot != null)
            {
                HoverCharacterImages.GetComponent<ChangeHoverImage>().playerImage[playerInput.playerIndex].GetComponent<SelectCharaAnimation>().set = false;
                charaTransform = charaSelectImage[selectSlot.characterID].gameObject.transform.position;
                moveOn = true;
                selectSlot.Deselect(playerInput.playerIndex);
                selectSlot = null;

            }
        }

        if(moveOn)
        {
            Debug.Log(charaTransform);
            transform.position = Vector2.MoveTowards(transform.position, charaTransform, speed);
            if (Mathf.Abs(transform.position.x - charaTransform.x) < 0.2f && Mathf.Abs(transform.position.y - charaTransform.y) < 0.2f) moveOn = false;
        }
        
    }
    }
