using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
    public AElement[] axiom;

    public SequenceTransformation[] transformations;

    public List<AElement> GetSequenceFromAxiom(int iterations)
    {
        List<AElement> resultingSequence = new List<AElement>();

        resultingSequence.AddRange(axiom);

        resultingSequence = TransformSequence(resultingSequence, iterations);

        return resultingSequence;
    }

    private List<AElement> TransformSequence(List<AElement> sequence, int n)
    {
        List<AElement> resultingSequence;

        resultingSequence = TransformSequence(sequence);

        for (int i = 1; i < n; ++i)
        {
            resultingSequence = TransformSequence(resultingSequence);
        }

        return resultingSequence;
    }

    private List<AElement> TransformSequence(List<AElement> sequence)
    {
        List<AElement> resultingSequence = new List<AElement>();

        bool transformationFound = false;
        foreach (AElement element in sequence)
        {
            transformationFound = false;
            foreach (SequenceTransformation transformation in transformations)
            {
                if (transformation.transformFrom == element)
                {
                    resultingSequence.AddRange(transformation.transformation);
                    transformationFound = true;
                    break;
                }
            }
            if (!transformationFound)
            {
                resultingSequence.Add(element);
            }
        }

        return resultingSequence;
    }

    private List<AElement> StringToSequence(string s)
    {
        List<AElement> result = new List<AElement>();

        foreach (char c in s)
        {
            if (CharToAElement(c) != AElement.None)
            {
                result.Add(CharToAElement(c));
            }
        }

        return result;
    }

    private AElement CharToAElement(char c)
    {
        switch (c)
        {
            case 'F':
                return AElement.Forward;
            case 'P':
                return AElement.Place;
            case '^':
                return AElement.Up;
            case 'v':
                return AElement.Down;
            case '<':
                return AElement.Left;
            case '>':
                return AElement.Right;
            case '[':
                return AElement.Save;
            case ']':
                return AElement.Return;
            default:
                return AElement.None;

        }
    }

    [System.Serializable]
    public struct SequenceTransformation
    {
        public AElement transformFrom;
        public AElement[] transformation;
    }
}

public enum AElement
{
    Left,
    Right,
    Up,
    Down,
    Forward,
    Place,
    Save,
    Return,
    None
}