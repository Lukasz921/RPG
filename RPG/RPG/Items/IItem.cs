using RPG.Attacks;
using RPG.Decorators;
using RPG.Defenses;
using RPG.Players;
using System.Text.Json.Serialization;

namespace RPG.Items
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Currency), "Currency")]
    [JsonDerivedType(typeof(LightWeapon), "LightWeapon")]
    [JsonDerivedType(typeof(HeavyWeapon), "HeavyWeapon")]
    [JsonDerivedType(typeof(MagicWeapon), "MagicWeapon")]
    [JsonDerivedType(typeof(Potion), "Potion")]
    [JsonDerivedType(typeof(Unusable), "Unusable")]
    [JsonDerivedType(typeof(StrongEffect), "StrongEffect")]
    [JsonDerivedType(typeof(WeakEffect), "WeakEffect")]
    [JsonDerivedType(typeof(CursedEffect), "CursedEffect")]
    internal interface IItem
    {
        string RawName { get; set; }
        string Name { get; set; }
        char Symbol { get; set; }
        int Damage {  get; set; }
        bool IsTwoHanded { get; set; }
        void Use(List<PlayerEffect> effects, PlayerStats stats);
        bool IsUsable();
        bool ApplyOnPickUp(PlayerStats stats);
        bool DeApplyOnThrow(PlayerStats stats);
        void ApplyOnPlayer(PlayerStats stats);
        void DeApplyOnPlayer(PlayerStats stats);
        int Accept(IAttack attack);
        int Accept(IDefense defense, PlayerStats stats);
    }
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(HeavyWeapon), "HeavyWeapon")]
    [JsonDerivedType(typeof(LightWeapon), "LightWeapon")]
    [JsonDerivedType(typeof(MagicWeapon), "MagicWeapon")]
    [JsonDerivedType(typeof(CursedEffect), "CursedEffect")]
    internal interface IWeapon : IItem { }
    internal interface ICurrency : IItem
    {
        int Value { get; set; }
    }
    internal interface IPotion : IItem { }
    internal interface IUnusable : IItem { }
}