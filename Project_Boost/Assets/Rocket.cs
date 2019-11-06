using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f; //SerializedField makes the vairable available in the inspector but stops it from being edited in other scripts.
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip playerDeath;

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
            RespondToThrustInput();
            Rotate();
        }
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }

    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (audioSource.isPlaying == false) //prevents Audio Repeating
        {
            audioSource.PlayOneShot(mainEngine);
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
                startSuccessSequence();
                break;
            default:
                startDeathSequence();
                break;
        }
    }

    private void startDeathSequence()
    {
        state = State.Dying;
        Invoke("LoadFirstScene", 1f); //Loads the first level after one second
        audioSource.Stop();
        audioSource.PlayOneShot(playerDeath);
    }

    private void startSuccessSequence()
    {
        state = State.Transcending;
        Invoke("LoadNextScene", 1f); //Loads next Level after 1 second
        audioSource.Stop();
        audioSource.PlayOneShot(success);
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
