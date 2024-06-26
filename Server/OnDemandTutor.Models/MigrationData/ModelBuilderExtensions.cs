using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.MigrationData
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // seed blog 

            #region User
     modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                FireBaseid = "firebaseid1", 
                FirstName = "John", 
                LastName = "Doe", 
                Phone = "1234567890", 
                Email = "john.doe@example.com", 
                Address = "123 Main St", 
                AvatarImageUrl = "http://example.com/avatar1.png", 
                Dob = new DateTime(1990, 1, 1), 
                Role = RoleStatus.Admin, 
                Balance = 100.0m, 
                TutorFeePerHour = 50.0m, 
                Rating = 4.5, 
                IdCardImageUrl = "http://example.com/id1.png", 
                ScheduleDesciption = "Available on weekends", 
                Password = "password123", 
                Sex = Sex.Male ,
                IsActive = true
            },
            new User 
            { 
                Id = 2, 
                FireBaseid = "firebaseid2", 
                FirstName = "Jane", 
                LastName = "Smith", 
                Phone = "0987654321", 
                Email = "jane.smith@example.com", 
                Address = "456 Elm St", 
                AvatarImageUrl = "http://example.com/avatar2.png", 
                Dob = new DateTime(1985, 2, 2), 
                Role = RoleStatus.Tutor, 
                Balance = 200.0m, 
                TutorFeePerHour = 60.0m, 
                Rating = 4.7, 
                IdCardImageUrl = "http://example.com/id2.png", 
                ScheduleDesciption = "Available on weekdays", 
                Password = "password123", 
                Sex = Sex.Female, 
                IsActive = true

            },
            // Add three more users
            new User 
            { 
                Id = 3, 
                FireBaseid = "firebaseid3", 
                FirstName = "Alice", 
                LastName = "Johnson", 
                Phone = "1122334455", 
                Email = "alice.johnson@example.com", 
                Address = "789 Pine St", 
                AvatarImageUrl = "http://example.com/avatar3.png", 
                Dob = new DateTime(1995, 3, 3), 
                Role = RoleStatus.Customer, 
                Balance = 300.0m, 
                TutorFeePerHour = 70.0m, 
                Rating = 4.8, 
                IdCardImageUrl = "http://example.com/id3.png", 
                ScheduleDesciption = "Available in the evenings", 
                Password = "password123", 
                Sex = Sex.Female, 
                IsActive = true

            },
            new User 
            { 
                Id = 4, 
                FireBaseid = "firebaseid4", 
                FirstName = "Bob", 
                LastName = "Williams", 
                Phone = "2233445566", 
                Email = "bob.williams@example.com", 
                Address = "101 Maple St", 
                AvatarImageUrl = "http://example.com/avatar4.png", 
                Dob = new DateTime(1975, 4, 4), 
                Role = RoleStatus.Tutor, 
                Balance = 400.0m, 
                TutorFeePerHour = 80.0m, 
                Rating = 4.9, 
                IdCardImageUrl = "http://example.com/id4.png", 
                ScheduleDesciption = "Available on weekends", 
                Password = "password123", 
                Sex = Sex.Male, 
                IsActive = true

            },
            new User 
            { 
                Id = 5, 
                FireBaseid = "firebaseid5", 
                FirstName = "Charlie", 
                LastName = "Brown", 
                Phone = "3344556677", 
                Email = "charlie.brown@example.com", 
                Address = "202 Oak St", 
                AvatarImageUrl = "http://example.com/avatar5.png", 
                Dob = new DateTime(1980, 5, 5), 
                Role = RoleStatus.Customer, 
                Balance = 500.0m, 
                TutorFeePerHour = 90.0m, 
                Rating = 5.0, 
                IdCardImageUrl = "http://example.com/id5.png", 
                ScheduleDesciption = "Available all day", 
                Password = "password123", 
                Sex = Sex.Male ,
                IsActive = false

            }
        );

            

            #endregion
            
            
            #region  Blog
            var createAt = DateTime.Now;
            modelBuilder.Entity<Blog>().HasData(
                  new Blog { Id = 1, Title = "First Blog", Content = "Content of the first blog.", CreateById = 1, CreateAt = createAt },
                  new Blog { Id = 2, Title = "Second Blog", Content = "Content of the second blog.", CreateById = 2, CreateAt = createAt },
                  new Blog { Id = 3, Title = "Third Blog", Content = "Content of the third blog.", CreateById = 1, CreateAt = createAt },
                  new Blog { Id = 4, Title = "Fourth Blog", Content = "Content of the fourth blog.", CreateById = 3, CreateAt = createAt },
                  new Blog { Id = 5, Title = "Fifth Blog", Content = "Content of the fifth blog.", CreateById = 2, CreateAt = createAt }
              );


            modelBuilder.Entity<ConsultationRequest>().HasData(
              new ConsultationRequest
              {
                  Id = 1,
                  Name = "John Doe",
                  Phone = "1234567890",
                  HandleById = 1,
                  ConsultationContent = "Consultation content for request 1.",
                  RequestDate = DateOnly.MinValue.AddDays(1),
                  Status = ConsultationRequestStatus.Proccesing,
                  ReasonFailed = null
              },
              new ConsultationRequest
              {
                  Id = 2,
                  Name = "Jane Smith",
                  Phone = "9876543210",
                  HandleById = 2,
                  ConsultationContent = "Consultation content for request 2.",
                  RequestDate = DateOnly.MinValue.AddDays(2),
                  Status = ConsultationRequestStatus.Completed,
                  ReasonFailed = null
              },
              new ConsultationRequest
              {
                  Id = 3,
                  Name = "Michael Brown",
                  Phone = "5551234567",
                  HandleById = 1,
                  ConsultationContent = "Consultation content for request 3.",
                  RequestDate = DateOnly.MinValue.AddDays(3),
                  Status = ConsultationRequestStatus.Proccesing,
                  ReasonFailed = null
              },
              new ConsultationRequest
              {
                  Id = 4,
                  Name = "Emily Johnson",
                  Phone = "4449876543",
                  HandleById = 2,
                  ConsultationContent = "Consultation content for request 4.",
                  RequestDate = DateOnly.MinValue.AddDays(4),
                  Status = ConsultationRequestStatus.Failed,
                  ReasonFailed = "Unavailable at requested time."
              },
              new ConsultationRequest
              {
                  Id = 5,
                  Name = "David Lee",
                  Phone = "7775551234",
                  HandleById = 1,
                  ConsultationContent = "Consultation content for request 5.",
                  RequestDate = DateOnly.MinValue.AddDays(5),
                  Status = ConsultationRequestStatus.Proccesing,
                  ReasonFailed = null
              }
          );


            #endregion

            #region  FAQ
            modelBuilder.Entity<FAQ>().HasData(
                new FAQ
                {
                    Id = 1,
                    Question = "What is Lorem Ipsum?",
                    Answer = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                    CreateById = 1,
                    CreateAt = createAt
                },
                new FAQ
                {
                    Id = 2,
                    Question = "Why do we use it?",
                    Answer = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.",
                    CreateById = 2,
                    CreateAt = createAt.AddDays(-1)
                },
                new FAQ
                {
                    Id = 3,
                    Question = "Where does it come from?",
                    Answer = "Contrary to popular belief, Lorem Ipsum is not simply random text.",
                    CreateById = 1,
                    CreateAt = createAt.AddDays(-2)
                },
                new FAQ
                {
                    Id = 4,
                    Question = "Where can I get some?",
                    Answer = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form.",
                    CreateById = 2,
                    CreateAt = createAt.AddDays(-3)
                },
                new FAQ
                {
                    Id = 5,
                    Question = "What is the standard Lorem Ipsum passage?",
                    Answer = "The standard Lorem Ipsum passage, used since the 1500s, is reproduced below for those interested.",
                    CreateById = 1,
                    CreateAt = createAt.AddDays(-4)
                }
            );


            #endregion
            
            #region  notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = 1,
                    Content = "New message received",
                    ReceiverId = 1,
                    RefUrl = "/messages/1",
                    RefImageUrl = null,
                    ViewStatus = 0
                },
                new Notification
                {
                    Id = 2,
                    Content = "Meeting reminder",
                    ReceiverId = 2,
                    RefUrl = "/events/5",
                    RefImageUrl = null,
                    ViewStatus = 0
                },
                new Notification
                {
                    Id = 3,
                    Content = "Payment received",
                    ReceiverId = 1,
                    RefUrl = "/payments/123",
                    RefImageUrl = null,
                    ViewStatus = 0
                },
                new Notification
                {
                    Id = 4,
                    Content = "New article published",
                    ReceiverId = 2,
                    RefUrl = "/articles/45",
                    RefImageUrl = "/images/articles/45.jpg",
                    ViewStatus = 0
                },
                new Notification
                {
                    Id = 5,
                    Content = "Account updated",
                    ReceiverId = 1,
                    RefUrl = "/account/settings",
                    RefImageUrl = null,
                    ViewStatus = 0
                }
            );


            #endregion

            #region  Slot
            modelBuilder.Entity<Slot>().HasData(
            new Slot
            {
                Id = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                CreateById = 1,
                TeachAddress = "123 Main St",
                ClassId = 1,
                SubjectId = 1,
                IsOnline = false,
                NumberOfStudents = 5,
                PaymentStatus = PaymentStatus.Paid,
                ActualEndTime = null
            },
            new Slot
            {
                Id = 2,
                StartTime = DateTime.Now.AddHours(3),
                EndTime = DateTime.Now.AddHours(4),
                CreateById = 2,
                TeachAddress = "456 Elm St",
                ClassId = 2,
                SubjectId = 2,
                IsOnline = true,
                NumberOfStudents = 3,
                PaymentStatus = PaymentStatus.Unpaid,
                ActualEndTime = DateTime.Now.AddHours(4).AddMinutes(30)
            },
            new Slot
            {
                Id = 3,
                StartTime = DateTime.Now.AddHours(5),
                EndTime = DateTime.Now.AddHours(6),
                CreateById = 1,
                TeachAddress = "789 Oak St",
                ClassId = 1,
                SubjectId = 3,
                IsOnline = false,
                NumberOfStudents = 7,
                PaymentStatus = PaymentStatus.Paid,
                ActualEndTime = null
            },
            new Slot
            {
                Id = 4,
                StartTime = DateTime.Now.AddHours(7),
                EndTime = DateTime.Now.AddHours(8),
                CreateById = 2,
                TeachAddress = "101 Pine St",
                ClassId = null,
                SubjectId = 4,
                IsOnline = true,
                NumberOfStudents = 2,
                PaymentStatus = PaymentStatus.Paid,
                ActualEndTime = null
            },
            new Slot
            {
                Id = 5,
                StartTime = DateTime.Now.AddHours(9),
                EndTime = DateTime.Now.AddHours(10),
                CreateById = 1,
                TeachAddress = "111 Cedar St",
                ClassId = null,
                SubjectId = 5,
                IsOnline = true,
                NumberOfStudents = 4,
                PaymentStatus = PaymentStatus.Paid,
                ActualEndTime = null
            }
        );


            #endregion

            #region Subject

            modelBuilder.Entity<Subject>().HasData(
                new Subject
                {
                    Id = 1,
                    Name = "Mathematics",
                    SubjectType = "Science",
                    CreateById = 1,
                    Description = "Basic mathematics subject",
                    CreateAt = createAt,
                    Status = true
                },
                new Subject
                {
                    Id = 2,
                    Name = "English",
                    SubjectType = "Language",
                    CreateById = 2,
                    Description = "Language arts subject",
                    CreateAt = createAt.AddDays(-1),
                    Status = true
                },
                new Subject
                {
                    Id = 3,
                    Name = "Physics",
                    SubjectType = "Science",
                    CreateById = 1,
                    Description = "Study of matter and energy",
                    CreateAt = createAt.AddDays(-2),
                    Status = true
                },
                new Subject
                {
                    Id = 4,
                    Name = "History",
                    SubjectType = "Social Science",
                    CreateById = 2,
                    Description = "Study of past events",
                    CreateAt = createAt.AddDays(-3),
                    Status = true
                },
                new Subject
                {
                    Id = 5,
                    Name = "Computer Science",
                    SubjectType = "Science",
                    CreateById = 1,
                    Description = "Study of computers and computational systems",
                    CreateAt = createAt.AddDays(-4),
                    Status = true
                }
            );

            #endregion

            #region Transactions
            modelBuilder.Entity<Transaction>().HasData(
                    new Transaction
                    {
                        Id = 1,
                        TransactionCode = "TRX001",
                        PaymentMethod = "Credit Card",
                        Amount = 100.00m,
                        CreatedDate = DateTime.Now,
                        Status = 1, // Assuming 1 means successful, adjust as per your enum or logic
                        Notes = "Payment for tutoring session",
                        SlotId = 1,
                        CreatedById = 1
                    },
                    new Transaction
                    {
                        Id = 2,
                        TransactionCode = "TRX002",
                        PaymentMethod = "PayPal",
                        Amount = 50.00m,
                        CreatedDate = DateTime.Now.AddDays(-1),
                        Status = 1,
                        Notes = "Payment for online class",
                        SlotId = 2,
                        CreatedById = 2
                    },
                    new Transaction
                    {
                        Id = 3,
                        TransactionCode = "TRX003",
                        PaymentMethod = "Bank Transfer",
                        Amount = 75.00m,
                        CreatedDate = DateTime.Now.AddDays(-2),
                        Status = 1,
                        Notes = "Payment for tutoring session",
                        SlotId = 3,
                        CreatedById = 1
                    },
                    new Transaction
                    {
                        Id = 4,
                        TransactionCode = "TRX004",
                        PaymentMethod = "Credit Card",
                        Amount = 120.00m,
                        CreatedDate = DateTime.Now.AddDays(-3),
                        Status = 1,
                        Notes = "Payment for online class",
                        SlotId = 4,
                        CreatedById = 2
                    },
                    new Transaction
                    {
                        Id = 5,
                        TransactionCode = "TRX005",
                        PaymentMethod = "PayPal",
                        Amount = 90.00m,
                        CreatedDate = DateTime.Now.AddDays(-4),
                        Status = 1,
                        Notes = "Payment for tutoring session",
                        SlotId = 5,
                        CreatedById = 1
                    }
                    );


            #endregion

            #region TutorDegree

            modelBuilder.Entity<TutorDegree>().HasData(
            new TutorDegree
            {
                Id = 1,
                TutorId = 1,
                DegreeImgUrl = "https://example.com/degree1.jpg",
                Description = "Bachelor's in Mathematics",
                SubjectId = 1,
                DegreeNumber = "12345",
                IssuranceDate = new DateOnly(2023, 5, 15),
                TutorSubjectStatus = TutorSubjectDegreeStatus.Approved
            },
            new TutorDegree
            {
                Id = 2,
                TutorId = 2,
                DegreeImgUrl = "https://example.com/degree2.jpg",
                Description = "Master's in English Literature",
                SubjectId = 2,
                DegreeNumber = "54321",
                IssuranceDate = new DateOnly(2022, 9, 30),
                TutorSubjectStatus = TutorSubjectDegreeStatus.Pending
            },
            new TutorDegree
            {
                Id = 3,
                TutorId = 1,
                DegreeImgUrl = "https://example.com/degree3.jpg",
                Description = "PhD in Physics",
                SubjectId = 3,
                DegreeNumber = "98765",
                IssuranceDate = new DateOnly(2024, 2, 10),
                TutorSubjectStatus = TutorSubjectDegreeStatus.Approved
            },
            new TutorDegree
            {
                Id = 4,
                TutorId = 2,
                DegreeImgUrl = "https://example.com/degree4.jpg",
                Description = "Bachelor's in History",
                SubjectId = 4,
                DegreeNumber = "24680",
                IssuranceDate = new DateOnly(2021, 12, 5),
                TutorSubjectStatus = TutorSubjectDegreeStatus.Approved
            },
            new TutorDegree
            {
                Id = 5,
                TutorId = 1,
                DegreeImgUrl = "https://example.com/degree5.jpg",
                Description = "Master's in Computer Science",
                SubjectId = 5,
                DegreeNumber = "13579",
                IssuranceDate = new DateOnly(2023, 8, 20),
                TutorSubjectStatus = TutorSubjectDegreeStatus.Pending
            }
        );

            #endregion

            #region  TutorVideo
            modelBuilder.Entity<TutorVideo>().HasData(
                new TutorVideo
                {
                    Id = 1,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video1.mp4",
                    Description = "Introduction to Mathematics",
                },
                new TutorVideo
                {
                    Id = 2,
                    TutorId = 2,
                    VideoUrl = "https://example.com/video2.mp4",
                    Description = "English Literature Analysis",
                },
                new TutorVideo
                {
                    Id = 3,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video3.mp4",
                    Description = "Physics Fundamentals",
                },
                new TutorVideo
                {
                    Id = 4,
                    TutorId = 2,
                    VideoUrl = "https://example.com/video4.mp4",
                    Description = "Historical Events Overview",
                },
                new TutorVideo
                {
                    Id = 5,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video5.mp4",
                    Description = "Introduction to Programming",
                }
            );


            #endregion

            #region SlotStudent

            modelBuilder.Entity<SlotStudent>().HasData(
                new SlotStudent { SlotId = 1, UserId = 1 },
                new SlotStudent { SlotId = 1, UserId = 2 },
                new SlotStudent { SlotId = 2, UserId = 3 },
                new SlotStudent { SlotId = 2, UserId = 4 },
                new SlotStudent { SlotId = 3, UserId = 1 }
                // Add more as needed
            );

            #endregion

            #region Class

            modelBuilder.Entity<Class>().HasData(
                new Class
                {
                    Id = 1,
                    Name = "Mathematics 101",
                    TutorId = 1, // Replace with an existing TutorId
                    SubjectId = 1, // Replace with an existing SubjectId
                    StudentName = "John Doe",
                    SlotId = 1, // Replace with an existing SlotId
                },
                new Class
                {
                    Id = 2,
                    Name = "Literature 201",
                    TutorId = 2, // Replace with an existing TutorId
                    SubjectId = 2, // Replace with an existing SubjectId
                    StudentName = "Jane Smith",
                    SlotId = 2, // Replace with an existing SlotId
                },
                new Class
                {
                    Id = 3,
                    Name = "Physics Lab",
                    TutorId = 1, // Replace with an existing TutorId
                    SubjectId = 3, // Replace with an existing SubjectId
                    StudentName = "Alice Johnson",
                    SlotId = 3, // Replace with an existing SlotId
                },
                new Class
                {
                    Id = 4,
                    Name = "History Class",
                    TutorId = 3, // Replace with an existing TutorId
                    SubjectId = 4, // Replace with an existing SubjectId
                    StudentName = "Michael Brown",
                    SlotId = 4, // Replace with an existing SlotId
                },
                new Class
                {
                    Id = 5,
                    Name = "Chemistry 301",
                    TutorId = 2, // Replace with an existing TutorId
                    SubjectId = 5, // Replace with an existing SubjectId
                    StudentName = "Emily Davis",
                    SlotId = 5, // Replace with an existing SlotId
                }
            );

            #endregion
            
            #region StudentClass

            modelBuilder.Entity<StudentClass>().HasData(
                new StudentClass { StudentId = 1, ClassId = 1 },
                new StudentClass { StudentId = 2, ClassId = 1 },
                new StudentClass { StudentId = 3, ClassId = 2 },
                new StudentClass { StudentId = 4, ClassId = 2 },
                new StudentClass { StudentId = 5, ClassId = 3 }
                // Add more as needed
            );

            #endregion

            #region TutorSubject
            modelBuilder.Entity<TutorSubject>().HasData(
                new TutorSubject { UserId = 1, SubjectId = 1 },
                new TutorSubject { UserId = 1, SubjectId = 2 },
                new TutorSubject { UserId = 2, SubjectId = 3 },
                new TutorSubject { UserId = 3, SubjectId = 1 },
                new TutorSubject { UserId = 3, SubjectId = 4 }
                // Add more as needed
            );


            #endregion

            #region EmailTemplate
            modelBuilder.Entity<EmailTemplate>().HasData(

              new EmailTemplate
              {
                  Id = 1,
                  Name = "Welcome_Email",
                  Status = true,
                  Body = "Welcome to OnDemandTutor! Dear {Name}, thank you for joining us.",
                  Params = "{Name}",
                  Subject = "Welcome to OnDemandTutor!",
                  Description = "This email is sent to welcome new users."
              },
            new EmailTemplate
            {
                Id = 2,
                Name = "Reminder_Email",
                Status = true,
                Body = "Hello {Name}, this is a reminder for your upcoming class on {Date}.",
                Params = "{Name}, {Date}",
                Subject = "Reminder for Your Class",
                Description = "This email is sent as a reminder for scheduled classes."
            },
            new EmailTemplate
            {
                Id = 3,
                Name = "Payment_Confirmation",
                Status = true,
                Body = "Dear {Name}, your payment of {Amount} has been confirmed.",
                Params = "{Name}, {Amount}",
                Subject = "Payment Confirmation",
                Description = "This email confirms the successful payment for services."
            },
            new EmailTemplate
            {
                Id = 4,
                Name = "Feedback_Request",
                Status = true,
                Body = "Hi {Name}, we would love to hear your feedback about our services.",
                Params = "{Name}",
                Subject = "Feedback Request",
                Description = "This email requests feedback from users about their experience."
            },
            new EmailTemplate
            {
                Id = 5,
                Name = "Account_Activation",
                Status = true,
                Body = "Dear {Name}, please click the link to activate your account: {ActivationLink}.",
                Params = "{Name}, {ActivationLink}",
                Subject = "Account Activation",
                Description = "This email contains instructions to activate user accounts."
            }
            // Add more email templates as needed
        );
            #endregion


        }
    }
}
