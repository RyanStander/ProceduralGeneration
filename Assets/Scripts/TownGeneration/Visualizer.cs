using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownGeneration
{
    public class Visualizer : MonoBehaviour
    {
        public LSystemGenerator lsystem;

        public RoadHelper roadHelper;
        public StructureHelper structureHelper;
        public int roadLength = 8;
        private int length = 8;
        private float angle = 90;

        public int Length
        {
            get => length > 0 ? length : 1;
            set => length = value;
        }

        private void Start()
        {
            CreateTown();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                roadHelper.Reset();
                structureHelper.Reset();
                CreateTown();
            }
        }

        private void CreateTown()
        {
            length = roadLength;
            roadHelper.Reset();
            var sequence = lsystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            var savePoints = new Stack<AgentParameters>();
            var currentPosition = Vector3.zero;

            var direction = Vector3.forward;
            var tempPosition = Vector3.zero;


            foreach (var encoding in sequence.Select(letter => (EncodingLetters)letter))
            {
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
                            throw new Exception("No saved points found");
                        }
                        break;
                    case EncodingLetters.Draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * length;
                        roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                        Length -= 2;
                        break;
                    case EncodingLetters.TurnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.TurnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.Unknown:
                        throw new Exception("The encoding letter is unknown");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            roadHelper.FixRoad();

            structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoadPositions());
        }
    }
}