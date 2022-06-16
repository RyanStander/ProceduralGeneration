using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownGeneration
{
    public class Visualizer : MonoBehaviour
    {
        public LSystemGenerator lSystem;

        //a list of positions that the agent has traveled to
        private List<Vector3> positions = new List<Vector3>();

        public RoadHelper roadHelper;

        //the length that the agent moves
        [SerializeField] private float length = 8;

        [SerializeField] private float lengthDecrease = 1;

        //the angle at which the agent turns
        [Range(0, 360)] [SerializeField] private float angle = 90;

        private float setLength;

        public float Length
        {
            get => length > 0 ? length : 1;
            set => length = value;
        }

        private void Start()
        {
            setLength = length;
            var sequence = lSystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                roadHelper.ClearGeneration();
                positions = new List<Vector3>();
                length = setLength;
                var sequence = lSystem.GenerateSentence();
                VisualizeSequence(sequence);
            }
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
                        if (savePoints.Count > 0)
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
                        roadHelper.PlaceStreetPositions(tempPosition,Vector3Int.RoundToInt(direction),length);
                        Length -= lengthDecrease;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.TurnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.TurnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }
        


    }
}