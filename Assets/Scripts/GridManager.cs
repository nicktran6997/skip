using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    //Help From http://answers.unity3d.com/questions/587380/linerenderer-drawing-in-pink.html

    //Testing/Temporary Variables. Will be removed later.
    private Boolean updating; //Necessary so that you cannot input multiple calls to zoom the camera.
                              //This functionality is unnecessary in the final version as the user will not
                              //be controlling when the camera zooms.

	public Material shader;

    //Grid Line Variables:
    private Color lineColor = Color.red;
    private float lineWidth = 0.04f;
    private ArrayList lines;
    private int minY;
    private int maxY;

    //Game Boundary Variables:
    private GameObject leftBoundary;
    private GameObject rightBoundary;
    private float boundarySize; //The length of the sides of the Wall hitboxes.
    private float boundaryY; //This is the y value of the boundary Wall GameObjects.

    //Camera Variables:
    private Camera myCam;
    private CameraController camCtrl;
    private static float scaleSpeed = 0.005f; //The smaller the number, the faster it will zoom.
    private float targetScale; //This is the orthographicSize of the camera.
    private static float STARTINGSCALE = 2;


    // Use this for initialization
    void Start()
    {
        InitCam();
        InitGrid();
        InitBoundaries();

    }

    // Update is called once per frame
    /*
     * This is where we will be able to add or remove lanes while in game and also be able to adjust the boundaries to match the camera.
     * */
    void Update()
    {
        if (!updating) {
            if (Input.GetKeyDown(KeyCode.S)) {
                targetScale += 1;
                StartCoroutine("ScaleCamera");
            } else if (Input.GetKeyDown(KeyCode.A)) {
                targetScale -= 1;
                StartCoroutine("ScaleCamera");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

    }

    /*Initializes the camera variables used in the GridManager.
	 */
    private void InitCam()
    {
        targetScale = STARTINGSCALE;
        myCam = Camera.main.GetComponent<Camera>();
        camCtrl = Camera.main.GetComponent<CameraController>();
        myCam.orthographicSize = STARTINGSCALE;
        camCtrl.UpdateCameraController();
    }

    /*Initializes the left and right boundaries of the player.
	 */
    private void InitBoundaries() {
        boundarySize = camCtrl.getMaxY() - camCtrl.getMinY();
        boundaryY = (camCtrl.getMaxY() + camCtrl.getMinY()) / 2;

        leftBoundary = new GameObject("LeftBoundary", typeof(BoxCollider2D));
        leftBoundary.tag = "Wall";
        leftBoundary.layer = 8;
        leftBoundary.transform.parent = gameObject.transform;
        BoxCollider2D leftB = leftBoundary.GetComponent<BoxCollider2D>();
        leftB.size = new Vector2(boundarySize, boundarySize);
        leftBoundary.transform.position = new Vector3(camCtrl.getMinX() - boundarySize / 2, boundaryY, 0);

        rightBoundary = new GameObject("RightBoundary", typeof(BoxCollider2D));
        rightBoundary.tag = "Wall";
        rightBoundary.layer = 8;
        rightBoundary.transform.parent = gameObject.transform;
        BoxCollider2D rightB = rightBoundary.GetComponent<BoxCollider2D>();
        rightB.size = new Vector2(boundarySize, boundarySize);
        rightBoundary.transform.position = new Vector3(camCtrl.getMaxX() + boundarySize / 2, boundaryY, 0);
    }

    /*Updates the left and right boundaries of the screen to match the current Camera.
	 */
    private void UpdateBoundaries() {
        boundarySize = camCtrl.getMaxY() - camCtrl.getMinY();
        boundaryY = (camCtrl.getMaxY() + camCtrl.getMinY()) / 2;
        BoxCollider2D leftB = leftBoundary.GetComponent<BoxCollider2D>();
        leftB.size = new Vector2(boundarySize, boundarySize);
        leftBoundary.transform.position = new Vector3(camCtrl.getMinX() - boundarySize / 2, boundaryY, 0);
        BoxCollider2D rightB = rightBoundary.GetComponent<BoxCollider2D>();
        rightB.size = new Vector2(boundarySize, boundarySize);
        rightBoundary.transform.position = new Vector3(camCtrl.getMaxX() + boundarySize / 2, boundaryY, 0);
    }

    //Creates the initial grid lines.
    private void InitGrid() {
        updating = false;
        lines = new ArrayList();
        minY = (int)Math.Floor(camCtrl.getMinY());
        maxY = (int)Math.Ceiling(camCtrl.getMaxY());
        for (int y = minY; y <= maxY; y++) {
            CreateLine(y, new Vector3(camCtrl.getMinX(), y), new Vector3(camCtrl.getMaxX(), y));
        }
    }

    /*Creates a LineRenderer type GameObject given its level number and it's endpoints.
     *Also sets the color of the lines to the lineColor variable and the width to lineWidth.
     */
    private void CreateLine(int yVal, Vector3 pos1, Vector3 pos2) {
        GameObject lineObj = new GameObject("LevelLine: " + yVal, typeof(LineRenderer));
        lineObj.transform.parent = gameObject.transform;
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        //Material whiteDiffuseMat = new Material(Shader.Find("Particles/Additive"));
		Material whiteDiffusemat = shader;
		line.material = whiteDiffusemat;

        //Color randomization here:
        lineColor = randomColor();

        line.startColor = lineColor;
        line.endColor = lineColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);
        lines.Add(lineObj);
    }

    /*Updates each line in lines to scale with the current Camera's vision.
     */
    private void UpdateLines() {
        //Gets the new min and max y values.
        int updatedMinY = (int)Math.Floor(camCtrl.getMinY());
        int updatedMaxY = (int)Math.Ceiling(camCtrl.getMaxY());

        //Checks every line in lines and destroys it if it lies outside of the Camera bounds.
        ArrayList toDel = new ArrayList();
        foreach (GameObject lineObj in lines) {
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            Vector3 pos1 = line.GetPosition(0);
            if (pos1.y > updatedMaxY || pos1.y < updatedMinY) {
                toDel.Add(lineObj);
            }
        }
        for (int i = 0; i < toDel.Count; i++) {
            lines.Remove(toDel[i]);
            Destroy(toDel[i] as GameObject);
        }

        //If the new minY is lower than the current minY, new lines are added.
        for (int y = updatedMinY; y < minY; y++) {
            CreateLine(y, new Vector3(camCtrl.getMinX(), y), new Vector3(camCtrl.getMaxX(), y));
        }

        //If the new maxY is greater than the current maxY, new lines are added.
        for (int y = updatedMaxY; y > maxY; y--) {
            CreateLine(y, new Vector3(camCtrl.getMinX(), y), new Vector3(camCtrl.getMaxX(), y));
        }

        //Update values of y;
        minY = updatedMinY;
        maxY = updatedMaxY;

        UpdateLineLengths();
    }

    //Randomizes the colors of the lines.
    public void RandomizeColors()
    {
        foreach (GameObject lineObj in lines) {
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            Color newColor = randomColor();
            line.startColor = newColor;
            line.endColor = newColor;
			line.material = shader;
        }
    }

    //Updates the endpoints of each line in lines to match the width of the camera.
    private void UpdateLineLengths()
    {
        foreach (GameObject lineObj in lines)
        {
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            Vector3 pos1 = line.GetPosition(0);
            Vector3 pos2 = line.GetPosition(1);
            pos1.Set(camCtrl.getMinX(), pos1.y, 0);
            pos2.Set(camCtrl.getMaxX(), pos2.y, 0);
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
        }
    }

    /*Always zooms within 50 iterations but the speed at which the zoom happens is dependent on
	 *scaleSpeed. The currentScale sometimes doesn't exactly reach the targetScale but for the
	 *purposes of this project it does not matter as it only deviates a few decimals.
	 */
    IEnumerator ScaleCamera() {
        updating = true;
        float currentScale = myCam.orthographicSize;
        float increment = (targetScale - currentScale) / 50;
        for (int i = 0; i <= 50; i++) {
            yield return new WaitForSeconds(scaleSpeed);
            myCam.orthographicSize = currentScale;
            camCtrl.UpdateCameraController();
            UpdateLines();
            UpdateBoundaries();
            currentScale += increment;
        }
        updating = false;
    }

    public void ScaleUp() {
        targetScale += 1;
        StartCoroutine("ScaleCamera");
    }

    public void ScaleDown() {
        targetScale -= 1;
        StartCoroutine("ScaleCamera");
    }

	private Color randomColor() {
		//inputs for color random function:
		// hueMin,hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax
		return UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
	}

	public int getMinY(){
		return minY;
	}

	public int getMaxY(){
		return maxY;
	}

	public CameraController getCC(){
		return camCtrl;
	}
}