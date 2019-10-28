using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppraisalPickConfirm : MonoBehaviour {
    public AppraisalPick pickManager;
    public GameObject appraisalScreen;
    public GameObject nullArchetype;
    public GameObject filledArchetype;
    public FilledArchetype filledManager;
    public GameObject nextArrow;

    void OnMouseDown() {
        pickManager.ProcessConfirmation();
        appraisalScreen.SetActive(false);
        nullArchetype.SetActive(false);
        filledArchetype.SetActive(true);
        filledManager.SetFilled();
        filledManager.type = 1;
        nextArrow.SetActive(true);
    }
}
