using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModelPrefab;
    public List<GameObject> controllerPrefabs;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHand;
    private Animator handAnimator;

    void Start() { tryInit(); }

    void tryInit()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if(devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            
            if(!prefab) { prefab = controllerPrefabs[0]; }
            spawnedController = Instantiate(prefab, transform);

            spawnedHand = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHand.GetComponent<Animator>();
        }
    }

    void updateHandAnim()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal)) { handAnimator.SetFloat("Trigger", triggerVal); }
        else { handAnimator.SetFloat("Trigger", triggerVal); }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripVal)) { handAnimator.SetFloat("Grip", gripVal); }
        else { handAnimator.SetFloat("Grip", gripVal); }
    }

    void Update()
    {
        if(!targetDevice.isValid) { tryInit(); }

        spawnedController.SetActive(showController);
        spawnedHand.SetActive(!showController);

        if(!showController) updateHandAnim();
    }
}
