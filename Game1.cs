using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MultiplayerSpawningDemonstration
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //spawn location list
        SpawnCircle[] spawnLocations = new SpawnCircle[5];
        //player list
        List<Player> players = new List<Player>();
        Queue<Player> playerQueue = new Queue<Player>();
        Queue<Player> notSpawnedPlayerQueue = new Queue<Player>();
        int amountOfPlayers = 100;
        Texture2D playerTexture;

        //text font
        SpriteFont font;
        //random
        Random rand = new Random();

        //spawner timer
        int maxTimer = 60;
        int countdownTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            playerTexture = Content.Load<Texture2D>("Textures/red");

            countdownTimer = maxTimer;

            //spawn locations
            spawnLocations[0] = new SpawnCircle(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), 50, playerTexture);
            spawnLocations[1] = new SpawnCircle(new Vector2(100, 100), 50, playerTexture);
            spawnLocations[2] = new SpawnCircle(new Vector2(Window.ClientBounds.Width - 100, 100), 50, playerTexture);
            spawnLocations[3] = new SpawnCircle(new Vector2(100, Window.ClientBounds.Height - 100), 50, playerTexture);
            spawnLocations[4] = new SpawnCircle(new Vector2(Window.ClientBounds.Width - 100, Window.ClientBounds.Height - 100), 50, playerTexture);

            //players
            SpawnPlayers();

            base.Initialize();
        }

        //spawns the players in the world
        void SpawnPlayers()
        {
            for (int i = 0; i < amountOfPlayers; i++)
            {
                players.Add(new Player(playerTexture, rand));
                playerQueue.Enqueue(players[i]);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");
        }

        protected override void UnloadContent()
        {

        }

        //if the user restarts the program
        void Restart()
        {
            //clear everything
            players.Clear();
            playerQueue.Clear();
            //spawn players
            SpawnPlayers();
            //reset timer
            countdownTimer = maxTimer;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //spawn timer
            countdownTimer--;
            if (countdownTimer <= 0 && playerQueue.Count > 0)
            {
                //spawn the player
                playerQueue.Peek().Spawn(players, spawnLocations, playerQueue.Peek());
                //if the player can't spawn, add it to a temp list
                if (!playerQueue.Peek().Spawned)
                    notSpawnedPlayerQueue.Enqueue(playerQueue.Peek());
                //dequeue the player
                playerQueue.Dequeue();
                //reset timer
                countdownTimer = maxTimer;
            }
            //make the current party queue equal to the people who 
            //can't spawn and check them again 
            if (playerQueue.Count == 0)
                playerQueue = notSpawnedPlayerQueue;

            //if the player hits r, restart
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Restart();

            //update players
            foreach (Player player in players)
                player.Update(gameTime, Window);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw players
            foreach (Player player in players)
                player.Draw(gameTime, spriteBatch);
            //draw spawn points
            foreach (SpawnCircle spawn in spawnLocations)
                spawn.Draw(gameTime, spriteBatch);

            //draw text
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Press R to restart at any time.", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, "Hold P to make the players move.", new Vector2(10, 30), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
