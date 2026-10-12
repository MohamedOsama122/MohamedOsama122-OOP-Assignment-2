using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;
var db = new DatabaseService(); var ui = new UiService(); db.Config.Theme = "Dark"; ui.Render(); Console.WriteLine(ReferenceEquals(db.Config, ui.Config)); Console.WriteLine(AppConfig.LoadCount);
Parallel.For(0, 20, index => { _ = AppConfig.Instance; }); Console.WriteLine($"Concurrent singleton loads: {AppConfig.LoadCount}");
Enemy original = new Orc(); Enemy copy = original.Clone(); copy.Weapon.Damage = 999; copy.Abilities.Add("Fire Breath"); Console.WriteLine($"{original.ModelId} {copy.ModelId} originalDamage={original.Weapon.Damage} originalAbilities={string.Join(',', original.Abilities)}");
Console.WriteLine(RegistrationCallSites.CreateLiveStudentUgly()); Console.WriteLine(RegistrationCallSites.CreateVideosOnlyUgly());
try { CourseRegistration.For("bad@mail.com", "SEF").LiveGroup("").Build(); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
try { CourseRegistration.For("bad@mail.com", "SEF").VideosOnly().WithGroupCode("G1").Build(); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
