using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzleItem : MonoBehaviour
{
    public enum Type {
        Cylinder,
        Arch,
        Cone,
        Cube,
        Rectangle,
        Triangle,
        HalfArch
    }
    [SerializeField] bool isFrist = false;
    public Type type;
    bool isBlockIn = false;
    bool canBlockIn = false;
    bool isRightBlock = false;
    DragObject playerDrag;

    private void Start()
    {
        playerDrag = FindObjectOfType<DragObject>();

        switch (type)
        {
            case Type.Cylinder:
                if (isFrist)
                {
                    canBlockIn = true;
                    isRightBlock = true;
                    this.gameObject.layer = LayerMask.NameToLayer("Default");
                    Destroy(this.GetComponent<Rigidbody>());
                }
                else
                {
                    canBlockIn = false;
                    isRightBlock = false;
                }
                break;

            case Type.Arch:
                canBlockIn = false;
                isRightBlock = false;
                break;

            case Type.Cone:
                canBlockIn = false;
                isRightBlock = false;
                break;

            case Type.Cube:
                canBlockIn = false;
                isRightBlock = false;
                break;

            case Type.Rectangle:
                canBlockIn = false;
                isRightBlock = false;
                break;

            case Type.Triangle:
                canBlockIn = false;
                isRightBlock = false;
                break;

            case Type.HalfArch:
                canBlockIn = false;
                isRightBlock = false;
                break;

            default:
                break;
        }
    }

    public void SetBlockIn(bool canBlock) {
        this.canBlockIn = canBlock;
    }

    public bool GetIsRightBlock() {
        return isRightBlock;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block") && this.canBlockIn)
        {
            BlockPuzzleItem otherBlock = other.GetComponent<BlockPuzzleItem>();
            if (otherBlock != null && !otherBlock.GetIsRightBlock())
            {
                switch (other.GetComponent<BlockPuzzleItem>().type)
                {
                    case Type.Cylinder:
                        other.transform.parent = this.transform.parent;
                        Vector3 newPosition = this.transform.localPosition;
                        other.transform.DOLocalMove(newPosition + new Vector3(0,2,0), 1);
                        other.transform.DOLocalRotate(Vector3.zero + new Vector3(0,0,90), 1);

                        playerDrag.StopDrag();
                        Destroy(other.GetComponent<Rigidbody>());

                        this.canBlockIn = false;
                        otherBlock.SetBlockIn(true);
                    break;

                    case Type.Arch:


                        break;
                    case Type.Cone:
                        break;
                    case Type.Cube:
                        break;
                    case Type.Rectangle:
                        break;
                    case Type.Triangle:
                        break;
                    case Type.HalfArch:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
