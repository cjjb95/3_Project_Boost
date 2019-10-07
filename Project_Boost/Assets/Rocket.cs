using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f; //SerializedField makes the vairable available in the inspector but stops it from being edited in other scripts.
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (audioSource.isPlaying == false) //prevents Audio Repeating
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return; //Ignores collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene",1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Rotate()
    {

        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(Vector3.right * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.right * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }


}
