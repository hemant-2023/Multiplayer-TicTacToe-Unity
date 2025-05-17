using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class BoardManager : NetworkBehaviour
{
    Button[,] buttons = new Button[3,3];
    public override void OnNetworkSpawn()
    {
        var cells = GetComponentsInChildren<Button>();
        int n = 0; 

        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                buttons[i, j] = cells[n];
                n++;

                int r = i;
                int c = j;

                buttons[i, j].onClick.AddListener(delegate { OnClickCell(r, c); });
            }
        }
    }

    [SerializeField] private Sprite _xSprite, _0Sprite;

    private void OnClickCell(int r,int c)
    {
        //if button clicked by host , button scprite will change as x

        if(NetworkManager.Singleton.IsHost && GameManager.Instance._currentTurn.Value == 0)
        {
            buttons[r, c].GetComponent<Image>().sprite = _xSprite;

            //making button non interactable
            buttons[r, c].interactable = false;

            //change it on client side also
            ChangeSpriteClientRpc(r, c);

            //changing turn to client
            GameManager.Instance._currentTurn.Value = 1;
        }

        //if button clicked by client , button sprite will change as o

        else if(NetworkManager.Singleton.IsClient && GameManager.Instance._currentTurn.Value == 1)
        {
            buttons[r,c].GetComponent<Image>().sprite = _0Sprite;

            //making button non interactable
            buttons[r,c].interactable = false;

            //change it on host side also
            ChangeSpriteServerRpc(r, c);

            //changing turn to host but client cant write value on server ,check server rpc function

        }
    }

    [ClientRpc]
    private void ChangeSpriteClientRpc(int r,int c)
    {
        buttons[r, c].GetComponent<Image>().sprite = _xSprite;
        buttons[r, c].interactable = false;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ChangeSpriteServerRpc(int r,int c)
    {
        buttons[r,c].GetComponent <Image>().sprite = _0Sprite;
        buttons[r, c].interactable = false;
        GameManager.Instance._currentTurn.Value = 0;
    }
}
