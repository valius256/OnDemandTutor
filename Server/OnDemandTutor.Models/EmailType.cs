namespace OnDemandTutor.Models;

public static class EmailType
{
    public const string Welcome_Email = "Welcome_Email"; //     Params = "{Name}",
    public const string Reminder_Email = "Reminder_Email";
    public const string Payment_Confirmation = "Payment_Confirmation";
    public const string Feedback_Request = "Feedback_Request";
    public const string Account_Activation = "Tutor_Registration_Approval";
    public const string Tutor_Registration_Approval = "TutorRegistrationApproval";
    public const string Request_Withdraw_Notification = "Request_Withdraw_Notification"; // [UserName],[Amount],[BankAccountNumber],[BankName],[Reason]
    public const string WithDraw_Approval_Notification = "WithDraw_Approval_Notification"; // [UserName], [Status], [Reply]
    public const string Slot_Payment_Reminder = "Slot_Payment_Reminder";  // [Name] [ClassId]
    public const string Remove_Unpaid_Slots = "Remove_Unpaid_Slots";   // [Name]  [ClassId]
}