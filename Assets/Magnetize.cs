using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Magnetize : MonoBehaviour
{
    public int count = 0;
    Color SouthColor = Color.blue;
    Color NorthColor = Color.red;
    Color DefaultColor;

    public Material defaultMat;
    public Material redMat;
    public Material blueMat;
    Renderer rend;

    // public float ColorTransitionSpeed = 1.0f;

    // float elapsedTime = 0f; 

     float duration = 2.0f;
    // SkinnedMeshRenderer skinrend;
    public char polarity = 'd'; // d for default, n for north, s for south
    // Start is called before the first frame update

    public float magneticFieldStrength = 10f;

    public float overlapPreventionDistance = 0.0f;

    public AudioSource AudioSource1;
    public AudioClip magnetSfx;

    public AudioClip ClickSfx;

    public GameObject Player1;
      public GameObject Player2;
    private GameObject currentPlayer;
   // public bool giveParentPolarity = false;

   public  float threshold = 1.0f;

   private float PlayerPull = 5f;

   public float maxDistance = 5f;
   
    void Start()
    {
        if(this.gameObject.tag=="Player"){
            rend = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>();
        }
        else{
            rend = GetComponent<Renderer>();
        }
        DefaultColor = rend.material.color;


        Player1 = GameObject.FindWithTag("Player");
        Player2 = GameObject.FindWithTag("Player2");
        currentPlayer = Player1.GetComponent<playerMovement>().currentPlayer;
    
    }

    // Update is called once per frame
    void Update()
    {

            if (count == 0)
        {
           // rend.material = defaultMat;
            //rend.material.color = DefaultColor;
            rend.material.CopyPropertiesFromMaterial(defaultMat);
            polarity = 'd';
          //  AssignPolarity();

        }
        else if (count == 1)
        {
           // rend.material = redMat;
           if(redMat == null)
                rend.material.color = NorthColor;
            else{
                //   float t = Mathf.Clamp01(elapsedTime / ColorTransitionSpeed);
                // rend.material.color = Color.Lerp(DefaultColor,NorthColor,t);

              //   float lerp = Mathf.PingPong(Time.time, duration) / duration;
              //rend.material.Lerp(defaultMat, redMat, lerp);
                rend.material.CopyPropertiesFromMaterial(redMat);
            }
             
            polarity = 'n';
            //AssignPolarity();
        }
        else if (count == 2)
        {
           // rend.material = blueMat;
           if(blueMat==null)
                rend.material.color = SouthColor;
           else
                rend.material.CopyPropertiesFromMaterial(blueMat);

            polarity = 's';
           // AssignPolarity();
        }




    // if(giveParentPolarity==true){
    //     this.count =  transform.parent.GetComponent<Magnetize>().count;

    // }
    // else{
    if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) 
    {
         currentPlayer = Player1.GetComponent<playerMovement>().currentPlayer;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                AudioSource1.PlayOneShot(ClickSfx);
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                if(currentPlayer==Player1){
                    if(count!=1){
                        count=1;
                    }
                    else{
                        count=0;
                    }
                }
                else if(currentPlayer==Player2){
                    if(count!=2){
                        count=2;
                    }
                    else{
                        count=0;
                    }
                }
            }
        }
    }

   // }

    }

    void FixedUpdate()
    {

        // if(polarity !='d'){
        //     Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);
        //     foreach (Collider collider in colliders)
        //     {
        
        //         if (collider.attachedRigidbody != null && collider.tag =="Magnetic" && collider.GetComponent<Magnetize>().polarity != 'd')
        //         {
        //             Vector3 direction = transform.position - collider.transform.position;
        //             float distance = direction.magnitude;
        //             if(distance >0){
        //             direction.Normalize();
        //             float forceMagnitude = magneticFieldStrength * collider.attachedRigidbody.mass / Mathf.Pow(distance, 2f);
        //             Vector3 force = direction * forceMagnitude;
        //            
        //             if(polarity=='n' && collider.GetComponent<Magnetize>().polarity=='s'|| polarity=='s' && collider.GetComponent<Magnetize>().polarity=='n'){
        //             //collider.attachedRigidbody.AddForce(force);
        //             collider.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
        //             }
        //             else if(polarity=='n' && collider.GetComponent<Magnetize>().polarity=='n'|| polarity=='s' && collider.GetComponent<Magnetize>().polarity=='s'){
        //            // collider.attachedRigidbody.AddForce(-force);
        //             collider.GetComponent<Rigidbody>().AddForce(-force, ForceMode.Acceleration);
        //             }
        //             }   
        //         }
        //     }
        // }

        if (polarity != 'd')
{
    Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);
    foreach (Collider collider in colliders)
    {
        if (collider.attachedRigidbody != null && collider.tag == "Magnetic" && collider.GetComponent<Magnetize>().polarity != 'd' )
        {
            Vector3 direction = transform.position - collider.transform.position;
            float distance = direction.magnitude;

            if (distance > 0)
            {
                direction.Normalize();

                float forceMagnitude = magneticFieldStrength * collider.attachedRigidbody.mass / Mathf.Pow(distance, 2f);

                // Adjust force magnitude if distance is below a threshold
                //float threshold = 1.0f; // Set your desired threshold here
                // if (distance < threshold)
                // {
                //     forceMagnitude *= distance / threshold;
                // }

                Vector3 force = direction * forceMagnitude;

                if (polarity == 'n' && collider.GetComponent<Magnetize>().polarity == 's' || polarity == 's' && collider.GetComponent<Magnetize>().polarity == 'n')
                {
                    collider.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
                }
                else if (polarity == 'n' && collider.GetComponent<Magnetize>().polarity == 'n' || polarity == 's' && collider.GetComponent<Magnetize>().polarity == 's')
                {
                    collider.GetComponent<Rigidbody>().AddForce(-force, ForceMode.Acceleration);
                }
            }
        }

        if( collider.tag == "Player" & Player1.GetComponent<playerMovement>().Player1Magnetized == 1f ||  collider.tag == "Player2" & Player1.GetComponent<playerMovement>().Player2Magnetized == 1f){
            
            Vector3 direction = transform.position - collider.transform.position;
            float distance = direction.magnitude;

            if (distance > 0)
            {
                direction.Normalize();

                float forceMagnitude = magneticFieldStrength * (collider.attachedRigidbody.mass + PlayerPull) / Mathf.Pow(distance, 2f);
            Vector3 force = direction * forceMagnitude;
                if (polarity == 's' & collider.tag == "Player" || polarity=='n' & collider.tag == "Player2")
                {
                    //Debug.Log(force);
                    collider.GetComponent<Rigidbody>().AddForce(force * 10, ForceMode.Acceleration);
                }
                else if (polarity == 'n'  & collider.tag == "Player" || polarity=='s' & collider.tag == "Player2")
                {
                    collider.GetComponent<Rigidbody>().AddForce(-force * 10, ForceMode.Acceleration);
                }
        }
    }
    }
}




    //    RaycastHit hitInfo;
    //     bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, overlapPreventionDistance);

    //   //  Ray ray = new Ray(transform.position, transform.forward);

    //     if (hit && hitInfo.collider.gameObject != gameObject)
    //     {
    //          Debug.DrawRay(transform.position, transform.forward * overlapPreventionDistance, Color.red);

    //         // Move the object away from the hit point to prevent overlapping
    //         Vector3 moveDirection = transform.position - hitInfo.point;
    //         transform.position += moveDirection.normalized * (overlapPreventionDistance - moveDirection.magnitude);
    //     }
    

    }

    public char getPolarity()
    {
        return polarity;
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if(gameObject.tag=="Player"){
    //         if(other.tag=="RMagnetSphere" )
    //             count=1;
    //         else if(other.tag=="BMagnetSphere" )
    //             count=2;
    //         else if(other.tag=="DMagnetSphere" )
    //             count=0;
    //     Object.Destroy(other.gameObject,0.2f);
    //     }
    // }

        void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "RMagnetSphere"){
            if(other.tag=="Player"){
            Player1.GetComponent<playerMovement>().Player1Magnetized = 1f;
            Player1.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Player1.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            Object.Destroy(gameObject,0.2f);
            }
        }
        if(gameObject.tag== "BMagnetSphere"){
            if(other.tag=="Player2"){
            Player1.GetComponent<playerMovement>().Player2Magnetized = 1f;
            Player2.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Player2.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            Object.Destroy(gameObject,0.2f);
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
            if(collisionInfo.gameObject.tag == "Magnetic" || collisionInfo.gameObject.tag =="StoppingObject" || collisionInfo.gameObject.tag == "Player" & Player1.GetComponent<playerMovement>().Player1Magnetized==1f || collisionInfo.gameObject.tag == "Player2"  & Player1.GetComponent<playerMovement>().Player2Magnetized==1f){
                // AudioSource1.time = AudioSource1.clip.length * 0.1f;
                  AudioSource1.pitch = 1.1f;
                  AudioSource1.PlayOneShot(magnetSfx);

        }
        
    }

}


    // void OnCollisionStay(Collision collisionInfo)
    // {
    //     if (collisionInfo.gameObject.CompareTag("Player"))
    //     {
    //         // if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().name == "Level3")
    //         // {
    //         // //collisionInfo.gameObject.transform.SetParent(transform);
    //         // collisionInfo.gameObject.transform.position = new Vector3(collisionInfo.gameObject.transform.position.x ,collisionInfo.gameObject.transform.position.y+7 ,collisionInfo.gameObject.transform.position.z);
    //         // if(collisionInfo.gameObject.transform.position.z >){

    //         // }
    //         // if(collisionInfo.gameObject.transform.position.z <){

    //         // }
    //         // }
    //         // Attach the player to the cube
    //        // collisionInfo.gameObject.transform.SetParent(transform);
    //       // collisionInfo.gameObject.transform.position = new Vector3(collisionInfo.gameObject.transform.position.x ,collisionInfo.gameObject.transform.position.y ,gameObject.transform.position.z);
    //     }
    // }


    // void OnCollisionExit(Collision collisionInfo)
    // {
    //                    if (collisionInfo.gameObject.CompareTag("Player"))
    //     {
    //                     if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().name == "Level3")
    //         {
    //              collisionInfo.gameObject.transform.SetParent(null);
    //         }
    //         // Attach the player to the cube

    //     }
    // }

