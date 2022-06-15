using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownGeneration
{
    public class SimpleVisualizer : MonoBehaviour
    {
        public LSystemGenerator lSystem;

        //a list of positions that the agent has traveled to
        private List<Vector3> positions = new List<Vector3>();

        //prefab to display where the agent has visited
        public GameObject prefab;

        //using a line renderer to draw a line to show the path
        public Material lineMaterial;

        //the length that the agent moves
        private int length = 8;

        //the angle at which the agent turns
        private float angle = 90;

        public int Length
        {
            get => length > 0 ? length : 1;
            set => length = value;
        }

        private void Start()
        {
            var sequence = lSystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            //will be performing saving and loading of points and need a last in first out method
            var savePoints = new Stack<AgentParameters>();
            var currentPosition = Vector3.zero;

            var direction = Vector3.forward;
            var tempPosition = Vector3.zero;
            
            positions.Add(currentPosition);
            
            //generate an encoding value based on the sequence to identify the output
            foreach (var letter in sequence)
            {
                var encoding = (EncodingLetters) letter;
                switch (encoding)
                {
                    case EncodingLetters.Save:
                        savePoints.Push(new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.Load:
                        if (savePoints.Count>0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("No saved points found in stack");
                        }
                        break;
                    case EncodingLetters.Draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * Length;
                        DrawLine(tempPosition, currentPosition, Color.red);
                        Length -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.TurnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up)*direction;
                        break;
                    case EncodingLetters.TurnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up)*direction;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            var line = new GameObject("Line");
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0,start);
            lineRenderer.SetPosition(1,end);
        }

        public enum EncodingLetters
        {
            Unknown='1',
            Save='[',
            Load=']',
            Draw='F',
            TurnRight='+',
            TurnLeft='-',
        }
    }
}