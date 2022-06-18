using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownGeneration
{
    [RequireComponent(typeof(LSystemGenerator))]
    [RequireComponent(typeof(RoadHelper))]
    [RequireComponent(typeof(StructureHelper))]
    [RequireComponent(typeof(RandomGenerator))]
    public class TownGenerator : MonoBehaviour
    {
        public RandomGenerator randomGenerator;
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

        private void OnValidate()
        {
            if (randomGenerator==null)
                randomGenerator = GetComponent<RandomGenerator>();
            if (structureHelper==null)
                structureHelper = GetComponent<StructureHelper>();
            if (roadHelper==null)
                roadHelper = GetComponent<RoadHelper>();
            if (lsystem==null)
                lsystem = GetComponent<LSystemGenerator>();
        }

        public void CreateTown()
        {
            ResetTown();
            randomGenerator.InitializeRandom();
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

        public void ClearObjects()
        {
            ResetTown();
            
            var children = transform.Cast<Transform>().ToList();

            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        public void ResetTown()
        {
            roadHelper.Reset();
            structureHelper.Reset();
            length = roadLength;
        }
    }
}