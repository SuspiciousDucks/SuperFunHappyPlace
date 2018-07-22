using System;
using UnityEngine;

namespace Assets.Scripts
{
    class PlayerManager
    {
        PlayerScriptableData player1;
        PlayerScriptableData player2;

        bool player1Turn;

        public PlayerManager()
        {
            player1Turn = true;
            LoadPlayerData();
        }

        void LoadPlayerData()
        {
            player1 = new PlayerScriptableData();
            player1.name = "Player 1";
            player1.PlayerColor = Color.red;
            player1.Score = 0;
            player1.PlayerId = 1;

            player2 = new PlayerScriptableData();
            player2.name = "Player 2";
            player2.PlayerColor = Color.blue;
            player2.Score = 0;
            player2.PlayerId = 2;
        }

        public PlayerScriptableData GetActivePlayersTurn()
        {
            if (player1Turn)
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }

        public void SwapTurn()
        {
            player1Turn = !player1Turn;
        }

        public void AwardWin()
        {
            GetActivePlayersTurn().Score++;
        }
    }
}
