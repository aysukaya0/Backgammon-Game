using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNG.Save;
public class PieceCreator : MonoBehaviour
{
    public Position Positions;
    public GameObject WhitePrefab;
    public GameObject BlackPrefab;
    public Pieces[] Slots;
    public ShopItemInfos PieceImagesAndMoney;
    public SpriteRenderer BlackSprite;
    public SpriteRenderer WhiteSprite;

    private int _pieceIndex;
    

    [SerializeField] DiceController DiceController;

    // Start is called before the first frame update
    void Start()
    {
        _pieceIndex = SaveGame.Instance.PieceData.CurrentPieceIndex;
        if (SaveGame.Instance.PlayerData.Money >= 100)
        {
            SaveGame.Instance.PlayerData.ChangeMoney(-100);
        }
        StartCoroutine(CallTheFunctions());
    }

    IEnumerator CallTheFunctions()
    {
        BlackSprite.sprite = PieceImagesAndMoney.PieceImages[2 * _pieceIndex];
        WhiteSprite.sprite = PieceImagesAndMoney.PieceImages[2 * _pieceIndex + 1];
        yield return new WaitForSeconds(0.1f);
        CreatePieces();
        yield return new WaitForSeconds(1f);
        DiceController.DetermineFirstPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        Pieces.Collidable = true;
    }

    void CreatePieces()
    {
        int j = 0;
        float yOffset = 0f;
        Pieces slot0_pieces = Slots[0];
        Pieces slot23_pieces = Slots[23];
        for (int i = 0; i < 2; i++)
        {
            GameObject whitePiece = Instantiate(WhitePrefab, new Vector3(Positions.PositionsOfWhites[0].x, Positions.PositionsOfWhites[0].y + yOffset, Positions.PositionsOfWhites[0].z), Quaternion.identity);
            GameObject blackPiece = Instantiate(BlackPrefab, new Vector3(Positions.PositionsOfBlacks[0].x, Positions.PositionsOfBlacks[0].y - yOffset, Positions.PositionsOfBlacks[0].z), Quaternion.identity);
            Draggable whiteDraggable = whitePiece.GetComponent<Draggable>();
            Draggable blackDraggable = blackPiece.GetComponent<Draggable>();
            whiteDraggable.LastPosition = whiteDraggable.transform.position;
            blackDraggable.LastPosition = blackDraggable.transform.position;
            slot23_pieces.PiecesInSlot.Add(whiteDraggable);
            slot0_pieces.PiecesInSlot.Add(blackDraggable);
            if (i == 0)
            {
                whiteDraggable.Slot_number = 23;
                blackDraggable.Slot_number = 0;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 1)
            {
                whiteDraggable.Slot_number = 23;
                blackDraggable.Slot_number = 0;
                whiteDraggable.IsDraggable = true;
                blackDraggable.IsDraggable = true;
            }
            yOffset = yOffset + 0.80f;
            string ext = j.ToString();
            whitePiece.name = "White" + ext;
            blackPiece.name = "Black" + ext;
            j++;
        }
        yOffset = 0f;
        Pieces slot5_pieces = Slots[5];
        Pieces slot18_pieces = Slots[18];
        for (int i = 0; i < 5; i++)
        {
            GameObject whitePiece = Instantiate(WhitePrefab, new Vector3(Positions.PositionsOfWhites[1].x, Positions.PositionsOfWhites[1].y - yOffset, Positions.PositionsOfWhites[1].z), Quaternion.identity);
            GameObject blackPiece = Instantiate(BlackPrefab, new Vector3(Positions.PositionsOfBlacks[1].x, Positions.PositionsOfBlacks[1].y + yOffset, Positions.PositionsOfBlacks[1].z), Quaternion.identity);
            Draggable whiteDraggable = whitePiece.GetComponent<Draggable>();
            Draggable blackDraggable = blackPiece.GetComponent<Draggable>();
            whiteDraggable.LastPosition = whiteDraggable.transform.position;
            blackDraggable.LastPosition = blackDraggable.transform.position;
            slot5_pieces.PiecesInSlot.Add(whiteDraggable);
            slot18_pieces.PiecesInSlot.Add(blackDraggable);
            if (i == 0)
            {
                whiteDraggable.Slot_number = 5;
                blackDraggable.Slot_number = 18;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 1)
            {
                whiteDraggable.Slot_number = 5;
                blackDraggable.Slot_number = 18;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 2)
            {
                whiteDraggable.Slot_number = 5;
                blackDraggable.Slot_number = 18;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 3)
            {
                whiteDraggable.Slot_number = 5;
                blackDraggable.Slot_number = 18;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 4)
            {
                whiteDraggable.Slot_number = 5;
                blackDraggable.Slot_number = 18;
                whiteDraggable.IsDraggable = true;
                blackDraggable.IsDraggable = true;
            }
            yOffset = yOffset + 0.80f;
            string ext = j.ToString();
            whitePiece.name = "White" + ext;
            blackPiece.name = "Black" + ext;
            j++;
        }
        yOffset = 0f;
        Pieces slot7_pieces = Slots[7];
        Pieces slot16_pieces = Slots[16];
        for (int i = 0; i < 3; i++)
        {
            GameObject whitePiece = Instantiate(WhitePrefab, new Vector3(Positions.PositionsOfWhites[2].x, Positions.PositionsOfWhites[2].y - yOffset, Positions.PositionsOfWhites[2].z), Quaternion.identity);
            GameObject blackPiece = Instantiate(BlackPrefab, new Vector3(Positions.PositionsOfBlacks[2].x, Positions.PositionsOfBlacks[2].y + yOffset, Positions.PositionsOfBlacks[2].z), Quaternion.identity);
            Draggable whiteDraggable = whitePiece.GetComponent<Draggable>();
            Draggable blackDraggable = blackPiece.GetComponent<Draggable>();
            whiteDraggable.LastPosition = whiteDraggable.transform.position;
            blackDraggable.LastPosition = blackDraggable.transform.position;
            slot7_pieces.PiecesInSlot.Add(whiteDraggable);
            slot16_pieces.PiecesInSlot.Add(blackDraggable);
            if (i == 0)
            {
                whiteDraggable.Slot_number = 7;
                blackDraggable.Slot_number = 16;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 1)
            {
                whiteDraggable.Slot_number = 7;
                blackDraggable.Slot_number = 16;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 2)
            {
                whiteDraggable.Slot_number = 7;
                blackDraggable.Slot_number = 16;
                whiteDraggable.IsDraggable = true;
                blackDraggable.IsDraggable = true;
            }
            yOffset = yOffset + 0.80f;
            string ext = j.ToString();
            whitePiece.name = "White" + ext;
            blackPiece.name = "Black" + ext;
            j++;
        }
        yOffset = 0f;
        Pieces slot12_pieces = Slots[12];
        Pieces slot11_pieces = Slots[11];
        for (int i = 0; i < 5; i++)
        {
            GameObject whitePiece = Instantiate(WhitePrefab, new Vector3(Positions.PositionsOfWhites[3].x, Positions.PositionsOfWhites[3].y + yOffset, Positions.PositionsOfWhites[3].z), Quaternion.identity);
            GameObject blackPiece = Instantiate(BlackPrefab, new Vector3(Positions.PositionsOfBlacks[3].x, Positions.PositionsOfBlacks[3].y - yOffset, Positions.PositionsOfBlacks[3].z), Quaternion.identity);
            Draggable whiteDraggable = whitePiece.GetComponent<Draggable>();
            Draggable blackDraggable = blackPiece.GetComponent<Draggable>();
            whiteDraggable.LastPosition = whiteDraggable.transform.position;
            blackDraggable.LastPosition = blackDraggable.transform.position;
            slot12_pieces.PiecesInSlot.Add(whiteDraggable);
            slot11_pieces.PiecesInSlot.Add(blackDraggable);
            if (i == 0)
            {
                whiteDraggable.Slot_number = 12;
                blackDraggable.Slot_number = 11;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 1)
            {
                whiteDraggable.Slot_number = 12;
                blackDraggable.Slot_number = 11;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 2)
            {
                whiteDraggable.Slot_number = 12;
                blackDraggable.Slot_number = 11;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 3)
            {
                whiteDraggable.Slot_number = 12;
                blackDraggable.Slot_number = 11;
                whiteDraggable.IsDraggable = false;
                blackDraggable.IsDraggable = false;
            }
            if (i == 4)
            {
                whiteDraggable.Slot_number = 12;
                blackDraggable.Slot_number = 11;
                whiteDraggable.IsDraggable = true;
                blackDraggable.IsDraggable = true;
            }
            yOffset = yOffset + 0.80f;
            string ext = j.ToString();
            whitePiece.name = "White" + ext;
            blackPiece.name = "Black" + ext;
            j++;
        }
    }
}