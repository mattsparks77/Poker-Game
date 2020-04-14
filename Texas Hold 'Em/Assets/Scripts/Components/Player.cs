using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

    public class Player
    {
        public int id;
        public string username;
        public int tableIndex;

        public bool isPlayingHand;


        public bool isFolding;
        public bool isCheckCalling;
        public bool isRaising;
        public int raiseAmount;

        public int chipTotal = 1000;

        public CardScriptableObject[] cards;


        public Player(int _id, string _username)
        {
            id = _id;
            username = _username;
        }

        public Player()
        {

        }

        //private void Move(Vector2 _inputDirection)
        //{
        //    Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
        //    Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

        //    Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;

        //    position += _moveDirection * moveSpeed;

        //    ServerSend.PlayerPosition(this);
        //    ServerSend.PlayerRotation(this);
        //}
        /// <summary>
        /// Adds or subtracts chips from the players chip total.
        /// </summary>
        /// <param name="_chips"></param>
        public void MakeBet(int _chips)
        {
            chipTotal -= _chips;
        }

        public void AddChips(int _chips)
        {
            chipTotal += _chips;
        }


        public void SetInput(bool[] _actions, Quaternion _rotation)
        {
            //actions = _actions;
        }
    }

