using UnityEngine;
using System.Collections;

abstract public class APiece : MonoBehaviour
{
    public Tile ParentTile { get; set; }

    public bool IsMoving { get; set; }
    public float Velocity { get; set; }

    public bool IsDestroyed { get; set; }

    Transform thisTransform;

    public void Start()
    {
        thisTransform = this.transform;
    }

    public void OnDisable()
    {
        IsDestroyed = false;
        IsMoving = false;
        Velocity = 0.0f;
        ParentTile = null;
    }

    void Update()
    {
        if (BoardData.Instance && !BoardData.Instance.IsGameStarted)
        {
            return;
        }

        if (ParentTile)
        {
            MovePiece();
        }
    }

    private void MovePiece()
    {
        if (thisTransform.position != ParentTile.thisTransform.position)
        {
            Tile targetTile = null;
            APiece targetPiece = null;
            Vector3 moveVector = Vector3.zero;

            if (!IsMoving)
            {
                IsMoving = true;
                BoardData.Instance.Moving++;
                Velocity = 0.0f;
            }

            targetTile = ParentTile.GravityTargetTile;

            targetPiece = targetTile ? targetTile.Piece : null;

            MatchTargetPieceVelocity(targetPiece);
            AcceleratePiece();

            FixPiecePosition();            

            moveVector = moveDirection(moveVector);
            moveVector = moveVector.normalized * Velocity;
            thisTransform.position += moveVector * Time.deltaTime;
        }
        else
        {
            // Rechecks the gravity on the current parent tile as the piece falls
            // Makes the pieces falls smoothly from tile to tile
            ParentTile.GravityHandler();

            if (IsMoving)
            {
                IsMoving = false;
                Velocity = 0;
                BoardData.Instance.Moving--;
            }
        }
    }

    // Sets the velocity of this piece to the velocity of the one underneath
    // so they fall together
    private void MatchTargetPieceVelocity(APiece targetPiece)
    {
        if (targetPiece && targetPiece.IsMoving)
        {
            Velocity = targetPiece.Velocity;
        }
    }

    // Makes the pieces fall faster and faster to give some kind of gravity effect
    private void AcceleratePiece()
    {
        Velocity += BoardData.Instance.Acceleration * Time.deltaTime;
        if (Velocity > BoardData.Instance.MaxVelocity)
        {
            Velocity = BoardData.Instance.MaxVelocity;
        }
    }

    // Makes the piece center on the tile when the velocity would bring it just close to it
    private void FixPiecePosition()
    {
        Vector3 correctionVector = Vector3.zero;

        if (Mathf.Abs(thisTransform.position.x - ParentTile.thisTransform.position.x) < Velocity * Time.deltaTime)
        {
            correctionVector = thisTransform.position;
            correctionVector.x = ParentTile.thisTransform.position.x;
            thisTransform.position = correctionVector;
        }
        if (Mathf.Abs(thisTransform.position.y - ParentTile.thisTransform.position.y) < Velocity * Time.deltaTime)
        {
            correctionVector = thisTransform.position;
            correctionVector.y = ParentTile.thisTransform.position.y;
            thisTransform.position = correctionVector;
        }
    }

    // Gives the direction the piece need to move to fit into it's parent tile
    // Used when the gravity is not pointing in the same direction depending on the tile's gravity direction
    private Vector3 moveDirection(Vector3 moveVector)
    {
        if (thisTransform.position.x < ParentTile.thisTransform.position.x)
            moveVector.x = 10;
        if (thisTransform.position.x > ParentTile.thisTransform.position.x)
            moveVector.x = -10;
        if (thisTransform.position.y < ParentTile.thisTransform.position.y)
            moveVector.y = 10;
        if (thisTransform.position.y > ParentTile.thisTransform.position.y)
            moveVector.y = -10;
        return moveVector;
    }

    public virtual void DestroyPiece()
    {
        if (ParentTile && ParentTile.Piece)
            ParentTile.Piece = null;
        ParentTile = null;
        this.gameObject.SetActive(false);
    }
}
