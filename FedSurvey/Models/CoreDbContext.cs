using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Models
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataGroup> DataGroups { get; set; }
        public virtual DbSet<DataGroupString> DataGroupStrings { get; set; }
        public virtual DbSet<DataGroupLink> DataGroupLinks { get; set; }
        public virtual DbSet<Execution> Executions { get; set; }
        public virtual DbSet<MergeCandidateDTO> MergeCandidateDTOs { get; set; }
        public virtual DbSet<PossibleResponseString> PossibleResponseStrings { get; set; }
        public virtual DbSet<PossibleResponse> PossibleResponses { get; set; }
        public virtual DbSet<QuestionExecution> QuestionExecutions { get; set; }
        public virtual DbSet<QuestionTypeString> QuestionTypeStrings { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionLink> QuestionLinks { get; set; }
        public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }
        public virtual DbSet<QuestionDTO> QuestionDTOs { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<ResponseDTO> ResponseDTOs { get; set; }
        public virtual DbSet<ResultDTO> ResultDTOs { get; set; }
        public virtual DbSet<View> Views { get; set; }
        public virtual DbSet<ViewConfig> ViewConfigs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Database=SurveyDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<DataGroupString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DataGroup)
                    .WithMany(p => p.DataGroupStrings)
                    .HasForeignKey(d => d.DataGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DataGroupStrings_DataGroups_DataGroupId");
            });
            modelBuilder.Entity<DataGroupString>().ToTable("DataGroupStrings");

            modelBuilder.Entity<DataGroupLink>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.ParentLinks)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_DataGroupLinks_DataGroups_ParentId");

                entity.HasOne(d => d.Child)
                    .WithMany(p => p.ChildLinks)
                    .HasForeignKey(d => d.ChildId)
                    .HasConstraintName("FK_DataGroupLinks_DataGroups_ChildId");
            });
            modelBuilder.Entity<DataGroupLink>().ToTable("DataGroupLinks");

            modelBuilder.Entity<Execution>(entity =>
            {
                entity.HasIndex(e => e.Key)
                    .HasName("UQ__Executio__C41E02890F7150F6")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Key).IsUnicode(false);
            });

            modelBuilder.Entity<PossibleResponseString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.PossibleResponse)
                    .WithMany(p => p.PossibleResponseStrings)
                    .HasForeignKey(d => d.PossibleResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses");
            });

            modelBuilder.Entity<PossibleResponse>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.PossibleResponses)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponses_QuestionTypes");
            });

            modelBuilder.Entity<QuestionExecution>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Execution)
                    .WithMany(p => p.QuestionExecutions)
                    .HasForeignKey(d => d.ExecutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionExecutions_Executions");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionExecutions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionExecutions_Questions");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<QuestionTypeString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.QuestionTypeStrings)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId");
            });
            modelBuilder.Entity<QuestionTypeString>().ToTable("QuestionTypeStrings");

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_QuestionTypes");
            });

            modelBuilder.Entity<QuestionLink>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.QuestionGroup)
                    .WithMany(p => p.QuestionLinks)
                    .HasForeignKey(d => d.QuestionGroupId)
                    .HasConstraintName("FK_QuestionLinks_QuestionGroups_QuestionGroupId");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionLinks)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_QuestionLinks_Questions_QuestionId");
            });
            modelBuilder.Entity<QuestionLink>().ToTable("QuestionLinks");

            modelBuilder.Entity<QuestionGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<QuestionGroup>().ToTable("QuestionGroups");

            modelBuilder.Entity<Response>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.DataGroup)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.DataGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_DataGroups");

                entity.HasOne(d => d.PossibleResponse)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.PossibleResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_PossibleResponses");

                entity.HasOne(d => d.QuestionExecution)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.QuestionExecutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_QuestionExecutions");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<View>().ToTable("Views");

            modelBuilder.Entity<ViewConfig>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.View)
                    .WithMany(p => p.ViewConfigs)
                    .HasForeignKey(d => d.ViewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ViewConfigs_Views_ViewId");
            });
            modelBuilder.Entity<ViewConfig>().ToTable("ViewConfigs");

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasIndex(e => e.Body)
                    .HasName("UQ__Token__C41E02890F7150E6")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Token>().ToTable("Tokens");

            // Later this should probably go away, as ResultDTO does the same thing but better.
            modelBuilder.Entity<ResponseDTO>().ToSqlQuery(
                @"SELECT
                  Responses.Id,
                  Responses.Count,
                  Responses.QuestionExecutionId,
                  Responses.PossibleResponseId,
                  Responses.DataGroupId,
                  CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 OR PossibleResponses.PartOfPercentage = 0 THEN 0 ELSE Responses.Count / (CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 THEN 1 ELSE SUM(QuestionExecutionResponses.Count) END) * 100 END AS Percentage
                FROM Responses
                JOIN PossibleResponses
                ON PossibleResponses.Id = Responses.PossibleResponseId
                LEFT JOIN (
                    SELECT Responses.*
                    FROM Responses
                    JOIN PossibleResponses
                        ON PossibleResponses.Id = Responses.PossibleResponseId
                    WHERE PossibleResponses.PartOfPercentage = 1
                ) QuestionExecutionResponses
                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId
                    AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId
                GROUP BY
                Responses.Id,
                Responses.Count,
                Responses.QuestionExecutionId,
                Responses.PossibleResponseId,
                Responses.DataGroupId,
                PossibleResponses.PartOfPercentage"
            );
            modelBuilder.Entity<ResponseDTO>().ToTable(null);

            modelBuilder.Entity<ResultDTO>().HasNoKey();
            modelBuilder.Entity<ResultDTO>().ToTable(null);

            modelBuilder.Entity<QuestionDTO>().ToSqlQuery(
                @"WITH LatestOccurredTime AS (
	                SELECT
		                Questions.Id,
		                MAX(Executions.OccurredTime) AS Latest
	                FROM Questions
	                JOIN QuestionExecutions
	                ON QuestionExecutions.QuestionId = Questions.Id
	                JOIN Executions
	                ON Executions.Id = QuestionExecutions.ExecutionId
	                GROUP BY Questions.Id
                )
                SELECT
	                Questions.Id,
	                MAX(QuestionExecutions.Body) AS Body
                FROM Questions
                JOIN LatestOccurredTime
                ON LatestOccurredTime.Id = Questions.Id
                JOIN Executions
                ON Executions.OccurredTime = LatestOccurredTime.Latest
                JOIN QuestionExecutions
                ON QuestionExecutions.QuestionId = Questions.Id
                AND QuestionExecutions.ExecutionId = Executions.Id
                GROUP BY
                Questions.Id"
            );
            modelBuilder.Entity<QuestionDTO>().ToTable(null);

            modelBuilder.Entity<MergeCandidateDTO>().ToSqlQuery(
                @"WITH LatestOccurredTime AS (
	                SELECT
		                Questions.Id,
		                MAX(Executions.OccurredTime) AS Latest
	                FROM Questions
	                JOIN QuestionExecutions
	                ON QuestionExecutions.QuestionId = Questions.Id
	                JOIN Executions
	                ON Executions.Id = QuestionExecutions.ExecutionId
	                GROUP BY Questions.Id
                )
                SELECT
	                Questions.Id AS QuestionId,
                    Executions.""Key"" AS ExecutionKey,
	                MAX(QuestionExecutions.Body) AS Body,
                    MAX(QuestionExecutions.Position) AS Position
                FROM Questions
                JOIN LatestOccurredTime
                ON LatestOccurredTime.Id = Questions.Id
                JOIN Executions
                ON Executions.OccurredTime = LatestOccurredTime.Latest
                JOIN QuestionExecutions
                ON QuestionExecutions.QuestionId = Questions.Id
                AND QuestionExecutions.ExecutionId = Executions.Id
                LEFT JOIN QuestionExecutions OverallExecutions
                ON OverallExecutions.QuestionId = Questions.Id
                GROUP BY
                Questions.Id,
                Executions.""Key""
                HAVING COUNT(OverallExecutions.Id) = 1"
            );
            modelBuilder.Entity<MergeCandidateDTO>().ToTable(null);
            modelBuilder.Entity<MergeCandidateDTO>().HasNoKey();

            OnModelCreatingPartial(modelBuilder);
        }

        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Added)
                .ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedTime").CurrentValue = DateTime.UtcNow;
                E.Property("UpdatedTime").CurrentValue = DateTime.UtcNow;
            });

            var EditedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Modified)
                .ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("UpdatedTime").CurrentValue = DateTime.UtcNow;
            });

            return base.SaveChanges();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
