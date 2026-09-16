using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class Javelin : MagicSpell
	{
		public JavelinMode Mode = JavelinMode.Charging;
		public int PiercingNPC = -1;
		public float charge = 1f;

		public const int TimeLeft = 60 * 4;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Mode == JavelinMode.Grounded)
			{
				behindNPCsAndTiles.Add(index);
			}
			else if (Mode == JavelinMode.Piercing)
			{
				behindNPCs.Add(index);
			}
			else
			{
				overPlayers.Add(index);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)Mode);
			writer.Write(charge);
			writer.Write(PiercingNPC);
			writer.Write(Projectile.rotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Mode = (JavelinMode)reader.ReadByte();
			charge = reader.ReadSingle();
			PiercingNPC = reader.ReadInt32();
			Projectile.rotation = reader.ReadSingle();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.width = 96;
			Projectile.height = 32;
			Projectile.AverageDimensions();
			Projectile.localNPCHitCooldown = (TimeLeft / 4) + 1;
			Projectile.hide = true;
			Projectile.ArmorPenetration += 5;
		}

		public override void AI()
		{
			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.rotation.ToRotationVector2();

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				NetUpdate();
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			}

			if (Projectile.position != Projectile.oldPosition)
			{
				NetUpdate();
			}

			if (Mode == JavelinMode.Charging)
			{
				if (Owner.channel && charge < Circle.GlobalMaxCharge)
				{
					charge += Circle.GlobalChargeSpeed;
					Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
					Projectile.spriteDirection = Owner.direction;
					Owner.heldProj = Projectile.whoAmI;
					AOPlayerOwner.HeavySkillActive = true;
					Owner.itemAnimation = Owner.itemAnimationMax;
					Owner.itemTime = Owner.itemTimeMax;
					Owner.itemRotation = dir.ToRotation();
					if (Owner.direction != 1)
					{
						Owner.itemRotation += MathHelper.Pi;
					}
					Projectile.rotation = dir.ToRotation();
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter - new Vector2(0, 15f * Owner.gravDir));
					Projectile.timeLeft = TimeLeft;
				}
				else
				{
					Projectile.velocity = dir * 20f;
					Mode = JavelinMode.Flying;
					Owner.channel = false;
					Projectile.timeLeft = TimeLeft;
					NetUpdate();
					if (ArcaneOdysseyClientConfig.Instance.AbilityText && Owner is not null && Owner?.active == true && !Owner.DeadOrGhost && Main.myPlayer == Projectile.owner)
					{
						var name = (Imbue.PrettySpellPrefix + " " + DisplayName).Trim();
						if (SecondImbue is not null)
						{
							name = SecondImbue.PrettyAttackPrefix + " " + name;
						}
						CombatText.NewText(Owner.Hitbox, Imbue.Colour, (name + "!").Trim(), true);
					}
				}
			}
			if (Mode == JavelinMode.Flying)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.velocity.Y += ApplySpeed(0.13f, true);
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}
			if (Mode == JavelinMode.Piercing)
			{
				if (PiercingNPC != -1 && Main.npc.IndexInRange(PiercingNPC))
				{
					var npc = Main.npc[PiercingNPC];
					if (npc.active && npc.life > 0)
					{
						Projectile.Center = npc.Center;
						if (Projectile.timeLeft % (TimeLeft / 4) == 0)
						{
							SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
							if (!Main.dedServ)
							{
								for (int i = 0; i < 10; i++)
								{
									Imbue?.ExplosionEffects(Projectile.Center, Projectile.scale / 2f);
									SecondImbue?.ExplosionEffects(Projectile.Center, Projectile.scale / 2f);
								}
								PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
								Main.instance.CameraModifiers.Add(modifier);
							}
						}
					}
					else
					{
						Kill();
					}
				}
			}
			if (Mode == JavelinMode.Grounded)
			{
				if (Projectile.timeLeft % (TimeLeft / 4) == 0)
				{
					SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
					if (!Main.dedServ)
					{
						for (int i = 0; i < 10; i++)
						{
							Imbue?.ExplosionEffects(Projectile.Center, Projectile.scale / 2f);
							SecondImbue?.ExplosionEffects(Projectile.Center, Projectile.scale / 2f);
						}
						PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
						Main.instance.CameraModifiers.Add(modifier);
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Mode == JavelinMode.Flying)
			{
				Projectile.velocity = Vector2.Zero;
				Mode = JavelinMode.Piercing;
				Projectile.timeLeft = TimeLeft;
				PiercingNPC = target.whoAmI;
				NetUpdate();
			}
			SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= charge;
		}

		public override bool DrawWithImbueColours => true;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Mode == JavelinMode.Flying)
			{
				Projectile.velocity = Vector2.Zero;
				Mode = JavelinMode.Grounded;
				Projectile.timeLeft = TimeLeft;
				NetUpdate();
			}
			return false;
		}
	}

	public enum JavelinMode
	{
		Charging,
		Flying,
		Grounded,
		Piercing
	}
}
