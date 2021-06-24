﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SurveyBackend.Models
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext()
        {
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataGroups> DataGroups { get; set; }
        public virtual DbSet<Executions> Executions { get; set; }
        public virtual DbSet<PossibleResponseStrings> PossibleResponseStrings { get; set; }
        public virtual DbSet<PossibleResponses> PossibleResponses { get; set; }
        public virtual DbSet<QuestionExecutions> QuestionExecutions { get; set; }
        public virtual DbSet<QuestionTypes> QuestionTypes { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Responses> Responses { get; set; }

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
            modelBuilder.Entity<DataGroups>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Executions>(entity =>
            {
                entity.HasIndex(e => e.Key)
                    .HasName("UQ__Executio__C41E02890F7150F6")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Key).IsUnicode(false);
            });

            modelBuilder.Entity<PossibleResponseStrings>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.PossibleResponse)
                    .WithMany(p => p.PossibleResponseStrings)
                    .HasForeignKey(d => d.PossibleResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses");
            });

            modelBuilder.Entity<PossibleResponses>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.PossibleResponses)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponses_QuestionTypes");
            });

            modelBuilder.Entity<QuestionExecutions>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

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

            modelBuilder.Entity<QuestionTypes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_QuestionTypes");
            });

            modelBuilder.Entity<Responses>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
