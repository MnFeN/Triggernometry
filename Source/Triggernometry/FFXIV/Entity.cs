using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.PluginBridges;
using Triggernometry.Utilities;

namespace Triggernometry.FFXIV
{   
    public class Entity
    {
        #region Basic Properties
        public bool Exist { get; set; } = true;
        public virtual PluginSource PluginSource { get; set; } = PluginSource.None;

        public virtual IntPtr Address { get; set; }
        public virtual string Name { get; set; } = "";
        public virtual uint ID { get; set; }
        public virtual uint BNpcID { get; set; }
        public virtual uint OwnerID { get; set; }
        public virtual EntityType Type { get; set; }
        public virtual byte EffectiveDistance { get; set; }
        public virtual ObjectStatus ObjectStatus { get; set; }
        public virtual float PosX { get; set; }
        public virtual float PosY { get; set; }
        public virtual float PosZ { get; set; }
        public virtual float Heading { get; set; }
        public virtual float Radius { get; set; }
        public virtual ModelStatus ModelStatus { get; set; }
        public virtual bool IsTargetable { get; set; }
        public virtual uint CurrentHP { get; set; }
        public virtual uint MaxHP { get; set; }
        public virtual uint CurrentMP { get; set; }
        public virtual uint MaxMP { get; set; }
        public virtual ushort CurrentCP { get; set; }
        public virtual ushort MaxCP { get; set; }
        public virtual ushort CurrentGP { get; set; }
        public virtual ushort MaxGP { get; set; }
        public virtual short TransformationID { get; set; }
        public virtual Job Job { get; set; } = Job.EmptyJob;
        public virtual byte Level { get; set; }
        public virtual MonsterType MonsterType { get; set; }
        public virtual bool IsEnemy { get; set; }
        public virtual bool IsAggressive { get; set; }
        public virtual bool InCombat { get; set; }
        public virtual bool InParty { get; set; }
        public virtual bool InAlliance { get; set; }
        public virtual bool IsFriend { get; set; }
        public virtual byte WeaponID { get; set; }
        public virtual uint TargetID { get; set; }
        public virtual uint BNpcNameID { get; set; }
        public virtual ushort CurrentWorldID { get; set; }
        public virtual ushort WorldID { get; set; }
        public virtual List<Status> Statuses { get; set; } = new List<Status>();
        public virtual bool IsCasting { get; set; }
        public virtual byte CastType { get; set; }
        public virtual uint CastID { get; set; }
        public virtual uint CastTargetID { get; set; }
        public virtual float CastPosX { get; set; }
        public virtual float CastPosY { get; set; }
        public virtual float CastPosZ { get; set; }
        public virtual float CastTime { get; set; }
        public virtual float MaxCastTime { get; set; }

        public string HexAddress => Address.ToString("X");
        public string HexID => ID.ToString("X");
        public string OwnerHexID => OwnerID.ToString("X");
        public string TargetHexID => TargetID.ToString("X");
        public string CastHexID => CastID.ToString("X");
        public bool IsCharacter => Type == EntityType.Pc || Type == EntityType.BattleNpc
                                || Type == EntityType.EventNpc || Type == EntityType.Retainer;

        public Vector2 PosXY => new Vector2(PosX, PosY);
        public Vector3 Pos => new Vector3(PosX, PosY, PosZ);

        public Entity() { }
        public static Entity NullEntity() => new Entity() { Exist = false };
        public virtual Entity Snapshot() => new Entity
        {
            Exist = Exist, PluginSource = PluginSource.None,
            Address = Address, Name = Name, ID = ID, BNpcID = BNpcID, OwnerID = OwnerID, Type = Type,
            EffectiveDistance = EffectiveDistance, ObjectStatus = ObjectStatus,
            PosX = PosX, PosY = PosY, PosZ = PosZ, Heading = Heading, Radius = Radius,
            ModelStatus = ModelStatus, IsTargetable = IsTargetable,
            CurrentHP = CurrentHP, MaxHP = MaxHP, CurrentMP = CurrentMP, MaxMP = MaxMP,
            CurrentCP = CurrentCP, MaxCP = MaxCP, CurrentGP = CurrentGP, MaxGP = MaxGP,
            TransformationID = TransformationID,
            Job = Job, Level = Level,
            MonsterType = MonsterType, IsEnemy = IsEnemy,
            IsAggressive = IsAggressive, InCombat = InCombat,
            InParty = InParty, InAlliance = InAlliance, IsFriend = IsFriend,
            WeaponID = WeaponID, TargetID = TargetID, BNpcNameID = BNpcNameID, 
            CurrentWorldID = CurrentWorldID, WorldID = WorldID,
            Statuses = Statuses.Select(s => s.Snapshot()).ToList(),
            IsCasting = IsCasting, CastType = CastType, CastID = CastID, CastTargetID = CastTargetID,
            CastPosX = CastPosX, CastPosY = CastPosY, CastPosZ = CastPosZ,
            CastTime = CastTime, MaxCastTime = MaxCastTime,
        };

        #endregion Basic Properties

        #region Get Entities

        public static IEnumerable<Entity> GetEntities(Func<Entity, bool> filter)
            => GetEntities().Where(filter);

        public static IEnumerable<Entity> GetEntities(Func<Entity, bool> filter, bool useOverlay)
            => GetEntities(useOverlay).Where(filter);

        public static IEnumerable<Entity> GetEntities()
        {
            var result = ModuleCombatants.InternalGetEntities();
            if (!result.Any())
                result = BridgeFFXIV.InternalGetEntities();
            return result;
        }

        public static IEnumerable<Entity> GetEntities(bool useOverlay)
        { 
            return useOverlay ? ModuleCombatants.InternalGetEntities() : BridgeFFXIV.InternalGetEntities();
        }

        public static Entity GetEntityByID(string hexID) => GetEntityByID(uint.Parse(hexID, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
        public static Entity GetEntityByID(uint id)
        {
            var result = ModuleCombatants.InternalGetEntityByID(id);
            if (!result.Exist)
                result = BridgeFFXIV.InternalGetEntityByID(id);
            return result;
        }

        public static Entity GetEntityByID(string hexID, bool useOverlay) => GetEntityByID(uint.Parse(hexID, NumberStyles.HexNumber, CultureInfo.InvariantCulture), useOverlay);
        public static Entity GetEntityByID(uint id, bool useOverlay)
        { 
            return useOverlay ? ModuleCombatants.InternalGetEntityByID(id) : BridgeFFXIV.InternalGetEntityByID(id);
        }

        public static Entity GetMyself()
        {
            var result = ModuleCombatants.InternalGetMyself();
            if (!result.Exist)
                result = BridgeFFXIV.InternalGetMyself();
            return result;
        }

        public static Entity GetMyself(bool useOverlay)
        { 
            return useOverlay ? ModuleCombatants.InternalGetMyself() : BridgeFFXIV.InternalGetMyself();
        }

        /// <summary> Cache when changing zone / starting ACT.</summary>
        internal static void UpdateMySnapshot() => MySnapshot = GetMyself();
        public static Entity MySnapshot { get; private set; } = NullEntity();
        public static uint   MyID => MySnapshot.ID;
        public static string MyHexID => MySnapshot.HexID;
        public static string MyName => MySnapshot.Name;
        public static IntPtr MyAddress => MySnapshot.Address;

        #endregion Get Entities

        internal readonly static Dictionary<string, Func<Entity, object>> _propAccessors
            = new Dictionary<string, Func<Entity, object>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Exist",          e => e.Exist },
            { "PluginSource",   e => e.PluginSource },
            { "Address",        e => e.Address },
            { "HexAddress",     e => e.HexAddress },
            { "Name",           e => e.Name },
            { "ID",             e => e.HexID },
            { "IsSelf",         e => e.ID == Entity.GetMyself().ID },
            { "IsPlayer",       e => e.Type == EntityType.Pc },
            { "BNpcID",         e => e.BNpcID },
            { "OwnerID",        e => e.OwnerHexID },
            { "TypeName",       e => e.Type },
            { "Type",           e => (byte)e.Type },
            { "EffectiveDistance",  e => e.EffectiveDistance },
            { "Distance",       e => e.EffectiveDistance },
            { "ObjectStatus",   e => (byte)e.ObjectStatus },
            { "X",              e => e.PosX },
            { "PosX",           e => e.PosX },
            { "Y",              e => e.PosY },
            { "PosY",           e => e.PosY },
            { "Z",              e => e.PosZ },
            { "PosZ",           e => e.PosZ },
            { "XY",             e => new Vector2(e.PosX, e.PosY) },
            { "PosXY",          e => new Vector2(e.PosX, e.PosY) },
            { "XYZ",            e => new Vector3(e.PosX, e.PosY, e.PosZ) },
            { "Pos",            e => new Vector3(e.PosX, e.PosY, e.PosZ) },
            { "H",              e => e.Heading },
            { "Heading",        e => e.Heading },
            { "Radius",         e => e.Radius },
            { "ModelStatus",    e => (int)e.ModelStatus },
            { "IsTargetable",   e => e.IsTargetable },
            { "IsVisible",      e => e.ModelStatus == ModelStatus.Visible },
            { "HP",             e => e.CurrentHP },
            { "CurrentHP",      e => e.CurrentHP },
            { "MaxHP",          e => e.MaxHP },
            { "MP",             e => e.CurrentMP },
            { "CurrentMP",      e => e.CurrentMP },
            { "MaxMP",          e => e.MaxMP },
            { "CP",             e => e.CurrentCP },
            { "CurrentCP",      e => e.CurrentCP },
            { "MaxCP",          e => e.MaxCP },
            { "GP",             e => e.CurrentGP },
            { "CurrentGP",      e => e.CurrentGP },
            { "MaxGP",          e => e.MaxGP },
            { "TransformationID",   e => e.TransformationID },
            { "Level",          e => e.Level },
            { "MonsterType",    e => (byte)e.MonsterType },
            { "IsEnemy",        e => e.IsEnemy },
            { "IsAggressive",   e => e.IsAggressive },
            { "InCombat",       e => e.InCombat },
            { "InParty",        e => e.InParty },
            { "InAlliance",     e => e.InAlliance },
            { "IsFriend",       e => e.IsFriend },
            { "WeaponID",       e => e.WeaponID },
            { "TargetID",       e => e.TargetHexID },
            { "IsTargetingSelf",e => e.TargetID == GetMyself().TargetID },
            { "BNpcNameID",     e => e.BNpcNameID },
            { "CurrentWorldID", e => e.CurrentWorldID },
            { "WorldID",        e => e.WorldID },
            { "HomeWorldID",    e => e.WorldID },
            { "WorldName",      e => (e.Type == EntityType.Pc || e.Type == EntityType.Retainer /* Retainer needs to be tested */)
                                    ? BridgeFFXIV.GetIdEntity(e.HexID).GetValue("worldname").ToString()
                                    : "" },
            { "IsCasting",      e => e.IsCasting },
            { "CastType",       e => e.CastType },
            { "CastID",         e => e.CastID },
            { "CastHexID",      e => e.CastHexID },
            { "CastTargetID",   e => e.IsCasting ? e.CastTargetID.ToString("X") : "0" },
            { "CastX",          e => e.CastPosX },
            { "CastPosX",       e => e.CastPosX },
            { "CastY",          e => e.CastPosY },
            { "CastPosY",       e => e.CastPosY },
            { "CastZ",          e => e.CastPosZ },
            { "CastPosZ",       e => e.CastPosZ },
            { "CastPos",        e => new Vector3(e.CastPosX, e.CastPosY, e.CastPosZ) },
            { "CastTime",       e => e.CastTime },
            { "MaxCastTime",    e => e.MaxCastTime },
            { "Order",          e => 0 },  // Obsolete
            { "StatusIDs",      e => e.Statuses.Select(s => s.StatusID) },
            { "StatusHexIDs",   e => e.Statuses.Select(s => s.StatusHexID) },
            { "StatusCount",    e => e.Statuses.Count },
            { "Marker",         e => Memory.TargetMarkerOnEntity(e.ID) },
            { "MarkerID",       e => (int)Memory.TargetMarkerOnEntity(e.ID) },
        };

        internal readonly static Dictionary<string, Func<Entity, string[], object>> _methodAccessors
            = new Dictionary<string, Func<Entity, string[], object>>(StringComparer.OrdinalIgnoreCase)
            {
                // true if entity has any of the specified status IDs
                ["HasStatus"] = (e, args) => {
                    CheckArgCount(">=1", "HasStatus", args);
                    var ids = args.Select(a => (ushort)MathParser.Parse(a)).ToArray();
                    var currentStatuses = new HashSet<ushort>(e.Statuses.Select(s => s.StatusID));
                    return ids.Any(id => currentStatuses.Contains(id));
                },

                // true if entity has all specified status IDs
                ["HasAllStatus"] = (e, args) => {
                    CheckArgCount(">=1", "HasAllStatus", args);
                    var ids = args.Select(a => (ushort)MathParser.Parse(a)).ToArray();
                    var currentStatuses = new HashSet<ushort>(e.Statuses.Select(s => s.StatusID));
                    return ids.All(id => currentStatuses.Contains(id));
                },

                // remaining timer of status, or default if missing
                ["StatusTimer"] = (e, args) => {
                    CheckArgCount("1-2", "StatusTimer", args);
                    var statusID = (ushort)MathParser.Parse(args[0]);
                    var def = args.Length >= 2 ? MathParser.Parse(args[1]) : -1;
                    return e.Statuses.FirstOrDefault(s => s.StatusID == statusID)?.Timer ?? def;
                },

                // stack count (extraParam) of status, or default if missing
                ["StatusStack"] = (e, args) => {
                    CheckArgCount("1-2", "StatusStack", args);
                    var statusID = (ushort)MathParser.Parse(args[0]);
                    var def = args.Length >= 2 ? MathParser.Parse(args[1]) : -1;
                    return e.Statuses.FirstOrDefault(s => s.StatusID == statusID)?.Stack ?? def;
                },

                ["HasTankStance"] = (e, args) => {
                    CheckArgCount("0", "HasTankStance", args);
                    switch (e.Job.JobType)
                    {
                        case JobEnum.PLD: return e.Statuses.Any(s => s.StatusID == 0x4F);
                        case JobEnum.WAR: return e.Statuses.Any(s => s.StatusID == 0x5B);
                        case JobEnum.DRK: return e.Statuses.Any(s => s.StatusID == 0x2E7);
                        case JobEnum.GNB: return e.Statuses.Any(s => s.StatusID == 0x729);
                        case JobEnum.BLU: return e.Statuses.Any(s => s.StatusID == 0x6B7);
                        default: return false;
                    }
                },

                // XP percentage (current / max)
                ["PercentHP"] = (e, args) => PercentXP("PercentHP", e.CurrentHP, e.MaxHP, args),
                ["PercentMP"] = (e, args) => PercentXP("PercentMP", e.CurrentMP, e.MaxMP, args),
                ["PercentCP"] = (e, args) => PercentXP("PercentCP", e.CurrentCP, e.MaxCP, args),
                ["PercentGP"] = (e, args) => PercentXP("PercentGP", e.CurrentGP, e.MaxGP, args),
            };

        /// <summary>
        /// All "property" names that could be used to query a "property" (with no arguments). <br />
        /// Aliases are included, such as "H" and "Heading" are both for Entity.Heading. <br />
        /// Job-related properties or method names (with arguments) are not included.
        /// </summary>
        internal static readonly HashSet<string> ValidEntityPropNames
            = new HashSet<string>(_propAccessors.Keys, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// All "method" names that could be used to query a "method" (with arguments). <br />
        /// Aliases are included. <br />
        /// </summary>
        internal static readonly HashSet<string> ValidEntityMethodNames
            = new HashSet<string>(_methodAccessors.Keys, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Recommended property names that are used to query all properties when not specified. <br />
        /// Aliases are NOT included.
        /// </summary>
        internal static readonly HashSet<string> RecommendedEntityPropNames
            = new HashSet<string>(ValidEntityPropNames.Except(new string[] {
                "Exist", "PluginSource", "EffectiveDistance",
                "PosX", "PosY", "PosZ", "XY", "PosXY", "XYZ", "Pos",
                "HP", "MP", "CP", "GP",
                "WorldID", "WorldName",
                "CastPosX", "CastPosY", "CastPosZ", "CastPos",
                "Order", "StatusIDs", "Marker", "MarkerID"
            }, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);

        private static string PercentXP(string propName, float current, float max, string[] args)
        {
            CheckArgCount("0-1", propName, args);

            var percentage = max == 0 ? 0f : (100f * current / max);
            var round = args.Length == 0 ? -1 : (int)MathParser.Parse(args[0]);
            return round < 0
                ? percentage.ToString(CultureInfo.InvariantCulture)
                : percentage.ToString("F" + round, CultureInfo.InvariantCulture);
        }

        private static void CheckArgCount(string expectedCount, string methodName, string[] args)
            => ArgHelper.CheckArgCount(expectedCount, args?.Count() ?? 0, methodName);

    }

    #region Enums

    public enum PluginSource
    { 
        None,
        XivPlugin,
        OverlayPlugin
    }

    // FFXIVClientStructs/FFXIVClientStructs/FFXIV/Client/UI/Misc/RaptureTextModule.cs
    public enum EntityType : byte
    {
        None = 0,
        Pc = 1,
        BattleNpc = 2,
        EventNpc = 3,
        Treasure = 4,
        Aetheryte = 5,
        GatheringPoint = 6,
        EventObj = 7,
        Mount = 8,
        Companion = 9,
        Retainer = 10,
        AreaObject = 11,
        HousingEventObject = 12,
        Cutscene = 13,
        MjiObject = 14,
        Ornament = 15,
        CardStand = 16
    }

    public enum MonsterType : byte
    {
        Friendly = 0,
        Enemy = 4,
        Enemy2 = 10, // Observed, temp name
    }

    // OverlayPlugin/OverlayPlugin.Core/MemoryProcessors/Combatant/Common.cs
    public enum ObjectStatus : byte
    {
        NormalActorStatus = 191,
        NormalSubActorStatus = 190,
        TemporarilyUntargetable = 189,
        LoadsUntargetable = 188
    }

    // OverlayPlugin/OverlayPlugin.Core/MemoryProcessors/Combatant/Common.cs
    public enum ModelStatus : int
    {
        Visible = 0,
        Unloaded = 2048,
        Hidden = 16384,
    }

    // OverlayPlugin/OverlayPlugin.Core/MemoryProcessors/Combatant/Common.cs
    [Flags]
    public enum AggressionFlag : byte
    {
        IsAggressive = 0x1,
        IsInCombat = 0x2,
    }
    
    #endregion Enums
}