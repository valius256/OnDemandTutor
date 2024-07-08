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
                    FirstName = "Nguyễn",
                    LastName = "Văn A",
                    Phone = "0123456789",
                    Email = "nguyenvana@example.com",
                    Address = "123 Đường Chính",
                    AvatarImageUrl = "http://example.com/avatar1.png",
                    Dob = new DateTime(1990, 1, 1),
                    Role = RoleStatus.Admin,
                    Balance = 100.0m,
                    TutorFeePerHour = 50.0m,
                    Rating = 4.5,
                    IdCardImageUrl = "http://example.com/id1.png",
                    ScheduleDesciption = "Có mặt vào cuối tuần",
                    Password = "matkhau123",
                    Sex = Sex.Male,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    FireBaseid = "firebaseid2",
                    FirstName = "Trần",
                    LastName = "Thị B",
                    Phone = "0987654321",
                    Email = "tranthib@example.com",
                    Address = "456 Đường Phụ",
                    AvatarImageUrl = "http://example.com/avatar2.png",
                    Dob = new DateTime(1985, 2, 2),
                    Role = RoleStatus.Tutor,
                    Balance = 200.0m,
                    TutorFeePerHour = 60.0m,
                    Rating = 4.7,
                    IdCardImageUrl = "http://example.com/id2.png",
                    ScheduleDesciption = "Có mặt vào các ngày trong tuần",
                    Password = "matkhau123",
                    Sex = Sex.Female,
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    FireBaseid = "firebaseid3",
                    FirstName = "Lê",
                    LastName = "Văn C",
                    Phone = "0122334455",
                    Email = "levanc@example.com",
                    Address = "789 Đường Thông",
                    AvatarImageUrl = "http://example.com/avatar3.png",
                    Dob = new DateTime(1995, 3, 3),
                    Role = RoleStatus.Customer,
                    Balance = 300.0m,
                    TutorFeePerHour = 70.0m,
                    Rating = 4.8,
                    IdCardImageUrl = "http://example.com/id3.png",
                    ScheduleDesciption = "Có mặt vào buổi tối",
                    Password = "matkhau123",
                    Sex = Sex.Female,
                    IsActive = true
                },
                new User
                {
                    Id = 4,
                    FireBaseid = "firebaseid4",
                    FirstName = "Phạm",
                    LastName = "Văn D",
                    Phone = "0233445566",
                    Email = "phamvand@example.com",
                    Address = "101 Đường Phong",
                    AvatarImageUrl = "http://example.com/avatar4.png",
                    Dob = new DateTime(1975, 4, 4),
                    Role = RoleStatus.Tutor,
                    Balance = 400.0m,
                    TutorFeePerHour = 80.0m,
                    Rating = 4.9,
                    IdCardImageUrl = "http://example.com/id4.png",
                    ScheduleDesciption = "Có mặt vào cuối tuần",
                    Password = "matkhau123",
                    Sex = Sex.Male,
                    IsActive = true
                },
                new User
                {
                    Id = 5,
                    FireBaseid = "firebaseid5",
                    FirstName = "Đặng",
                    LastName = "Văn E",
                    Phone = "0344556677",
                    Email = "dangvane@example.com",
                    Address = "202 Đường Sồi",
                    AvatarImageUrl = "http://example.com/avatar5.png",
                    Dob = new DateTime(1980, 5, 5),
                    Role = RoleStatus.Customer,
                    Balance = 500.0m,
                    TutorFeePerHour = 90.0m,
                    Rating = 5.0,
                    IdCardImageUrl = "http://example.com/id5.png",
                    ScheduleDesciption = "Có mặt cả ngày",
                    Password = "matkhau123",
                    Sex = Sex.Male,
                    IsActive = false
                }
            );
            #endregion


            #region  Blog
            var createAt = DateTime.Now;
            modelBuilder.Entity<Blog>().HasData(
                new Blog { Id = 1, Title = "Bài Blog Đầu Tiên", Content = "Nội dung của bài blog đầu tiên.", CreateById = 1, CreateAt = createAt },
                new Blog { Id = 2, Title = "Bài Blog Thứ Hai", Content = "Nội dung của bài blog thứ hai.", CreateById = 2, CreateAt = createAt },
                new Blog { Id = 3, Title = "Bài Blog Thứ Ba", Content = "Nội dung của bài blog thứ ba.", CreateById = 1, CreateAt = createAt },
                new Blog { Id = 4, Title = "Bài Blog Thứ Tư", Content = "Nội dung của bài blog thứ tư.", CreateById = 3, CreateAt = createAt },
                new Blog { Id = 5, Title = "Bài Blog Thứ Năm", Content = "Nội dung của bài blog thứ năm.", CreateById = 2, CreateAt = createAt }
              );
            #endregion


            #region ConsultationRequets 
            modelBuilder.Entity<ConsultationRequest>().HasData(
                new ConsultationRequest
                {
                    Id = 1,
                    Name = "Nguyễn Văn A",
                    Phone = "0123456789",
                    HandleById = 1,
                    ConsultationContent = "Nội dung tư vấn cho yêu cầu 1.",
                    RequestDate = DateTime.UtcNow.Date,
                    Status = ConsultationRequestStatus.Pending,
                },
                new ConsultationRequest
                {
                    Id = 2,
                    Name = "Trần Thị B",
                    Phone = "0987654321",
                    HandleById = 2,
                    ConsultationContent = "Nội dung tư vấn cho yêu cầu 2.",
                    RequestDate = DateTime.Today,
                    Status = ConsultationRequestStatus.Solved,
                },
                new ConsultationRequest
                {
                    Id = 3,
                    Name = "Lê Văn C",
                    Phone = "0122334455",
                    HandleById = 1,
                    ConsultationContent = "Nội dung tư vấn cho yêu cầu 3.",
                    RequestDate = DateTime.UtcNow.AddDays(-3),
                    Status = ConsultationRequestStatus.Pending,

                },
                new ConsultationRequest
                {
                    Id = 4,
                    Name = "Phạm Thị D",
                    Phone = "0233445566",
                    HandleById = 2,
                    ConsultationContent = "Nội dung tư vấn cho yêu cầu 4.",
                    RequestDate = DateTime.UtcNow.AddDays(-5),
                    Status = ConsultationRequestStatus.Solved,
                },
                new ConsultationRequest
                {
                    Id = 5,
                    Name = "Đặng Văn E",
                    Phone = "0344556677",
                    HandleById = 1,
                    ConsultationContent = "Nội dung tư vấn cho yêu cầu 5.",
                    RequestDate = DateTime.UtcNow.AddDays(-7),
                    Status = ConsultationRequestStatus.Pending,
                }
              );


            #endregion

            #region  FAQ
            modelBuilder.Entity<FAQ>().HasData(
                new FAQ
                {
                    Id = 1,
                    Question = "Lorem Ipsum là gì?",
                    Answer = "Lorem Ipsum chỉ đơn giản là văn bản giả của ngành in ấn và sắp chữ.",
                    CreateById = 1,
                    CreateAt = createAt
                },
                new FAQ
                {
                    Id = 2,
                    Question = "Tại sao chúng ta sử dụng nó?",
                    Answer = "Nó là một sự thật lâu đời rằng một người đọc sẽ bị phân tâm bởi nội dung dễ đọc của một trang khi nhìn vào bố cục của nó.",
                    CreateById = 2,
                    CreateAt = createAt.AddDays(-1)
                },
                new FAQ
                {
                    Id = 3,
                    Question = "Nó xuất phát từ đâu?",
                    Answer = "Trái với quan niệm phổ biến, Lorem Ipsum không chỉ là văn bản ngẫu nhiên.",
                    CreateById = 1,
                    CreateAt = createAt.AddDays(-2)
                },
                new FAQ
                {
                    Id = 4,
                    Question = "Tôi có thể lấy nó ở đâu?",
                    Answer = "Có nhiều biến thể của các đoạn văn Lorem Ipsum có sẵn, nhưng phần lớn đã bị thay đổi dưới một số hình thức.",
                    CreateById = 2,
                    CreateAt = createAt.AddDays(-3)
                },
                new FAQ
                {
                    Id = 5,
                    Question = "Đoạn văn Lorem Ipsum chuẩn là gì?",
                    Answer = "Đoạn văn Lorem Ipsum chuẩn, được sử dụng từ những năm 1500, được tái tạo dưới đây cho những ai quan tâm.",
                    CreateById = 1,
                    CreateAt = createAt.AddDays(-4)
                }
            );


            #endregion

            #region Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = 1,
                    Content = "Nhận được tin nhắn mới",
                    ReceiverId = 1,
                    RefUrl = "/messages/1",
                    RefImageUrl = null,
                    IsViewed = true
                },
                new Notification
                {
                    Id = 2,
                    Content = "Nhắc nhở cuộc họp",
                    ReceiverId = 2,
                    RefUrl = "/events/5",
                    RefImageUrl = null,
                    IsViewed = true
                },
                new Notification
                {
                    Id = 3,
                    Content = "Đã nhận được thanh toán",
                    ReceiverId = 1,
                    RefUrl = "/payments/123",
                    RefImageUrl = null,
                    IsViewed = true
                },
                new Notification
                {
                    Id = 4,
                    Content = "Bài viết mới được xuất bản",
                    ReceiverId = 2,
                    RefUrl = "/articles/45",
                    RefImageUrl = "/images/articles/45.jpg",
                    IsViewed = false
                },
                new Notification
                {
                    Id = 5,
                    Content = "Cập nhật tài khoản",
                    ReceiverId = 1,
                    RefUrl = "/account/settings",
                    RefImageUrl = null,
                    IsViewed = false
                }
            );
            #endregion


            #region Slot
            modelBuilder.Entity<Slot>().HasData(
                new Slot
                {
                    Id = 1,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2),
                    CreateById = 1,
                    TeachAddress = "123 Đường Chính",
                    ClassId = 1,
                    SubjectId = 1,
                    IsOnline = false,
                    NumberOfStudents = 5,
                    ActualEndTime = DateTime.Today
                },
                new Slot
                {
                    Id = 2,
                    StartTime = DateTime.Now.AddHours(3),
                    EndTime = DateTime.Now.AddHours(4),
                    CreateById = 2,
                    TeachAddress = "456 Đường Elm",
                    ClassId = 2,
                    SubjectId = 2,
                    IsOnline = true,
                    NumberOfStudents = 3,
                    ActualEndTime = DateTime.Now.AddHours(4).AddMinutes(30)
                },
                new Slot
                {
                    Id = 3,
                    StartTime = DateTime.Now.AddHours(5),
                    EndTime = DateTime.Now.AddHours(6),
                    CreateById = 1,
                    TeachAddress = "789 Đường Oak",
                    ClassId = 1,
                    SubjectId = 3,
                    IsOnline = false,
                    NumberOfStudents = 7,
                    ActualEndTime = DateTime.Today
                },
                new Slot
                {
                    Id = 4,
                    StartTime = DateTime.Now.AddHours(7),
                    EndTime = DateTime.Now.AddHours(8),
                    CreateById = 2,
                    TeachAddress = "101 Đường Pine",
                    ClassId = null,
                    SubjectId = 4,
                    IsOnline = true,
                    NumberOfStudents = 2,
                    ActualEndTime = DateTime.Today
                },
                new Slot
                {
                    Id = 5,
                    StartTime = DateTime.Now.AddHours(9),
                    EndTime = DateTime.Now.AddHours(10),
                    CreateById = 1,
                    TeachAddress = "111 Đường Cedar",
                    ClassId = null,
                    SubjectId = 5,
                    IsOnline = true,
                    NumberOfStudents = 4,
                    ActualEndTime = DateTime.Today
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
                    IsEnable = true,
                },
                new Subject
                {
                    Id = 2,
                    Name = "English",
                    SubjectType = "Language",
                    CreateById = 2,
                    Description = "Language arts subject",
                    CreateAt = createAt.AddDays(-1),
                    IsEnable = true,
                },
                new Subject
                {
                    Id = 3,
                    Name = "Physics",
                    SubjectType = "Science",
                    CreateById = 1,
                    Description = "Study of matter and energy",
                    CreateAt = createAt.AddDays(-2),
                    IsEnable = true,
                },
                new Subject
                {
                    Id = 4,
                    Name = "History",
                    SubjectType = "Social Science",
                    CreateById = 2,
                    Description = "Study of past events",
                    CreateAt = createAt.AddDays(-3),
                    IsEnable = true,
                },
                new Subject
                {
                    Id = 5,
                    Name = "Computer Science",
                    SubjectType = "Science",
                    CreateById = 1,
                    Description = "Study of computers and computational systems",
                    CreateAt = createAt.AddDays(-4),
                    IsEnable = false,
                }
            );

            #endregion

            #region Transactions

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    TransactionCode = "GDT001",
                    PaymentMethod = "Thẻ tín dụng",
                    Amount = 100.00m,
                    CreatedDate = DateTime.Now,
                    Status = PaymentStatus.Paid,
                    Notes = "Thanh toán cho buổi học gia sư",
                    SlotId = 1,
                    CreatedById = 1
                },
                new Transaction
                {
                    Id = 2,
                    TransactionCode = "GDT002",
                    PaymentMethod = "PayPal",
                    Amount = 50.00m,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Status = PaymentStatus.Paid,
                    Notes = "Thanh toán cho lớp học trực tuyến",
                    SlotId = 2,
                    CreatedById = 2
                },
                new Transaction
                {
                    Id = 3,
                    TransactionCode = "GDT003",
                    PaymentMethod = "Chuyển khoản ngân hàng",
                    Amount = 75.00m,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    Status = PaymentStatus.Paid,
                    Notes = "Thanh toán cho buổi học gia sư",
                    SlotId = 3,
                    CreatedById = 1
                },
                new Transaction
                {
                    Id = 4,
                    TransactionCode = "GDT004",
                    PaymentMethod = "Thẻ tín dụng",
                    Amount = 120.00m,
                    CreatedDate = DateTime.Now.AddDays(-3),
                    Status = PaymentStatus.Paid,
                    Notes = "Thanh toán cho lớp học trực tuyến",
                    SlotId = 4,
                    CreatedById = 2
                },
                new Transaction
                {
                    Id = 5,
                    TransactionCode = "GDT005",
                    PaymentMethod = "PayPal",
                    Amount = 90.00m,
                    CreatedDate = DateTime.Now.AddDays(-4),
                    Status = PaymentStatus.Paid,
                    Notes = "Thanh toán cho buổi học gia sư",
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
                    Description = "Cử nhân Toán học",
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
                    Description = "Thạc sĩ Văn học Anh",
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
                    Description = "Tiến sĩ Vật lý",
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
                    Description = "Cử nhân Lịch sử",
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
                    Description = "Thạc sĩ Khoa học Máy tính",
                    SubjectId = 5,
                    DegreeNumber = "13579",
                    IssuranceDate = new DateOnly(2023, 8, 20),
                    TutorSubjectStatus = TutorSubjectDegreeStatus.Pending
                }
            );

            #endregion


            #region TutorVideo

            modelBuilder.Entity<TutorVideo>().HasData(
                new TutorVideo
                {
                    Id = 1,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video1.mp4",
                    Description = "Giới thiệu về Toán học",
                },
                new TutorVideo
                {
                    Id = 2,
                    TutorId = 2,
                    VideoUrl = "https://example.com/video2.mp4",
                    Description = "Phân tích Văn học Anh",
                },
                new TutorVideo
                {
                    Id = 3,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video3.mp4",
                    Description = "Những kiến thức cơ bản về Vật lý",
                },
                new TutorVideo
                {
                    Id = 4,
                    TutorId = 2,
                    VideoUrl = "https://example.com/video4.mp4",
                    Description = "Tổng quan về các sự kiện Lịch sử",
                },
                new TutorVideo
                {
                    Id = 5,
                    TutorId = 1,
                    VideoUrl = "https://example.com/video5.mp4",
                    Description = "Giới thiệu về Lập trình",
                }
            );

            #endregion


            #region SlotStudent

            modelBuilder.Entity<SlotStudent>().HasData(
                new SlotStudent { Id = 1, SlotId = 1, UserId = 1, Feedback = "Phản hồi về buổi học của anh John." },
                new SlotStudent { Id = 2, SlotId = 1, UserId = 2, Feedback = "Phản hồi về buổi học của Jane." },
                new SlotStudent { Id = 3, SlotId = 2, UserId = 3, Feedback = "Phản hồi về buổi học của Alice." },
                new SlotStudent { Id = 4, SlotId = 2, UserId = 4, Feedback = "Phản hồi về buổi học của Bob." },
                new SlotStudent { Id = 5, SlotId = 3, UserId = 1, Feedback = "Phản hồi về buổi học của anh John 2." }
            // Thêm nếu cần
            );

            #endregion


            #region Class

            modelBuilder.Entity<Class>().HasData(
                new Class
                {
                    Id = 1,
                    Name = "Toán học cơ bản",
                    TutorId = 1, // Thay bằng TutorId đã có
                    SubjectId = 1, // Thay bằng SubjectId đã có
                    StudentName = "Quang Phát",
                    SlotId = 1, // Thay bằng SlotId đã có
                },
                new Class
                {
                    Id = 2,
                    Name = "Văn học",
                    TutorId = 2, // Thay bằng TutorId đã có
                    SubjectId = 2, // Thay bằng SubjectId đã có
                    StudentName = "Quang Lâm ",
                    SlotId = 2, // Thay bằng SlotId đã có
                },
                new Class
                {
                    Id = 3,
                    Name = "Thí nghiệm Vật lý",
                    TutorId = 1, // Thay bằng TutorId đã có
                    SubjectId = 3, // Thay bằng SubjectId đã có
                    StudentName = "Thanh Hưng",
                    SlotId = 3, // Thay bằng SlotId đã có
                },
                new Class
                {
                    Id = 4,
                    Name = "Lịch sử thế giới",
                    TutorId = 3, // Thay bằng TutorId đã có
                    SubjectId = 4, // Thay bằng SubjectId đã có
                    StudentName = "Thanh Phúc",
                    SlotId = 4, // Thay bằng SlotId đã có
                },
                new Class
                {
                    Id = 5,
                    Name = "Hóa học nâng cao",
                    TutorId = 2, // Thay bằng TutorId đã có
                    SubjectId = 5, // Thay bằng SubjectId đã có
                    StudentName = "Nhât Linh",
                    SlotId = 5, // Thay bằng SlotId đã có
                }
            );

            #endregion


            #region StudentClass

            modelBuilder.Entity<StudentClass>().HasData(
                new StudentClass { Id = 1, StudentId = 1, ClassId = 1, Rating = 5, RatingForTutorId = 1 },
                new StudentClass { Id = 2, StudentId = 2, ClassId = 1, Rating = 2, RatingForTutorId = 2 },
                new StudentClass { Id = 3, StudentId = 3, ClassId = 2, Rating = 3, RatingForTutorId = 1 },
                new StudentClass { Id = 4, StudentId = 4, ClassId = 2, Rating = 4, RatingForTutorId = 2 },
                new StudentClass { Id = 5, StudentId = 5, ClassId = 3, Rating = 4, RatingForTutorId = 1 }
                // Add more as needed
            );

            #endregion

            #region TutorSubject
            modelBuilder.Entity<TutorSubject>().HasData(
                new TutorSubject {Id = 1,UserId = 1, SubjectId = 1 },
                new TutorSubject {Id = 2 ,UserId = 1, SubjectId = 2 },
                new TutorSubject {Id = 3 ,UserId = 2, SubjectId = 3 },
                new TutorSubject {Id = 4 ,UserId = 3, SubjectId = 1 },
                new TutorSubject {Id = 5 ,UserId = 3, SubjectId = 4 }
              
            );


            #endregion

            #region EmailTemplate
            modelBuilder.Entity<EmailTemplate>().HasData(

              new EmailTemplate
              {
                  Id = 1,
                  Name = "Welcome_Email",
                  Status = true,
                  Body = "Chào mừng bạn đến với OnDemandTutor! Kính gửi [Name], cảm ơn bạn đã tham gia cùng chúng tôi.",
                  Params = "[Name]",
                  Subject = "Welcome to OnDemandTutor!",
                  Description = "Email này được gửi để chào đón người dùng mới."
              },
            new EmailTemplate
            {
                Id = 2,
                Name = "Reminder_Email",
                Status = true,
                Body = "Xin chào [Name], đây là lời nhắc nhở cho buổi học sắp tới của bạn vào ngày [Date].",
                Params = "[Name], [Date]",
                Subject = "Reminder for Your Class",
                Description = "Email này được gửi nhắc nhở cho các buổi học đã lên lịch."
            },
            new EmailTemplate
            {
                Id = 3,
                Name = "Payment_Confirmation",
                Status = true,
                Body = "Chào bạn [Name], thanh toán của bạn là [Amount] đã được xác nhận.",
                Params = "[Name], [Amount]",
                Subject = "Payment Confirmation",
                Description = "Email này xác nhận thanh toán thành công cho dịch vụ."
            },
            new EmailTemplate
            {
                Id = 4,
                Name = "Feedback_Request",
                Status = true,
                Body = "Xin chào [Name], chúng tôi rất mong muốn nghe ý kiến phản hồi của bạn về dịch vụ của chúng tôi.",
                Params = "[Name]",
                Subject = "Feedback Request",
                Description = "Email này yêu cầu người dùng cho ý kiến về trải nghiệm của họ."
            },
            new EmailTemplate
            {
                Id = 5,
                Name = "Account_Activation",
                Status = true,
                Body = "Kính gửi [Name], vui lòng nhấp vào liên kết để kích hoạt tài khoản của bạn: [ActivationLink].",
                Params = "[Name], [ActivationLink]",
                Subject = "Account Activation",
                Description = "Email này chứa hướng dẫn để kích hoạt tài khoản người dùng."
            },
             new EmailTemplate
             {
                 Id = 6,
                 Name = "TutorRegistrationApproval",
                 Status = true,
                 Subject = "Your Tutor Registration Approval Status",
                 Body = @"
                            <!DOCTYPE html>
                    <html lang=""""vi"""">
                    <head>
                        <meta charset=""""UTF-8"""">
                        <meta Name=""""viewport"""" content=""""width=device-width, initial-scale=1.0"""">
                        <title>Tình trạng phê duyệt Đăng ký Giảng viên</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                            }
                            .container {
                                margin: 0 auto;
                                padding: 20px;
                                max-width: 600px;
                                border: 1px solid #ddd;
                                border-radius: 5px;
                                background-color: #f9f9f9;
                            }
                            .header, .footer {
                                text-align: center;
                            }
                            .content {
                                margin-top: 20px;
                            }
                            .content p {
                                margin: 10px 0;
                            }
                        </style>
                    </head>
                    <body>
                        <div class=""""container"""">
                            <div class=""""header"""">
                                <h2>Tình trạng phê duyệt Đăng ký Giảng viên</h2>
                            </div>
                            <div class=""""content"""">
                                <p>Kính gửi [TutorName],</p>
                                <p>Chúng tôi rất vui được thông báo rằng đăng ký của bạn làm giảng viên đã được xem xét.</p>
                                <p>[ApprovalStatus]</p>
                                <p>Nếu đăng ký của bạn đã được chấp nhận, bạn có thể bắt đầu sử dụng nền tảng của chúng tôi để cung cấp dịch vụ gia sư của mình. Nếu đăng ký của bạn bị từ chối, vui lòng tìm lý do bên dưới:</p>
                                <p>[RejectionReason]</p>
                                <p>Cảm ơn bạn đã quan tâm đến việc tham gia nền tảng gia sư của chúng tôi. Nếu có bất kỳ câu hỏi nào, xin vui lòng liên hệ đội ngũ hỗ trợ của chúng tôi.</p>
                            </div>
                            <div class=""""footer"""">
                                <p>Trân trọng,</p>
                                <p>Đội ngũ On Demand Tutor Platform</p>
                            </div>
                        </div>
                    </body>
                    </html>
            ",
                 Params = "[TutorName], [ApprovalStatus], [RejectionReason]",
                 Description = "Email template for notifying tutors about their registration approval status."
             },

                   new EmailTemplate
                   {
                       Id = 7,
                       Name = "Request_Withdraw_Notification",
                       Status = true,
                       Subject = "Withdrawal Request Received",
                       Body = @"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta Name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Withdrawal Request Received</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                            }
                            .container {
                                margin: 0 auto;
                                padding: 20px;
                                max-width: 600px;
                                border: 1px solid #ddd;
                                border-radius: 5px;
                                background-color: #f9f9f9;
                            }
                            .header, .footer {
                                text-align: center;
                            }
                            .content {
                                margin-top: 20px;
                            }
                            .content p {
                                margin: 10px 0;
                            }
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <div class=""header"">
                                <h2>Withdrawal Request Received</h2>
                            </div>
                            <div class=""content"">
                                <p>Dear [UserName],</p>
                                <p>We have received your request to withdraw funds. Below are the details of your request:</p>
                                <p><strong>Amount:</strong> [Amount]</p>
                                <p><strong>Bank Account Number:</strong> [BankAccountNumber]</p>
                                <p><strong>Bank Name:</strong> [BankName]</p>
                                <p><strong>Reason:</strong> [Reason]</p>
                                <p>Our team will process your request as soon as possible. If you have any questions, please feel free to contact our support team.</p>
                            </div>
                            <div class=""footer"">
                                <p>Best regards,</p>
                                <p>The Support Team</p>
                            </div>
                        </div>
                    </body>
                    </html>
                ",
                       Params = "[UserName],[Amount],[BankAccountNumber],[BankName],[Reason]",
                       Description = "Email template for notifying users about their withdrawal request."
                   },
              new EmailTemplate
              {
                  Id = 8,
                  Name = "WithDraw_Approval_Notification",
                  Status = true,
                  Subject = "Withdrawal Request Status Update",
                  Body = @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta Name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Withdrawal Request Status Update</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                }
                .container {
                    margin: 0 auto;
                    padding: 20px;
                    max-width: 600px;
                    border: 1px solid #ddd;
                    border-radius: 5px;
                    background-color: #f9f9f9;
                }
                .header, .footer {
                    text-align: center;
                }
                .content {
                    margin-top: 20px;
                }
                .content p {
                    margin: 10px 0;
                }
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h2>Withdrawal Request Status Update</h2>
                </div>
                <div class=""content"">
                    <p>Dear [UserName],</p>
                    <p>Your withdrawal request has been reviewed and updated to the following status:</p>
                    <p><strong>Status:</strong> [Status]</p>
                    <p><strong>Amount:</strong> [Amount]</p>
                    <p><strong>Reply:</strong> [Reply]</p>
                    <p>If you have any questions, please feel free to contact our support team.</p>
                </div>
                <div class=""footer"">
                    <p>Best regards,</p>
                    <p>The Support Team</p>
                </div>
            </div>
        </body>
        </html>
    ",
                  Params = "[UserName], [Status], [Amount],[Reply]",
                  Description = "Email template for notifying users about the status of their withdrawal request."
              }


            );
            #endregion

            #region RequestWithDraw
            modelBuilder.Entity<RequestWithDraw>().HasData(
                new RequestWithDraw
                {
                    Id = 1,
                    UserId = 2, // User with Id = 2 (Trần Thị B)
                    Amount = 100.0m,
                    BankAccountNumber = "123456789",
                    BankName = "Example Bank",
                    Description = "Withdrawal for tutoring services",
                    OperatorId = 1, // Assuming Operator with Id = 1 (Nguyễn Văn A)
                    Reply = "Withdrawal processed successfully",
                    Status = WithDrawStatus.Success
                },
                new RequestWithDraw
                {
                    Id = 2,
                    UserId = 4, // User with Id = 4 (Phạm Văn D)
                    Amount = 150.0m,
                    BankAccountNumber = "987654321",
                    BankName = "Another Bank",
                    Description = "Withdrawal for teaching materials",
                    OperatorId = 1, // Assuming Operator with Id = 1 (Nguyễn Văn A)
                    Reply = "Withdrawal approved",
                    Status = WithDrawStatus.Success
                },
                new RequestWithDraw
                {
                    Id = 3,
                    UserId = 3, // User with Id = 3 (Lê Văn C)
                    Amount = 200.0m,
                    BankAccountNumber = "555555555",
                    BankName = "Bank C",
                    Description = "Withdrawal for consultation services",
                    OperatorId = 2, // Assuming Operator with Id = 2 (Trần Thị B)
                    Reply = "Withdrawal pending",
                    Status = WithDrawStatus.Pending
                },
                new RequestWithDraw
                {
                    Id = 4,
                    UserId = 1, // User with Id = 1 (Nguyễn Văn A)
                    Amount = 300.0m,
                    BankAccountNumber = "111111111",
                    BankName = "Bank A",
                    Description = "Withdrawal for administrative purposes",
                    OperatorId = 3, // Assuming Operator with Id = 3 (Lê Văn C)
                    Reply = "Withdrawal under review",
                    Status = WithDrawStatus.Pending
                },
                new RequestWithDraw
                {
                    Id = 5,
                    UserId = 5, // User with Id = 5 (Đặng Văn E)
                    Amount = 250.0m,
                    BankAccountNumber = "999999999",
                    BankName = "Bank E",
                    Description = "Withdrawal for customer support",
                    OperatorId = 4, // Assuming Operator with Id = 4 (Phạm Văn D)
                    Reply = "Withdrawal processed",
                    Status = WithDrawStatus.Pending
                }
            );
            #endregion

        }
    }
}
