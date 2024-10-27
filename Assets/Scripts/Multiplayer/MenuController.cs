using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button JoinBtn;
    private void Start()
    {
        HostBtn.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton == null) { return; }
            NetworkManager.Singleton.StartHost();
            HideMenu();
        });
        JoinBtn.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton == null) { return; }
            NetworkManager.Singleton.StartClient();
            HideMenu();
        });
    }
    public void HideMenu()
    {
        canvas.SetActive(false);
    }
}