namespace LibrarySystem;

public enum LoanStatus { Borrowed, Returned, Lost }

public class Person
{
    public string PersonId { get; }
    public string FullName { get; }
    public string Phone { get; }
    protected Person(string personId, string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(personId) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Person ID, full name, and phone are required.");
        PersonId = personId; FullName = fullName; Phone = phone;
    }
}

public class Member : Person
{
    private readonly List<Loan> _loans = new();
    public IReadOnlyList<Loan> Loans { get { return _loans.AsReadOnly(); } }
    protected int MaxActiveLoans { get; }
    protected decimal LateFeeDiscountPercent { get; }
    protected Member(string id, string name, string phone, int maxActiveLoans, decimal discount) : base(id, name, phone)
    { MaxActiveLoans = maxActiveLoans; LateFeeDiscountPercent = discount; }
    public Loan BorrowItem(string loanId, LibraryItem item, DateOnly borrowDate)
    {
        if (item.IsWithdrawn) throw new InvalidOperationException("Withdrawn items cannot be borrowed.");
        if (item.IsOnLoan) throw new InvalidOperationException("This item is already on loan.");
        var active = 0; foreach (var loan in _loans) if (loan.Status == LoanStatus.Borrowed) active++;
        if (active >= MaxActiveLoans) throw new InvalidOperationException("Member has reached the active loan limit.");
        var loanCreated = new Loan(loanId, borrowDate, this, item); _loans.Add(loanCreated); item.MarkOnLoan(); return loanCreated;
    }
    internal decimal ApplyDiscount(decimal fee) { return Math.Max(0m, fee * (1m - LateFeeDiscountPercent / 100m)); }
}
public sealed class StudentMember : Member { public StudentMember(string id, string name, string phone) : base(id, name, phone, 3, 0m) { } }
public sealed class PremiumMember : Member
{
    public int ReadingPoints { get { var returned = 0; foreach (var loan in Loans) if (loan.Status == LoanStatus.Returned) returned++; return returned * 5; } }
    public PremiumMember(string id, string name, string phone, decimal discountPercent) : base(id, name, phone, 10, ValidateDiscount(discountPercent)) { }
    private static decimal ValidateDiscount(decimal value) { if (value < 0m || value > 100m) throw new ArgumentOutOfRangeException(nameof(value), "Discount must be between 0 and 100."); return value; }
}

public class Staff : Person
{
    public DateOnly HireDate { get; }
    public decimal MonthlySalary { get; private set; }
    protected decimal ResponsibilityAllowance { get; }
    protected Staff(string id, string name, string phone, DateOnly hireDate, decimal salary, decimal allowance) : base(id, name, phone)
    { if (salary <= 0m) throw new ArgumentOutOfRangeException(nameof(salary), "Salary must be positive."); HireDate = hireDate; MonthlySalary = salary; ResponsibilityAllowance = allowance; }
    public void GiveRaise(decimal percentage) { if (percentage <= 0m) throw new ArgumentOutOfRangeException(nameof(percentage), "Raise percentage must be positive."); MonthlySalary += MonthlySalary * percentage / 100m; }
    public decimal GetMonthlyPay() { return MonthlySalary + ResponsibilityAllowance; }
}
public sealed class Librarian : Staff
{ public Librarian(string id, string name, string phone, DateOnly date, decimal salary) : base(id, name, phone, date, salary, 0m) { } public void ProcessReturn(Loan loan, DateOnly date) { loan.Return(date); } public void MarkItemLost(Loan loan) { loan.MarkLost(); } }
public sealed class Shelver : Staff
{ public string Section { get; private set; } public Shelver(string id, string name, string phone, DateOnly date, decimal salary, string section) : base(id, name, phone, date, salary, 0m) { if (string.IsNullOrWhiteSpace(section)) throw new ArgumentException("Section is required."); Section = section; } public void Reassign(string section) { if (string.IsNullOrWhiteSpace(section)) throw new ArgumentException("Section is required."); Section = section; } }
public sealed class HeadLibrarian : Staff
{ public HeadLibrarian(string id, string name, string phone, DateOnly date, decimal salary) : base(id, name, phone, date, salary, 400m) { } public void ChangeLateFee(LibraryItem item, decimal fee) { item.ChangeBaseLateFee(fee); } public void WithdrawItem(LibraryItem item) { item.Withdraw(); } public void RestoreItem(LibraryItem item) { item.Restore(); } }

public class LibraryItem
{
    public string CatalogNumber { get; } public string Title { get; } public decimal BaseLateFee { get; private set; } public bool IsWithdrawn { get; private set; } public bool IsOnLoan { get; internal set; }
    protected int LoanPeriodDays { get; } protected decimal LateFeeMultiplier { get; }
    protected LibraryItem(string catalogNumber, string title, decimal baseLateFee, int period, decimal multiplier)
    { if (string.IsNullOrWhiteSpace(catalogNumber) || string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Catalog number and title are required."); if (baseLateFee <= 0m) throw new ArgumentOutOfRangeException(nameof(baseLateFee), "Late fee must be positive."); CatalogNumber = catalogNumber; Title = title; BaseLateFee = baseLateFee; LoanPeriodDays = period; LateFeeMultiplier = multiplier; }
    public int GetLoanPeriodDays() { return LoanPeriodDays; } public decimal GetDailyLateFee() { return BaseLateFee * LateFeeMultiplier; }
    public void ChangeBaseLateFee(decimal fee) { if (fee <= 0m) throw new ArgumentOutOfRangeException(nameof(fee), "Late fee must be positive."); BaseLateFee = fee; }
    public void Withdraw() { IsWithdrawn = true; } public void Restore() { IsWithdrawn = false; } internal void MarkOnLoan() { IsOnLoan = true; } internal void MarkAvailable() { IsOnLoan = false; }
}
public sealed class Book : LibraryItem { public Book(string no, string title, decimal fee) : base(no, title, fee, 21, 1m) { } }
public sealed class Dvd : LibraryItem { public Dvd(string no, string title, decimal fee) : base(no, title, fee, 7, 2m) { } }
public sealed class Magazine : LibraryItem { public Magazine(string no, string title, decimal fee) : base(no, title, fee, 3, .5m) { } }

public sealed class Loan
{
    public string LoanId { get; } public DateOnly BorrowDate { get; } public Member Member { get; } public LibraryItem Item { get; } public LoanStatus Status { get; private set; } public DateOnly? ReturnDate { get; private set; }
    public DateOnly DueDate { get { return BorrowDate.AddDays(Item.GetLoanPeriodDays()); } }
    public decimal LateFee { get { if (Status != LoanStatus.Returned || ReturnDate is null || ReturnDate.Value <= DueDate) return 0m; return Member.ApplyDiscount((ReturnDate.Value.DayNumber - DueDate.DayNumber) * Item.GetDailyLateFee()); } }
    internal Loan(string id, DateOnly date, Member member, LibraryItem item) { if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Loan ID is required."); LoanId = id; BorrowDate = date; Member = member; Item = item; Status = LoanStatus.Borrowed; }
    internal void Return(DateOnly date) { if (Status != LoanStatus.Borrowed) throw new InvalidOperationException("Only a borrowed loan can be returned."); if (date < BorrowDate) throw new ArgumentException("Return date cannot be earlier than borrow date."); ReturnDate = date; Status = LoanStatus.Returned; Item.MarkAvailable(); }
    internal void MarkLost() { if (Status != LoanStatus.Borrowed) throw new InvalidOperationException("Only a borrowed loan can be marked lost."); Status = LoanStatus.Lost; Item.MarkAvailable(); }
}
