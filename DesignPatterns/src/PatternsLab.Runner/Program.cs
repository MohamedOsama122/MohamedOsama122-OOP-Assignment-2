using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;
var db = new DatabaseService(); var ui = new UiService(); db.Config.Theme = "Dark"; ui.Render(); Console.WriteLine(ReferenceEquals(db.Config, ui.Config)); Console.WriteLine(AppConfig.LoadCount);
Enemy original = new Orc(); Enemy copy = original.Clone(); copy.Weapon.Damage = 999; copy.Abilities.Add("Fire Breath"); Console.WriteLine($"{original.ModelId} {copy.ModelId} originalDamage={original.Weapon.Damage}");
Console.WriteLine(RegistrationCallSites.CreateLiveStudentUgly()); Console.WriteLine(RegistrationCallSites.CreateVideosOnlyUgly());
