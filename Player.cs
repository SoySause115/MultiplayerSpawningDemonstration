using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MultiplayerSpawningDemonstration
{
    class Player
    {
        Vector2 position;
        Texture2D texture;
        Random rand = new Random(); //random numbers
        //if it has spawned into the world
        bool spawned = false;

        //timers for movement
        int maxTimer = 60;
        int countdownTimer;
        //direction the player goes
        int direction;

        public Vector2 Position
        {
            get { return position; }
        }
        public bool Spawned
        {
            get { return spawned; }
        }

        public Player(Texture2D texture, Random random)
        {
            this.texture = texture;
            rand = random;

            countdownTimer = maxTimer;
        }

        public void Spawn(List<Player> players, SpawnCircle[] spawnLocations, Player thisP)
        {
            bool canSpawn = false;

            //random location
            int randLocation;
            //keeps track of how many spawns it checked
            List<bool> locationsSearched = new List<bool>();
            for (int i = 0; i < spawnLocations.Length; i++)
                locationsSearched.Add(false);
            //go through this as many times as it takes
            //to find a location nobody is at
            do
            {
                int counter = 0;
                randLocation = rand.Next(0, spawnLocations.Length);
                locationsSearched[randLocation] = true;

                for (int i = 0; i < players.Count; i++)
                {
                    //if there isn't another player in the same circle and isn't this player
                    if (!spawnLocations[randLocation].Contains(players[i].position) && players[i] != thisP)
                        counter++;
                }

                //if the player goes through all the spawnpoints and is in the clear (minus itself)
                if (counter == players.Count - 1)
                    canSpawn = true;

                int tempCounter = 0;
                for (int i = 0; i < locationsSearched.Count; i++)
                {
                    //if the location has been checked
                    if (locationsSearched[i])
                        tempCounter++;
                }
                //if all locations have been checked
                if (tempCounter == locationsSearched.Count)
                    break;
            } while (!canSpawn);

            if (canSpawn)
            {
                //choose between 4 different starting locations
                switch (rand.Next(0, 4))
                {
                    case 0:
                        position = new Vector2(
                            spawnLocations[randLocation].Center.X + rand.Next(0, (int)spawnLocations[randLocation].Radius),
                            spawnLocations[randLocation].Center.Y + rand.Next(0, (int)spawnLocations[randLocation].Radius));
                        break;
                    case 1:
                        position = new Vector2(
                            spawnLocations[randLocation].Center.X - rand.Next(0, (int)spawnLocations[randLocation].Radius),
                            spawnLocations[randLocation].Center.Y + rand.Next(0, (int)spawnLocations[randLocation].Radius));
                        break;
                    case 2:
                        position = new Vector2(
                            spawnLocations[randLocation].Center.X + rand.Next(0, (int)spawnLocations[randLocation].Radius),
                            spawnLocations[randLocation].Center.Y - rand.Next(0, (int)spawnLocations[randLocation].Radius));
                        break;
                    case 3:
                        position = new Vector2(
                            spawnLocations[randLocation].Center.X - rand.Next(0, (int)spawnLocations[randLocation].Radius),
                            spawnLocations[randLocation].Center.Y - rand.Next(0, (int)spawnLocations[randLocation].Radius));
                        break;
                }

                spawned = true;
            }
        }

        private void Movement(int direction)
        {
            switch (direction)
            {
                case 0: //north
                    position.Y++;
                    break;
                case 1: //north east
                    position.X++;
                    position.Y++;
                    break;
                case 2: //east
                    position.X++;
                    break;
                case 3: //south east
                    position.X++;
                    position.Y--;
                    break;
                case 4: //south
                    position.Y--;
                    break;
                case 5: //south west
                    position.X--;
                    position.Y--;
                    break;
                case 6: //west
                    position.X--;
                    break;
                case 7: //north west
                    position.X--;
                    position.Y++;
                    break;
            }
        }

        public virtual void Update(GameTime gameTime, GameWindow gameWindow)
        {
            //if it has spawned, start moving  
            if (spawned)
            {
                countdownTimer--;
                if (countdownTimer <= 0)
                {
                    direction = rand.Next(0, 8);
                    countdownTimer = rand.Next(0, maxTimer);
                }

                //small movement
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                    Movement(direction);
            }

            //if it hits any wall, bounce it back
            if (position.X < 0)
                position.X++;
            if (position.Y < 0)
                position.Y++;
            if (position.X > gameWindow.ClientBounds.Width - 10)
                position.X--;
            if (position.Y > gameWindow.ClientBounds.Height - 10)
                position.Y--;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //only draw if spawned
            if (spawned)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 10, 10), Color.White);
                spriteBatch.End();
            }
        }
    }
}
