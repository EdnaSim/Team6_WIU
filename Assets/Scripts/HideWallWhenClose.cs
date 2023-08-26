using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideWallWhenClose : MonoBehaviour
{
    [Tooltip("The number of tiles around the player to make transparent when in trigger area")]
    [SerializeField] int ShowRadius = 3;
    Tilemap tm;
    Vector3Int PlayerCell; //player's cell coords
    Color FadedCol;

    private struct tile {
        public Vector3Int pos; //position
        public Color OgCol; //original color
        public Tile tileRef; //the tile

        //constructor
        public tile(Vector3Int p, Color c, Tile t) {
            pos = p;
            OgCol = c;
            tileRef = t;
        }
    }
    private List<tile> structTiles;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Tilemap>();
        tm.CompressBounds();

        structTiles = new List<tile>();
        foreach(Vector3Int pos in tm.cellBounds.allPositionsWithin) {
            structTiles.Add(new tile(pos, tm.GetColor(pos), (Tile)tm.GetTile(pos)));
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Vector3Int oldcells = PlayerCell;
        PlayerCell = tm.WorldToCell(Player_Controller.Instance.transform.position);
        if (oldcells != PlayerCell) {
            //player is moving, old pos area should no longer be transparent
            ResetTiles();
        }
        for (int x=PlayerCell.x- ShowRadius; x < PlayerCell.x + ShowRadius; x++) {
            for (int y=PlayerCell.y- ShowRadius; y < PlayerCell.y + ShowRadius; y++) {
                //find the tile
                tile tile = structTiles.Find((tile t) => t.pos == new Vector3Int(x, y, 0));
                if (tile.tileRef != null) {
                    //set to transparent
                    FadedCol = tile.OgCol;
                    FadedCol.a = 0.5f;
                    tm.SetColor(tile.pos, FadedCol);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        ResetTiles();
    }

    private void ResetTiles() {
        foreach (tile tile in structTiles) {
            if (tile.tileRef != null) {
                //set colours back to original
                tm.SetColor(tile.pos, tile.OgCol);
            }
        }
    }
}
