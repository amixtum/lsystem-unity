using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Turtle : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(0, 0, 0);

    public GameObject objectToPlace;

    public GameObject LSystemObject;

    public float forwardDistance = 1f;

    public float turnAngle = 45f;

    public int numberOfIterations = 4;

    private LSystem lSystemComponent;

    private Stack<Vector3> savedPositions = new Stack<Vector3>();

    private Dictionary<AElement, UnityAction> eventActionMap = new Dictionary<AElement, UnityAction>();

    void Start()
    {
        this.transform.position = initialPosition;

        lSystemComponent = LSystemObject.GetComponent<LSystem>();

        constructEventActionMap();

        ExecuteSequence(lSystemComponent.GetSequenceFromAxiom(numberOfIterations));
    }

    private void ExecuteSequence(List<AElement> sequence)
    {
        foreach (AElement element in sequence)
        {
            if (eventActionMap.ContainsKey(element))
            {
                eventActionMap[element].Invoke();
            }
        }
    }

    private void constructEventActionMap()
    {
        UnityAction forwardAction = new UnityAction(() =>
        {
            this.transform.Translate(this.transform.forward * this.forwardDistance);
            Instantiate(objectToPlace, this.transform.position, Quaternion.identity);
        });

        UnityAction placeAction = new UnityAction(() =>
        {
            Instantiate(objectToPlace, this.transform.position, Quaternion.identity);
        });

        UnityAction turnLeftAction = new UnityAction(() =>
        {
            this.transform.Rotate(-this.transform.up, this.turnAngle);
        });

        UnityAction turnRightAction = new UnityAction(() =>
        {
            this.transform.Rotate(this.transform.up, this.turnAngle);
        });

        UnityAction turnUpAction = new UnityAction(() =>
        {
            this.transform.Rotate(this.transform.right, this.turnAngle);
        });

        UnityAction turnDownAction = new UnityAction(() =>
        {
            this.transform.Rotate(-this.transform.right, this.turnAngle);
        });

        UnityAction savePositionAction = new UnityAction(() =>
        {
            savedPositions.Push(this.transform.position);
        });

        UnityAction returnToPositionAction = new UnityAction(() =>
        {
            this.transform.position = savedPositions.Pop();
        });

        eventActionMap.Add(AElement.Forward, forwardAction);
        eventActionMap.Add(AElement.Place, placeAction);
        eventActionMap.Add(AElement.Down, turnDownAction);
        eventActionMap.Add(AElement.Up, turnUpAction);
        eventActionMap.Add(AElement.Left, turnLeftAction);
        eventActionMap.Add(AElement.Right, turnRightAction);
        eventActionMap.Add(AElement.Save, savePositionAction);
        eventActionMap.Add(AElement.Return, returnToPositionAction);
    }

}
