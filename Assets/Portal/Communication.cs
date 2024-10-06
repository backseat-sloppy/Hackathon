using System;
using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Communication : MonoBehaviour
{
    [Header("UDP Networking")]
    private const int ListenPort = 1000;
    private IPEndPoint _groupEp;
    private Socket _socket;

    [Header("Frequency Bands")]
    [SerializeField] private int waitingTimeForBaseline = 5;
    public float[] frequencyBandsAverages = new float[7];
    public float[] frequencyBandsBaselines = new float[7];
    public bool baselineWaitingTimeFinished = false;
    public int _baseLineCounter = 0;

    [Header("Concentration Data")]
    public float concentrationLevel = 0f;          // To track the concentration level
    public float concentrationThreshold = 50f;     // Threshold to trigger portal
    public VFXArcController vfxController;         // Reference to VFX controller script

    private void Start()
    {
        StartListener();
        StartCoroutine(GetValuesForBaseline());
    }

    private void Update()
    {
        try
        {
            // Check if there is data available to read
            if (_socket.Poll(1000, SelectMode.SelectRead))
            {
                byte[] receiveBufferByte = new byte[1024];
                int numberOfBytesReceived = _socket.Receive(receiveBufferByte);
                if (numberOfBytesReceived > 0)
                {
                    byte[] messageByte = new byte[numberOfBytesReceived];
                    Array.Copy(receiveBufferByte, messageByte, numberOfBytesReceived);
                    string message = Encoding.ASCII.GetString(messageByte);
                    var split = message.Split(',');
                    var counter = 0;

                    Debug.Log("Data received from UDP. Processing data...");

                    for (int i = 56; i < 63; i++)
                    {
                        if (baselineWaitingTimeFinished)
                        {
                            frequencyBandsAverages[counter] = float.Parse(split[i]);

                            // Calculate difference to baseline
                            float difference = Mathf.Abs(frequencyBandsBaselines[counter] - frequencyBandsAverages[counter]);

                            // Accumulate concentration level based on beta band (60-62)
                            if (i >= 60 && i <= 62)
                            {
                                concentrationLevel += difference;
                                Debug.Log($"Concentration level increased by {difference}. Current: {concentrationLevel}");
                            }

                            Debug.Log($"Band {counter} average data processed: {frequencyBandsAverages[counter]}");
                        }
                        else
                        {
                            // Collect baseline values
                            frequencyBandsBaselines[counter] += float.Parse(split[i]);
                            Debug.Log($"Baseline collection: Band {counter}, Incremental value: {frequencyBandsBaselines[counter]}");
                        }

                        counter++;
                    }

                    // Check if concentration exceeds the threshold to trigger the portal
                    if (concentrationLevel > concentrationThreshold)
                    {
                        Debug.Log("Concentration threshold exceeded, triggering portal.");
                        vfxController.TriggerPortal(true);
                    }
                    else
                    {
                        Debug.Log("Concentration below threshold, closing portal.");
                        vfxController.TriggerPortal(false); // Reset or close the portal
                    }

                    // Reset concentration level after every frame
                    concentrationLevel = 0f;

                    if (!baselineWaitingTimeFinished) _baseLineCounter++;
                }
            }
        }
        catch (SocketException e)
        {
            Debug.LogError($"SocketException: {e.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing data: {e.Message}");
        }
    }

    private void StartListener()
    {
        _groupEp = new IPEndPoint(IPAddress.Any, ListenPort);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) { Blocking = false };
        _socket.Bind(_groupEp);
        Debug.Log("UDP listener started, waiting for data...");
    }

    private IEnumerator GetValuesForBaseline()
    {
        Debug.Log($"Collecting baseline for {waitingTimeForBaseline} seconds...");
        yield return new WaitForSeconds(waitingTimeForBaseline);
        CalculateBaseline();
        baselineWaitingTimeFinished = true;
        Debug.Log("Baseline collection finished. Final baseline values:");
        for (int i = 0; i < frequencyBandsBaselines.Length; i++)
        {
            Debug.Log($"Band {i}, Baseline Value: {frequencyBandsBaselines[i]}");
        }
    }

    private void CalculateBaseline()
    {
        Debug.Log("Calculating final baseline by averaging...");
        for (int i = 0; i < frequencyBandsBaselines.Length; i++)
        {
            frequencyBandsBaselines[i] = frequencyBandsBaselines[i] / _baseLineCounter; // Average baseline
            Debug.Log($"Band {i} Final Baseline: {frequencyBandsBaselines[i]}");
        }
    }

    private void OnDisable() => _socket.Close();
    private void OnDestroy() => _socket.Close();
    private void OnApplicationQuit() => _socket.Close();
}
