using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public int id = 0;
    public Animator animator;
    public GameObject StartMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        //OnDeselect();
       //animator.Play("death");
    }

    //add more animations later
    //public void OnMouseEnter()
    //{
    //    animator.Play("damaged");
    
    //}

    public void OnMouseDown()
    {
        UIManager.instance.prefabId = id;

        animator.Play("attack");
        OnSelect();
    }
    //public void OnMouseExit()
    //{
    //    animator.Play("death");
    //}
    public void OnSelect()
    {
        StartMenu.SetActive(true);
        animator.enabled = true;
        UIManager.instance.prefabId = id;
    }

    public void OnDeselect()
    {
        animator.enabled = false;
    }
}
