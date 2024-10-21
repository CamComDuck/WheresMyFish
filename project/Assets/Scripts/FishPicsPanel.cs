using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishPicsPanel : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject fishPicsPanel;
    [SerializeField] private GameObject[] picturesOnPanel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject lastPageButton;
    private GameObject itemHoldingCopy;
    private List<Texture2D> picturesTaken = new List<Texture2D>();

    private bool isPicturesPanelVisible = false;

    [SerializeField] private TMP_Text pageCountText;
    private int currentPage = 1;
    private double totalPages = 1;


    private void Start() {
        fishPicsPanel.SetActive(false);
        gameInput.OnUseItem += GameInput_OnUseItem;
        gameInput.OnPicturesUI += GameInput_OnPicturesUI;
    }

    private void GameInput_OnPicturesUI(object sender, System.EventArgs e) {

        if (!isPicturesPanelVisible) {
            DisplayNext6Pictures();
            UpdatePages();
        }
        isPicturesPanelVisible = !isPicturesPanelVisible;
        fishPicsPanel.SetActive(isPicturesPanelVisible);
        
    }

    private void GameInput_OnUseItem(object sender, System.EventArgs e) {
        itemHoldingCopy = playerController.GetItemHoldingCopy();
        if (itemHoldingCopy.name == "PhotoCamera(Clone)" && !isPicturesPanelVisible) {
            PhotoCamera photoCameraScript = itemHoldingCopy.GetComponent<PhotoCamera>();
            Texture2D lastPic = photoCameraScript.CamCapture();

            picturesTaken.Add(lastPic);
        }
    }

    private void DisplayNext6Pictures() {
        int picsPerPage = 6;
        int minPics = (currentPage - 1) * picsPerPage;
        int maxPics = currentPage * picsPerPage;
        if (maxPics > picturesTaken.Count) {
            maxPics = picturesTaken.Count;
        }

        for (int i = 0; i < picsPerPage; i++) {
            int panelIndex = i;
            int pictureIndex = minPics + i;

            if (pictureIndex < maxPics) {
                picturesOnPanel[panelIndex].GetComponent<RawImage>().texture = picturesTaken[pictureIndex];
            } else {
                picturesOnPanel[panelIndex].GetComponent<RawImage>().texture = null; 
            }
        }
    }

    private void UpdatePages() {
        if (picturesTaken.Count != 0) totalPages = Math.Ceiling(picturesTaken.Count/6.0);
        DisplayButtons();
        pageCountText.text = "Page " + currentPage + " / " + totalPages;
    }

    public void NextPageButtonClick() {
        currentPage++;
        UpdatePages();
        DisplayNext6Pictures();
    }

    public void LastPageButtonClick() {
        currentPage--;
        UpdatePages();
        DisplayNext6Pictures();
    }

    private void DisplayButtons() {
        if (currentPage > 1) {
            lastPageButton.SetActive(true);
        } else {
            lastPageButton.SetActive(false);
        }

        if (picturesTaken.Count > 6*currentPage) {
            nextPageButton.SetActive(true);
        } else {
            nextPageButton.SetActive(false);
        }
    }
}
