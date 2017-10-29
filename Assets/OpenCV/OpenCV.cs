using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Cuda;
using System.IO;

public class OpenCV : AbstractSensor<RadarSensorBehaviour>
{
    public Camera cam1;
    public Camera cam2;

    public bool EnableOpenCV = false;
    public float OpenCVRate = 2f;

    public bool TextLog = false;

    private ScreenRecorder screenRecorder;
    private StopSignDetector stopSignDetector;

    private void Start()
    {
        screenRecorder = (ScreenRecorder)ScriptableObject.CreateInstance(typeof(ScreenRecorder)); //Initiate screenrecorder
        stopSignDetector = new StopSignDetector(new Image<Bgr, System.Byte>("stop-sign-model.png")); //Initiate stopsign detector with example sign

        //System.Windows.Forms.Application.EnableVisualStyles();
        //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
    }

    private void Update()
    {
        if (EnableOpenCV) // Check if OpenCV is enabled.
        {
            InvokeRepeating("RepeatingSearch", 2f, OpenCVRate); // After 2 seconds call OpenCV every OpenCVRate secondes
        }
        else
        {
            CancelInvoke("RepeatingSearch"); // Cancel call for OpenCV
        }
    }

    private void RepeatingSearch()
    {
        DetectStopsign(cam1, "Camera1");
        DetectStopsign(cam2, "Camera2");
        DetectPedestrian(cam1, screenRecorder.UniqueFilename(cam1));
        DetectPedestrian(cam2, screenRecorder.UniqueFilename(cam2));
    }

    private void DetectPedestrian(Camera cam, string name)
    {
        Texture2D camtexture = screenRecorder.CaptureScreenshot2D(cam);
        Mat image = new Mat();
        Texture2dToOutputArray(camtexture, image);

        CvInvoke.Flip(image, image, FlipType.Vertical);
        CvInvoke.CvtColor(image, image, ColorConversion.Rgb2Bgr);

        using (image)
        {
            long processingTime;
            Rectangle[] results;

            if (CudaInvoke.HasCuda)
            {
                using (GpuMat gpuMat = new GpuMat(image))
                    results = FindPedestrian.Find(gpuMat, out processingTime);
            }
            else
            {
                using (UMat uImage = image.GetUMat(AccessType.ReadWrite))
                    results = FindPedestrian.Find(uImage, out processingTime);
            }
            if (results.Length > 0)
            {
                if (!TextLog)
                {
                    foreach (Rectangle rect in results)
                    {
                        CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Red).MCvScalar);
                    }
                    //CvInvoke.Imshow(name, image);
                    image.Bitmap.Save(screenRecorder.uniqueFilename(1920, 1080));
                }
                else
                {
                    //Log through Logger class
                }
            }
        }
    }

    private void DetectStopsign(Camera cam, string name)
    {
        Texture2D camtexture = screenRecorder.CaptureScreenshot2D(cam);
        Mat image = new Mat();
        Texture2dToOutputArray(camtexture, image);

        CvInvoke.Flip(image, image, FlipType.Vertical);
        CvInvoke.CvtColor(image, image, ColorConversion.Rgb2Bgr);

        ProcessImage(image, name);
    }

    public static void Texture2dToOutputArray(Texture2D texture, IOutputArray result)
    {
        int width = texture.width;
        int height = texture.height;
        try
        {
            Color32[] colors = texture.GetPixels32();
            GCHandle handle = GCHandle.Alloc(colors, GCHandleType.Pinned);

            using (Mat rgba = new Mat(height, width, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), width * 4))
            {
                rgba.CopyTo(result);
            }
            handle.Free();
        }
        catch (System.Exception excpt)
        {
            if (texture.format == TextureFormat.ARGB32 || texture.format == TextureFormat.RGB24)
            {
                byte[] jpgBytes = texture.EncodeToJPG();
                using (Mat tmp = new Mat())
                {
                    CvInvoke.Imdecode(jpgBytes, ImreadModes.AnyColor, tmp);
                    tmp.CopyTo(result);
                }
            }
            else
            {
                throw new System.Exception(System.String.Format("We are not able to handle Texture format of {0} type", texture.format), excpt);
            }
        }
    }

    private void ProcessImage(Mat image, string name)
    {
        Stopwatch watch = Stopwatch.StartNew(); // time the detection process

        List<Mat> stopSignList = new List<Mat>();
        List<Rectangle> stopSignBoxList = new List<Rectangle>();
        stopSignDetector.DetectStopSign(image, stopSignList, stopSignBoxList);

        watch.Stop(); //stop the timer
        UnityEngine.Debug.Log(System.String.Format("Stop Sign Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds));

        Point startPoint = new Point(10, 10);
        if (!TextLog)
        {
            for (int i = 0; i < stopSignList.Count; i++)
            {
                Rectangle rect = stopSignBoxList[i];
                AddLabelAndImage(
                   ref startPoint,
                   System.String.Format("Stop Sign [{0},{1}]:", rect.Location.Y + rect.Width / 2, rect.Location.Y + rect.Height / 2),
                   stopSignList[i]);
                CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Aquamarine).MCvScalar, 2);
            }
            image.Bitmap.Save(screenRecorder.uniqueFilename(1920, 1080));
            //CvInvoke.Imshow(name, image);
        }
        else
        {
            //Log through Logger class
        }
    }

    private void AddLabelAndImage(ref Point startPoint, System.String labelText, IImage image)
    {
        Label label = new Label
        {
            Text = labelText,
            Width = 100,
            Height = 30,
            Location = startPoint
        };
        startPoint.Y += label.Height;

        ImageBox box = new ImageBox
        {
            ClientSize = image.Size,
            Image = image,
            Location = startPoint
        };
        startPoint.Y += box.Height + 10;
    }
}