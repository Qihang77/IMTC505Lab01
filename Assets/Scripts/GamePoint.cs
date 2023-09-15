using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

namespace IMTC505.starter.SampleGame
{
    [RequireComponent(typeof(Collider))]
    public class GamePoint : MonoBehaviour
    {
        [Tooltip("Points scored by touching this object.")]
        public float points = 10;

        [Tooltip("Event to trigger when controller interacts with point object.")]
        public System.Action<GamePoint> OnTriggerEnterAction;

        private enum behaviourCase
        {
            doingNothing,
            floating,
            changingColour,
            movingLeft,
            movingUp,
            rotating
        }
        Collider rootCollider;

        [Tooltip("Choose the behaviour you want before player touching it")]
        [SerializeField] behaviourCase behaviour;

        [Tooltip("for floating max distance")]
        [SerializeField] int floatingYLength;
        private bool goingDown = false;

        [Tooltip("for moving max distance")]
        [SerializeField] int movingMax;
        private bool movingLeft = false;
        private bool movingUp = false;


        [Tooltip("For changing Material")]
        Material originalMaterial;
        [SerializeField] Material newMaterial;
        [SerializeField] Transform targetPlayer;
        [SerializeField] int colourDistance;

        [Tooltip("for rotation")]
        [SerializeField] Transform targetRotation;

        [SerializeField] bool isTriggerParticle;
        [SerializeField] ParticleSystem particle;
        [SerializeField] GameObject destoryObject;
        

        void Start()
        {
            // Make sure non of the colliders in child objects are active
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            // Make sure the root collider is a trigger and enabled
            rootCollider = GetComponent<Collider>();
            rootCollider.enabled = true;
            rootCollider.isTrigger = true;

            if(behaviour == behaviourCase.floating)
            {
                StartCoroutine(Floating(transform.position));
            }
            if(behaviour == behaviourCase.movingLeft)
            {
                StartCoroutine(LeftRight(transform.position));
            }
            if(behaviour == behaviourCase.changingColour)
            {
                originalMaterial = gameObject.GetComponent<Renderer>().material;
                StartCoroutine(ChangingColour());
            }
            if(behaviour == behaviourCase.rotating)
            {
                StartCoroutine(Rotating());
            }
            if(behaviour == behaviourCase.movingUp)
            {
                StartCoroutine(UpDown(transform.position));
            }
        }

        private IEnumerator ChangingColour()
        {
            while (true)
            {
                if(Vector3.Distance(targetPlayer.transform.position, transform.position) > colourDistance)
                {
                    gameObject.GetComponent<Renderer>().material = originalMaterial;
                }
                else
                {
                    gameObject.GetComponent<Renderer>().material = newMaterial;
                }

                yield return new WaitForSeconds(0.5f);
            }
            
        }

        private IEnumerator Floating(Vector3 originalPosition)
        {
            while (true)
            {            

                if(transform.position.y > originalPosition.y + floatingYLength)
                {
                    goingDown = true;
                }
                if(transform.position.y < originalPosition.y - floatingYLength)
                {
                    goingDown = false;
                }

                if(goingDown)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                }

                yield return new WaitForSeconds(0.05f);
            }
            
        }

        private IEnumerator LeftRight(Vector3 originalPosition)
        {
            while (true)
            {
                if (transform.position.x > originalPosition.x + movingMax)
                {
                    movingLeft = true;
                }
                if (transform.position.x < originalPosition.x - movingMax)
                {
                    movingLeft = false;
                }

                if (movingLeft)
                {
                    transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        private IEnumerator UpDown(Vector3 originalPosition)
        {
            while (true)
            {
                if (transform.position.z > originalPosition.z + movingMax)
                {
                    movingUp = true;
                }
                if (transform.position.z < originalPosition.z - movingMax)
                {
                    movingUp = false;
                }

                if (movingUp)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.05f);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.05f);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        private IEnumerator Rotating()
        {
            while (true)
            {
                transform.RotateAround(targetRotation.transform.position, Vector3.up, 45 * Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            OnTriggerEnterAction?.Invoke(this);
            rootCollider.enabled = false;
            if(isTriggerParticle)
            {
                TriggerParticle();
            }
            else
            {
                StartCoroutine(Smaller());
            }
            
            Debug.Log("we hit the item!");
        }

        IEnumerator Smaller()
        {
            while (gameObject.transform.localScale.y >= 0)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - 0.1f, gameObject.transform.localScale.y - 0.1f, gameObject.transform.localScale.z - 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(gameObject);
        }

        void TriggerParticle()
        {
            Destroy(destoryObject);
            particle.Play();
            Destroy(gameObject,2f);
        }
    }
}
