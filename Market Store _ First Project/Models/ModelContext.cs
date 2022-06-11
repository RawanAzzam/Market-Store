using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aboutus> Aboutus { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CodeSale> CodeSale { get; set; }
        public virtual DbSet<Contactus> Contactus { get; set; }
        public virtual DbSet<Contactususer> Contactususer { get; set; }
        public virtual DbSet<Home> Home { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductStore> ProductStore { get; set; }
        public virtual DbSet<Productorder> Productorder { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Systemuser> Systemuser { get; set; }
        public virtual DbSet<Testimonial> Testimonial { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<Userorder> Userorder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("USER ID=Tah14_User102;PASSWORD=Rram1210.;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "TAH14_USER102");

            modelBuilder.Entity<Aboutus>(entity =>
            {
                entity.ToTable("ABOUTUS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Info)
                    .HasColumnName("INFO")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OurFeatures1)
                   .HasColumnName("OUR_FEATURES1")
                   .HasMaxLength(300)
                   .IsUnicode(false);

                entity.Property(e => e.OurFeatures2)
                    .HasColumnName("OUR_FEATURES2")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.OurFeatures3)
                    .HasColumnName("OUR_FEATURES3")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                   .HasColumnName("IMAGEPATH")
                   .HasMaxLength(100)
                   .IsUnicode(false);

            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("CARD");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Balance)
                    .HasColumnName("BALANCE")
                    .HasColumnType("FLOAT");

                entity.Property(e => e.Expiredate)
                    .HasColumnName("EXPIREDATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.Tcb)
                    .HasColumnName("TCB")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName)
                    .HasColumnName("CATEGORY_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .HasColumnName("IMAGE_PATH")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CodeSale>(entity =>
            {
                entity.ToTable("CODESALE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.code)
                    .HasColumnName("CODE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.sale)
                    .HasColumnName("SALE")
                    .HasColumnType("FLOAT");


            });

            modelBuilder.Entity<Contactus>(entity =>
            {
                entity.ToTable("CONTACTUS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("NUMBER");
            });

            modelBuilder.Entity<Contactususer>(entity =>
            {
                entity.ToTable("CONTACTUSUSER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .HasColumnName("MESSAGE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Home>(entity =>
            {
                

                entity.ToTable("HOME");

                entity.Property(e => e.Id)
                   .HasColumnName("ID")
                   .HasColumnType("NUMBER")
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Logoimage)
                    .HasColumnName("LOGOIMAGE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

               

                entity.Property(e => e.Slide1)
                    .HasColumnName("SLIDE1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Slide2)
                    .HasColumnName("SLIDE2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Slide3)
                    .HasColumnName("SLIDE3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Websitename)
                    .HasColumnName("WEBSITENAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DateOfAdd)
                    .HasColumnName("DATE_OF_ADD")
                    .HasColumnType("DATE");

                entity.Property(e => e.ImagePath)
                    .HasColumnName("IMAGE_PATH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Namee)
                    .HasColumnName("NAMEE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("PRICE")
                    .HasColumnType("FLOAT");

                entity.Property(e => e.ProductCategoryId)
                    .HasColumnName("PRODUCT_CATEGORY_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Sale)
                    .HasColumnName("SALE")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("PRODUCT_FK1");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("PRODUCT_CATEGORY");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ImagePath)
                    .HasColumnName("IMAGE_PATH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductStore>(entity =>
            {
                entity.ToTable("PRODUCT_STORE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Count)
                    .HasColumnName("COUNT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Productid)
                    .HasColumnName("PRODUCTID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Storeid)
                    .HasColumnName("STOREID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStore)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("PRODUCT_STORE_FK1");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ProductStore)
                    .HasForeignKey(d => d.Storeid)
                    .HasConstraintName("PRODUCT_STORE_FK2");
            });

            modelBuilder.Entity<Productorder>(entity =>
            {
                entity.ToTable("PRODUCTORDER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Orderid)
                    .HasColumnName("ORDERID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Productid)
                    .HasColumnName("PRODUCTID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Quntity)
                    .HasColumnName("QUNTITY")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Productorder)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("PRODUCTORDER_FK1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productorder)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PRODUCTORDER_FK3");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("RATE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Feedback)
                    .HasColumnName("FEEDBACK")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId)
                    .HasColumnName("PRODUCT_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RateNum)
                    .HasColumnName("RATE_NUM")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Rate)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RATE_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rate)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RATE_FK1");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("REPORT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Mesaage)
                    .HasColumnName("MESAAGE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Storeid)
                    .HasColumnName("STOREID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Storeid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Rolename)
                    .IsRequired()
                    .HasColumnName("ROLENAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("STORE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Categoryid)
                    .HasColumnName("CATEGORYID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Ownername)
                    .HasColumnName("OWNERNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StoreLogo)
                    .HasColumnName("STORE_LOGO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Storelocation)
                    .HasColumnName("STORELOCATION")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Storename)
                    .HasColumnName("STORENAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.Categoryid)
                    .HasConstraintName("STORE_FK1");
            });

            modelBuilder.Entity<Systemuser>(entity =>
            {
                entity.ToTable("SYSTEMUSER");

                entity.HasIndex(e => e.Email)
                    .HasName("SYSTEMUSER_UK1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .HasColumnName("IMAGE_PATH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasColumnName("LOCATION")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.ToTable("TESTIMONIAL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Isverfiy)
                    .HasColumnName("ISVERFIY")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Rate)
                    .HasColumnName("RATE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Testimonial)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("TESTIMONIAL_FK2");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("USER_LOGIN");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsVerfiy)
                    .HasColumnName("IS_VERFIY")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Passwordd)
                    .HasColumnName("PASSWORDD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ROLE_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_LOGIN_FK1");
            });

            modelBuilder.Entity<Userorder>(entity =>
            {
                entity.ToTable("USERORDER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Cost)
                    .HasColumnName("COST")
                    .HasColumnType("FLOAT");

                entity.Property(e => e.Dateoforder)
                    .HasColumnName("DATEOFORDER")
                    .HasColumnType("DATE");

                entity.Property(e => e.IsCheckout)
                    .HasColumnName("IS_CHECKOUT")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("NUMBER");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userorder)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("USERORDER_FK1");
            });

            modelBuilder.HasSequence("COURSE_SEQ");

            modelBuilder.HasSequence("DEPARTMENT_ID");

            modelBuilder.HasSequence("EMPLOYEE_SEQ");

            modelBuilder.HasSequence("EXAM_SEQ");

            modelBuilder.HasSequence("ID_SEQUENCE");

            modelBuilder.HasSequence("POSITION_SEQ");

            modelBuilder.HasSequence("TEACHER_SEQ");

            modelBuilder.HasSequence("TEACHERCOURSE_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
