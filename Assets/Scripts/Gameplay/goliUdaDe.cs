using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goliUdaDe : MonoBehaviour
{

    private bool turn;

    private GameManager GMScript;
    void Start()
    {
        if (gameObject.name.Contains("player"))
        {
            turn = true;
        }
        else
        {
            turn = false;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Block"))
        {
            col.GetComponent<Block>().HitBlock(turn);

            col.GetComponent<Block>().ResetBlock(turn);
         
	//	if(!col.GetComponent<BlockToggle>().isActiveAndEnabled)
            Destroy(this.gameObject);
        }
        else if (col.gameObject.name.Contains("Wall"))
        {
            col.GetComponent<Animation>().Play();
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("AI") || col.gameObject.CompareTag("player"))
        {
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Ball"))
        {
            Destroy(this.gameObject);
        }
    }

    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Block"))
        {   col.gameObject.GetComponent<Block>().HitBlock(turn);

            col.gameObject.GetComponent<Block>().ResetBlock(turn);
			if(!col.gameObject.GetComponent<BlockToggle>().isActiveAndEnabled && (int)col.gameObject.GetComponent<Block>().blockType!=3)
			{
				if (turn)
	                GameManager.Instance.player_BlokePoint++;
	            else
	                GameManager.Instance.AI_BlokePoint++;
				Destroy(this.gameObject);
			}
        }
        else if (col.gameObject.name.Contains("Wall"))
        {
            col.gameObject.GetComponent<Animation>().Play();
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("AI") || col.gameObject.CompareTag("player"))
        {
            Destroy(this.gameObject);
        }
        else if(col.gameObject.CompareTag("Ball"))
        {
            Destroy(this.gameObject);
        }

    }
}