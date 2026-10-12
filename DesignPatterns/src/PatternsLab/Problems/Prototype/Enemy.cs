namespace PatternsLab.Problems.Prototype;
public class Weapon { public string Name { get; set; } = ""; public int Damage { get; set; } public Weapon Copy() { return new Weapon { Name = Name, Damage = Damage }; } }
public abstract class Enemy
{
    private string _modelData; public string Name { get; set; } = ""; public int Health { get; set; } public Weapon Weapon { get; set; } = new Weapon(); public List<string> Abilities { get; set; } = new(); public string ModelId { get { return _modelData; } }
    protected Enemy() { Console.WriteLine("   ...loading 3D model (slow)..."); Thread.Sleep(500); _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6]; }
    protected Enemy(Enemy source) { _modelData = source._modelData; Name = source.Name; Health = source.Health; Weapon = source.Weapon.Copy(); Abilities = new List<string>(source.Abilities); }
    public abstract Enemy Clone();
}
public sealed class Orc : Enemy { public Orc() { Name="Orc"; Health=100; Weapon=new Weapon { Name="Axe", Damage=25 }; Abilities.Add("Rage"); } private Orc(Orc source) : base(source) { } public override Enemy Clone() { return new Orc(this); } }
public sealed class Elf : Enemy { public Elf() { Name="Elf"; Health=70; Weapon=new Weapon { Name="Bow", Damage=18 }; Abilities.Add("Stealth"); } private Elf(Elf source) : base(source) { } public override Enemy Clone() { return new Elf(this); } }
public static class EnemyCopyHelper { public static Enemy CopyEnemy(Enemy enemy) { return enemy.Clone(); } }
