using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectCable : MonoBehaviour
{
    public GameObject DisconnectCanvas;
    [SerializeField] GameObject AcceptCableCanvas;
    [SerializeField] GameObject CancelCableCanvas;
    [SerializeField] GameObject connectorOtherside;

    public GameObject DBRegister;

    public GameObject touchedObject;
    public int currentConnectionOnConnector = 0;

    public bool canMove = true;

    Bounds b;

    // Start is called before the first frame update
    void Start()
    {
        DBRegister = GameObject.FindGameObjectWithTag("DBRegister");

        //Creates a Bounding Box around SpawnObjectQuad
        b = new Bounds(gameObject.transform.parent.transform.parent.transform.position, new Vector3(0.7f, 1.4f, 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!b.Contains(gameObject.transform.position))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        touchedObject = col.gameObject;
        gameObject.transform.position = touchedObject.transform.position;
        currentConnectionOnConnector = touchedObject.GetComponent<ConnectConnector>().CheckArrayNumber();
        gameObject.transform.rotation = touchedObject.transform.rotation * Quaternion.Euler(0, currentConnectionOnConnector * 90.0f, 0);

        gameObject.GetComponent<ConnectCable>().canMove = false;
        gameObject.GetComponent<ConnectCable>().AcceptCableCanvas.SetActive(true);
        gameObject.GetComponent<ConnectCable>().CancelCableCanvas.SetActive(true);
    }

    public void AcceptCableConnector()
    {
        if (connectorOtherside.GetComponent<ConnectCable>().touchedObject)
        {
            //Get Cable Index
            currentConnectionOnConnector = touchedObject.GetComponent<ConnectConnector>().CheckArrayNumber();

            //Tags interchange
            //touchedObject.GetComponent<ConnectConnector>().endTag[currentConnectionOnConnector] = connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().iniTag;
            //connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().endTag[connectorOtherside.GetComponent<ConnectCable>().currentConnectionOnConnector] = touchedObject.GetComponent<ConnectConnector>().iniTag;

            //GameObject Interchange
            touchedObject.GetComponent<ConnectConnector>().endGameObjects[currentConnectionOnConnector] = connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().gameObject;

            currentConnectionOnConnector = connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().CheckArrayNumber();
            connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().endGameObjects[currentConnectionOnConnector] = touchedObject.GetComponent<ConnectConnector>().gameObject;

            //Canvas activation
            gameObject.GetComponent<ConnectCable>().DisconnectCanvas.SetActive(true);
            connectorOtherside.GetComponent<ConnectCable>().DisconnectCanvas.SetActive(true);
            gameObject.GetComponent<ConnectCable>().AcceptCableCanvas.SetActive(false);
            gameObject.GetComponent<ConnectCable>().CancelCableCanvas.SetActive(false);
            connectorOtherside.GetComponent<ConnectCable>().AcceptCableCanvas.SetActive(false);
            connectorOtherside.GetComponent<ConnectCable>().CancelCableCanvas.SetActive(false);

            //Para BD ... Asignar el dispositivo conectado y la borna de conexion.
            gameObject.transform.parent.GetComponent<CableInfo>()._deviceTouched1 = touchedObject.transform.parent.transform.parent.name;
            gameObject.transform.parent.GetComponent<CableInfo>()._deviceTouched2 = connectorOtherside.GetComponent<ConnectCable>().touchedObject.transform.parent.transform.parent.name;

            gameObject.transform.parent.GetComponent<CableInfo>()._connectorTouched1 = touchedObject.name;
            gameObject.transform.parent.GetComponent<CableInfo>()._connectorTouched2 = connectorOtherside.GetComponent<ConnectCable>().touchedObject.name;

            gameObject.transform.parent.GetComponent<CableInfo>()._cableState = "activated";

            //DBRegister.GetComponent<TxtController>().WriteCableInfo(gameObject.transform.parent.GetComponent<CableInfo>());
        }
    }

    public void DisconnectCable()
    {   
        GameObject _powerSupply = GameObject.FindGameObjectWithTag("PowerSupply");
        if (!_powerSupply.GetComponent<PowerSupplyScript>().m_InstrumentState)
        {
            //Tag elimitaion
            //touchedObject.GetComponent<ConnectConnector>().endTag[currentConnectionOnConnector] = null;
            //connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().endTag[connectorOtherside.GetComponent<ConnectCable>().currentConnectionOnConnector] = null;

            //GameObject Elimination
            touchedObject.GetComponent<ConnectConnector>().endGameObjects[currentConnectionOnConnector] = null;
            connectorOtherside.GetComponent<ConnectCable>().touchedObject.GetComponent<ConnectConnector>().endGameObjects[connectorOtherside.GetComponent<ConnectCable>().currentConnectionOnConnector] = null;

            gameObject.transform.parent.GetComponent<CableInfo>()._deleteTimeCable = System.DateTime.Now;
            gameObject.transform.parent.GetComponent<CableInfo>()._cableState = "deleted";

            DBRegister.GetComponent<TxtController>().WriteCableInfo(gameObject.transform.parent.GetComponent<CableInfo>());

            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void CancelCableConnector()
    {
        gameObject.GetComponent<ConnectCable>().canMove = true;
        gameObject.transform.position = touchedObject.transform.position + new Vector3(0, 0.050f, 0);

        gameObject.GetComponent<ConnectCable>().touchedObject = null;

        gameObject.GetComponent<ConnectCable>().AcceptCableCanvas.SetActive(false);
        gameObject.GetComponent<ConnectCable>().CancelCableCanvas.SetActive(false);
    }
}
