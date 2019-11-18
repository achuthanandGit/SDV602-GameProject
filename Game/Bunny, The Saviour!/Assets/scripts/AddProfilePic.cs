using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddProfilePic : MonoBehaviour
{
    // Button to take the picture
    public Button TakePictureButton;

    // To define the camera texture
    WebCamTexture webCamTexture = null;

    // To define picture details which then can access by RegisterManager when clicks Register button
    public static byte[] PictureDetailsBytes;

    /// <summary>
    /// Start method is used to initialize or assign values or actions to required 
    /// variable or components before the first frame update.
    /// </summary>
    void Start()
    {  
        // getting the available devices
        WebCamDevice[] deviceList = WebCamTexture.devices;
        // getting front camera for taking picture
        for (int index = 0; index < deviceList.Length; ++index)
        {
            // checking for front camera
            if (deviceList[index].isFrontFacing)
            {
                webCamTexture = new WebCamTexture(deviceList[index].name);
            }
        }
        if (webCamTexture == null)
            webCamTexture = new WebCamTexture();
        webCamTexture.Play();
    }

    /// <summary>Updates this instance for each frame update</summary>
    private void Update()
    {
        // Assigning the texture to the raw image component
        GetComponent<RawImage>().texture = webCamTexture;
    }

    public void TaskOnClick()
    {
        Debug.Log("Clicked the take photo button");
        StartCoroutine(TakePhoto());
    }

    /// <summary>
    /// Coroutine to take and save the picture
    /// </summary>
    public IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {
        Debug.Log("Starting coroutine");

        // NOTE - you almost certainly have to do this here:

         yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(33, 31);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG;
        PictureDetailsBytes = photo.EncodeToPNG();
        Debug.Log("Took picture");
    }
}