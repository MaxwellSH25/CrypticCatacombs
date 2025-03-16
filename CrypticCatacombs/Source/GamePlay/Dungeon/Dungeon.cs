﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrypticCatacombs
{
    public class Dungeon
    {
        public Vector2 offset;

        public UI ui;

        public User user;
        public AIPlayer aIPlayer;

        public Map background;

        public List<Projectile2d> projectiles = new List<Projectile2d>();

        PassObject ResetDungeon;


		public Dungeon(PassObject RESETDUNGEON)
        {
            ResetDungeon = RESETDUNGEON;


            GameGlobals.PassProjectile = AddProjectile;
			GameGlobals.PassMob = AddMob;
            GameGlobals.PassSpawnPoint = AddSpawnPoint;
            GameGlobals.CheckScroll = CheckScroll;

            user = new User(1);
            aIPlayer = new AIPlayer(2);

			offset = new Vector2(0, 0);

            ui = new UI();

            //background = new TileBackground2d("2d/Map/floor", new Vector2(-100, -100), new Vector2(100, 100), new Vector2(grid.totalPhysicalDims.X + 100, grid.totalPhysicalDims.Y + 100));
			background = new Map("2d/Map/floor", "2d/Map/wall", new Vector2(0, 0), new Vector2(30, 31), new Vector2(1680, 930));
		}

        public virtual void Update()
        {
            if (!user.wizard.dead)
            {
				user.Update(aIPlayer, offset);
                aIPlayer.Update(user, offset);



                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Update(offset, aIPlayer.units.ToList<Unit>());

                    //removing a projectile once it hit something or expired
                    if (projectiles[i].done)
                    {
                        projectiles.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                if(Globals.keyboard.GetPress("Enter"))
                {
                    ResetDungeon(null);
                }
            }
                ui.Update(this);
		}

        public virtual void AddMob(object INFO)
        {
            Unit tempUnit = (Unit)INFO;

            if(user.id == tempUnit.ownerId)
            {
                user.AddUnit(tempUnit);
            }
            else if(aIPlayer.id == tempUnit.ownerId)
            {
                aIPlayer.AddUnit(tempUnit);
            }

                aIPlayer.AddUnit((Mob)INFO);
        }

        //only pass in projectiles
        public virtual void AddProjectile(object INFO)
        {
            projectiles.Add((Projectile2d)INFO);
        }

		public virtual void AddSpawnPoint(object INFO)
		{
			SpawnPoint tempSpawnPoint = (SpawnPoint)INFO;

			if (user.id == tempSpawnPoint.ownerId)
			{
				user.AddSpawnPoint(tempSpawnPoint);
			}
			else if (aIPlayer.id == tempSpawnPoint.ownerId)
			{
				aIPlayer.AddSpawnPoint(tempSpawnPoint);
			}

			aIPlayer.AddUnit((Mob)INFO);
		}

		public virtual void CheckScroll(object INFO)
        {
            Vector2 tempPos = (Vector2)INFO;
            bool screenScrollingEnabled = false; //Enable or Disable screenscrolling

            if(screenScrollingEnabled == true)
            {
				if (tempPos.X < -offset.X + (Globals.screenWidth * .4f))
				{
					offset = new Vector2(offset.X + user.wizard.speed * 2, offset.Y);
				}
				if (tempPos.X > -offset.X + (Globals.screenWidth * .6f))
				{
					offset = new Vector2(offset.X - user.wizard.speed * 2, offset.Y);
				}
				if (tempPos.Y < -offset.Y + (Globals.screenHeight * .4f))
				{
					offset = new Vector2(offset.X, offset.Y + user.wizard.speed * 2);
				}
				if (tempPos.Y > -offset.Y + (Globals.screenHeight * .6f))
				{
					offset = new Vector2(offset.X, offset.Y - user.wizard.speed * 2);
				}
			}
            
		}

        public virtual void Draw(Vector2 OFFSET)
        {
            background.Draw(offset);

			user.Draw(offset);
            aIPlayer.Draw(offset);

			for (int i = 0; i < projectiles.Count; i++)
			{
				projectiles[i].Draw(offset);
			}

			ui.Draw(this);
		}


	}
}
