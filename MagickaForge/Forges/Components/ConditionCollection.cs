﻿using MagickaForge.Forges.Components.Events;
using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Forges.Components
{
    public class ConditionCollection
    {
        [JsonConverter(typeof(JsonStringEnumConverter<EventConditionType>))]
        public EventConditionType ConditionType { get; set; }
        public float Hitpoints { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Threshold { get; set; }
        public float Time { get; set; }
        public bool Repeat { get; set; }
        public Event[]? Events { get; set; }

        public ConditionCollection() { }
        public ConditionCollection(BinaryReader br)
        {
            ConditionType = (EventConditionType)br.ReadByte();
            Hitpoints = br.ReadSingle();
            Element = (Elements)br.ReadInt32();
            Threshold = br.ReadSingle();
            Time = br.ReadSingle();
            Repeat = br.ReadBoolean();

            Events = new Event[br.ReadInt32()];
            for (int i = 0; i < Events.Length; i++)
            {
                EventType type = (EventType)br.ReadByte();
                switch (type)
                {
                    case (EventType.Damage):
                        Events[i] = new DamageEvent()
                        {
                            AttackProperty = (AttackProperties)br.ReadInt32(),
                            Element = (Elements)br.ReadInt32(),
                            Amount = br.ReadSingle(),
                            Magnitude = br.ReadSingle(),
                            VelocityBased = br.ReadBoolean(),
                        };
                        break;
                    case (EventType.Splash):
                        Events[i] = new SplashEvent()
                        {
                            AttackProperty = (AttackProperties)br.ReadInt32(),
                            Element = (Elements)br.ReadInt32(),
                            Amount = br.ReadInt32(),
                            Magnitude = br.ReadSingle(),
                            Radius = br.ReadSingle()
                        };
                        break;
                    case (EventType.Sound):
                        Events[i] = new SoundEvent()
                        {
                            Bank = (Banks)br.ReadInt32(),
                            Cue = br.ReadString(),
                            Magnitude = br.ReadSingle(),
                            StopOnRemove = br.ReadBoolean(),
                        };
                        break;
                    case (EventType.Effect):
                        {
                            Events[i] = new EffectEvent()
                            {
                                Follow = br.ReadBoolean(),
                                WorldAligned = br.ReadBoolean(),
                                Effect = br.ReadString()
                            };
                        }
                        break;
                    case (EventType.Remove):
                        {
                            Events[i] = new RemoveEvent()
                            {
                                Bounces = br.ReadInt32(),
                            };
                        }
                        break;
                    case (EventType.CameraShake):
                        {
                            Events[i] = new CameraShakeEvent()
                            {
                                Time = br.ReadSingle(),
                                Magnitude = br.ReadSingle(),
                                AtPosition = br.ReadBoolean()
                            };
                        }
                        break;
                    case (EventType.Decal):
                        {
                            Events[i] = new DecalEvent()
                            {
                                X = br.ReadInt32(),
                                Y = br.ReadInt32(),
                                Scale = br.ReadInt32()
                            };
                        }
                        break;
                    case (EventType.Spawn):
                        {
                            Events[i] = new SpawnEvent()
                            {
                                Type = br.ReadString(),
                                IdleAnimation = br.ReadString(),
                                SpawnAnimation = br.ReadString(),
                                Health = br.ReadSingle(),
                                Order = (Order)br.ReadByte(),
                                ReactTo = (ReactTo)br.ReadByte(),
                                Reaction = (Order)br.ReadByte(),
                                Rotation = br.ReadSingle(),
                                Offset = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()]
                            };
                        }
                        break;
                    case (EventType.SpawnGibs):
                        {
                            Events[i] = new SpawnGibsEvent()
                            {
                                StartIndex = br.ReadInt32(),
                                EndIndex = br.ReadInt32()
                            };
                        }
                        break;
                    case (EventType.SpawnItem):
                        {
                            Events[i] = new SpawnItemEvent()
                            {
                                Item = br.ReadString()
                            };
                        }
                        break;
                    case (EventType.SpawnMagick):
                        {
                            Events[i] = new SpawnMagickEvent()
                            {
                                MagickType = br.ReadString(),
                            };
                        }
                        break;
                    case (EventType.SpawnMissile):
                        {
                            Events[i] = new SpawnMissileEvent()
                            {
                                Type = br.ReadString(),
                                Velocity = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()],
                                Facing = br.ReadBoolean(),
                            };
                        }
                        break;
                    case (EventType.Light):
                        {
                            Events[i] = new LightEvent()
                            {
                                Radius = br.ReadSingle(),
                                DiffuseColor = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()],
                                AmbientColor = [br.ReadSingle(), br.ReadSingle(), br.ReadSingle()],
                                SpecularAmount = br.ReadSingle(),
                                LightVariationType = (LightVariationType)br.ReadByte(),
                                VariationAmount = br.ReadSingle(),
                                VariationSpeed = br.ReadSingle(),
                            };
                        }
                        break;
                    case (EventType.CastMagick):
                        {
                            Events[i] = new CastMagickEvent()
                            {
                                MagickType = br.ReadString(),
                                Elements = new int[br.ReadInt32()]
                            };
                            for (int x = 0; x < (Events[i] as CastMagickEvent)!.Elements!.Length; x++)
                            {
                                (Events[i] as CastMagickEvent)!.Elements![x] = br.ReadInt32();
                            }
                        }
                        break;
                    case (EventType.DamageOwner):
                        Events[i] = new DamageOwnerEvent()
                        {
                            AttackProperty = (AttackProperties)br.ReadInt32(),
                            Element = (Elements)br.ReadInt32(),
                            Amount = br.ReadSingle(),
                            Magnitude = br.ReadSingle(),
                            VelocityBased = br.ReadBoolean(),
                        };
                        break;
                }
            }
        }
    }
}
