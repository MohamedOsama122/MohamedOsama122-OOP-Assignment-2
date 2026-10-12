using LibrarySystem;

// new Person(...); new Member(...); new Staff(...); new LibraryItem(...); // must NOT compile: protected constructors
var student = new StudentMember("M-1", "Ali", "0100");
// student.FullName = "Other"; student.Loans.Add(null); // must NOT compile
var librarian = new Librarian("S-1", "Nora", "0101", new DateOnly(2020, 1, 1), 5000m);
var head = new HeadLibrarian("S-2", "Hany", "0102", new DateOnly(2019, 1, 1), 7000m);
var shelver = new Shelver("S-3", "Mona", "0103", new DateOnly(2021, 1, 1), 4000m, "Fiction");
foreach (var staff in new List<Staff> { librarian, head, shelver }) Console.WriteLine($"{staff.FullName} pay={staff.GetMonthlyPay():0.00}");
var book = new Book("B-1", "Clean Code", 2m); var dvd = new Dvd("D-1", "Film", 3m); var magazine = new Magazine("G-1", "Science", 1m);
foreach (var item in new List<LibraryItem> { book, dvd, magazine }) Console.WriteLine($"{item.Title}: {item.GetLoanPeriodDays()} days, fee={item.GetDailyLateFee():0.00}");
head.WithdrawItem(book); try { student.BorrowItem("L-X", book, new DateOnly(2026, 1, 1)); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); } head.RestoreItem(book);
var l1 = student.BorrowItem("L-1", book, new DateOnly(2026, 1, 1)); try { student.BorrowItem("L-2", book, new DateOnly(2026, 1, 1)); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
student.BorrowItem("L-2", new Book("B-2", "Two", 1m), new DateOnly(2026, 1, 1)); student.BorrowItem("L-3", new Book("B-3", "Three", 1m), new DateOnly(2026, 1, 1)); try { student.BorrowItem("L-4", new Book("B-4", "Four", 1m), new DateOnly(2026, 1, 1)); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
librarian.ProcessReturn(l1, new DateOnly(2026, 1, 2));
var premium = new PremiumMember("M-2", "Sara", "0104", 10m); var premiumLoan = premium.BorrowItem("L-P", dvd, new DateOnly(2026, 2, 1)); librarian.ProcessReturn(premiumLoan, new DateOnly(2026, 2, 13)); Console.WriteLine($"due={premiumLoan.DueDate}, late fee={premiumLoan.LateFee:0.00}, points={premium.ReadingPoints}");
try { librarian.ProcessReturn(premiumLoan, new DateOnly(2026, 2, 14)); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); } try { librarian.MarkItemLost(premiumLoan); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
var lostItem = new Book("B-L", "Lost", 1m); var lostLoan = premium.BorrowItem("L-L", lostItem, new DateOnly(2026, 3, 1)); librarian.MarkItemLost(lostLoan); try { student.BorrowItem("L-AfterLost", lostItem, new DateOnly(2026, 3, 2)); } catch (Exception ex) { Console.WriteLine("Rejected: " + ex.Message); }
