using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CircleRenderer : MonoBehaviour
    {
        private Vector3 startingPosition = Vector3.zero;
        private float startRadius;
        private float targetRadius;
        private float speed;
        private int skipSteps;
        private int rotationSteps;

        public bool IsActive { get; private set; } = false;

        private const int CIRCLE_STEPS = 120;
        private const float CIRCLE_WIDTH = .25f;

        private LineRenderer circleRenderer;
        private EdgeCollider2D edgeCollider;
        private float currentRadius;

        private void Start()
        {
            circleRenderer = GetComponent<LineRenderer>();
            edgeCollider = GetComponent<EdgeCollider2D>();
            //circleRenderer.enabled = false;
            circleRenderer.startWidth = CIRCLE_WIDTH;
            circleRenderer.endWidth = CIRCLE_WIDTH;
            edgeCollider.offset = new Vector2(startingPosition.x * -1f, startingPosition.y * -1f);

            DrawCircle(CIRCLE_STEPS, currentRadius);
        }

        public void InitCircleRenderer(
            Vector3 startingPosition,
            float startRadius,
            float targetRadius,
            float speed,
            float openSlice,
            float rotation)
        {
            this.startingPosition = startingPosition;
            this.startRadius = startRadius;
            this.targetRadius = targetRadius;
            this.speed = speed;

            rotationSteps = (int)((CIRCLE_STEPS * Mathf.Abs(rotation)) / 360);
            currentRadius = this.startRadius;
            skipSteps = (int)((openSlice / 100) * CIRCLE_STEPS);
            IsActive = true; // trigger render start
        }

        private void Update()
        {
            if (IsActive && currentRadius <= targetRadius)
            {
                currentRadius += speed * Time.deltaTime;
                DrawCircle(CIRCLE_STEPS, currentRadius, skipSteps, rotationSteps);
                //DrawCircle(CIRCLE_STEPS, currentRadius, skipSteps, Random.Range(0,26));
                SetEdgeCollider();
                //circleRenderer.enabled = true;
            }
        }

        private void DrawCircle(int steps, float radius, int skipSteps = 10, int rotationSteps = 0)
        {
            Vector3[] circlePoints = new Vector3[steps];

            for (int currentStep = 0; currentStep < steps; currentStep++)
            {
                float circumferenceProgress = (float)currentStep / steps;

                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector3 currentPosition = startingPosition + new Vector3(x, y, 0);

                // circleRenderer.SetPosition(currentStep, currentPosition);

                circlePoints[currentStep] = currentPosition;

            }

            circleRenderer.positionCount = steps - skipSteps;
            Vector3[] circleRotatedPoints = circlePoints;
            Helpers.ShiftArrayContent(ref circleRotatedPoints, rotationSteps);

            for (int i = 0; i < steps - skipSteps; i++)
            {
                circleRenderer.SetPosition(i, circleRotatedPoints[i]);
            }

        }

        private void SetEdgeCollider()
        {
            List<Vector2> edges = new List<Vector2>();

            for (int point = 0; point < circleRenderer.positionCount; point++)
            {
                Vector3 circleRendererPoint = circleRenderer.GetPosition(point);
                edges.Add(new Vector2(circleRendererPoint.x, circleRendererPoint.y));
            }

            edgeCollider.SetPoints(edges);
        }

    }
}
